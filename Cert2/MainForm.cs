using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using System.Data.Entity.Migrations;
using System.Security.Cryptography.X509Certificates;
using DevExpress.DataAccess.Native.DataFederation.QueryBuilder;
using System.Security.Cryptography;

namespace Cert2
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        private CPDOEntities cpdoEntities;
        private OPEntities opEntities;


        public MainForm()
        {
            InitializeComponent();
            
            cpdoEntities = new CPDOEntities();
            opEntities = new OPEntities();
            DevelopmentPermit dev = new DevelopmentPermit();
            OP oP = new OP();
            
            populate(dev);
            
        }
        bool devPermit = false;
        public static string selvalue;
       
        

        private void OpenReport()
        {
            
            Certification report = new Certification();

            
            ReportPrintTool printTool = new ReportPrintTool(report);

            
            printTool.ShowPreviewDialog();
        }

        private void OpenReport_1()
        {
            
            OpReport report = new OpReport();

            
            ReportPrintTool printTool = new ReportPrintTool(report);

           
            printTool.ShowPreviewDialog();
        }

        private void save(DevelopmentPermit dev)
        {
            try
            {
                if (decimal.TryParse(amountPaidTxt.Text, out decimal amountPaid))
                {
                    dev.AmountPaid = amountPaid;
                }
                else
                {
                    MessageBox.Show("Invalid Amount Paid. Please enter a numeric value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                dev.CompanyName = companyTxt.Text;
                dev.OfficeAddress = officeTxt.Text;
                dev.ProjectName = projectTxt.Text;
                dev.ProjectAddress = projectAddressTxt.Text;
                dev.Permit = devPermit;
                dev.DateIssued = dateTimePicker1.Value;
                dev.DateCreated = DateTime.Now;
                checkDuplicate(dev);


                cpdoEntities.DevelopmentPermits.Add(dev);
                cpdoEntities.SaveChanges();
                MessageBox.Show("Saved Successfully!");
                populate(dev);
            }
            catch (Exception)
            {
                MessageBox.Show("Error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void delete(DevelopmentPermit dev)
        {
            try
            {
                var existingOP = cpdoEntities.DevelopmentPermits.SingleOrDefault(db => db.OPNumber == labelControl1.Text);

                if (existingOP != null)
                {
                    cpdoEntities.DevelopmentPermits.Remove(existingOP);
                    cpdoEntities.SaveChanges();
                    MessageBox.Show("Deleted Successfully");
                    populate(dev);
                }
                else { MessageBox.Show("Record not found.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void update(DevelopmentPermit dev)
        {
            var existingOP = cpdoEntities.DevelopmentPermits.SingleOrDefault(db => db.OPNumber == labelControl1.Text);
            
            if (existingOP != null)
            {
                if (decimal.TryParse(amountPaidTxt.Text, out decimal amountPaid))
                {
                    existingOP.AmountPaid = amountPaid;
                }
                else
                {
                    MessageBox.Show("Invalid Amount Paid. Please enter a numeric value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                existingOP.CompanyName = companyTxt.Text;
                existingOP.OfficeAddress = officeTxt.Text;
                existingOP.ProjectName = projectTxt.Text;
                existingOP.ProjectAddress = projectAddressTxt.Text;
                existingOP.Permit = devPermit;
                existingOP.DateIssued = dateTimePicker1.Value;
                existingOP.DateCreated = DateTime.Now;             
                cpdoEntities.DevelopmentPermits.AddOrUpdate<DevelopmentPermit>(existingOP);
                cpdoEntities.SaveChanges();
                MessageBox.Show("Updated Successfully!");
                populate(dev);
            }
            else
            {
                MessageBox.Show("Record not found.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            
        }
        private int GenerateRandomOPNumber()
        {
            // Generate a random OP number
            Random rnd = new Random();
            return rnd.Next(1000000, 9999999);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(companyTxt.Text) && string.IsNullOrEmpty(officeTxt.Text) && string.IsNullOrEmpty(projectTxt.Text) && string.IsNullOrEmpty(projectAddressTxt.Text))
            {

                MessageBox.Show("All fields are required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
            else
            {
                DevelopmentPermit dev = new DevelopmentPermit();
                save(dev);


            }

        }
        private void CompanyTxt_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox != null)
            {
                devPermit = checkBox.Checked;
            }
        }

        private void officeTxt_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void subdivisionTxt_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void subAddressTxt_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void amountPaidTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void populate(DevelopmentPermit dev)
        {
            try
            {
                using (var context = new CPDOEntities())
                {
                    var data = context.DevelopmentPermits.Select(db => new
                    {
                        db.CompanyName,
                        db.OfficeAddress,
                        db.ProjectName,
                        db.ProjectAddress,
                        db.Permit,
                        db.DateIssued,
                        db.OPNumber,
                        db.ORNumber,
                        //ORDate = db.ORDate,
                        db.AmountPaid,
                        db.DateCreated,
                    })
                        .ToList();
                    developmentPermitBindingSource.DataSource = data;
                    // dataGrid.DataSource = data;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
        }

        private void dataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void certificate_Click(object sender, EventArgs e)
        {
            
            DevelopmentPermit dev = new DevelopmentPermit();
            if (selvalue != null)
            {
                isORNull(dev);

                populate(dev);
            }
            else
            {
                MessageBox.Show("No record selected");
            }
        }
        private void isORNull(DevelopmentPermit dev)
        {
            try
            {
                var existingOP = opEntities.OPs.SingleOrDefault(db => db.OpNumber == labelControl1.Text);
                var i = cpdoEntities.DevelopmentPermits.SingleOrDefault(db => db.OPNumber == labelControl1.Text);
                if (existingOP != null)
                {
                    var j = existingOP.OrNumber;
                    
                    if (j != null) 
                    {
                        i.ORNumber = j;
                        existingOP.OrDate = i.DateCreated;
                        cpdoEntities.DevelopmentPermits.AddOrUpdate<DevelopmentPermit>(i);
                        cpdoEntities.SaveChanges();
                        populate(dev);
                        
                        OpenReport();
                    }
                    else
                    {
                        MessageBox.Show("No OR number found");
                    }   
                }
                else
                {
                    MessageBox.Show("No OR number Found");
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
           
        }

        private void OPnumberInput_EditValueChanged(object sender, EventArgs e)
        {

        }
        private void checkDuplicate(DevelopmentPermit dev)
        {
            bool isDuplicate = true;

            while (isDuplicate)
            {
                int randomNum = GenerateRandomOPNumber();
                string OPNumber = "CPDO-" + DateTime.Now.ToString("yy") + "-CERT-" + randomNum;

                var op = from DB in cpdoEntities.DevelopmentPermits
                        where DB.OPNumber == OPNumber
                         select DB;

                // Use Any() to check if any duplicate exists
                isDuplicate = op.Any();

                if (!isDuplicate)
                {
                    labelControl1.Text = OPNumber;
                    dev.OPNumber = OPNumber;
                }
            }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            
        }

        private void gridControl1_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                // Handle left mouse button click
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hitInfo = gridView1.CalcHitInfo(e.Location);
                try
                {
                    if (hitInfo.InRow)
                    {
                        // Clicked on a row
                        int clickedRowHandle = hitInfo.RowHandle;

                        // Access the clicked row
                        object CompanyName = gridView1.GetRowCellValue(clickedRowHandle, "CompanyName");
                        object OfficeAddress = gridView1.GetRowCellValue(clickedRowHandle, "OfficeAddress");
                        object ProjectName = gridView1.GetRowCellValue(clickedRowHandle, "ProjectName");
                        object ProjectAddress = gridView1.GetRowCellValue(clickedRowHandle, "ProjectAddress");
                        object AmountPaid = gridView1.GetRowCellValue(clickedRowHandle, "AmountPaid");

                        object cellValue6 = gridView1.GetRowCellValue(clickedRowHandle, "Permit");

                        object DateIssued = gridView1.GetRowCellValue(clickedRowHandle, "DateIssued");
                        object OPnumber = gridView1.GetRowCellValue(clickedRowHandle, "OPNumber");
                        bool Permit = (bool)cellValue6;
                        dateTimePicker1.Value = (DateTime)DateIssued;

                        companyTxt.Text = CompanyName.ToString();
                        officeTxt.Text = OfficeAddress.ToString();
                        projectTxt.Text = ProjectName.ToString();
                        projectAddressTxt.Text = ProjectAddress.ToString();
                        amountPaidTxt.Text = AmountPaid.ToString();
                        labelControl1.Text = OPnumber.ToString();
                        if (Permit)
                        {
                            devpermitCbox.Checked = true;  
                        }
                        else
                        {
                            devpermitCbox.Checked = false;
                        }
                        selvalue = OPnumber.ToString();
                        

                    }
                }
                catch (Exception) {
                    MessageBox.Show("Error");
                }
                
            }
        }

        private void devpermitCbox_CheckStateChanged(object sender, EventArgs e)
        {
            
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
          
            DevelopmentPermit dev = new DevelopmentPermit();
            delete(dev);    
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(companyTxt.Text) && string.IsNullOrEmpty(officeTxt.Text) && string.IsNullOrEmpty(projectTxt.Text) && string.IsNullOrEmpty(projectAddressTxt.Text))
            {

                MessageBox.Show("All fields are required", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
            else
            {
                DevelopmentPermit dev = new DevelopmentPermit();
                update(dev);
            }
        }

        private void OrderButton_Click(object sender, EventArgs e)
        {
            if(selvalue != null)
            {
                OpenReport_1();
            }
            else
            {
                MessageBox.Show("No record selected");
            }
            
        }
    }
}