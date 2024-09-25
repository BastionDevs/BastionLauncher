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
            // Check if the "elyusers" token is null
            JToken elyusersToken = Util.LauncherUserProfiles["elyusers"];

            if (elyusersToken != null && elyusersToken.HasValues) // Check if "elyusers" exists and has players
            {
                comboBox2.Items.Add("= Ely.by Accounts = ");
                comboBox2.Items.Clear(); // Clear existing items first

                foreach (JProperty player in elyusersToken) // Loop through each player
                {
                    comboBox2.Items.Add(player.Name); // Add player names to ComboBox
                }

                // Automatically select the first player if any exist
                if (comboBox2.Items.Count > 0)
                {
                    comboBox2.SelectedIndex = 0; // Select the first item
                }
            }
            else
            {
                // If no users exist, add the placeholder item "<Add an account to continue>"
                comboBox2.Items.Clear();
                comboBox2.Items.Add("<Add an account to continue>");
                comboBox2.SelectedIndex = 0; // Select the placeholder
            }

            // Check if the "elyusers" token is null
            JToken launchprofilesToken = Util.LauncherGameProfiles["profiles"];

            if (launchprofilesToken != null && launchprofilesToken.HasValues) // Check if "elyusers" exists and has players
            {
                comboBox1.Items.Clear(); // Clear existing items first

                foreach (JProperty player in launchprofilesToken) // Loop through each player
                {
                    comboBox1.Items.Add(player.Name); // Add player names to ComboBox
                }

                // Automatically select the first player if any exist
                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0; // Select the first item
                }
            }
            else
            {
                // If no users exist, add the placeholder item "<Add an account to continue>"
                comboBox1.Items.Clear();
                comboBox1.Items.Add("<Add profiles to continue>");
                comboBox1.SelectedIndex = 0; // Select the placeholder
            }

            comboBox1.Items.Add("Manage profiles");
            comboBox2.Items.Add("Manage accounts");

            pictureBox1.Load($"https://minotar.net/helm/{comboBox2.SelectedItem}/100.png");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Load($"https://minotar.net/helm/{comboBox2.SelectedItem}/100.png");
        }
    }
}
