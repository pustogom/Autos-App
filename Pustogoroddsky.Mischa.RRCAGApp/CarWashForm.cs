/*
 * Name: Mischa Pustogorodsky
 * Program: Business Information Technology
 * Course: ADEV-2008 Programming 
 * Created: 2023-11-26
 * Updated:
 */

using ACE.BIT.ADEV.CarWash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pustogorodsky.Mischa.Business;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Net.Mail;
using ACE.BIT.ADEV.Forms;

namespace Pustogoroddsky.Mischa.RRCAGApp
{
    /// <summary>
    /// Represents a car wash form for an automotive company.
    /// </summary>
    internal class CarWashForm : ACE.BIT.ADEV.Forms.CarWashForm
    {

        private BindingSource packageSource;
        private BindingList<Package> packages;

        private BindingSource fragranceSource;
        private BindingList<CarWashItem> fragrances;

        private BindingSource interiorListSource;
        private BindingList<string> interiorServiceList;

        private BindingSource exteriorListSource;
        private BindingList<string> exteriorServiceList;

        private Pustogorodsky.Mischa.Business.CarWashInvoice summaryList;

        /// <summary>
        /// Creates an instance of CarWashFrom.
        /// </summary>
        public CarWashForm()
        {
            this.packageSource = new BindingSource();
            this.packages = new BindingList<Package>();

            this.fragranceSource = new BindingSource();
            this.fragrances = new BindingList<CarWashItem>();

            this.interiorListSource = new BindingSource();
            this.interiorServiceList = new BindingList<string>();

            this.exteriorListSource = new BindingSource();
            this.exteriorServiceList = new BindingList<string>();

            this.summaryList = null;

            this.packages.Add(new Package("Standard", 7.50M, new List<string> { "Fragrance" },
                                            new List<string> { "Hand Wash" }));
            this.packages.Add(new Package("Deluxe", 15.00M, new List<string> { "Fragrance", "Shampoo Carpets" },
                                            new List<string> { "Hand Wash", "Hand Wax" }));
            this.packages.Add(new Package("Executive", 35.00M, new List<string> {"Fragrance",
                                            "Shampoo Carpets", "Shampoo Upholstery",},
                                             new List<string> { "Hand Wash", "Hand Wax", "Wheel Polish" }));
            this.packages.Add(new Package("Luxury", 55.00M, new List<string> { "Fragrance",
                                            "Shampoo Carpets", "Shampoo Upholstery", "Interior Protection Coat"},
                                             new List<string> { "Hand Wash", "Hand Wax", "Wheel Polish",
                                             "Detail Engine Compartment" }));

            AddFragrance();
            BindControls();
            InitializeComponent();

            this.cboPackage.SelectedValueChanged += PackageItem_SelectedValueChanged;
            this.cboPackage.SelectedValueChanged += CboPackage_SelectedValueChanged;
            this.cboFragrance.SelectedValueChanged += PackageItem_SelectedValueChanged;
            this.mnuFileClose.Click += MnuFileClose_Click;
            this.mnuToolsGenerateInvoice.Click += MnuToolsGenerateInvoice_Click;

        }


        /// <summary>
        /// Handles the click event of the generate invoice item.
        /// </summary>
        private void MnuToolsGenerateInvoice_Click(object sender, EventArgs e)
        {

            CarWashInvoice invoice = new CarWashInvoice(summaryList);

            invoice.FormClosed += Invoice_FormClosed;

            invoice.ShowDialog();

        }

        /// <summary>
        /// Handles the close event of the invoice.
        /// </summary>
        private void Invoice_FormClosed(object sender, FormClosedEventArgs e)
        {
            string initialFragranceName = "Pine";
            SetInitialFragranceSelection(initialFragranceName);
            this.cboPackage.SelectedItem = null;

            this.interiorListSource.List.Clear();
            this.exteriorListSource.List.Clear();

            this.lblSubtotal.Text = string.Empty;
            this.lblGoodsAndServicesTax.Text = string.Empty;
            this.lblProvincialSalesTax.Text = string.Empty;
            this.lblTotal.Text = string.Empty;

            this.mnuToolsGenerateInvoice.Enabled = false;

        }

        /// <summary>
        /// Handles the click event of the close item.
        /// </summary>
        private void MnuFileClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the value changed event of the package and fragrance combo boxes.
        /// </summary>
        private void PackageItem_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateServices();
            UpdateSelectedPackagePrice();
        }

        private void CboPackage_SelectedValueChanged(object sender, EventArgs e)
        {
            this.mnuToolsGenerateInvoice.Enabled = true;
        }

        /// <summary>
        /// Updates the service list boxes.
        /// </summary>
        private void UpdateServices()
        {
            CarWashItem selectedFragrance = (CarWashItem)cboFragrance.SelectedItem;
            Package selectedPackage = (Package)cboPackage.SelectedItem;

            if (selectedPackage != null)
            {
                interiorServiceList.Clear();

                foreach (string service in selectedPackage.InteriorServices)
                {
                    if (service.StartsWith("Fragrance"))
                    {
                        string description = null;
                        if (selectedFragrance != null)
                        {
                            description = selectedFragrance.Description;
                        }

                        string updatedService = "Fragrance - " + description;
                        interiorServiceList.Add(updatedService);
                    }
                    else
                    {
                        interiorServiceList.Add(service);
                    }
                }
            }

            exteriorServiceList.Clear();
            if (selectedPackage != null)
            {
                foreach (string exteriorService in selectedPackage.ExteriorServices)
                {
                    exteriorServiceList.Add(exteriorService);
                }
            }
        }

