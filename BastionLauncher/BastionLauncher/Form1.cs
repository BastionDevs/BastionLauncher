using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BastionLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            // Populate ComboBox with player names
            foreach (var player in Util.LauncherUserProfiles["elyusers"].OfType<JProperty>())
            {
                comboBox2.Items.Add(player.Name);
            }

            // Remove "<Add an account to continue>" if users exist
            if (comboBox2.Items.Count == 0)
            {
                // If no users exist, add the placeholder item back
                comboBox2.Items.Add("<Add an account to continue>");
            }

            comboBox2.Items.Add("Manage accounts");

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }
    }
}
