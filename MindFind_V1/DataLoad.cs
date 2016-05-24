using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindFind_V1
{
    public partial class DataLoad : Form
    {
        public List<string> realrefs = new List<string>();
        public DataLoad()
        {
            InitializeComponent();
            radioButton1.Select();
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        public static bool IsRecognisedImageFile(string fileName)
        {
            string targetExtension = System.IO.Path.GetExtension(fileName);
            if (String.IsNullOrEmpty(targetExtension))
                return false;
            else
                targetExtension = "*" + targetExtension.ToLowerInvariant();

            List<string> recognisedImageExtensions = new List<string>();

            foreach (System.Drawing.Imaging.ImageCodecInfo imageCodec in System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders())
                recognisedImageExtensions.AddRange(imageCodec.FilenameExtension.ToLowerInvariant().Split(";".ToCharArray()));

            foreach (string extension in recognisedImageExtensions)
            {
                if (extension.Equals(targetExtension))
                {
                    return true;
                }
            }
            return false;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            listView1.Items.Clear();
            string [] d = (string [])e.Data.GetData(DataFormats.FileDrop, false);
            ImageList img = new ImageList();
            var reflist = d.ToList();
            foreach (string i in reflist)
            {
                if (IsRecognisedImageFile(i))
                {
                    img.Images.Add(Image.FromFile(i));
                    realrefs.Add(i);
                }
                else
                    MessageBox.Show("Failas nėra nuotrauka");
            }
            this.listView1.View = View.LargeIcon;
            img.ImageSize = new Size(64, 64);
            this.listView1.LargeImageList = img;
            for (int i = 0; i < img.Images.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = i;
                this.listView1.Items.Add(item);
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            realrefs.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            if (radioButton1.Checked)
            {
                //Agnei užklausą reikia čia parašyti. viskas saugoma realrefs liste.
                using(var db = new MindFind_DBEntities())
                {
                    string name = textBox1.Text;


                    Tags t = new Tags();
                    t.Name = name;
                    db.Tags.Add(t);
                    db.SaveChanges();

                    foreach (string sourcePath in realrefs)
                    {
                        var fileName1 = Path.GetFileName(sourcePath);
                        string fileNameNew = String.Format("{0}_{1}", DateTime.Now.Millisecond, fileName1);
                        Image original = Image.FromFile(sourcePath);
                        string currentDirectory = Directory.GetCurrentDirectory();

                        // Specify the directory you want to manipulate.
                        string path = currentDirectory + "\\Nuotraukos\\";

                             // Determine whether the directory exists.
                             if (Directory.Exists(path))
                             {
                                 Console.WriteLine("That path exists already.");
                             }
                             // Try to create the directory.
                             Directory.CreateDirectory(currentDirectory + "\\Nuotraukos\\");

                        string savePath = currentDirectory+"\\Nuotraukos\\" + fileNameNew;

                        using (MemoryStream memory = new MemoryStream())
                        {
                            using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.ReadWrite))
                            {
                                original.Save(memory, ImageFormat.Jpeg);
                                byte[] bytes = memory.ToArray();
                                fs.Write(bytes, 0, bytes.Length);
                            }
                        }

                        Photos ph = new Photos();
                        ph.Tag_id = t.Tag_id;
                        ph.ImagePath = savePath;

                        db.Photos.Add(ph);
                        db.SaveChanges();

                    }


                }
                this.Close();

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