        /// <summary>
        /// Updates the price of the selected package.
        /// </summary>
        private void UpdateSelectedPackagePrice()
        {
            CarWashItem selectedFragrance = (CarWashItem)cboFragrance.SelectedItem;
            Package selectedPackage = (Package)cboPackage.SelectedItem;

            if (selectedPackage != null)
            {
                Pustogorodsky.Mischa.Business.CarWashInvoice summaryInvoice = CreateSummaryInvoice(selectedPackage, selectedFragrance);
            }
        }

        /// <summary>
        /// Creates an instance of CarWashInvoice. 
        /// </summary>
        private Pustogorodsky.Mischa.Business.CarWashInvoice CreateSummaryInvoice(Package package, CarWashItem fragrance)
        {
            decimal pst = 0;
            decimal gst = 0.05M; ;
            decimal price = package.Price;
            decimal fragranceCost = fragrance.Price;

            if (summaryList == null)
            {
                summaryList = new Pustogorodsky.Mischa.Business.CarWashInvoice(pst, gst, price, fragranceCost);
            }
            else
            {
                summaryList.PackageCost = price;
                summaryList.FragranceCost = fragranceCost;
            }

            lblSubtotal.DataBindings.Clear();
            lblProvincialSalesTax.DataBindings.Clear();
            lblGoodsAndServicesTax.DataBindings.Clear();
            lblTotal.DataBindings.Clear();

            Binding subtotalBind = new Binding("Text", this.summaryList, "SubTotal");
            Binding pstBind = new Binding("Text", this.summaryList, "ProvincialSalesTaxCharged");
            Binding gstBind = new Binding("Text", this.summaryList, "GoodsAndServicesTaxCharged");
            Binding totalBind = new Binding("Text", this.summaryList, "Total");

            subtotalBind.FormattingEnabled = true;
            pstBind.FormattingEnabled = true;
            gstBind.FormattingEnabled = true;
            totalBind.FormattingEnabled = true;

            subtotalBind.FormatString = "C";
            gstBind.FormatString = "N2";
            pstBind.FormatString = "N2";
            totalBind.FormatString = "C";


            this.lblSubtotal.DataBindings.Add(subtotalBind);
            this.lblProvincialSalesTax.DataBindings.Add(pstBind);
            this.lblGoodsAndServicesTax.DataBindings.Add(gstBind);
            this.lblTotal.DataBindings.Add(totalBind);

            return summaryList;
        }

        /// <summary>
        /// Populates BindingList<CarWashItem> fragrances;
        /// </summary>
        private void AddFragrance()
        {
            List<CarWashItem> fragrancesList = new List<CarWashItem>();

            try
            {
                FileStream stream = new FileStream("fragrances.txt", FileMode.Open, FileAccess.Read);

                StreamReader reader;
                reader = new StreamReader(stream);

                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine();

                    char[] delimeters = { ',' };

                    string[] fields = line.Split(delimeters);

                    string description = fields[0];
                    decimal price = decimal.Parse(fields[1]);

                    CarWashItem fragranceItem = new CarWashItem(description, price);

                    fragrancesList.Add(fragranceItem);
                }

                fragrancesList.Add(new CarWashItem("Pine", 0.0M));

                fragrancesList.Sort();

                foreach (CarWashItem fragranceItem in fragrancesList)
                {
                    fragrances.Add(fragranceItem);
                }


            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Fragrance Data File Not Found", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while reading the data file. "
                    + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Creates bind controls for the WinForm controls.
        /// </summary>
        private void BindControls()
        {
            this.packageSource.DataSource = this.packages;
            this.cboPackage.DataSource = this.packageSource;
            this.cboPackage.DisplayMember = "Description";

            this.fragranceSource.DataSource = this.fragrances;
            this.cboFragrance.DataSource = this.fragranceSource;
            this.cboFragrance.DisplayMember = "Description";

            this.interiorListSource.DataSource = this.interiorServiceList;
            this.lstInterior.DataSource = this.interiorListSource;

            this.exteriorListSource.DataSource = this.exteriorServiceList;
            this.lstExterior.DataSource = this.exteriorListSource;
        }

        /// <summary>
        /// Initializes CarWashForm.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CarWashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(395, 429);
            this.Name = "CarWashForm";
            this.Text = "Car Wash";
            this.ResumeLayout(false);
            this.PerformLayout();

            this.cboPackage.SelectedItem = null;

            this.mnuToolsGenerateInvoice.Enabled = false;

            string initialFragranceName = "Pine";
            SetInitialFragranceSelection(initialFragranceName);

        }

        /// <summary>
        /// Sets the initial fragrance.
        /// </summary>
        private void SetInitialFragranceSelection(string fragranceName)
        {
            int index = cboFragrance.FindStringExact(fragranceName);
            if (index != -1)
            {
                cboFragrance.SelectedIndex = index;
            }
        }

    }

}