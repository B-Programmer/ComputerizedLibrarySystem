using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace ComputerizedLibrarySystem
{
    public partial class BookBorrowForm : Form
    {
        private OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\DbLibrarySystem.accdb;Persist Security Info=True");
        private OleDbCommand cmd = new OleDbCommand();
        private OleDbDataReader dr;

        public BookBorrowForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void BookBorrowForm_Load(object sender, EventArgs e)
        {
            cmd.Connection = cn;
            loadAllBookAccessionNo();
            loadAllUserLibraryID();
        }

        private void loadAllBookAccessionNo()
        {
            try
            {
                cboAccessionNo.Items.Clear();
                cn.Open();
                cmd.CommandText = "select Accession_No from tblBookReg";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                    cboAccessionNo.Items.Add((dr["Accession_No"]).ToString());
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

        private void loadAllUserLibraryID()
        {
            try
            {
                cboLibraryId.Items.Clear();
                cn.Open();
                cmd.CommandText = "select Library_ID from tblLibraryReg";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                    cboLibraryId.Items.Add((dr["Library_ID"]).ToString());
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

        private void cboAccessionNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAccessionNo.Text == "")
            {
                MessageBox.Show("No book Info available. Please select the book accession number.", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboAccessionNo.Focus();
                refreshBookInfo();
            }
            else
            {
                refreshBookInfo();
                getBookInfo(cboAccessionNo.Text);
            }
        }

        private void refreshBookInfo()
        {
            txtAuthorName.Clear(); txtAuthorName.Enabled = false;
            txtBookTitle.Clear(); txtBookTitle.Enabled = false;
            txtCourse.Clear(); txtCourse.Enabled = false;
            txtBookStatus.Clear(); txtBookStatus.Enabled = false;
        }
        private void getBookInfo(string ascNo)
        {
            try
            {
                cn.Open();
                cmd.CommandText = "select Author_Name, Book_Title, Course, Status from tblBookReg where Accession_No = '"+ascNo + "'";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtAuthorName.Text = dr["Author_Name"].ToString();
                    txtBookTitle.Text = dr["Book_Title"].ToString();
                    txtCourse.Text = dr["Course"].ToString();
                    txtBookStatus.Text = dr["Status"].ToString().Replace("_", "");
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

        private void cboLibraryId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLibraryId.Text == "")
            {
                MessageBox.Show("No Library user Info available. Please select the user Library ID.", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboLibraryId.Focus();
                refreshLibraryUserInfo();
            }
            else
            {
                refreshLibraryUserInfo();
                getLibraryUserInfo(cboLibraryId.Text);
            }
        }

        private void refreshLibraryUserInfo()
        {
            txtName.Clear(); txtName.Enabled = false;
            txtUserType.Clear(); txtUserType.Enabled = false;
            txtUserNo.Clear(); txtUserNo.Enabled = false;
            txtEmail.Clear(); txtEmail.Enabled = false;
            txtPhoneNo.Clear(); txtPhoneNo.Enabled = false;
            txtBorrowStatus.Clear(); txtBorrowStatus.Enabled = false;
            txtBorrowDate.Clear(); txtBorrowDate.Enabled = false;
            txtDueDate.Clear(); txtDueDate.Enabled = false;
            txtNoOfDays.Clear(); 
        }

        private void getLibraryUserInfo(string libID)
        {
            try
            {
                cn.Open();
                cmd.CommandText = "select Username, User_Type, Matric_No, Staff_Id, Email, Phone_No, Borrow_Status from tblLibraryReg where Library_ID = '" + libID + "'";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtName.Text = dr["Username"].ToString();
                    txtUserType.Text = dr["User_Type"].ToString();
                    if (txtUserType.Text == "STUDENT")
                    {
                        lblUserNo.Text = "Matric Number:";
                        txtUserNo.Text = dr["Matric_No"].ToString();
                    }
                    else
                    {
                        lblUserNo.Text = "Staff ID:";
                        txtUserNo.Text = dr["Staff_Id"].ToString();
                    }
                    txtEmail.Text = dr["Email"].ToString();
                    txtPhoneNo.Text = dr["Phone_No"].ToString();
                    string borrowStatus = dr["Borrow_Status"].ToString();
                    if (borrowStatus == "NOT_BORROW") txtBorrowStatus.Text = "No Book Borrowed Yet";
                    else if (borrowStatus == "BORROW_ONE") txtBorrowStatus.Text = "One Book Borrowed";
                    else if (borrowStatus == "BORROW_TWO") txtBorrowStatus.Text = "Two Book Borrowed";
                    else if (borrowStatus == "BORROW_THREE") txtBorrowStatus.Text = "Three Book Borrowed";
                    txtBorrowDate.Text = DateTime.Today.ToShortDateString();
                    txtDueDate.Text = DateTime.Today.AddDays(7).ToShortDateString();
                    txtNoOfDays.Text = "7";
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

        private void txtNoOfDays_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if ((txtBorrowDate.Text != "") && (txtNoOfDays.Text != ""))
                {
                    DateTime borrowDate = Convert.ToDateTime(txtBorrowDate.Text);
                    txtDueDate.Text = borrowDate.AddDays(Convert.ToDouble(txtNoOfDays.Text)).ToShortDateString();
                }
            }
            catch (Exception Er)
            {
                MessageBox.Show(Er.Message, "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBorrowBook_Click(object sender, EventArgs e)
        {
            if (cboAccessionNo.Text == "")
            {
                MessageBox.Show("No book Info available. Please select the book accession number.", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboAccessionNo.Focus();
                refreshBookInfo();
            }
            else if (cboLibraryId.Text == "")
            {
                MessageBox.Show("No Library user Info available. Please select the user Library ID.", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboLibraryId.Focus();
                refreshLibraryUserInfo();
            }
            else if (txtBookStatus.Text != "AVAILABLE")
            {
                MessageBox.Show("You can't borrow this book; book is not available. Please select another book.", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboAccessionNo.Focus();
            }
            else if (txtBorrowStatus.Text == "Three Book Borrowed")
            {
                MessageBox.Show("User can't borrow this book until he/she return one of the book he/she borrowed. Please select another user.", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboLibraryId.Focus();
            }
            else
            {
                try
                {
                    
                    {
                        string AccessionNumber = cboAccessionNo.Text;
                        string LibraryId = cboLibraryId.Text;
                        int BorrowStatus = 1;
                        string BorrowDate = txtBorrowDate.Text;
                        string DueDate = txtDueDate.Text;
                        double NoOfDays = Convert.ToDouble(txtNoOfDays.Text);
                        string ReturnDate = "00-00-0000";
                        double charge = 0.00;

                        {
                            cn.Open();
                            cmd.CommandText = "UPDATE tblBookReg SET Status ='BOOK_ALREADY_BORROWED' WHERE Accession_No ='" + AccessionNumber + "'";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "UPDATE tblLibraryReg SET Borrow_Status ='" + setUserBorrowStatus(txtBorrowStatus.Text) + "' WHERE Library_ID ='" + LibraryId + "'";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "insert into tblBookBorrow values('" + AccessionNumber + "','" + LibraryId + "'," + BorrowStatus + ",'" + BorrowDate + "','" + DueDate + "'," + NoOfDays + ",'" + ReturnDate + "'," + charge + ")";
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Book Successfully Borrowed!!!", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.Close();
                            refreshBookInfo();
                            refreshLibraryUserInfo();
                        }

                    }
                }
                catch (Exception er)
                {
                    cn.Close();
                    MessageBox.Show(er.Message, "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private string setUserBorrowStatus(string borrowStatus)
        {
            string newBorrowStatus = "";
            if (borrowStatus == "No Book Borrowed Yet") newBorrowStatus = "BORROW_ONE";
            else if (borrowStatus == "One Book Borrowed") newBorrowStatus = "BORROW_TWO";
            else if (borrowStatus == "Two Book Borrowed") newBorrowStatus = "BORROW_THREE";
            return newBorrowStatus;
        }

       
        
    }
}
