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

        // MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        string font;
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        
        //nuotrauka, kuri eina i folderi
        Image<Gray, byte> result, TrainedFace = null;

        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;








        /*
        public void loadFacesIfPossible()
        {
            
            InitializeComponent();
            //Load haarcascades for face detection
            //face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


            */




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



                t = t + 1;


                //--------------------veidas-------------------------------------------------------
                result = imgOriginal.Copy(acFace).Convert<Gray, byte>().Resize(10, Inter.Cubic);//currentFrame.Copy(acFace).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);//, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);


                trainingImages.Add(result);
                labels.Add(tbName.Text);




                /*

                t = t + 1;
                result = imgOriginal.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);


                */

                imgOriginal.Draw(acFace, new Bgr(Color.Red), 2);

                if (trainingImages.ToArray().Length != 0)
                {
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);
                    /*
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                    trainingImages.ToArray(),
                    labels.ToArray(),
                    3000,
                    ref termCrit);*/
                    

                 //   name = recognizer.Recognize(result);

                    tbFounded.Text = name;
                    //currentFrame.Draw(name, ref font, new Point(acFace.X - 2/*.rect.X - 2*/, acFace.Y - 2/*.rect.Y - 2*/), new Bgr(Color.LightGreen));

                }


                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                //Write the labels of triained faces in a file text for further load
                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                }

                MessageBox.Show(tbFounded.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);




            }
                foreach (var acEye in acEyes)
                {
                    imgOriginal.Draw(acEye, new Bgr(Color.Blue), 2);
                }

            System.Drawing.Image imgOrg = imgOriginal.ToBitmap();//imgOriginal.ToBitmap();
            ibImage.Image = imgOrg;










            //label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");


            //Get the current frame form capture device
           // currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            //gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            /*MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));
          */
            //Action for each element detected
            /*foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       3000,
                       ref termCrit);

                    name = recognizer.Recognize(result);

                    //Draw the label for each face detected and recognized
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }

                NamePersons[t - 1] = name;
                NamePersons.Add("");


                //Set the number of faces detected on the scene
                label3.Text = facesDetected[0].Length.ToString();

                /*
                //Set the region of interest on the faces

                gray.ROI = f.rect;
                MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                   eye,
                   1.1,
                   10,
                   Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                   new Size(20, 20));
                gray.ROI = Rectangle.Empty;

                foreach (MCvAvgComp ey in eyesDetected[0])
                {
                    Rectangle eyeRect = ey.rect;
                    eyeRect.Offset(f.rect.X, f.rect.Y);
                    currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                }
                 */

        /*    }
            t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                names = names + NamePersons[nnn] + ", ";
            }
            //Show the faces procesed and recognized
            imageBoxFrameGrabber.Image = currentFrame;
            label4.Text = names;
            names = "";
            //Clear the list(vector) of names
            NamePersons.Clear();








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
            //InitializeComponent();
            //Load haarcascades for face detection
            //face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }



        }

        public Form1(string img)
        {
            ft = img;
        }

        private void label1_Click(object sender, EventArgs e)
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

            if (ofdImage.FileName != "")
                loadAndProcessImage();
        }

        private void txtImageFile_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
