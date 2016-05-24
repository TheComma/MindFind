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
    public partial class Meniu : Form
    {
        List<string> refs;
        public Meniu()
        {
            InitializeComponent();
        }

        private void ribbon1_Click(object sender, EventArgs e)
        {



        }

        private void Meniu_Load(object sender, EventArgs e)
        {

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
                        this.panel1.Controls.Add(pav);
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
                    this.panel1.Controls.Add(pav);
                }
            }

        }

        private void click_event(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.ShowDialog();
        }
    }
}
