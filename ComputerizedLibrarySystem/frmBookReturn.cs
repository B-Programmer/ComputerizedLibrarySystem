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
    public partial class frmBookReturn : Form
    {
        private OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\DbLibrarySystem.accdb;Persist Security Info=True");
        private OleDbCommand cmd = new OleDbCommand();
        private OleDbDataReader dr;

        public frmBookReturn()
        {
            InitializeComponent();
        }

        private void frmBookReturn_Load(object sender, EventArgs e)
        {
            cmd.Connection = cn;
            loadAllUserLibraryID();
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            //Add Column Header for listView 1
            listView1.Columns.Add("Accession No", 120);
            listView1.Columns.Add("Book Title", 200);
            listView1.Columns.Add("Author Name", 150);
            listView1.Columns.Add("Borrow Date", 150);
            listView1.Columns.Add("Due Date", 150);
            listView1.Columns.Add("Charge", 150);
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
                getAllBooksBorrowedByUser(cboLibraryId.Text, txtBorrowStatus.Text);
            }
        }

        private void refreshLibraryUserInfo()
        {
            txtName.Clear(); txtName.Enabled = false;
            txtUserType.Clear(); txtUserType.Enabled = false;
            txtUserNo.Clear(); txtUserNo.Enabled = false;
            txtBorrowStatus.Clear(); txtBorrowStatus.Enabled = false;
            txtReturnDate.Clear(); txtReturnDate.Enabled = false;
            listView1.Items.Clear();
            txtAmountCharge.Clear();
        }

        private void getLibraryUserInfo(string libID)
        {
            try
            {
                cn.Open();
                cmd.CommandText = "select Username, User_Type, Matric_No, Staff_Id, Borrow_Status from tblLibraryReg where Library_ID = '" + libID + "'";
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
                    string borrowStatus = dr["Borrow_Status"].ToString();
                    if (borrowStatus == "NOT_BORROW") txtBorrowStatus.Text = "No Book Borrowed Yet";
                    else if (borrowStatus == "BORROW_ONE") txtBorrowStatus.Text = "One Book Borrowed";
                    else if (borrowStatus == "BORROW_TWO") txtBorrowStatus.Text = "Two Book Borrowed";
                    else if (borrowStatus == "BORROW_THREE") txtBorrowStatus.Text = "Three Book Borrowed";
                    txtReturnDate.Text = DateTime.Today.ToShortDateString();
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

        private void getAllBooksBorrowedByUser(string libraryId, string borrowStatus)
        {
            if (borrowStatus == "No Book Borrowed Yet")
            {
                MessageBox.Show("The selected user has no book to return. Please select another library Id", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboLibraryId.Focus();
            }
            else
            {
                    listView1.Items.Clear();
                    getBookBorrowList(libraryId);
            }

        }

        private void getBookBorrowList(string lId)
        {
            string[] arrRpt = new string[6];
            ListViewItem item;

            try
            {
                if (cn.State == ConnectionState.Open) cn.Close();

                cn.Open();
                cmd.CommandText = "select a.Accession_No, b.Book_Title, b.Author_Name, a.Borrow_Date, a.Due_Date, a.Charge from tblBookBorrow a, tblBookReg b WHERE (a.Library_Id = '" + lId + "' AND a.Borrow_Status = 1 AND a.Accession_No = b.Accession_No)";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    arrRpt[0] = ((dr[0]).ToString()); //populate the Accession No
                    arrRpt[1] = ((dr[1]).ToString()); //populate the Book Title
                    arrRpt[2] = ((dr[2]).ToString()); //populate the Author Name
                    arrRpt[3] = ((dr[3]).ToString()); //populate the Borrow_Date
                    arrRpt[4] = ((dr[4]).ToString()); //populate the Due_Date
                    arrRpt[5] = ((dr[5]).ToString()); //populate the Charge
                    //add item into list
                    item = new ListViewItem(arrRpt);
                    listView1.Items.Add(item);
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

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            if (cboLibraryId.Text == "")
            {
                MessageBox.Show("No Library user Info available. Please select the user Library ID.", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboLibraryId.Focus();
                refreshLibraryUserInfo();
            }
            else if (txtBorrowStatus.Text == "No Book Borrowed Yet")
            {
                MessageBox.Show("The selected user has no book to return. Please select another library Id", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboLibraryId.Focus();
            }
            else
            {
                try
                {

                    {
                        string AccessionNumber = listView1.SelectedItems[0].SubItems[0].Text; ;
                        string LibraryId = cboLibraryId.Text;
                        int BorrowStatus = 0;
                        string ReturnDate = txtReturnDate.Text;
                        double charge = Convert.ToDouble(txtAmountCharge.Text);
                        if (AccessionNumber != "")
                        {
                            cn.Open();
                            cmd.CommandText = "UPDATE tblBookReg SET Status ='AVAILABLE' WHERE Accession_No ='" + AccessionNumber + "'";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "UPDATE tblLibraryReg SET Borrow_Status ='" + setUserBorrowStatus(txtBorrowStatus.Text) + "' WHERE Library_ID ='" + LibraryId + "'";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "UPDATE tblBookBorrow SET Borrow_Status =" + BorrowStatus + ", Return_Date ='" + ReturnDate + "', Charge =" + charge + " WHERE (Library_ID ='" + LibraryId + "' AND Accession_No ='" + AccessionNumber + "' AND Borrow_Status = 1)";
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Book Successfully Returned!!!", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.Close();
                            refreshLibraryUserInfo();
                        }
                        else
                        {
                            MessageBox.Show("You have selected any book to return; No book to return. Please select a book from list.", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            listView1.Focus();
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
            if (borrowStatus == "One Book Borrowed") newBorrowStatus = "NOT_BORROW";
            else if (borrowStatus == "Two Book Borrowed") newBorrowStatus = "BORROW_ONE";
            else if (borrowStatus == "Three Book Borrowed") newBorrowStatus = "BORROW_TWO";
            return newBorrowStatus;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
