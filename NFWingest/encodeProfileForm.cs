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
    public partial class encodeProfileForm : Form
    {
        public mainDataClassesDataContext db;
        public encodeProfileForm()
        {
            db = new mainDataClassesDataContext();
            InitializeComponent();
      
        }

        private void encodeProfileForm_Load(object sender, EventArgs e)
        {
            reloadForm();
        }
        private void reloadForm()
        {
            panel1.Controls.Clear();
           
                db.t_profiles.Where(r => r.isDeleted == false).ToList().ForEach(profile =>
                {
                    var item = new profileItem();
                    item.pre = profile.pre;
                    item.middle = profile.middle;
                    item.name = profile.name;
                    item.id = profile.id;
                    item.ext = profile.ext;
                    if (panel1.Controls.Count > 0)
                        item.Top = panel1.Controls[panel1.Controls.Count - 1].Bottom;
                    panel1.Controls.Add(item);
                    item.itemDel += Item_itemDel;
                    item.onChange += Item_onChange;
                });
               
          
        }

        private void Item_onChange(object sender, int e)
        {
                List<t_profiles> list = db.t_profiles.ToList();
                foreach(var item in list)
                {
                var id= ((profileItem)sender).id;
                    if (item.id == id)
                    {
                        item.pre = ((profileItem)sender).pre;
                        item.name = ((profileItem)sender).name;
                        item.middle = ((profileItem)sender).middle;
                        item.ext = ((profileItem)sender).ext.Replace(".","");
                }
                }
                db.SubmitChanges();
               
           
          //  reloadForm();

        }

        private void Item_itemDel(object sender, int e)
        {
            using (var db = new mainDataClassesDataContext())
            {
                db.t_profiles.Where(r => r.id == ((profileItem)sender).id).First().isDeleted = true;
                db.SubmitChanges();
               


            }
            reloadForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new mainDataClassesDataContext())
            {
                db.t_profiles.InsertOnSubmit(new t_profiles()
                {
                    name = "",
                    pre = "",
                    middle = "",
                    ext="mp4"
                });
                db.SubmitChanges();
                reloadForm();
            }
          

        }
    }
}
