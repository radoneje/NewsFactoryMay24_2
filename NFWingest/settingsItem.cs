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
    public partial class settingsItem : UserControl
    {

        public event EventHandler changed;
        public string key { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public string value { get { return textBox2.Text; } set { textBox2.Text = value; } }
        public settingsItem()
        {

            InitializeComponent();
        }

        private void settingsItem_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(this.changed!=null)
                this.changed(this, e);
        }
    }
}
