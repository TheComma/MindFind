using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.Drawing;

namespace MindFind_V1
{
    public partial class TakeAPhotocs : Form
    {
        public TakeAPhotocs()
        {
            InitializeComponent();
            ImageViewer viewer = new ImageViewer(); //create an image viewer
            Capture capture = new Capture(); //create a camera captue
            Application.Idle += new EventHandler(delegate (object sender, EventArgs e)
            {  //run this until application closed (close button click on image viewer)
                viewer.Image = capture.QueryFrame(); //draw the image obtained from camera
            });
            viewer.ShowDialog(); //show the image viewer
        }
    }
}
