using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace ComputerizedLibrarySystem
{
    public class ManageSMS
    {
        public static int noOfSMSSent;
        public static int noOfSMSFailed;

        public static void sendSMSToAllBorrowedUsersList(DataTable tblBorrowedUsers)
        {
            try
            {
                noOfSMSSent = 0;
                noOfSMSFailed = 0;
                foreach (DataRow dataRow in tblBorrowedUsers.Rows)
                {
                    //send SMS to each user if it has phone no
                    if (!string.IsNullOrEmpty(dataRow[5].ToString()))
                    {
                        string smsReply = SendSMS(AddPhoneCode(dataRow[5].ToString()));
                        SMSAlert smsAlert = new SMSAlert(dataRow);
                        smsAlert.Remarks = smsReply;
                        if (!string.IsNullOrEmpty(smsReply))
                        {
                            if (smsReply.IndexOf("OK") >= 0)
                            {
                                smsAlert.SmsStatus = "SENT";
                                new DbSystem(Application.StartupPath).save(smsAlert); //save to db on successfully sent
                                noOfSMSSent++;
                            }
                            else
                                //smsAlert.SmsStatus = "FAIL";
                                noOfSMSFailed++;//increment counter if SMSReply is not OK
                        }
                        else
                            //smsAlert.SmsStatus = "FAIL";
                            noOfSMSFailed++; //increment counter if SMSReply is null or empty
                        
                    }
                    else
                        //smsAlert.SmsStatus = "FAIL";
                        noOfSMSFailed++; //increment counter if Phone No is null or empty
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        private static string SendSMS(string phoneNo)
        {
            string reply = "";
            try
            {
                //check for internet connectivity
                if (isConnectedToInternet())
                {
                    string user = "BProgrammer";
                    string password = "123456";
                    string senderID = "LibrarySMS";
                    string message = "Dear Library User, This is to inform you as one of our patrons in the Library that the book(s) you borrowed is/are due " +
                                    "for return today.";
                    string urlForSMS = "http://www.adorablesms.com/components/com_spc/smsapi.php?username=" + user + "&password=" + password + "&sender=" + senderID + "&recipient=" + phoneNo + "&message=" + message;
                    reply = SendSMSProcess(urlForSMS);
                }
                else
                {
                    Console.WriteLine("Fail to send SMS to this number: {0}. No Internet connectivity", phoneNo);
                    reply = "Fail to send SMS to this number. No Internet connectivity";
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
            return reply;
        }

        //Creating a method to add phone country code to phone no for SMS
        private static string AddPhoneCode(string PhoneNo)
        {
            string phone = "";
            if (PhoneNo.StartsWith("234")) phone = PhoneNo;
            else if (PhoneNo.StartsWith("0") && (PhoneNo.Length == 11)) phone = "234" + PhoneNo.Substring(1);
            return phone;
        }

        //creating a function to check for internet connectivity
        public static bool isConnectedToInternet()
        {
            string host = "www.google.com";
            bool result = false;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 10000);//trying to send and receive ICMP echo message to and fro google.com within a timeout of 10secs(10000)
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine("reply: " + reply.Status);
                    return true;
                }
                else
                    result = WebRequestTest(); //check the connectivity using webrequest
            }
            catch (PingException e) { Console.WriteLine(e.Message); }
            return result;
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

        //Creating a web process function that excute the url for sending the SMS at the background with launching the browser
        protected static String SendSMSProcess(string url)
        {
            string result = "";
            // Using WebRequest
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Console.WriteLine("SMS result: " + result);
            }
            catch (Exception e)
            {
                //result = e.Message;
                Console.WriteLine(e.Message);
                return result;
            }
            return result;
        }
    }
}
