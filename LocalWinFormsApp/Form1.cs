using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LocalDbLibrary.Classes;
using LocalWinFormsApp.Extensions;

namespace LocalWinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!DataOperations.DatabaseFolderExists)
            {
                Directory.CreateDirectory(DataOperations.DatabaseFolder);
            }
        }
        private async void CreateButton_Click(object sender, EventArgs e)
        {
            CreateButton.Enabled = false;
            await NullifyGrid();
            await DataOperations.CreateDatabase();
            CreateButton.Enabled = true;
            MessageBox.Show("Done");
        }

        private async Task NullifyGrid()
        {
            dataGridView1.DataSource = null;
            await Task.Delay(500);
        }

        private async void ReadButton_Click(object sender, EventArgs e)
        {

            await NullifyGrid();

            if (await DataOperations.DatabaseExists())
            {
                dataGridView1.DataSource = DataOperations.ReadList();
                dataGridView1.ExpandColumns();
            }
            else
            {
                MessageBox.Show(@"Use create button");
            }
        }

        private async void ExistsButton_Click(object sender, EventArgs e)
        {
            if (await DataOperations.DatabaseExists())
            {
                MessageBox.Show(@$"{DataOperations.APP_DATA_DB_NAME} exists");
            }
            else
            {
                MessageBox.Show(@$"{DataOperations.APP_DATA_DB_NAME} does not exists");
            }
        }
    }
}
