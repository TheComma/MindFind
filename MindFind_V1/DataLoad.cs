using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindFind_V1
{
    public partial class DataLoad : Form
    {
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
                    img.Images.Add(Image.FromFile(i));
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
