using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraRichEdit.API.Native;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace Cert2
{
    public partial class Certification : DevExpress.XtraReports.UI.XtraReport
    {
        private CPDOEntities cpdoEntities;
        private List<DevelopmentPermit> dataList;
        string selValue = MainForm.selvalue;
        public Certification()
        {
            InitializeComponent();
            cpdoEntities = new CPDOEntities();
            DevelopmentPermit dev = new DevelopmentPermit();
            
            
            var data = from DB in cpdoEntities.DevelopmentPermits
                       where DB.OPNumber == selValue
                       select DB;
            // Materialize the query result
            dataList = data.ToList();
            

        }
        

        private void xrRichText1_BeforePrint(object sender, CancelEventArgs e)
        {
            try
            {
                ReplacePlaceholder("[Company_Name]", dataList[0].CompanyName);
                ReplacePlaceholder("[Company_Address]", dataList[0].OfficeAddress);
                ReplacePlaceholder("[Subdivision_Name]", dataList[0].ProjectName);
                ReplacePlaceholder("[Subdivision_Address]", dataList[0].ProjectAddress);
                if (dataList[0].Permit == true)
                {
                    ReplacePlaceholder("[Permit]", "Development Permit");
                }
                else
                {
                    ReplacePlaceholder("[Permit]", "No Development Permit");
                }
                DateTime dateTimeIssued = (DateTime)dataList[0].DateIssued;
                ReplacePlaceholder("[Day]", dateTimeIssued.Day.ToString());
                var monthNames = new Dictionary<int, string>()
                {
                    { 1, "January" },
                    { 2, "February" },
                    { 3, "March" },
                    { 4, "April" },
                    { 5, "May" },
                    { 6, "June" },
                    { 7, "July" },
                    { 8, "August" },
                    { 9, "September" },
                    { 10, "October" },
                    { 11, "November" },
                    { 12, "December" }
                };
                switch (dateTimeIssued.Day)
                {
                    case 11:
                    case 12:
                    case 13:
                        ReplacePlaceholder("[Ordinal]", "th");
                        break;
                    case 1:
                        ReplacePlaceholder("[Ordinal]", "st");
                        break;
                    case 2:
                        ReplacePlaceholder("[Ordinal]", "nd");
                        break;
                    case 3:
                        ReplacePlaceholder("[Ordinal]", "rd");
                        break;

                    default:
                        ReplacePlaceholder("[Ordinal]", "th");
                        break;


                }
                ReplacePlaceholder("[Month]", monthNames[dateTimeIssued.Month]);
                ReplacePlaceholder("[Year]", dateTimeIssued.Year.ToString());
                ReplacePlaceholder("[OPNumber]", dataList[0].OPNumber);
                ReplacePlaceholder("[ORnumber]", dataList[0].ORNumber);
                DateTime dateTimeCreated = (DateTime)dataList[0].DateIssued;
                double newAmount = (double)dataList[0].AmountPaid;
               
                ReplacePlaceholder("[AmountPaid]", "₱." + newAmount.ToString());

                ReplacePlaceholder("[DateCreated]", dateTimeCreated.ToString("yyyy-MMMM-dd"));



            }
            catch (Exception ex) { MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void ReplacePlaceholder(string placeholder, string replacement)
        {
            // Check if the placeholder exists in the RTF text
            if (xrRichText1.Rtf.Contains(placeholder))
            {
                // Replace the placeholder with the specified value
                xrRichText1.Rtf = xrRichText1.Rtf.Replace(placeholder, replacement);
            }
        }

    }
}
