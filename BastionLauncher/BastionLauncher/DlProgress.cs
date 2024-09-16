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
    public partial class DlProgress : Form
    {
        public System.Windows.Forms.ProgressBar progressBar;

        public DlProgress()
        {
            InitializeComponent();

            // Initialize the form
            this.Size = new System.Drawing.Size(400, 150);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize the ProgressBar
            progressBar = new System.Windows.Forms.ProgressBar();
            progressBar.Dock = DockStyle.Fill;
            progressBar.Minimum = 0;
            progressBar.Step = 1;
            progressBar.Value = 0;

            // Add ProgressBar to the form
            this.Controls.Add(progressBar);
        }
    }
}
