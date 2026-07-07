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
using Pustogorodsky.Mischa.Business;
using ACE.BIT.ADEV;

namespace Pustogorodsky.Mischa.RRCAGApp
{
    /// <summary>
    /// Represents a sales quote form for an automotive group.
    /// </summary>
    public partial class SalesQuoteForm : Form
    {
        public SalesQuoteForm()
        {
            InitializeComponent();

            InitialState();
           
            this.btnCalculateQuote.Click += BtnCalculateQuote_Click;
            this.btnReset.Click += BtnReset_Click;
            this.nudAnnualInterestRate.ValueChanged += NudFinance_ValueChanged;
            this.nudNoOfYears.ValueChanged += NudFinance_ValueChanged;
            this.txtVehiclesSalePrice.TextChanged += VehicleValues_TextChanged;
            this.txtTradeInValue.TextChanged += VehicleValues_TextChanged;
            this.chkStereoSytem.CheckedChanged += Options_CheckChanged;
            this.chkLeatherInterior.CheckedChanged+= Options_CheckChanged;
            this.chkComputerNavigation.CheckedChanged += Options_CheckChanged;
            this.radStandard.CheckedChanged += Options_CheckChanged;
            this.radPearlized.CheckedChanged += Options_CheckChanged;
            this.radCustom.CheckedChanged += Options_CheckChanged;

        }

  
        /// <summary>
        /// Handles the check changes event of the options items.
        /// </summary>
        private void Options_CheckChanged(object sender, EventArgs e)
        {
            if(lblAmountDue.Text != string.Empty)
            {
                CalculateQuote(); 
            }
        }

        /// <summary>
        ///  Handles the text changed event of the Vehicle's sale amount and trade in amount items.
        /// </summary>
        private void VehicleValues_TextChanged(object sender, EventArgs e)
        {
            ClearSummaryAndFinance();
        }

        /// <summary>
        /// Handles the value changed event of the finance numeric up down items.
        /// </summary>
        private void NudFinance_ValueChanged(object sender, EventArgs e)
        {
            CalculateQuote();

            this.errorProvider.SetError(this.txtVehiclesSalePrice, string.Empty);
            this.errorProvider.SetError(this.txtTradeInValue, string.Empty);

        }

        /// <summary>
        /// Handles the click event of the reset button item.
        /// </summary>
        private void BtnReset_Click(object sender, EventArgs e)
        {
            string message = "Do you want to reset the form?";
            string caption = "Reset Form";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon, MessageBoxDefaultButton.Button2);
            // Don't need a switch because case 2 doesn't do anything
            if (result == DialogResult.Yes)
            {
                InitialState();
            }
        }

        /// <summary>
        /// Handles the click event of the calculate button item.
        /// </summary>
        private void BtnCalculateQuote_Click(object sender, EventArgs e)
        {
            this.errorProvider.SetError(this.txtVehiclesSalePrice, string.Empty);
            this.errorProvider.SetError(this.txtTradeInValue, string.Empty);

            CalculateQuote();
        }

