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
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Diagnostics;

namespace MindFind_V1
{
    public partial class Form1 : Form
    {
        //kintamieji, skirti, kad image box kistų kartu kai didinam arba mažinam langą
/*
        bool blnFirstTimeInResizeEvent = true;
        int intOrigFormWidth = 0;
        int intOrigFormHeight = 0;
        int intOrigImageBoxWidth = 0;
        int intOrigImageBoxHeight = 0;
        */

        Image<Bgr, Byte> imgOriginal;
        Image<Gray, Byte> imgGray;
        //Image<Bgr, Byte> imgBlank;
        public static string ft = null;

        private static readonly CascadeClassifier hcFaceDetector = new CascadeClassifier("haarcascade_frontalface_default.xml");
        private static readonly CascadeClassifier hcEyeDetector = new CascadeClassifier("haarcascade_eye.xml");



        //Kintamieji, skirti stopwatch
        Stopwatch swStopWatch = new Stopwatch();
        /*
        TimeSpan tsTimeSpan;
        string strElapsedTime = "";
        */

        private void loadAndProcessImage()
        {

                imgOriginal = new Image<Bgr, byte>(ft);
                imgGray = imgOriginal.Convert<Gray, Byte>();
                Application.DoEvents();
                swStopWatch.Start();





                var acFaces = hcFaceDetector.DetectMultiScale(imgGray, 1.1, 10, Size.Empty);
                var acEyes = hcEyeDetector.DetectMultiScale(imgGray, 1.1, 25, Size.Empty);
                foreach (var acFace in acFaces)
                {
                    imgOriginal.Draw(acFace, new Bgr(Color.Red), 2);
                }
                foreach (var acEye in acEyes)
                {
                    imgOriginal.Draw(acEye, new Bgr(Color.Blue), 2);
                }

                System.Drawing.Image imgOrg = imgOriginal.ToBitmap();
                ibImage.Image = imgOrg;


           
            //stopwatch, jei norėsim kažkur įterpt per kiek laiko aptinka mordą
            /*  
              swStopWatch.Stop();
              tsTimeSpan = swStopWatch.Elapsed;
              strElapsedTime = string.Format("processing time = {0:00}.{1:00} sec", tsTimeSpan.Seconds, tsTimeSpan.Milliseconds / 10);
              swStopWatch.Reset();*/
        }





        public Form1()
        {
            InitializeComponent();
            DataLoad dad = new DataLoad();
            dad.Show();
        }

        public Form1(string img)
        {
            ft = img;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txtImageFile_Click(object sender, EventArgs e)
        {
            DialogResult drImageFile;

            drImageFile = ofdImage.ShowDialog();
            ft = ofdImage.FileName;

            if (ofdImage.FileName != "")
                loadAndProcessImage();
        }

        private void txtImageFile_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
