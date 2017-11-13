﻿using System;
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
    public partial class frmEditStudentReg : Form
    {
        private OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\DbLibrarySystem.accdb;Persist Security Info=True");
        private OleDbCommand cmd = new OleDbCommand();
        private OleDbDataReader dr;
        private string picture;
        
        public frmEditStudentReg()
        {
            InitializeComponent();
        }

        

        private void frmEditStudentReg_Load(object sender, EventArgs e)
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
            optSearchByLibraryId.Checked = false;
            optSearchByMatricNo.Checked = true;
            txtEditStudentMatricNo.Focus();
        }

        private void btnRefresh()
        {
            txtMatricNo.Clear(); txtMatricNo.ReadOnly = true;
            txtLibraryId.Clear(); txtLibraryId.ReadOnly = true;
            txtName.Clear(); txtName.ReadOnly = true;
            txtEmail.Clear(); txtEmail.ReadOnly = true;
            txtPhoneNo.Clear(); txtPhoneNo.ReadOnly = true;
            cboSex.ResetText(); cboSex.Enabled = false;
            cboLevel.ResetText(); cboLevel.Enabled = false;
            cboSchool.ResetText(); cboSchool.Enabled = false;
            cboDepartment.ResetText(); cboDepartment.Enabled = false;
            cboProgrammeOfStudy.ResetText(); cboProgrammeOfStudy.Enabled = false;
            //txtStudentMatricNo.Focus();
            picture = "";
            pictureBox1.Image = null;
            btnUpdate.Enabled = false;
            btnUpload.Enabled = false;
            //dr.Close();
            //cn.Close();
        }

        private void disableRefresh()
        {
            txtMatricNo.ReadOnly = true;
            txtLibraryId.ReadOnly = true;
            txtName.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtPhoneNo.ReadOnly = false;
            cboSex.Enabled = true;
            cboLevel.Enabled = true;
            cboSchool.Enabled = true;
            cboDepartment.Enabled = true;
            cboProgrammeOfStudy.Enabled = true;
            txtMatricNo.Focus();
            btnUpdate.Enabled = true;
            btnUpload.Enabled = true;
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
        }

        private void txtEditStudentMatricNo_TextChanged(object sender, EventArgs e)
        {
            btnRefresh();
            if ((txtEditStudentMatricNo.Text != ""))
            {
                if ((optSearchByMatricNo.Checked))
                {
                    loadStudentDetails(txtEditStudentMatricNo.Text, "MATRIC");
                }
                else
                {
                    loadStudentDetails(txtEditStudentMatricNo.Text, "LIBRARY");
                }

                if (File.Exists(picture))
                {
                    Bitmap uploadImage = new Bitmap(picture);
                    pictureBox1.Image = uploadImage;
                    pictureBox1.Size = pictureBox1.Image.Size;

                }

            }
        }

        private void loadStudentDetails(string uID, string searchBy)
        {
            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                if (searchBy == "MATRIC")
                    cmd.CommandText = "select * from tblLibraryReg Where User_Type = 'STUDENT' and Matric_No = '" + uID + "'";
                else if (searchBy == "LIBRARY")
                    cmd.CommandText = "select * from tblLibraryReg Where User_Type = 'STUDENT' and Library_ID = '" + uID + "'";
                //if (dr.IsClosed == false) 
                //        dr.Close();

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtLibraryId.Text = dr["Library_ID"].ToString();
                    txtMatricNo.Text = dr["Matric_No"].ToString();
                    txtName.Text = dr["Username"].ToString();
                    cboSex.Text = dr["Sex"].ToString();
                    cboLevel.Text = dr["Level"].ToString();
                    cboSchool.Text = dr["School"].ToString();
                    cboDepartment.Text = dr["Department"].ToString();
                    cboProgrammeOfStudy.Text = dr["Programme_Of_Study"].ToString();
                    txtEmail.Text = dr["Email"].ToString();
                    txtPhoneNo.Text = dr["Phone_No"].ToString();
                    //upload picture to picture box
                    picture = Application.StartupPath + "\\Pictures\\" + txtMatricNo.Text.Replace("/", "_") + ".jpg";


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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (isAllEntryFilled())
                {
                    string StudentLibraryID = txtLibraryId.Text;
                    string StudentMatricNo = txtMatricNo.Text;
                    string StudentName = txtName.Text;
                    string StudentSex = cboSex.Text;
                    string StudentLevel = cboLevel.Text;
                    string StudentSchool = cboSchool.Text;
                    string StudentDepartment = cboDepartment.Text;
                    string ProgrammeOfStudy = cboProgrammeOfStudy.Text;
                    string StudentEmail = txtEmail.Text;
                    string StudentPhoneNo = txtPhoneNo.Text;

                    {
                        cn.Open();
                        cmd.CommandText = "UPDATE tblLibraryReg SET Username ='" + StudentName + "', Sex ='" + StudentSex + "', Level ='" + StudentLevel + "', School ='" + StudentSchool + "', Department ='" + StudentDepartment + "', Programme_Of_Study ='" + ProgrammeOfStudy + "' , Email ='" + StudentEmail + "', Phone_No ='" + StudentPhoneNo + "' WHERE (Matric_No = '" + txtMatricNo.Text + "')";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Student Registration Details Successfully Updated!!!", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.Close();
                        SavePicture();//then save picture
                        btnRefresh(); //refresh input entries 
                        txtEditStudentMatricNo.Text = "";
                    }

                }
            }
            catch (Exception er)
            {
                cn.Close();
                MessageBox.Show(er.Message, "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool isAllEntryFilled()
        {
            bool isEntryFilled = false;

            if ((txtMatricNo.Text == ""))
            {
                MessageBox.Show("Please type the Student Matric No", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMatricNo.Focus();
                return isEntryFilled;
            }
            else if ((txtName.Text == ""))
            {
                MessageBox.Show("Please type the Student Name", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return isEntryFilled;
            }
            else if ((cboSex.Text == ""))
            {
                MessageBox.Show("Please set the Student Sex", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboSex.Focus();
                return isEntryFilled;
            }
            else if ((cboLevel.Text == ""))
            {
                MessageBox.Show("Please set the Student Level", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboLevel.Focus();
                return isEntryFilled;
            }
            else if ((cboSchool.Text == ""))
            {
                MessageBox.Show("Please set the Student School/Faculty", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboSchool.Focus();
                return isEntryFilled;
            }
            else if ((cboDepartment.Text == ""))
            {
                MessageBox.Show("Please type the Student Department ", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboDepartment.Focus();
                return isEntryFilled;
            }
            else if ((cboProgrammeOfStudy.Text == ""))
            {
                MessageBox.Show("Please type the Student Programme of Study ", "Automated Library System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboProgrammeOfStudy.Focus();
                return isEntryFilled;
            }
            else
                return true;
        }

        private void SavePicture()
        {
            //bool isSaved = false;
            if ((picture != "") && (pictureBox1.Image != null))
            {
                if (txtMatricNo.Text != "")
                {
                    string pictureFileName = txtMatricNo.Text.Replace("/", "_");
                    if (File.Exists(Application.StartupPath + "\\Pictures\\" + pictureFileName + ".jpg"))
                    {
                        // MessageBox.Show("Picture Cannot be Saved!!!, Duplicate Found", "Filename Already Exists");
                        // Bitmap Pict = new Bitmap(Application.StartupPath + "\\Pictures\\" + pictureFileName + ".jpg");
                        //File.Delete(Application.StartupPath + "\\Pictures\\" + pictureFileName + ".jpg");
                        //Pict.Dispose();
                        //Bitmap Pict1 = new Bitmap(pictureBox1.Image); pictureBox1.Image.Dispose(); pictureBox1.Image = null;
                        //Pict1.Save(Application.StartupPath + "\\Pictures\\" + pictureFileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        // Pict1.Dispose();
                        MessageBox.Show("Picture Saved!!!");
                    }
                    else
                    {
                        pictureBox1.Image.Save(Application.StartupPath + "\\Pictures\\" + pictureFileName + ".jpg");
                        MessageBox.Show("Picture Saved!!!");
                        picture = Application.StartupPath + "\\Pictures\\" + pictureFileName + ".jpg";
                        //isSaved = true;
                    }
                }
            }
            //return isSaved;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Always ensure that upload the recent passport of the present student");
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
                    uploadImage.Dispose();
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

        private void optSearchByLibraryId_CheckedChanged(object sender, EventArgs e)
        {
            btnRefresh();
            if (optSearchByLibraryId.Checked == true)
            {
                label9.Text = "Type Student Library Id:";
            }
        }

        private void optSearchByMatricNo_CheckedChanged(object sender, EventArgs e)
        {
            btnRefresh();
            if (optSearchByMatricNo.Checked == true)
            {
                label9.Text = "Type Student Matric No:";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }






    }
}
