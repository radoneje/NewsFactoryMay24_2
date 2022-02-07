using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; //add this namespace also 
using System.Diagnostics;

namespace NFWingest
{
    public partial class Form1 : Form
    {
        private List<KeyValuePair<string, string>> settings = new List<KeyValuePair<string, string>>();
        private List<profile> profiles = new List<profile>();
        public Form1()
        {
            InitializeComponent();
            
     
        }


       

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
                textBox1.Text = dlg.SelectedPath;
            panel1.Controls.Clear();
 try
            {
                // string[] filePaths;

                var filePaths = Directory.EnumerateFiles(textBox1.Text, "*.*", SearchOption.AllDirectories)
                      .Where(s => s.EndsWith(".mxf") || s.EndsWith(".mts") || s.EndsWith(".MTS") || s.EndsWith(".mp4"))
                      .OrderBy(s => s);
                    ;
           
           // Array.Sort(filePaths, (x, y) => String.Compare(x, y));
            var i = 0;
            foreach(var file in filePaths)
            {
                var fi = new FileInfo(file);
                var ctrl = new videoItem()
                {
                    isCheked = true,
                    size = fi.Length,
                    name = fi.Name,
                    path = fi.FullName,
                    index = i
                };
                ctrl.Top = 0;
                if(panel1.Controls.Count>0)
                    ctrl.Top = panel1.Controls[panel1.Controls.Count - 1].Bottom;
                panel1.Controls.Add(ctrl);
                ctrl.WasClicked += Ctrl_WasClicked;
            i++;
            }
            }
            catch(Exception ex) {
                textBox3.Text += ex.Message;
            }



        }

        int selectedIndex = -1;
        
        private void Ctrl_WasClicked(object sender, EventArgs e)
        {
           

            if (((videoItem)sender).isCheked)
            {
                if (Control.ModifierKeys != Keys.Control && Control.ModifierKeys != Keys.Shift)
                {
                    selectedIndex = ((videoItem)sender).index;
                    return;
                }
                else if (selectedIndex < 0)
                {
                    selectedIndex = ((videoItem)sender).index;
                }
                else
                {
                    var to = Math.Max(selectedIndex, ((videoItem)sender).index);
                    var from = Math.Min(selectedIndex, ((videoItem)sender).index);

                    for (int j = from; j <= to; j++)
                    {
                        ((videoItem)panel1.Controls[j]).isCheked = false;
                    }
                    selectedIndex = -1;
                }

            }
            else
            {
                selectedIndex = -1;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = textBox2.Text;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dlg.SelectedPath;
                Microsoft.Win32.RegistryKey exampleRegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("may24ToencodeFolder");
                exampleRegistryKey.SetValue("path", textBox2.Text);
                exampleRegistryKey.Close();
            }
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            settinsForm dlg = new settinsForm();
            dlg.ShowDialog();
        }
       

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            textBox3.Visible = true;

            
            string text = "";
            foreach(videoItem item in panel1.Controls)
            {
                if(item.isCheked)
                    text += "file '" + item.path + "'\r\n";
            }
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".txt";
            File.AppendAllText(fileName, text);

           
            var args = "";
            var preArgs = "";
            var ext = "";
            var startFile = "";

            using (var db = new mainDataClassesDataContext())
            {
                var rec= db.t_settings.Where(r => r.Id == "ffMpegPath");
                if(rec.Count()==0)
                {
                    textBox3.Text = "проверьте настроки, нет параметра ffMpegPath";
                    return;
                }
                startFile = rec.First().value.Replace("\\","/");
                if (!File.Exists(startFile))
                {
                    textBox3.Text = "неверно указан путь к ffmpeg, проверьте настроки";
                    return;
                }
                if (comboBox1.SelectedIndex <0)
                {
                    textBox3.Text = "не выбран профиль кодирования";
                    return;
                }
                preArgs = ((t_profiles)((NFWingest.ComboboxItem)comboBox1.Items[comboBox1.SelectedIndex]).Value).pre;
                args = ((t_profiles)((NFWingest.ComboboxItem)comboBox1.Items[comboBox1.SelectedIndex]).Value).middle;
                ext = ((t_profiles)((NFWingest.ComboboxItem)comboBox1.Items[comboBox1.SelectedIndex]).Value).ext;
            }

              


            var startArgs= "-f concat -safe 0 "+ preArgs +" -i \"" + fileName + "\" "+ args +" \"" +textBox2.Text+"\\"+DateTime.Now.ToString("yyyyMMdd_HHmm_")+ "."+ext+"\"";
          
                textBox3.Text = startArgs;
                System.Threading.Thread tr = new System.Threading.Thread(() => { startFFmpeg(startFile, startArgs); });
            
            tr.Start();
            
           
        }
        private void startFFmpeg(string startFile, string args)
        {
            Process p = new Process();

            try { 

                p.StartInfo.FileName = startFile;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.Arguments = args;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.EnableRaisingEvents = true;
                p.ErrorDataReceived += P_ErrorDataReceived;
                p.OutputDataReceived += P_ErrorDataReceived;
                p.Exited += P_Exited;
                p.Start();
                p.BeginErrorReadLine();
                p.BeginOutputReadLine();
                p.WaitForExit();
            }
            catch (Exception ex)
            {
             

                if (textBox3.InvokeRequired)
                    textBox3.Invoke((MethodInvoker)delegate { textBox3.Text = ex.Message; });
                else
                    button3.Text = ex.Message;
            }
            finally
            {
                if (p != null)
                    p.Dispose();
            }
        }
        private void P_Exited(object sender, EventArgs e)
        {
          

            if (button3.InvokeRequired)
                button3.Invoke((MethodInvoker)delegate { button3.Visible = true; });
            else
                button3.Visible = true;

            Console.Beep(2000, 400);
            Console.Beep(5000, 200);
            Console.Beep(2000, 400);
        }

        private void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
    

            if (textBox3.InvokeRequired)
                textBox3.Invoke((MethodInvoker)delegate { textBox3.Text= e.Data + "\r\n" + textBox3.Text; });
            else
                textBox3.Text = e.Data + "\r\n";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "encodeProfileDataSet.t_profiles". При необходимости она может быть перемещена или удалена.


            reloadProfileList();
        }
        private void reloadProfileList()
        { 
            using (var db = new mainDataClassesDataContext())
            {
                comboBox1.Items.Clear();
                db.t_profiles.Where(r => r.isDeleted == false).ToList().ForEach(l =>
                {
                    comboBox1.Items.Add(new ComboboxItem() { Text = l.name, Value = l });
                });
                if (comboBox1.Items.Count>0)
                {
                    button3.Visible = true;
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    button3.Visible = false;
                }

            }
        }
        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new encodeProfileForm();
            dlg.ShowDialog();
            reloadProfileList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }
    }
    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
// <add name="NFWingest.Properties.Settings.encodeProfileConnectionString"
//connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|encodeProfile.mdf;Integrated Security=True"
//            providerName="System.Data.SqlClient" />
