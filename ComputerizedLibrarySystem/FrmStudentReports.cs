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
    public partial class FrmStudentReports : Form
    {
        private OleDbConnection cn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\DbLibrarySystem.accdb;Persist Security Info=True");
        private OleDbCommand cmd = new OleDbCommand();
        private OleDbDataReader dr;
        
        public FrmStudentReports()
        {
            InitializeComponent();
        }

        private void FrmStudentReports_Load(object sender, EventArgs e)
        {
            cmd.Connection = cn;
            listView1.View = View.Details; 
            listView1.GridLines = true; 
            listView1.FullRowSelect = true; 
            //Add Column Header for listView 1
            listView1.Columns.Add("Library ID", 150);
            listView1.Columns.Add("Student Name", 200);
            listView1.Columns.Add("Matric No", 150);
            listView1.Columns.Add("Level", 80);
            listView1.Columns.Add("Department", 200);
            listView1.Columns.Add("Gender", 80);
            listView1.Columns.Add("School", 150);
            listView1.Columns.Add("Email", 150);
            listView1.Columns.Add("Phone No", 150);
            genLibraryStudentReport();
            
        }

        private void genLibraryStudentReport()
        {
            string[] arrRpt = new string[9];
            ListViewItem item;

            try
            {
                if (cn.State == ConnectionState.Open) cn.Close();

                cn.Open();
                cmd.CommandText = "select Library_ID, Username, Matric_No, [Level], Department, Sex, School, Email, Phone_No from tblLibraryReg Where User_Type = 'STUDENT'";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    arrRpt[0] = ((dr[0]).ToString()); //populate the Library Id
                    arrRpt[1] = ((dr[1]).ToString()); //populate the Username
                    arrRpt[2] = ((dr[2]).ToString()); //populate the Matric_No
                    arrRpt[3] = ((dr[3]).ToString()); //populate the Level
                    arrRpt[4] = ((dr[4]).ToString()); //populate the Department
                    arrRpt[5] = ((dr[5]).ToString()); //populate the User Gender
                    arrRpt[6] = ((dr[6]).ToString()); //populate the School 
                    arrRpt[7] = ((dr[7]).ToString()); //populate the Email 
                    arrRpt[8] = ((dr[8]).ToString()); //populate the Phone No 
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
