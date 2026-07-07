/*
 * Name: Mischa Pustogorodsky
 * Program: Business Information Technology
 * Course: ADEV-2008 Programming 
 * Created: 2023-11-26
 * Updated:
 */

using ACE.BIT.ADEV.CarWash;
using Pustogorodsky.Mischa.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pustogoroddsky.Mischa.RRCAGApp
{
    internal class CarWashInvoice : ACE.BIT.ADEV.Forms.CarWashInvoiceForm
    {
        public CarWashInvoice(Pustogorodsky.Mischa.Business.CarWashInvoice invoice)
        {
            this.Text = "Car Wash Invoice";

            Binding packageBind = new Binding("Text", invoice, "PackageCost");
            Binding fragranceBind = new Binding("Text", invoice, "FragranceCost");
            Binding subtotalBind = new Binding("Text", invoice, "SubTotal");
            Binding pstBind = new Binding("Text", invoice, "ProvincialSalesTaxCharged");
            Binding gstBind = new Binding("Text", invoice, "GoodsAndServicesTaxCharged");
            Binding totalBind = new Binding("Text", invoice, "Total");

            lblPackagePrice.DataBindings.Add(packageBind);
            lblFragrancePrice.DataBindings.Add(fragranceBind);
            lblSubtotal.DataBindings.Add(subtotalBind);
            lblProvincialSalesTax.DataBindings.Add(pstBind);
            lblGoodsAndServicesTax.DataBindings.Add(gstBind);
            lblTotal.DataBindings.Add(totalBind);

            packageBind.FormattingEnabled = true;
            fragranceBind.FormattingEnabled = true;
            subtotalBind.FormattingEnabled = true;
            pstBind.FormattingEnabled = true;
            gstBind.FormattingEnabled = true;
            totalBind.FormattingEnabled = true;

            packageBind.FormatString = "C";
            fragranceBind.FormatString = "N2";
            subtotalBind.FormatString = "C";
            gstBind.FormatString = "N2";
            pstBind.FormatString = "N2";
            totalBind.FormatString = "C";
        }

    }
}