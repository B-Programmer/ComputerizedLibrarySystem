using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComputerizedLibrarySystem
{
    public partial class FrmSendEmailList : Form
    {
        DataTable borrowedUsersTable;

        public FrmSendEmailList()
        {
            InitializeComponent();
        }

        private void FrmSendEmailList_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            //Add Column Header for listView 1
            listView1.Columns.Add("Library ID", 100);
            listView1.Columns.Add("Library Username", 300);
            listView1.Columns.Add("Borrowed Book", 100);
            listView1.Columns.Add("Borrow Date", 100);
            listView1.Columns.Add("Due Date", 100);
            listView1.Columns.Add("User Email", 100);
            borrowedUsersTable = new DbSystem(Application.StartupPath).genBorrowedBookUsers("EMAIL");
            displayBorrowedUserlist(borrowedUsersTable);
        }

        private void displayBorrowedUserlist(DataTable tbl)
        {
            string[] arrRpt = new string[6];
            ListViewItem item;

            try
            {

                foreach (DataRow datarow in tbl.Rows)
                {
                    {
                        arrRpt[0] = (datarow[0].ToString()); //populate the Library ID
                        arrRpt[1] = (datarow[1].ToString()); //populate the Library Username
                        arrRpt[2] = (datarow[2].ToString()); //populate the Borrowed Book
                        arrRpt[3] = (datarow[3].ToString()); //populate the Borrow Date
                        arrRpt[4] = (datarow[4].ToString()); //populate the Due Date
                        arrRpt[5] = (datarow[5].ToString()); //populate the User Phone No 
                        //add item into list
                        item = new ListViewItem(arrRpt);
                        listView1.Items.Add(item);
                    }
                }

            }
            catch (Exception Er)
            {
                MessageBox.Show(Er.Message, "Automated Library System ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (borrowedUsersTable.Rows.Count > 0)
                {
                    ManageEmail.sendEmailToAllBorrowedUsersList(borrowedUsersTable);
                    if (ManageEmail.noOfEmailSent > 0 && ManageEmail.noOfEmailFailed == 0)
                        MessageBox.Show("Email has been Successfully Sent!!!", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        string serverResponse = "Sending mail not successful....\n\r" +
                            "Attempt to Send Email fail due to either there is no internet connectivity, invalid email address(es), or SMTP failure to communicate to the mail server.\n\r" +
                            "Email Sending Status: \n\rNumber of Email Sent: " + ManageEmail.noOfEmailSent.ToString() + "\n\rNumber of Email Failed: " + ManageEmail.noOfEmailFailed.ToString();
                        MessageBox.Show(serverResponse, "Automated Library System ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                    MessageBox.Show("Fail to Send to Send Email!!! Nothing to Send Email to", "Automated Library System ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Hide();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
