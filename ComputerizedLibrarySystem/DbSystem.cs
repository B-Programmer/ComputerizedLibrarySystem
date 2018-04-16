using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using Microsoft.Vbe.Interop;
using System.Windows.Forms;
using System.Data;

namespace ComputerizedLibrarySystem
{
    public class DbSystem
    {
        private OleDbConnection cn;
        private OleDbCommand cmd;
        private OleDbDataReader dr;
        //private Book book;

        public DbSystem(string startUpPath)
        {
            cn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + startUpPath + "\\DbLibrarySystem.accdb;Persist Security Info=True");
            cmd = new OleDbCommand();
            cmd.Connection = cn;
        }

        //Save function for saving book into book table
        public void save(Book book)
        {
            
            cn.Open();
            cmd.CommandText = "insert into tblBookReg values('" + book.AccessionNo + "','" + book.AuthorName + "','" + book.BookTitle + "','" + book.Publisher + "','" + book.PlaceOfPublication + "','" + book.NoOfCopies + "','" + book.Isbn + "','" + book.DateOfRegistration + "','" + book.DateOfPublished + "','" + book.Status + "')";
            cmd.ExecuteNonQuery();
            //MessageBox.Show("New Book Registration Details Successfully Saved!!!", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cn.Close();
        }

        //Save function for saving SMSAlert into Alert table
        public void save(SMSAlert smsAlert)
        {

            cn.Open();
            cmd.CommandText = "insert into tblSMSAlert(Borrow_Id, Library_Id, Date_Due, SMS_Status, SMS_Date, Phone_No, SMS_Sender, Trials, Remarks) values('" + smsAlert.BorrowId + "','" + smsAlert.LibraryId + "','" + smsAlert.DateDue + "','" + smsAlert.SmsStatus + "','" + smsAlert.SmsDate + "','" + smsAlert.PhoneNo + "','" + smsAlert.SmsSender + "'," + smsAlert.Trials + ",'" + smsAlert.Remarks + "')";
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        //Save function for saving EmailAlert into Alert table
        public void save(EmailAlert emailAlert)
        {

            cn.Open();
            cmd.CommandText = "insert into tblSMSAlert(Borrow_Id, Library_Id, Date_Due, Email_Status, Email_Date, Email, Email_Sender, Trials, Remarks) values('" + emailAlert.BorrowId + "','" + emailAlert.LibraryId + "','" + emailAlert.DateDue + "','" + emailAlert.EmailStatus + "','" + emailAlert.EmailDate + "','" + emailAlert.Email + "','" + emailAlert.EmailSender + "'," + emailAlert.Trials + ",'" + emailAlert.Remarks + "')";
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public string genAccessionNo()
        {
            string AccessionNo = "LIB/ASC/";
            try
            {
                int rowCount = 0;
                cn.Open();
                cmd.CommandText = "select count(*) as RowCount from tblBookReg";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                    rowCount = (int)(dr["RowCount"]);
                rowCount++;
                if (rowCount < 10)
                {
                    AccessionNo = AccessionNo + "00000" + rowCount;
                }
                else if (rowCount < 100)
                {
                    AccessionNo = AccessionNo + "0000" + rowCount;
                }
                else if (rowCount < 1000)
                {
                    AccessionNo = AccessionNo + "000" + rowCount;
                }
                else if (rowCount < 10000)
                {
                    AccessionNo = AccessionNo + "00" + rowCount;
                }
                else if (rowCount < 100000)
                {
                    AccessionNo = AccessionNo + "0" + rowCount;
                }
                else
                {
                    AccessionNo = AccessionNo + rowCount;
                }
                dr.Close();
                cn.Close();

            }
            catch (Exception Er)
            {
                dr.Close();
                cn.Close();
                MessageBox.Show(Er.Message, "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return AccessionNo;
        }

        //This function is to generate all users that borrowed books and they are due for sending SMS or Email Alert
        public DataTable genBorrowedBookUsers(string Alert)
        {
            string[] arrRpt = new string[7];
            DataTable itemTable = new DataTable();
            itemTable.Columns.Add("Library ID", typeof(string));
            itemTable.Columns.Add("Library Username", typeof(string));
            itemTable.Columns.Add("Borrowed Book", typeof(string));
            itemTable.Columns.Add("Borrow Date", typeof(string));
            itemTable.Columns.Add("Due Date", typeof(string));
            itemTable.Columns.Add("User Phone No", typeof(string));
            itemTable.Columns.Add("Borrow Id", typeof(string));

            try
            {
                if (cn.State == ConnectionState.Open) cn.Close();

                cn.Open();
                if (Alert.Equals("SMS"))
                    cmd.CommandText = "SELECT a.Library_Id, b.Username, c.Book_Title, a.Borrow_Date, a.Due_Date, b.Phone_No, a.id from tblBookBorrow a, tblLibraryReg b, tblBookReg c WHERE a.Borrow_Status = 1 AND a.Accession_No = c.Accession_No AND a.Library_Id = b.Library_ID AND a.id NOT IN (Select borrow_id from tblSMSAlert)";
                else
                    cmd.CommandText = "SELECT a.Library_Id, b.Username, c.Book_Title, a.Borrow_Date, a.Due_Date, b.Email, a.id from tblBookBorrow a, tblLibraryReg b, tblBookReg c WHERE a.Borrow_Status = 1 AND a.Accession_No = c.Accession_No AND a.Library_Id = b.Library_ID AND a.id NOT IN (Select borrow_id from tblEmailAlert)";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    
                    arrRpt[0] = ((dr[0]).ToString()); //populate the Library ID
                    arrRpt[1] = ((dr[1]).ToString()); //populate the Library Username
                    arrRpt[2] = ((dr[2]).ToString()); //populate the Borrowed Book
                    arrRpt[3] = ((dr[3]).ToString()); //populate the Borrow Date
                    arrRpt[4] = ((dr[4]).ToString()); //populate the Due Date
                    arrRpt[5] = ((dr[5]).ToString()); //populate the User Phone No or Email 
                    arrRpt[6] = ((dr[6]).ToString()); //populate the Borrow Id 
                    //add item into Table
                    DateTime date1 = DateTime.Parse(arrRpt[4]);
                    DateTime date2 = DateTime.Today;
                    int result = DateTime.Compare(date1, date2);
                    Console.WriteLine("result: " + result);
                    if (result <= 0)
                    {
                        itemTable.Rows.Add(arrRpt[0], arrRpt[1], arrRpt[2], arrRpt[3], arrRpt[4], arrRpt[5], arrRpt[6]);
                        Console.WriteLine("save item to table: ");
                    }
                }
                dr.Close();
                cn.Close();

            }
            catch (Exception Er)
            {
                MessageBox.Show(Er.Message, "Automated Library System ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return itemTable;
        }
    }
}
