using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace CSINVManage
{
    public partial class Form2 : Form
    {
        Form1 frm1;
        public Form2(Form1 callingForm)
        {
            InitializeComponent();
            frm1 = callingForm;
        }
        private void F2_Load(object sender, EventArgs e)
        {
            PictureBox picbox = new PictureBox();
            picbox = frm1.PictureBox;
        }
        private void lockScreenControl1_PassCodeSubmitted(object sender, GestureLockApp.GestureLockControl.PassCodeSubmittedEventArgs e)
        {
            Bitmap bmpSS = new Bitmap(this.Bounds.Width, this.Bounds.Height);
            lockScreenControl1.DrawToBitmap(bmpSS, lockScreenControl1.DisplayRectangle);
            Graphics gfxSS = Graphics.FromImage(bmpSS);
            frm1.PictureBox.Image = bmpSS;
            this.Close();
        }
    }
}