        /// <summary>
        /// Calculates the sales quote.
        /// </summary>
        private void CalculateQuote()
        {
            decimal firstNumber = 0;
            decimal secondNumber = 0;

            if (this.txtVehiclesSalePrice.Text.Equals(string.Empty))
            {
                this.errorProvider.SetError(this.txtVehiclesSalePrice, "Vehicle price is a required field.");
            }
            else
            {
                try
                {
                    firstNumber = decimal.Parse(this.txtVehiclesSalePrice.Text);
                    if (firstNumber <= 0)
                    {
                        this.errorProvider.SetError(this.txtVehiclesSalePrice, "Vehicle's Sales Price cannot be less than or equal to 0.");
                    }
                    else
                    {
                        this.errorProvider.SetError(this.txtVehiclesSalePrice, string.Empty);
                    }
                }
                catch (FormatException)
                {
                    this.errorProvider.SetError(this.txtVehiclesSalePrice, "Vehicle's Sales Price cannot contain letters or special characters.");
                }
            }

            if (this.txtTradeInValue.Text.Equals(string.Empty))
            {
                this.errorProvider.SetError(this.txtTradeInValue, "Trade-in value is a required field.");
            }
            else
            {
                try
                {
                    secondNumber = decimal.Parse(this.txtTradeInValue.Text);
                    if (secondNumber < 0)
                    {
                        this.errorProvider.SetError(this.txtTradeInValue, "Trade-in Value cannot be less than 0.");
                    }
                    else if (firstNumber > 0 && secondNumber > firstNumber)
                    {
                        this.errorProvider.SetError(this.txtTradeInValue, "Trade-in Value cannot exceed the vehicle sale price.");
                    }
                    else
                    {
                        this.errorProvider.SetError(this.txtTradeInValue, string.Empty);
                    }
                }
                catch (FormatException)
                {
                    this.errorProvider.SetError(this.txtTradeInValue, "Trade-in Value cannot contain letters or special characters.");
                }
            }

            if (this.errorProvider.GetError(this.txtVehiclesSalePrice).Equals(string.Empty) &&
               this.errorProvider.GetError(this.txtTradeInValue).Equals(string.Empty))
            {
                decimal salesTaxRate = 0.12M;
                Accessories accessoriesChosen = Accessories.None;
                ExteriorFinish finishChosen = ExteriorFinish.None;

                if (chkStereoSytem.Checked && chkStereoSytem.Checked && chkComputerNavigation.Checked)
                {
                    accessoriesChosen = Accessories.All;
                }
                else if (chkLeatherInterior.Checked && chkComputerNavigation.Checked)
                {
                    accessoriesChosen = Accessories.LeatherAndNavigation;
                }
                else if (chkLeatherInterior.Checked && chkStereoSytem.Checked)
                {
                    accessoriesChosen = Accessories.StereoAndLeather;
                }
                else if (chkStereoSytem.Checked && chkComputerNavigation.Checked)
                {
                    accessoriesChosen = Accessories.StereoAndNavigation;
                }
                else if (chkStereoSytem.Checked)
                {
                    accessoriesChosen = Accessories.StereoSystem;
                }
                else if (chkLeatherInterior.Checked)
                {
                    accessoriesChosen = Accessories.LeatherInterior;
                }
                else if (chkComputerNavigation.Checked)
                {
                    accessoriesChosen = Accessories.ComputerNavigation;
                }
                else
                {
                    accessoriesChosen = Accessories.None;
                }

                if (radStandard.Checked)
                {
                    finishChosen = ExteriorFinish.Standard;
                }
                else if (radPearlized.Checked)
                {
                    finishChosen = ExteriorFinish.Pearlized;
                }
                else if (radCustom.Checked)
                {
                    finishChosen = ExteriorFinish.Custom;
                }

                SalesQuote salesQuote = new SalesQuote(firstNumber, secondNumber, salesTaxRate, accessoriesChosen, finishChosen);


                this.lblVehicleSalePrice.Text = firstNumber.ToString("C");
                this.lblTradeIn.Text = (-secondNumber).ToString("F2");
                this.lblOptions.Text = salesQuote.TotalOptions.ToString();
                this.lblSubtotal.Text = salesQuote.SubTotal.ToString("C");
                this.lblSalesTax.Text = salesQuote.SalesTax.ToString("F2");
                this.lblTotal.Text = salesQuote.Total.ToString("C");
                this.lblAmountDue.Text = salesQuote.AmountDue.ToString("C");

                decimal rate = nudAnnualInterestRate.Value / 100;
                int numberOfYears = (int)nudNoOfYears.Value * 12;
                decimal totalCost = salesQuote.AmountDue;
                decimal monthlyPayment = Financial.GetPayment(rate, numberOfYears, totalCost);
                this.lblMonthlyPayment.Text = monthlyPayment.ToString("C");
            }
        }

        /// <summary>
        /// Sets the initial state of the form.
        /// </summary>
        private void InitialState()
        {
            SalesQuote salesQuote = null;

            ClearSummaryAndFinance();
            this.txtVehiclesSalePrice.Focus();
            this.txtVehiclesSalePrice.Text = string.Empty;
            this.txtTradeInValue.Text = "0";
            this.chkLeatherInterior.Checked = false;
            this.chkStereoSytem.Checked = false;
            this.chkComputerNavigation.Checked = false;
            this.radStandard.Checked = true;
            this.nudNoOfYears.Value = 1;
            this.nudAnnualInterestRate.Value = 5;            
            this.AcceptButton = btnCalculateQuote;
            this.nudNoOfYears.Minimum = 1;
            this.nudNoOfYears.Maximum = 10;
            this.nudNoOfYears.Increment = 1;
            this.nudAnnualInterestRate.Minimum = 0;
            this.nudAnnualInterestRate.Maximum = 25;
            this.nudAnnualInterestRate.DecimalPlaces = 2;
            this.nudAnnualInterestRate.Increment = 0.25m;
            this.errorProvider.SetIconPadding(txtVehiclesSalePrice, 3);
            this.errorProvider.SetIconPadding(txtTradeInValue, 3);

            this.errorProvider.SetError(this.txtVehiclesSalePrice, string.Empty);
            this.errorProvider.SetError(this.txtTradeInValue, string.Empty);

            foreach (Control control in this.grpSummary.Controls)
            {
                if (control is TextBox)
                {
                    TextBox textbox = (TextBox)control;

                    textbox.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Clears the summary and finance fields.
        /// </summary>
        private void ClearSummaryAndFinance()
        {
            this.lblVehicleSalePrice.Text = string.Empty;
            this.lblOptions.Text = string.Empty;
            this.lblSubtotal.Text = string.Empty;
            this.lblSalesTax.Text = string.Empty;
            this.lblTotal.Text = string.Empty;
            this.lblTradeIn.Text = string.Empty;
            this.lblAmountDue.Text = string.Empty;
            this.lblMonthlyPayment.Text = string.Empty;
        }
    }
}
