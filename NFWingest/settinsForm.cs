using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NFWingest
{
    public partial class settinsForm : Form
    {
        public settinsForm()
        {
            InitializeComponent();
            using (var db = new mainDataClassesDataContext())
            {
                db.t_settings.ToList().ForEach(l => {
                    addKeyValue(l.Id, l.value);
                });
            }
        }
        private void addKeyValue(string key, string value)
        {
            var ctrl = new settingsItem() { key = key, value=value }; //value = "c:\\\\ffmpeg\\bin\\ffmpeg.exe" };
            panel1.Controls.Add(ctrl);
            if (panel1.Controls.Count > 1)
                ctrl.Top = panel1.Controls[panel1.Controls.Count - 2].Bottom;
            ctrl.changed += Ctrl_changed;
        }

        private void Ctrl_changed(object sender, EventArgs e)
        {
            using (var db = new mainDataClassesDataContext())
            {
                foreach (var ctrl in panel1.Controls)
                {
                    var res = (settingsItem)ctrl;
                    var rec=db.t_settings.Where(r => r.Id == res.key);
                    if (rec.Count() > 0)
                        rec.First().value = res.value;
                    else
                        db.t_settings.InsertOnSubmit(new t_settings() { Id = res.key, value = res.value });
                }
                db.SubmitChanges();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addKeyValue("", "");
        }
    }
}
