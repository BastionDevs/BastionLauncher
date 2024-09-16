using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BastionLauncher
{
    public partial class FormSplash : Form
    {
        public FormSplash()
        {
            InitializeComponent();
        }

        private void FormSplash_Load(object sender, EventArgs e)
        {
            //Fonts
            LoadFonts();

            //Load account configuration
            GetAccounts();
        }

        void LoadFonts()
        {
            label3.Text = "Loading fonts...";

            //Create your private font collection object.
            PrivateFontCollection pfc = new PrivateFontCollection();

            //Select your font from the resources.
            int fontLength = Properties.Resources.Montserrat_Light.Length;

            // create a buffer to read in to
            byte[] fontdata = Properties.Resources.Montserrat_Light;

            // create an unsafe memory block for the font data
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);

            // copy the bytes to the unsafe memory block
            Marshal.Copy(fontdata, 0, data, fontLength);

            // pass the font to the font collection
            pfc.AddMemoryFont(data, fontLength);
            progressBar1.Value += 5;

            label3.Text = "Applying fonts...";
            label1.Font = new Font(pfc.Families[0], label1.Font.Size);
            progressBar1.Value += 5;
            label2.Font = new Font(pfc.Families[0], label2.Font.Size);
            progressBar1.Value += 5;
            label3.Font = new Font(pfc.Families[0], label3.Font.Size);
            progressBar1.Value += 5;
            label4.Font = new Font(pfc.Families[0], label4.Font.Size);
            progressBar1.Value += 5;
        }

        void GetAccounts()
        {

        }
    }
}
