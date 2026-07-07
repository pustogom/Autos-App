/*
 * Name: Mischa Pustogorodsky
 * Program: Business Information Technology
 * Course: ADEV-2008 Programming 5
 * Created: 2023-11-01
 * Updated:
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACE.BIT.ADEV;
using ACE.BIT.ADEV.Forms;
using Pustogorodsky.Mischa.Business;


namespace Pustogorodsky.Mischa.RRCAGApp
{
    /// <summary>
    /// Represents a windows form application for an automotive group.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.mnuFileExit.Click += MnuFileExit_Click;
            this.mnuHelpAbout.Click += MnuHelpAbout_Click;
            this.mnuFileOpenSalesQuote.Click += MnuFileOpenSalesQuote_Click;
            this.mnuDataVehicles.Click += MnuDataVehicles_Click;
            this.mnuFileCarWash.Click += MnuFileCarWash_Click;
 
        }

        /// <summary>
        /// Handles the Click event of the Car Wash menu item.
        /// </summary>
        private void MnuFileCarWash_Click(object sender, EventArgs e)
        {
            Pustogoroddsky.Mischa.RRCAGApp.CarWashForm carWashForm = 
                new Pustogoroddsky.Mischa.RRCAGApp.CarWashForm();

            carWashForm.MdiParent = this;

            carWashForm.Show();
        }

        /// <summary>
        /// Handles the Click event of the Vehicles menu item.
        /// </summary>
        private void MnuDataVehicles_Click(object sender, EventArgs e)
        {
            VehicleDataForm existingForm = null;

            foreach (Form form in this.MdiChildren)
            {
                if (form is VehicleDataForm)
                {
                    existingForm = (VehicleDataForm)form; 
                }
            }

            if (existingForm == null)
            {
               Pustogoroddsky.Mischa.RRCAGApp.VehicleDataForm vehicleDataForm = 
                    new Pustogoroddsky.Mischa.RRCAGApp.VehicleDataForm();

                vehicleDataForm.MdiParent = this;
                
                vehicleDataForm.Show();
            }
            else
            {
                existingForm.Activate();
            }
        }

        /// <summary>
        /// Handles the Click event of the Sales Quote menu item.
        /// </summary>
        private void MnuFileOpenSalesQuote_Click(object sender, EventArgs e)
        {
            SalesQuoteForm salesForm = new SalesQuoteForm();

            salesForm.MdiParent = this;

            salesForm.Show();
        }

        /// <summary>
        /// Handles the Click event of the about menu item.
        /// </summary>
      private void MnuHelpAbout_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();

            form.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the exit menu item.
        /// </summary>
        private void MnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the load event of the main form.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
