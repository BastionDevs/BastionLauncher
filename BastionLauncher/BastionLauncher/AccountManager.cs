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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BastionLauncher
{
    public partial class AccountManager : Form
    {
        public AccountManager()
        {
            InitializeComponent();
        }

        private void AccountManager_Load(object sender, EventArgs e)
        {
            RefreshUserList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RefreshUserList();
        }

        void RefreshUserList()
        {
            // Check if the "elyusers" token is null
            JToken elyusersToken = Util.LauncherUserProfiles["elyusers"];
            JToken mojangusersToken = Util.LauncherUserProfiles["mojangusers"];

            bool elyuserspresent;
            bool mojanguserspresent;

            listBox1.Items.Clear();

            if (elyusersToken != null && elyusersToken.HasValues) // Check if "elyusers" exists and has players
            {
                listBox1.Items.Add("= Ely.by Accounts =");

                foreach (JProperty player in elyusersToken) // Loop through each player
                {
                    listBox1.Items.Add(player.Name); // Add player names to ComboBox
                }

                // Automatically select the first player if any exist
                if (listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = 1; // Select the first item
                }
                elyuserspresent = true;
            }
            else
            {
                elyuserspresent = false;
            }

            if (mojangusersToken != null && mojangusersToken.HasValues) // Check if "mojangusers" exists and has players
            {
                listBox1.Items.Add("= Mojang/MS Accounts =");

                foreach (JProperty player in mojangusersToken) // Loop through each player
                {
                    listBox1.Items.Add(player.Name); // Add player names to ComboBox
                }

                // Automatically select the first player if any exist
                if (listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = 1; // Select the first item
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
                listBox1.Items.Clear();
                listBox1.Items.Add("<Add an account to continue>");
                listBox1.SelectedIndex = 0; // Select the placeholder
            }
        }
    }
}
