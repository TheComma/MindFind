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


using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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
        System.Drawing.Image imgOrg;



        //Image<Bgr, Byte> imgBlank;
        public static string ft = null;

        private static readonly CascadeClassifier hcFaceDetector = new CascadeClassifier("haarcascade_frontalface_default.xml");
        //private static readonly CascadeClassifier hcEyeDetector = new CascadeClassifier("haarcascade_eye.xml");

        // MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        //   string font;
        // Image<Bgr, Byte> currentFrame;
        // Capture grabber;

        //nuotrauka, kuri eina i folderi
        Image<Gray, byte> result = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;

        Stopwatch swStopWatch = new Stopwatch();
        private void loadAndProcessImage()
        {
                imgOriginal = new Image<Bgr, byte>(ft);
                imgGray = imgOriginal.Convert<Gray, Byte>();
                Application.DoEvents();
                swStopWatch.Start();

                var acFaces = hcFaceDetector.DetectMultiScale(imgGray, 1.1, 10, Size.Empty);
               // var acEyes = hcEyeDetector.DetectMultiScale(imgGray, 1.1, 25, Size.Empty);

                foreach (var acFace in acFaces) {
                    t = t + 1;


                //--------------------veidas-------------------------------------------------------
                    result = imgOriginal.Copy(acFace).Convert<Gray, byte>().Resize(10, Inter.Cubic);


                    trainingImages.Add(result);
                    labels.Add(tbName.Text);

                    imgOriginal.Draw(acFace, new Bgr(Color.Red), 2);

                    if (trainingImages.ToArray().Length != 0){
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);
                    }

                using (var db = new MindFind_DBEntities())
                {
                    int lastid;
                    string name = tbName.Text;
                    Tags t = new Tags();
                    if (db.Tags.Where(x => x.Name == name).FirstOrDefault() == null)
                    {
                        t.Name = name;
                        db.Tags.Add(t);
                        db.SaveChanges();
                        lastid = 0;
                    }
                    else
                    {
                        t = db.Tags.Where(x => x.Name == name).FirstOrDefault();
                        lastid = (int)db.Photos.Where(x => x.Tag_id == t.Tag_id).Count();
                    }
                    for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                    {
                        int index = i + lastid;
                        trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/"+ t.Name + index + ".bmp");
                        string savePath = Application.StartupPath + "/TrainedFaces/" + t.Name + index + ".bmp";
                        Photos ph = new Photos();
                        ph.Tags = t;
                        ph.ImageSrc = ft;
                        ph.ImagePath = savePath;
                        db.Photos.Add(ph);
                    }

                    
                    db.SaveChanges();
                }
            

                MessageBox.Show(tbName.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            /*
                foreach (var acEye in acEyes)
                {
                    imgOriginal.Draw(acEye, new Bgr(Color.Blue), 2);
                }*/

            imgOrg = imgOriginal.ToBitmap();//imgOriginal.ToBitmap();
            //imgOrg = ResizeImage(imgOrg, imgOrg.Width / 2, imgOrg.Height / 2);
            ibImage.Image = imgOrg;
            NamePersons.Add("");


        }



        public Form1()
        {
            InitializeComponent();
            //InitializeComponent();
            //Load haarcascades for face detection
            //face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        double i = 1;
        private void ribbonButton7_Click(object sender, EventArgs e)
        {
            i += 0.1;
            Size newSize = new Size((int)(imgOrg.Width * i), (int)(imgOrg.Height * i));
            Bitmap bmp = new Bitmap(imgOrg, newSize);

            //imgOrg = bmp;
            ibImage.Image = bmp;

        }

        private void ribbonButton8_Click(object sender, EventArgs e)
        {

            i -= 0.1;

            Size newSize = new Size((int)(imgOrg.Width * i), (int)(imgOrg.Height * i));
            Bitmap bmp = new Bitmap(imgOrg, newSize);

            //imgOrg = bmp;
            ibImage.Image = bmp;
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {

            if (ofdImage.FileName != "")
                loadAndProcessImage();
        }

    

        private void ribbonButton11_Click(object sender, EventArgs e)
        {
            DialogResult drImageFile;

            drImageFile = ofdImage.ShowDialog();
            ft = ofdImage.FileName;


            imgOriginal = new Image<Bgr, byte>(ft);

            imgOrg = imgOriginal.ToBitmap();//imgOriginal.ToBitmap();
            //imgOrg = ResizeImage(imgOrg, imgOrg.Width / 2, imgOrg.Height / 2);
            ibImage.Image = imgOrg;
        }

        private void ribbonButton10_Click(object sender, EventArgs e)
        {
            //System.Drawing.Image imgOr;
            imgOrg = null;
            ibImage.Image = imgOrg;
        }

        private void ribbonButton5_Click(object sender, EventArgs e)
        {
            //OpenFileDialog("Meniu");
            
            Meniu f = new Meniu();
            
            f.ShowDialog();
            Application.Exit();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {

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


            imgOriginal = new Image<Bgr, byte>(ft);

            imgOrg = imgOriginal.ToBitmap();//imgOriginal.ToBitmap();
            //imgOrg = ResizeImage(imgOrg, imgOrg.Width / 2, imgOrg.Height / 2);
            ibImage.Image = imgOrg;


        }

        private void txtImageFile_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
