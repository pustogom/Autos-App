/*
 * Name: Mischa Pustogorodsky
 * Program: Business Information Technology
 * Course: ADEV-2008 Programming 7
 * Created: 2023-12-01
 * Updated:
 */

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACE.BIT.ADEV.Forms;

namespace Pustogoroddsky.Mischa.RRCAGApp
{
    /// <summary>
    /// Represents a vehicle data form.
    /// </summary>
    internal class VehicleDataForm : ACE.BIT.ADEV.Forms.VehicleDataForm
    {
        private OleDbConnection connection;
        private OleDbDataAdapter adapter;
        private DataSet dataset;
        private BindingSource bindingSource;
        private bool hasChanges = false;

        /// <summary>
        /// Creates an instance of VehicleDataForm.
        /// </summary>
        public VehicleDataForm()
        {
            this.bindingSource = new BindingSource();

            this.Load += new EventHandler(VehicleDataForm_Load);

            this.dgvVehicles.CellValueChanged += DgvVehicles_CellValueChanged;
            this.mnuFileClose.Click += MnuFileClose_Click;
            this.dgvVehicles.SelectionChanged += DgvVehicles_SelectionChanged;
            this.mnuFileSave.Click += MnuFileSave_Click;
            this.mnuEditRemove.Click += MnuEditRemove_Click;
        }

        /// <summary>
        /// Handles the remove event of the form.
        /// </summary>
        private void MnuEditRemove_Click(object sender, EventArgs e)
        {
            string stockNumberName = "StockNumber";
            int stockNumberIndex = dgvVehicles.Columns[stockNumberName].Index;

            object stockNumberValue = dgvVehicles.SelectedRows[0].Cells[stockNumberIndex].Value;

            string message = $"Remove stock item {stockNumberValue}?";
            string caption = "Remove Stock Item";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons, icon, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                DataGridViewRow selectedRow = dgvVehicles.SelectedRows[0];
                dgvVehicles.Rows.Remove(selectedRow);
            }

            EnableSave();
        }

        /// <summary>
        /// Handles the save event.
        /// </summary>
        private void MnuFileSave_Click(object sender, EventArgs e)
        {
                InitialState();
        }

        /// <summary>
        /// Handles the selection changed event.
        /// </summary>
        private void DgvVehicles_SelectionChanged(object sender, EventArgs e)
        {
            this.mnuEditRemove.Enabled = dgvVehicles.SelectedRows.Count > 0;
        }

        /// <summary>
        /// Handles the close event.
        /// </summary>
        private void MnuFileClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the cell value changed event.
        /// </summary>
        private void DgvVehicles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            EnableSave();
        }

        /// <summary>
        /// Enables the save function of the form.
        /// </summary>
        private void EnableSave()
        {
            hasChanges = true;

            this.mnuFileSave.Enabled = true;

            this.Text = "*Vehicle Data";
        }

        /// <summary>
        /// Handles the form's load event.
        /// </summary>
        void VehicleDataForm_Load(object sender, EventArgs e)
        {
            try
            {
                InitialState();

                this.mnuFileSave.Click += new EventHandler(FileSave_Click);
                this.FormClosing += ContactsForm_FormClosing;

                connection = new OleDbConnection();
                connection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=AMDatabase.mdb";
                connection.Open();

                OleDbCommand command = new OleDbCommand();
                command.CommandText = "SELECT * From VehicleStock";
                command.Connection = connection;

                adapter = new OleDbDataAdapter();
                adapter.SelectCommand = command;

                RetrieveDataFromTheDatabase();

                dataset = new DataSet();
                adapter.Fill(dataset, "VehicleStock");

                this.adapter.RowUpdated += new OleDbRowUpdatedEventHandler(dataAdapter_RowUpdated);

                BindControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load vehicle data.", "Data Load Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                this.BeginInvoke(new MethodInvoker(this.Close));
            }
        }

        /// <summary>
        /// Handles the row updated event.
        /// </summary>
        void dataAdapter_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.StatementType == StatementType.Insert)
            {
                OleDbCommand cmd = new OleDbCommand("SELECT @@IDENTITY", connection);

                e.Row["ID"] = (int)cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Gives the user to save before the form closes.
        /// </summary>
        private void ContactsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hasChanges)
            {
                DialogResult result = MessageBox.Show("Do you wish to save the changes?", "Save",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button3);

                if (result == DialogResult.Yes)
                {
                    SaveChanges();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            connection.Close();
            connection.Dispose();
        }

        /// <summary>
        /// Enables the command builder commands.
        /// </summary>
        private void RetrieveDataFromTheDatabase()
        {
            OleDbCommandBuilder commandBuilder;
            commandBuilder = new OleDbCommandBuilder();

            commandBuilder.DataAdapter = adapter;

            commandBuilder.ConflictOption = ConflictOption.OverwriteChanges;

            adapter.InsertCommand = commandBuilder.GetInsertCommand();
            adapter.DeleteCommand = commandBuilder.GetDeleteCommand();
            adapter.UpdateCommand = commandBuilder.GetUpdateCommand();  
        }

        /// <summary>
        /// Handles the click event of the save function.
        /// </summary>
        void FileSave_Click(object sender, EventArgs e)
        {
            SaveChanges();

        }

        /// <summary>
        /// Creates bind controls for the form.
        /// </summary>
        private void BindControls()
        {
            this.bindingSource.DataSource = this.dataset.Tables["VehicleStock"];
            this.dgvVehicles.DataSource = this.bindingSource;
            this.dgvVehicles.Columns["ID"].Visible = false;
            //this.dgvVehicles.Columns["SoldBy"].Visible = false;  
        }

        /// <summary>
        /// Allows form data to be saved.
        /// </summary>
        private void SaveChanges()
        {
            try
            {
                this.dgvVehicles.EndEdit();
                this.bindingSource.EndEdit();
                adapter.Update(dataset, "VehicleStock");

                hasChanges = false;
                this.mnuFileSave.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occurred while saving the changes to the vehicle data.",
                    "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        /// <summary>
        /// Sets the initial state of the form.
        /// </summary>
        private void InitialState()
        {
            this.Text = "Vehicle Data";
            this.WindowState = FormWindowState.Maximized;
        }
    }
}