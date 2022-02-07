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
  
    public partial class profileItem : UserControl
    {
        
        public string name { set { textBox1.Text = value; } get { return textBox1.Text; } }
        public string middle { set { textBox3.Text = value; } get { return textBox3.Text; } }
        public string ext { set { textBox4.Text = value; } get { return textBox4.Text; } }

        public event EventHandler<int> itemDel;
        public event EventHandler<int> onChange;
        public string pre { set { textBox2.Text = value; } get { return textBox2.Text; } }
        public int id;

        public profileItem()
        {
            InitializeComponent();
        }

        private void profileItem_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updateRcord();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            updateRcord();
        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            updateRcord();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (itemDel != null)
                itemDel(this, id);
        }
        private void updateRcord()
        {
            if (onChange != null)
                onChange(this, 0);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (onChange != null)
                onChange(this, 0);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (onChange != null)
                onChange(this, 0);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (onChange != null)
                onChange(this, 0);
        }
    }
}
