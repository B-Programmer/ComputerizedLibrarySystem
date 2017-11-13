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
    public partial class MDILibrarySystem : Form
    {
        

        public MDILibrarySystem()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            frmStudentReg childForm = new frmStudentReg();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            frmStaffReg childForm = new frmStaffReg();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBookReg childForm = new FrmBookReg();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookBorrowReport childForm = new BookBorrowReport();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void indexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AboutSysLabApp childForm = new AboutSysLabApp();
            //childForm.MdiParent = this;
            //childForm.Show();
        }

        private void studentsReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStudentReports childForm = new FrmStudentReports();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void staffReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStaffReport childForm = new FrmStaffReport();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void departmentReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FrmDepartmentalAllocationRpt childForm = new FrmDepartmentalAllocationRpt();
            //childForm.MdiParent = this;
            //childForm.Show();
        }

        private void schoolReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FrmSchoolAllocationReport childForm = new FrmSchoolAllocationReport();
            //childForm.MdiParent = this;
            //childForm.Show();
        }

        private void individualUserSystemReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FrmIndividualAllocationReport childForm = new FrmIndividualAllocationReport();
            //childForm.MdiParent = this;
            ///childForm.Show();
        }

        private void editStudentRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditStudentReg childForm = new frmEditStudentReg();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editStaffRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditStaffReg childForm = new frmEditStaffReg();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void bookCatalogueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookCatalogue childForm = new BookCatalogue();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void borrowBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookBorrowForm childForm = new BookBorrowForm();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void returnBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBookReturn childForm = new frmBookReturn();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void sendSMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSendSMS childForm = new frmSendSMS();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void sendEMAILToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }       
    }
}
