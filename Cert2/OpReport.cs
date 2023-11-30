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
using DevExpress.XtraEditors;
using DevExpress.Xpo.DB;
using System.Security.Cryptography.X509Certificates;

namespace Cert2
{
    public partial class OpReport : DevExpress.XtraReports.UI.XtraReport
    {
        private CPDOEntities cpdoEntities;
        private List<DevelopmentPermit> dataList;
        public OpReport()
        {
            InitializeComponent();
            MainForm mainForm = new MainForm();
            cpdoEntities = new CPDOEntities();
                printOR();
                printDate();
        }
        string val = MainForm.selvalue;
        private void xrRichText1_BeforePrint(object sender, CancelEventArgs e)
        {
           
           

        }

        private void xrLabel2_BeforePrint(object sender, CancelEventArgs e)
        {
          
        }

        private void xrLabel1_BeforePrint(object sender, CancelEventArgs e)
        {
          

        }

        private void xrLabel3_BeforePrint(object sender, CancelEventArgs e)
        {
           
            
        }

        private void xrLabel4_BeforePrint(object sender, CancelEventArgs e)
        {

            

        }
        private void printDate()
        {
            try
            { 
                    DevelopmentPermit dev = new DevelopmentPermit();


                    var data = from DB in cpdoEntities.DevelopmentPermits
                               where DB.OPNumber == val
                               select DB;
                    dataList = data.ToList();

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
                    DateTime dateTimeCreated = (DateTime)dataList[0].DateCreated;
                if (dateTimeCreated != null)
                {
                    string dateM = monthNames[dateTimeCreated.Month];
                    xrLabel2.Text = dateM + " " + dateTimeCreated.Day + ", " + dateTimeCreated.Year;
                    xrLabel4.Text = dateM + " " + dateTimeCreated.Day + ", " + dateTimeCreated.Year;
                }
                else
                {
                    MessageBox.Show("Or date not found");
                }

            }
            catch (Exception) {
                MessageBox.Show("Error");
            }
        }
        private void printOR()
        {
            try {
                
                    DevelopmentPermit dev = new DevelopmentPermit();

                    var data = from DB in cpdoEntities.DevelopmentPermits
                               where DB.OPNumber == val
                               select DB;
                    dataList = data.ToList();

                    xrLabel1.Text = dataList[0].OPNumber;
                    xrLabel3.Text = dataList[0].OPNumber;
                
                
            }
            catch { MessageBox.Show("Error"); } 
            
        }
    }
}
