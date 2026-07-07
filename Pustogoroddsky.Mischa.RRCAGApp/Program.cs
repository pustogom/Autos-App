/*
 * Name: Mischa Pustogorodsky
 * Program: Business Information Technology
 * Course: ADEV-2008 Programming 5
 * Created: 2023-11-01
 * Updated:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACE.BIT.ADEV;
using ACE.BIT.ADEV.Forms;
using Pustogorodsky.Mischa.Business; 

namespace Pustogorodsky.Mischa.RRCAGApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
