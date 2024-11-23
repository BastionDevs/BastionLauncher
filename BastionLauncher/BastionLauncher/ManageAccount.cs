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
    public partial class ManageAccount : Form
    {
        bool modify = false;
        public ManageAccount()
        {
            InitializeComponent();
        }

        public ManageAccount(string username, bool ely)
        {
            InitializeComponent();
            if (ely)
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
                radioButton2.Enabled = false;
            } else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
                radioButton1.Enabled = false;
            }
            textBox1.Text = username;
            modify = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton2.Checked = false;
                button2.Enabled = true;
                textBox2.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
                button2.Enabled = false;
                textBox2.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Username and Password cannot be blank!", "Bastion Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else
            {
                string userlogin = ElyAccounts.PwdAuth(textBox1.Text, textBox2.Text, textBox1 + "-" + DateTime.Now.ToString("ddMMyyyyHmmss"), true);

                if (userlogin.Contains("ForbiddenOperationException"))
                {
                    MessageBox.Show("Invalid credentials! Please re-enter username and/or password.", "Bastion Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Credentials are valid!", "Bastion Launcher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string clientToken = textBox1 + "-" + DateTime.Now.ToString("ddMMyyyyHmmss");
            string accToken = ElyAccounts.PwdAuth(textBox1.Text, textBox2.Text, clientToken, true);
            string uuid = "";
            string refreshToken = "";
            Util.UpdateAccounts(textBox1.Text, "elyusers", uuid, accToken, refreshToken, clientToken);
        }
    }
}
