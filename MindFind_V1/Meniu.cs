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
            DataLoad f = new DataLoad();
            f.Show();
        }
    }
}
