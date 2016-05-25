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
    public partial class Meniu : Form
    {
        string[] array;//Originalų referensai
        List<string> refs;
        public Meniu()
        {
            InitializeComponent();
            ComboFill(comboBox1);
            listView2.View = View.Details;
            listView2.Columns.Add("Photo",75);
            listView2.Columns.Add("Label", 250);
            listView2.Columns.Add("Source", 250);
        }

        private void ribbon1_Click(object sender, EventArgs e)
        {



        }

        private void Meniu_Load(object sender, EventArgs e)
        {

        }

        private void ComboFill(ComboBox c)
        {
            using(var db = new MindFind_DBEntities())
            {
                List<Tags> list = db.Tags.ToList();
                foreach(Tags t in list)
                {
                    c.Items.Add(t.Name);
                }
            }
        }

        private void is_failo(object sender, EventArgs e)
        {
            using (var form = new DataLoad())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    refs = form.realrefs;
                    int x = 20;
                    int y = 20;
                    int maxHeight = -1;
                    foreach (string img in refs)
                    {
                        PictureBox pav = new PictureBox();
                        pav.Image = Image.FromFile(img);
                        pav.Location = new Point(x, y);
                        pav.SizeMode = PictureBoxSizeMode.CenterImage;
                        x += pav.Width + 10;
                        maxHeight = Math.Max(pav.Height, maxHeight);
                        if (x > this.ClientSize.Width - 100)
                        {
                            x = 20;
                            y += maxHeight + 10;
                        }
                        //this.panel1.Controls.Add(pav);
                    }
                }
            }
        }

        private void ikelimas(object sender, EventArgs e)
        {

            OpenFileDialog ff = new OpenFileDialog();
            ff.Title = "Pasirinkite kelis variantus";
            ff.Multiselect = true;
            ff.Filter = "JPG|*.jpg|JPEG|*.jpeg|GIF|*.gif|PNG|*.png";
            DialogResult rez = ff.ShowDialog();
            if (rez == System.Windows.Forms.DialogResult.OK)
            {
                string[] failai = ff.FileNames;
                int x = 20;
                int y = 20;
                int maxHeight = -1;
                foreach(string img in failai)
                {
                    PictureBox pav = new PictureBox();
                    pav.Image = Image.FromFile(img);
                    pav.Location = new Point(x, y);
                    pav.SizeMode = PictureBoxSizeMode.CenterImage;
                    x += pav.Width + 10;
                    maxHeight = Math.Max(pav.Height, maxHeight);
                    if (x > this.ClientSize.Width - 100)
                    {
                        x = 20;
                        y += maxHeight + 10;
                    }
                    //this.panel1.Controls.Add(pav);
                }
            }

        }

        private void click_event(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            using (var db = new MindFind_DBEntities())
            {
                Tags t = db.Tags.Where(x => x.Name == comboBox1.SelectedItem.ToString()).FirstOrDefault();
                List<Photos> list = db.Photos.Where(x => x.Tag_id == t.Tag_id).ToList();
                array = list.Select(x => x.ImageSrc).ToArray();
                ImageList imglist = new ImageList();
                imglist.ImageSize = new Size(75, 75);
                foreach(Photos p in list)
                {
                    imglist.Images.Add(Image.FromFile(p.ImagePath));
                }
                listView2.SmallImageList = imglist;
                for(int i = 0; i < list.Count; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = i;
                    item.SubItems.Add(t.Name);
                    item.SubItems.Add(array[i]);
                    listView2.Items.Add(item);
                    
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count == 0)
                MessageBox.Show("Sąrašas tuščias", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string directory;
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    directory = fbd.SelectedPath;
                    string[] realrefs = array;
                    foreach (string sourcePath in realrefs)
                    {
                        var fileName1 = Path.GetFileName(sourcePath);
                        string fileNameNew = String.Format("{0}_{1}", DateTime.Now.Millisecond, fileName1);
                        Image original = Image.FromFile(sourcePath);
                       

                        string savePath = directory + fileNameNew;

                        File.Copy(sourcePath, savePath, true);

                    }
                    MessageBox.Show("Nuotraukos sėkmingai išrinktos");
                }
                else
                {
                    MessageBox.Show("Kategorija nepasirinkta bandykite iš naujo");
                }
                
            }
        }

        private void ClearList(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            ComboFill(comboBox1);
        }
    }
}
