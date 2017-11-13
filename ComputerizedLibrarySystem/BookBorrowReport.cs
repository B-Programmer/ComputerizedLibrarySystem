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
    public partial class BookBorrowReport : Form
    {
        private OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\DbLibrarySystem.accdb;Persist Security Info=True");
        private OleDbCommand cmd = new OleDbCommand();
        private OleDbDataReader dr;
        
        public BookBorrowReport()
        {
            InitializeComponent();
        }

        private void BookBorrowReport_Load(object sender, EventArgs e)
        {
            cmd.Connection = cn;
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            //Add Column Header for listView 1
            listView1.Columns.Add("Accession No", 150);
            listView1.Columns.Add("Book Title", 200);
            listView1.Columns.Add("Borrowed By", 150);
            listView1.Columns.Add("Borrow Date", 100);
            listView1.Columns.Add("Due Date", 100);
            listView1.Columns.Add("Charge", 80);
            genBorrowedBookReport();
        }

        private void genBorrowedBookReport()
        {
            string[] arrRpt = new string[8];
            ListViewItem item;

            try
            {
                if (cn.State == ConnectionState.Open) cn.Close();

                cn.Open();
                cmd.CommandText = "select a.Accession_No, b.Book_Title, a.Library_Id, a.Borrow_Date, a.Due_Date, a.Charge from tblBookBorrow a, tblBookReg b Where a.Borrow_Status = 1 AND a.Accession_No = b.Accession_No";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    arrRpt[0] = ((dr[0]).ToString()); //populate the Accession No
                    arrRpt[1] = ((dr[1]).ToString()); //populate the Book Title
                    arrRpt[2] = ((dr[2]).ToString()); //populate the Library ID
                    arrRpt[4] = ((dr[3]).ToString()); //populate the Borrow Date
                    arrRpt[5] = ((dr[4]).ToString()); //populate the Due Date
                    arrRpt[6] = ((dr[5]).ToString()); //populate the Charge 
                    //add item into list
                    item = new ListViewItem(arrRpt);
                    listView1.Items.Add(item);
                }
                dr.Close();
                cn.Close();

            }
            catch (Exception Er)
            {
                //dr.Close();
                //cn.Close();
                MessageBox.Show(Er.Message, "Automated Library System ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
