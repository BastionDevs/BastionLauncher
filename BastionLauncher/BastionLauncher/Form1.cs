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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
            pictureBox1.Size = new Size(100, 100);
            RefreshAccountsList();

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

            comboBox2.SelectedIndex = 1;
            pictureBox1.Load($"https://minotar.net/helm/{comboBox2.SelectedItem}/100.png");
        }

        void RefreshAccountsList()
        {
            // Check if the "elyusers" token is null
            JToken elyusersToken = Util.LauncherUserProfiles["elyusers"];
            JToken mojangusersToken = Util.LauncherUserProfiles["mojangusers"];

            bool elyuserspresent;
            bool mojanguserspresent;

            comboBox2.Items.Clear();

            if (elyusersToken != null && elyusersToken.HasValues) // Check if "elyusers" exists and has players
            {
                comboBox2.Items.Add("= Ely.by Accounts =");

                foreach (JProperty player in elyusersToken) // Loop through each player
                {
                    comboBox2.Items.Add(player.Name); // Add player names to ComboBox
                }

                // Automatically select the first player if any exist
                if (comboBox2.Items.Count > 0)
                {
                    comboBox2.SelectedIndex = 1; // Select the first item
                }
                elyuserspresent = true;
            }
            else
            {
                elyuserspresent = false;
            }

            if (mojangusersToken != null && mojangusersToken.HasValues) // Check if "mojangusers" exists and has players
            {
                comboBox2.Items.Add("= Mojang/MS Accounts =");

                foreach (JProperty player in mojangusersToken) // Loop through each player
                {
                    comboBox2.Items.Add(player.Name); // Add player names to ComboBox
                }

                // Automatically select the first player if any exist
                if (comboBox2.Items.Count > 0)
                {
                    comboBox2.SelectedIndex = 1; // Select the first item
                }
                mojanguserspresent = true;
            }
            else
            {
                mojanguserspresent = false;
            }
            if (!elyuserspresent || !mojanguserspresent)
            {
                // If no users exist, add the placeholder item "<Add an account to continue>"
                comboBox2.Items.Clear();
                comboBox2.Items.Add("<Add an account to continue>");
                comboBox2.SelectedIndex = 0; // Select the placeholder
            }

            comboBox2.Items.Add("Manage accounts");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "Manage accounts")
            {
                new AccountManager().ShowDialog();
                RefreshAccountsList();

            } else if (comboBox2.SelectedItem.ToString() != "= Ely.by Accounts =" && comboBox2.SelectedItem.ToString() != "= Mojang/MS Accounts =")
            {
                pictureBox1.Load($"https://minotar.net/helm/{comboBox2.SelectedItem}/100.png");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() != "= Ely.by Accounts =" || comboBox2.SelectedItem.ToString() != "= Mojang/MS Accounts =" || comboBox2.SelectedItem.ToString() == "Manage accounts" || comboBox2.SelectedItem.ToString() == "<Add profiles to continue>")
            {
                JToken elyuser = Util.LauncherUserProfiles["elyusers"][comboBox2.SelectedItem];
                MessageBox.Show(ElyAccounts.ValidateToken((string)elyuser["accesstoken"]).ToString());
                //Ely.by account
                if (comboBox2.Items.IndexOf(comboBox2.SelectedText) > comboBox2.Items.IndexOf("= Mojang/MS Accounts ="))
                {
                    MessageBox.Show(ElyAccounts.ValidateToken(Util.LauncherUserProfiles["elyusers"][comboBox2.SelectedItem]["accesstoken"].ToString()).ToString());
                }
            }
        }
    }
}
