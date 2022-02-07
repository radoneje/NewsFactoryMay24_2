using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace barryArchiveFixer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void work() {
            using (var dc = new mainClassesDataContext())
            {
                if (button1.InvokeRequired)  button1.Invoke(new MethodInvoker(() => button1.Enabled=false));  else button1.Enabled = false;
             

                var i = 0;
                dc.ArcNews.Where(r => r.extId != null).ToList().ForEach(item =>
                {
                    dc.ArchBlocks.Where(block => block.NewsId == item.id).ToList().ForEach(block =>
                    {
                        i++;

                        block.CreatorId =1;
                        block.BlockText = System.Text.RegularExpressions.Regex.Replace(block.BlockText, @"^<\?xml.+<txt>", "");
                        block.BlockText = System.Text.RegularExpressions.Regex.Replace(block.BlockText, @"</txt></storyBlockData>$", "");
                        var matches = System.Text.RegularExpressions.Regex.Matches(block.BlockText, "<br />");

                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            block.BlockText = block.BlockText.Replace(match.Value, "\r\n");
                        }
                        matches = System.Text.RegularExpressions.Regex.Matches(block.BlockText, "<[^>]+>");

                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            block.BlockText = block.BlockText.Replace(match.Value, "\r\n");
                        }

                        
                        dc.SubmitChanges();
                        if (textBox2.InvokeRequired)
                        {
                            textBox2.Invoke(new MethodInvoker(() => textBox2.Text = i.ToString() + " " + item.Name + " " + block.Name + "\r\n" + textBox2.Text));
                        }
                        else
                        {
                            textBox2.Text =  i.ToString() + " " +  item.Name  +" "+ block.Name+"\r\n"+ textBox2.Text;
                        }
                    });
                });

                if (button1.InvokeRequired) button1.Invoke(new MethodInvoker(() => button1.Enabled = true)); else button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new System.Threading.Thread(() => work()).Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
