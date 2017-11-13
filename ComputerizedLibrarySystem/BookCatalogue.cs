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
    public partial class BookCatalogue : Form
    {
        private OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\DbLibrarySystem.accdb;Persist Security Info=True");
        private OleDbCommand cmd = new OleDbCommand();
        private OleDbDataReader dr;

        public BookCatalogue()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (isSearchParametersFilled())
            {
                listView1.Items.Clear();
                getBookCatalogueDetails(cboSearchParameter.Text, txtParameterValue.Text);
            }
        }

        private bool isSearchParametersFilled()
        {
            if ((cboSearchParameter.Text == ""))
            {
                MessageBox.Show("Please type the Search Parameter", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboSearchParameter.Focus();
                return false;
            }
            else if ((txtParameterValue.Text == ""))
            {
                MessageBox.Show("Please type the Parameter Value", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtParameterValue.Focus();
                return false;
            }
            else
                return true;
        }

        private void BookCatalogue_Load(object sender, EventArgs e)
        {
            cmd.Connection = cn;
            listView1.View = View.Details; 
            listView1.GridLines = true; 
            listView1.FullRowSelect = true; 
            //Add Column Header for listView 1
            listView1.Columns.Add("Accession No", 120);
            listView1.Columns.Add("Book Title", 200);
            listView1.Columns.Add("Author Name", 150);
            listView1.Columns.Add("Publisher", 200);
            listView1.Columns.Add("Course", 150);
            listView1.Columns.Add("Edition", 100);
            listView1.Columns.Add("Isbn", 150);
            listView1.Columns.Add("Date Published", 150);
            listView1.Columns.Add("Status", 150);
        }

        private void getBookCatalogueDetails(string searchParameter, string parameterValue)
        {
            string[] arrRpt = new string[9];
            ListViewItem item;

            try
            {
                if (cn.State == ConnectionState.Open) cn.Close();

                cn.Open();
                if (searchParameter == "Book Title")
                    cmd.CommandText = "select Accession_No, Book_Title, Author_Name, Publisher, Course, Edition, ISBN, Date_Published, Status from tblBookReg WHERE (Book_Title Like '%" + parameterValue + "%')";
                else if (searchParameter == "Author Name")
                    cmd.CommandText = "select Accession_No, Book_Title, Author_Name, Publisher, Course, Edition, ISBN, Date_Published, Status from tblBookReg WHERE (Author_Name Like '%" + parameterValue + "%')";
                else if (searchParameter == "ISBN")
                    cmd.CommandText = "select Accession_No, Book_Title, Author_Name, Publisher, Course, Edition, ISBN, Date_Published, Status from tblBookReg WHERE (ISBN Like '%" + parameterValue + "%')";
                else if (searchParameter == "Accession Number")
                    cmd.CommandText = "select Accession_No, Book_Title, Author_Name, Publisher, Course, Edition, ISBN, Date_Published, Status from tblBookReg WHERE (Accession_No = '" + parameterValue + "')";
                else if (searchParameter == "Course")
                    cmd.CommandText = "select Accession_No, Book_Title, Author_Name, Publisher, Course, Edition, ISBN, Date_Published, Status from tblBookReg WHERE (Course = '" + parameterValue + "')";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    arrRpt[0] = ((dr[0]).ToString()); //populate the Accession No
                    arrRpt[1] = ((dr[1]).ToString()); //populate the Book Title
                    arrRpt[2] = ((dr[2]).ToString()); //populate the Author Name
                    arrRpt[3] = ((dr[3]).ToString()); //populate the Publisher
                    arrRpt[4] = ((dr[4]).ToString()); //populate the Course
                    arrRpt[5] = ((dr[5]).ToString()); //populate the Edition
                    arrRpt[6] = ((dr[6]).ToString()); //populate the ISBN 
                    arrRpt[7] = ((dr[7]).ToString()); //populate the Date_Published 
                    arrRpt[8] = ((dr[8]).ToString()); //populate the Status
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

        private void btnBorrowBook_Click(object sender, EventArgs e)
        {
            string accessionNo = null;
            string bookTitle = null;
            string status = null;

            accessionNo = listView1.SelectedItems[0].SubItems[0].Text;
            bookTitle = listView1.SelectedItems[0].SubItems[1].Text;
            status = listView1.SelectedItems[0].SubItems[8].Text;

            if (status == null)
            {
                MessageBox.Show("No Book Selected!!! Please select the book you want to borrow", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (status != "AVAILABLE")
            {
                MessageBox.Show("The Selected Book, " + bookTitle + "with Accession No: " + accessionNo + "is not Available now. Please select another book to borrow", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                //load frmBorrow with the details of the currently selected book
             
            }
        }



    }
}
