using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace ComputerizedLibrarySystem
{
    public partial class frmStaffReg : Form
    {
        private OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\DbLibrarySystem.accdb;Persist Security Info=True");
        private OleDbCommand cmd = new OleDbCommand();
        private OleDbDataReader dr;
        private string picture;
        
        public frmStaffReg()
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
                    string StaffLibraryID = txtLibraryId.Text;
                    string StaffID = txtStaffID.Text;
                    string StaffName = txtName.Text;
                    string StaffSex = cboSex.Text;
                    string StaffSchool = cboSchool.Text;
                    string StaffDepartment = cboDepartment.Text;
                    string StaffEmail = txtEmail.Text;
                    string StaffPhoneNo = txtPhoneNo.Text;
                    string DateRegistered = new DateTime().ToShortDateString();
                    string BorrowStatus = "NOT_BORROW";

                    {
                        cn.Open();
                        cmd.CommandText = "insert into tblLibraryReg values('" + StaffLibraryID + "','" + StaffName + "','STAFF','" + StaffID + "','','" + StaffSex + "','','" + StaffSchool + "','" + StaffDepartment + "','','" + StaffEmail + "','" + StaffPhoneNo + "','" + DateRegistered + "','" + BorrowStatus + "')";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("New Staff Registration Details Successfully Saved!!!", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.Close();
                        SavePicture();//then save picture
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
            txtLibraryId.Text = genLibraryId();
            txtStaffID.Clear();
            txtName.Clear();
            cboSex.ResetText();
            cboSex.ResetText();
            cboSchool.ResetText();
            cboDepartment.ResetText();
            txtEmail.Clear();
            txtPhoneNo.Clear();
        }

        private void SavePicture()
        {
            //bool isSaved = false;
            if ((picture != "") && (pictureBox1.Image != null))
            {
                if (txtStaffID.Text != "")
                {
                    string pictureFileName = txtStaffID.Text.Replace("/", "_");
                    if (File.Exists(Application.StartupPath + "\\Pictures\\" + pictureFileName + ".jpg"))
                    {
                        MessageBox.Show("Picture Cannot be Saved!!!, Duplicate Found", "Filename Already Exists");
                    }
                    else
                    {
                        pictureBox1.Image.Save(Application.StartupPath + "\\Pictures\\" + pictureFileName + ".jpg");
                        MessageBox.Show("Picture Saved!!!");
                        picture = Application.StartupPath + "\\Pictures\\" + pictureFileName + ".jpg";
                        // isSaved = true;
                    }
                }
            }
            //return isSaved;
        }

        private bool isAllEntryFilled()
        {
            bool isEntryFilled = false;

            if ((txtStaffID.Text == ""))
            {
                MessageBox.Show("Please type the Staff ID", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtStaffID.Focus();
                return isEntryFilled;
            }
            else if ((txtName.Text == ""))
            {
                MessageBox.Show("Please type the Staff Name", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return isEntryFilled;
            }
            else if ((cboSex.Text == ""))
            {
                MessageBox.Show("Please set the Staff Sex", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboSex.Focus();
                return isEntryFilled;
            }
            else if ((cboSchool.Text == ""))
            {
                MessageBox.Show("Please set the Staff School/Faculty", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboSchool.Focus();
                return isEntryFilled;
            }
            else if ((cboDepartment.Text == ""))
            {
                MessageBox.Show("Please type the Staff Department ", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboDepartment.Focus();
                return isEntryFilled;
            }
            else
                return true;
        }

        private string genLibraryId()
        {
            string LibraryId = "LIB2016";
            try
            {
                int rowCount = 0;
                cn.Open();
                cmd.CommandText = "select count(*) as RowCount from tblLibraryReg";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                    rowCount = (int)(dr["RowCount"]);
                rowCount++;
                if (rowCount < 10)
                {
                    LibraryId = LibraryId + "0000" + rowCount;
                }
                else if (rowCount < 100)
                {
                    LibraryId = LibraryId + "000" + rowCount;
                }
                else if (rowCount < 1000)
                {
                    LibraryId = LibraryId + "00" + rowCount;
                }
                else if (rowCount < 10000)
                {
                    LibraryId = LibraryId + "0" + rowCount;
                }
                else
                {
                    LibraryId = LibraryId + rowCount;
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
            return LibraryId;
        }

        
        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Always ensure that you upload the recent passport of the staff");
                OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.gif;*.bmp)|*.jpg;*.jpeg;*.gif;*.bmp|All Files (*.*)|*.*";
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    Bitmap uploadImage = new Bitmap(openFileDialog.FileName);
                    pictureBox1.Image = resizeImage(uploadImage, 162, 162);
                    pictureBox1.Size = pictureBox1.Image.Size;
                    picture = openFileDialog.FileName;
                    //MessageBox.Show(picture);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private static Image resizeImage(Image image, int imgWidth, int imgHeight)
        {
            Bitmap newImage = new Bitmap(imgWidth, imgHeight);
            Graphics g = Graphics.FromImage((Image)newImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.DrawImage(image, 0, 0, imgWidth, imgHeight);
            return newImage;
        }

        private void frmStaffReg_Load(object sender, EventArgs e)
        {
            cmd.Connection = cn;
            txtLibraryId.Text = genLibraryId();
            txtLibraryId.Enabled = false;
            cboSchool.Items.Clear();
            cboSchool.Items.Add("Business and Management Studies");
            cboSchool.Items.Add("Engineering Technology");
            cboSchool.Items.Add("Environmental Science");
            cboSchool.Items.Add("General Studies");
            cboSchool.Items.Add("Communication and Information Technology");
            cboSchool.Items.Add("Applied Science");
            cboSchool.Items.Add("ADMIN for Non Academic staff");
        }

        private void cboSchool_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboDepartment.Items.Clear();
            if (cboSchool.Text == "Business and Management Studies")
            {
                cboDepartment.Items.Add("Business Administration");
                cboDepartment.Items.Add("Accountancy");
                cboDepartment.Items.Add("Marketing");
                cboDepartment.Items.Add("Insurance");
                cboDepartment.Items.Add("Banking and Finance");
            }
            else if (cboSchool.Text == "Engineering Technology")
            {
                cboDepartment.Items.Add("Civil Engineering Technology");
                cboDepartment.Items.Add("Electrical Electronic Engineering Technology");
                cboDepartment.Items.Add("Mechanical Engineering Technology");
                cboDepartment.Items.Add("Computer Engineering Technology");
            }
            else if (cboSchool.Text == "Environmental Science")
            {
                cboDepartment.Items.Add("Building Technology");
                cboDepartment.Items.Add("Architectural Technology");
                cboDepartment.Items.Add("Urban and Regional Planning");
                cboDepartment.Items.Add("Estate Management");
                cboDepartment.Items.Add("Surveying and Geo-Informatics");
                cboDepartment.Items.Add("Quantity Surveying");
            }
            else if (cboSchool.Text == "General Studies")
            {
                cboDepartment.Items.Add("Humanities and Social Studies");
                cboDepartment.Items.Add("Languages");
            }
            else if (cboSchool.Text == "Communication and Information Technology")
            {
                cboDepartment.Items.Add("Library and Information Science");
                cboDepartment.Items.Add("Mass Communication");
                cboDepartment.Items.Add("Office Technology Management");
            }
            else if (cboSchool.Text == "Applied Science")
            {
                cboDepartment.Items.Add("Computer Science");
                cboDepartment.Items.Add("Mathematics and Statistics");
                cboDepartment.Items.Add("Food Technology");
                cboDepartment.Items.Add("Science Laboratory Technology");
                cboDepartment.Items.Add("Hotel Management and Hospitality");
            }
            else if (cboSchool.Text == "ADMIN for Non Academic staff")
            {
                cboDepartment.Items.Add("Works");
                cboDepartment.Items.Add("Student Affairs");
                cboDepartment.Items.Add("ICT");
                cboDepartment.Items.Add("Senior Establishment");
                cboDepartment.Items.Add("Junior Establishment");
                cboDepartment.Items.Add("Alumni");
                cboDepartment.Items.Add("Store");
                cboDepartment.Items.Add("Bursary");
                cboDepartment.Items.Add("Academic Planning Unit");
                cboDepartment.Items.Add("Council");
                cboDepartment.Items.Add("Bursary");
            }
        }

        
    }
}
