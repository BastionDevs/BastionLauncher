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
            JToken elyusersToken = Util.LauncherUserProfiles["elyusers"];

            if (elyusersToken != null && elyusersToken.HasValues) // Check if "elyusers" exists and has players
            {
                listBox1.Items.Clear(); // Clear existing items first
                listBox1.Items.Add("= Ely.by Accounts =");

                foreach (JProperty player in elyusersToken) // Loop through each player
                {
                    listBox1.Items.Add(player.Name); // Add player names to ComboBox
                }

                // Automatically select the first player if any exist
                if (listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = 0; // Select the first item
                }
            }
            else
            {
                // If no users exist, add the placeholder item "<Add an account to continue>"
                listBox1.Items.Clear();
                listBox1.Items.Add("<Add an account to continue>");
                listBox1.SelectedIndex = 0; // Select the placeholder
            }
        }
    }
}
