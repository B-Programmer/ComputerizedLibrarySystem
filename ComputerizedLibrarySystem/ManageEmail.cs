using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ComputerizedLibrarySystem
{
    public class ManageEmail
    {
        static string response;
        public static int noOfEmailSent;
        public static int noOfEmailFailed;
        
        public static void sendEmailToAllBorrowedUsersList(DataTable tblBorrowedUsers)
        {
            try
            {
                noOfEmailSent = 0;
                noOfEmailFailed = 0;
                foreach (DataRow dataRow in tblBorrowedUsers.Rows)
                {
                    //send E-mail to each user if it has Email
                    if (!string.IsNullOrEmpty(dataRow[5].ToString()))
                    {
                        if (ValidateEmail(dataRow[5].ToString()))
                        {
                            response = "";
                            string emailReply = SendEmail((dataRow[5].ToString()));
                            EmailAlert emailAlert = new EmailAlert(dataRow);
                            emailAlert.Remarks = emailReply;
                            if (!string.IsNullOrEmpty(emailReply))
                            {
                                if (emailReply.IndexOf("Success") >= 0)
                                {
                                    emailAlert.EmailStatus = "SENT";
                                    new DbSystem(Application.StartupPath).save(emailAlert); //save to db on success
                                    noOfEmailSent++; //increase count for sent email
                                }
                                else
                                    //emailAlert.EmailStatus = "FAIL";
                                    noOfEmailFailed++; //increase count for failed email
                            }
                            else
                            {
                                //fail to send email, no response from server
                                //emailAlert.EmailStatus = "FAIL";
                                noOfEmailFailed++; //increase count for failed email
                            }
                            
                        }
                        else
                        {
                            //fail to send mail, invalid email address
                            noOfEmailFailed++; //increase count for failed email
                        }
                    }
                    else
                    {
                        //fail to send mail, invalid email address(null or empty)
                        noOfEmailFailed++; //increase count for failed email
                    }

                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        private static string SendEmail(string email)
        {
            string reply = "";
            try
            {
                //check for internet connectivity
                if (isConnectedToInternet())
                {
                    string user = "library.fedpoly@gmail.com";
                    string password = "library12345";
                    string subject = "FEDPOFFA LIBRARY COURTESY DUE NOTICE";
                    string message = "Dear Library User, This is to inform you as one of our patrons in the Library that the book(s) you borrowed is/are due " +
                                    "for return today. Therefore, in order to maintain your borrowing privileges in our Library, you are expected to return the book(s) at " +
                                    "its due date or come for renewal before the due date expires. Failure to do so would attract a FINE FEE which will be calculated immediately" +
                                    "after the due date. Thank you.";

                    NetworkCredential login = new NetworkCredential(user, password);
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);//587 is port number
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = login;
                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress(user);
                    msg.To.Add(email);
                    msg.Subject = subject;
                    msg.Body = message;
                    //client.Send(msg);
                    msg.Priority = MailPriority.Normal;
                    msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallBack);
                    string userState = "Sending....";
                    client.SendAsync(msg, userState);
                    //MessageBox.Show("Your message has been sent successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reply = response;
                }
                else
                {
                    Console.WriteLine("Fail to send email to this number: {0}. No Internet connectivity", email);
                    reply = "Fail to send email to this number. No Internet connectivity";
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
            return reply;
        }

        //Creating a method to perform check on if email has been sent completely or not
        private static void SendCompletedCallBack(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //MessageBox.Show(string.Format("{0} send canceled.", e.UserState), "Failure Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                response = "Send e-mail canceled.";
            }
            if (e.Error != null)
            {
                //MessageBox.Show(string.Format("{0}\n {1}", e.UserState, e.Error), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                response = "Send e-mail failed due to error";
            }
            else
            {
                //MessageBox.Show("Your message has been sent successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                response = "Success";
            }
        }

        //creating a function to check for internet connectivity
        public static bool isConnectedToInternet()
        {
            string host = "www.google.com";
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 5000);//trying to send and receive ICMP echo message to and fro google.com within a timeout of 5secs(5000)
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine("reply: " + reply.Status);
                    return true;
                }
                
            }
            catch (PingException e) { Console.WriteLine(e.Message); }
            return WebRequestTest(); //check the connectivity using webrequest;
        }

        //Creating a function for checking internet connectivity through webrequest test
        public static bool WebRequestTest()
        {
            string url = "http://www.google.com";
            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
                System.Net.WebResponse myResponse = myRequest.GetResponse();
            }
            catch (System.Net.WebException)
            {
                return false;
            }
            return true;
        }

        //Creating a function that validate email before email will be sent
        private static bool ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                //Console.WriteLine(email + " is Valid Email Address");
                return true;
            
            return false;
        }
    }
}
