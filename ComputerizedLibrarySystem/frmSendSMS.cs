using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;

namespace ComputerizedLibrarySystem
{
    public partial class frmSendSMS : Form
    {
        private OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\DbLibrarySystem.accdb;Persist Security Info=True");
        private OleDbCommand cmd = new OleDbCommand();
        private OleDbDataReader dr;
        bool nonNumberEntered;
        public frmSendSMS()
        {
            InitializeComponent();
        }

        private void frmSendSMS_Load(object sender, EventArgs e)
        {
            cmd.Connection = cn;
            cboSchool.Items.Clear();
            cboSchool.Items.Add("Business and Management Studies");
            cboSchool.Items.Add("Engineering Technology");
            cboSchool.Items.Add("Environmental Science");
            cboSchool.Items.Add("General Studies");
            cboSchool.Items.Add("Communication and Information Technology");
            cboSchool.Items.Add("Applied Science");
            btnRefresh();
            label9.Text = "Type Student Matric No:";
            label3.Text = "Matric No:";
            optSearchByStudentId.Checked = false;
            optSearchByStaffId.Checked = true;
            txtEditStaffId.Focus();

        }

        private void btnRefresh()
        {
            txtStaffID.Clear(); txtStaffID.ReadOnly = true;
            txtLibraryId.Clear(); txtLibraryId.ReadOnly = true;
            txtName.Clear(); txtName.ReadOnly = true;
            txtMessage.Clear(); txtMessage.ReadOnly = true;
            txtPhoneNo.Clear(); txtPhoneNo.ReadOnly = true;
            cboSchool.ResetText(); cboSchool.Enabled = false;
            cboDepartment.ResetText(); cboDepartment.Enabled = false;
            btnSendSMS.Enabled = false;
            //dr.Close();
            //cn.Close();
        }

        private void disableRefresh()
        {
            txtMessage.ReadOnly = false;
            txtPhoneNo.ReadOnly = false;
            txtMessage.Focus();
            btnSendSMS.Enabled = true;
        }

        //private void sendSMS()
        //{
        //    string user = "BProgrammer";
        //    string password = "123456";
        //    string recipient = "2348102375562, 2348126245069, 2348069018655";
        //    string message = "This is a sample message from Adorable SMS";
        //    string senderID = "LibrarySMS";
        //    Process.Start("http://www.adorablesms.com/components/com_spc/smsapi.php?username=" + user + "&password=" + password + "&sender=" + senderID + "&recipient=" + recipient + "&message=" + message);
        //}

        private void txtEditStaffId_TextChanged(object sender, EventArgs e)
        {
            btnRefresh();
            if ((txtEditStaffId.Text != ""))
            {
                if ((optSearchByStaffId.Checked))
                {
                    loadStaffDetails(txtEditStaffId.Text, "STAFF");
                }
                else
                {
                    loadStaffDetails(txtEditStaffId.Text, "STUDENT");
                }
                
            }
        }

        private void loadStaffDetails(string uID, string searchBy)
        {
            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                if (searchBy == "STAFF")
                    cmd.CommandText = "select * from tblLibraryReg Where User_Type = 'STAFF' and Matric_No = '" + uID + "'";
                else if (searchBy == "STUDENT")
                    cmd.CommandText = "select * from tblLibraryReg Where User_Type = 'STUDENT' and Matric_No = '" + uID + "'";
                //if (dr.IsClosed == false) 
                //        dr.Close();

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtLibraryId.Text = dr["Library_ID"].ToString();
                    txtStaffID.Text = dr["Matric_No"].ToString();
                    txtName.Text = dr["Username"].ToString();
                    cboSchool.Text = dr["School"].ToString();
                    cboDepartment.Text = dr["Department"].ToString();
                    txtPhoneNo.Text = dr["Phone_No"].ToString();

                    disableRefresh();
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
        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {
            try{
                if (isAllEntryFilled())
                {
                    
                    sendSMS(AddPhoneCode(txtPhoneNo.Text), txtMessage.Text);
                    MessageBox.Show("Library SMS Successfully Sent!!!", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Information);
 
                }

            }
            catch (Exception Er)
            {
                dr.Close();
                cn.Close();
                MessageBox.Show(Er.Message, "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sendSMS(string recipient, string message)
        {
            string user = "BProgrammer";
            string password = "123456";
            string senderID = "LibrarySMS";
            Process.Start("http://www.adorablesms.com/components/com_spc/smsapi.php?username=" + user + "&password=" + password + "&sender=" + senderID + "&recipient=" + recipient + "&message=" + message);
        }

        private string AddPhoneCode(string PhoneNo)
        {
            string phone = "";
            if (PhoneNo.StartsWith("234")) phone = PhoneNo;
            else if (PhoneNo.StartsWith("0") && (PhoneNo.Length == 11)) phone = "234" + PhoneNo.Substring(1);
            return phone;
        }

        private bool isAllEntryFilled()
        {
            bool isEntryFilled = false;

            if ((txtStaffID.Text == ""))
            {
                if (optSearchByStaffId.Checked == true)
                {
                    MessageBox.Show("Please type the Staff ID", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Please type the Student Matric No", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                
                txtStaffID.Focus();
                return isEntryFilled;
            }
            else if ((txtPhoneNo.Text == ""))
            {
                MessageBox.Show("Please type the Phone No", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPhoneNo.Focus();
                return isEntryFilled;
            }
            else if (txtPhoneNo.Text.Length != 11)
            {
                MessageBox.Show("Invalid Phone No, Please type correct Phone No", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPhoneNo.Focus();
                return isEntryFilled;
            }
            else if ((txtMessage.Text == ""))
            {
                MessageBox.Show("Please type the Message", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return isEntryFilled;
            }
            else
                return true;
        }

        private void optSearchByStaffId_CheckedChanged(object sender, EventArgs e)
        {
            btnRefresh();
            if (optSearchByStaffId.Checked == true)
            {
                label9.Text = "Type Staff Id:";
                label3.Text = "Staff Id:";
            }
        }

        private void optSearchByStudentId_CheckedChanged(object sender, EventArgs e)
        {
            btnRefresh();
            if (optSearchByStudentId.Checked == true)
            {
                label9.Text = "Type Student Matric No:";
                label3.Text = "Matric No:";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void txtPhoneNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = new TextBox();
            txt = (TextBox)sender;
            nonNumberEntered = false;
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    if ((e.KeyCode != Keys.Back))
                        nonNumberEntered = true;
                }

            }
            if (Control.ModifierKeys == Keys.Shift)
            {
                nonNumberEntered = true;
            }
        }

        private void txtPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = new TextBox();
            txt = (TextBox)sender;
            if (nonNumberEntered == true) e.Handled = true; //capture keytrapping i.e. keypress = 0 (null)
        }





    }
}
