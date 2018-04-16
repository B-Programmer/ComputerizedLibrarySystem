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
    public partial class FrmBulkBookReg : Form
    {
        public FrmBulkBookReg()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Always ensure that you select the correct Excel File for the New Book Upload", "Automated Library System");
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx";
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtFileName.Text = openFileDialog.FileName;
                    if (txtFileName.Text != "") MessageBox.Show("Click on upload button to start the upload process and save books to database", "Automated Library System");
                }
                else
                {
                    txtFileName.Text = "";
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFileName.Text !="")
                {
                    monitorUploadProcess(10);
                    ExcelConsole excelConsole = new ExcelConsole(txtFileName.Text);
                    monitorUploadProcess(30);
                    IList<Book> excelBooksUploaded = excelConsole.getBookExcelFile();
                    monitorUploadProcess(50);
                    DbSystem dbSystem = new DbSystem(Application.StartupPath);
                    monitorUploadProcess(75);
                    foreach (Book book in excelBooksUploaded)
                    {
                        if (book.AuthorName != "N/A" && book.BookTitle != "N/A")
                        {
                            book.AccessionNo = dbSystem.genAccessionNo();
                            dbSystem.save(book);
                        }
                    }
                    monitorUploadProcess(100);
                    MessageBox.Show("New Bulk of Book was uploaded and Saved Successfully!!!", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    excelConsole.cleanUpMemory();

                }
                else
                {
                    MessageBox.Show("Cannot continue process, Please ensure you upload the correct excel for bulk of book registration", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFileName.Focus();
                }
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

        private void FrmBulkBookReg_Load(object sender, EventArgs e)
        {
            txtFileName.Enabled = false;
        }

        private void monitorUploadProcess(int level)
        {
            progressBar1.Value =  level;
            if (progressBar1.Value == 10)
                lblUploadStatus.Text = "10% Excel upload begins.....";
            else if (progressBar1.Value == 30)
                lblUploadStatus.Text = "30% Excel upload in progress.....";
            else if (progressBar1.Value == 50)
                lblUploadStatus.Text = "50% Excel upload completed, Proceed to save books into database.....";
            else if (progressBar1.Value == 75)
                lblUploadStatus.Text = "75% Saving books into database.....";
            else if (progressBar1.Value == 100)
                lblUploadStatus.Text = "100% Upload and Save Process Completed Successfully!!!";
        }

    }
}
