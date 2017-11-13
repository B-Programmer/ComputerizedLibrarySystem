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
    public partial class FrmBookReg : Form
    {
        private OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\DbLibrarySystem.accdb;Persist Security Info=True");
        private OleDbCommand cmd = new OleDbCommand();
        private OleDbDataReader dr;

        public FrmBookReg()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (isAllEntryFilled())
                {
                    string AccessionNumber = txtAccessionNumber.Text;
                    string AuthorName = txtAuthorName.Text;
                    string BookTitle = txtBookTitle.Text;
                    string Publisher = txtPublisher.Text;
                    string Course = cboCourse.Text;
                    string Edition = txtEdition.Text;
                    string Isbn = txtIsbn.Text;
                    string DatePublished = txtDatePublished.Text;
                    string DateRegistered = new DateTime().ToShortDateString();
                    string status = "AVAILABLE";

                    {
                        cn.Open();
                        cmd.CommandText = "insert into tblBookReg values('" + AccessionNumber + "','" + AuthorName + "','" + BookTitle + "','" + Publisher + "','" + Course + "','" + Edition + "','" + Isbn + "','" + DateRegistered + "','" + DatePublished + "','" + status + "')";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("New Book Registration Details Successfully Saved!!!", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.Close();
                        btnRefresh();
                    }

                }
            }
            catch (Exception er)
            {
                cn.Close();
                MessageBox.Show(er.Message, "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh()
        {
            txtAccessionNumber.Text = genAccessionNo();
            txtAuthorName.Clear();
            txtBookTitle.Clear();
            cboCourse.ResetText();
            txtPublisher.Clear();
            txtEdition.Clear();
            txtIsbn.Clear();
            txtDatePublished.Clear();
        }

        
        private bool isAllEntryFilled()
        {
            bool isEntryFilled = false;

            if ((txtAuthorName.Text == ""))
            {
                MessageBox.Show("Please type the Author Name", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtAuthorName.Focus();
                return isEntryFilled;
            }
            else if ((txtBookTitle.Text == ""))
            {
                MessageBox.Show("Please type the Book Title", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBookTitle.Focus();
                return isEntryFilled;
            }
            else
                return true;
        }

        private string genAccessionNo()
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

        
        private void FrmBookReg_Load(object sender, EventArgs e)
        {
            cmd.Connection = cn;
            txtAccessionNumber.Text = genAccessionNo();
            txtAccessionNumber.Enabled = false;
        }

        


    }
}
