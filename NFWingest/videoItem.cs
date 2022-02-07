using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NFWingest
{
    public partial class videoItem : UserControl
    {
       // public bool isCheked { get { return checkBox1.Checked; } set { checkBox1.Checked = value; } }
        public string name { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public long size { get { return _size; } set { _size = value; textBox2.Text = (_size / 1024).ToString() + " kb"; } }
        public string path { get { return __path; } set { __path = value; } }
        public event EventHandler<EventArgs> WasClicked;
        public int index;
       // private bool _isSelected;
        public bool isCheked
        {
            get { return checkBox1.Checked; }
            set
            {
                panel1.BackColor = value ? Color.Green : Color.Gray;
                checkBox1.Checked = value;//? BorderStyle.Fixed3D : BorderStyle.None;
            }
        }

        private long _size;
        private string __path="";
        public videoItem()
        {
            InitializeComponent();
           
        }

        private void videoItem_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start( __path);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Click(object sender, EventArgs e)
        {
            var wasClicked = WasClicked;
            if (wasClicked != null)
            {
                WasClicked(this, EventArgs.Empty);
            }
            // Select this UC on click.
            isCheked = !isCheked;
        }
    }
}
