using SSHKeysGenerator.SSH;
using SSHKeysGenerator.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSHKeysGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            labelExports.Text = FoldersInit.ExportFolder;
        }

        private void buttonPaste_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                foreach (string ln in lines)
                {
                    AddName(ln.Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if(textBoxAddName.Text != string.Empty)
            {
                AddName(textBoxAddName.Text.Trim());
            }
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AddName(string name)
        {
            if (!listBoxNames.Items.Contains(name))
            {

                listBoxNames.Items.Add(name);
                listBoxNames.Sorted = true;
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxNames.SelectedIndex >= 0)
            {
                var selectedItems = listBoxNames.SelectedItems.Cast<String>().ToList();
                foreach (var item in selectedItems)
                {
                    listBoxNames.Items.Remove(item);
                }
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listBoxNames.Items.Clear();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            foreach (string item in listBoxNames.Items)
            {
                string password = string.Empty;
                bool error = false;
                string publickey = SSHGen.Generate(item, FoldersInit.ExportFolder, out password, out error);

                if(!error)
                {
                    dataGridView1.Rows.Add(item, password, publickey);
                }
            }
        }

        private void buttonOpenExports_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", FoldersInit.ExportFolder);
        }
    }
}
