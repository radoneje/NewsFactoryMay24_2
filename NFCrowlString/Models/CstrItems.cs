using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace NFCrowlString.Models
{
    public class CstrItems
    {
         public List<tSTR_item> _activeItems {get; set;}
         public List<tSTR_item> _passiveItems {get; set;}
         public System.Web.Mvc.SelectList playList { get { return new System.Web.Mvc.SelectList(new MainDataClassesDataContext().tSTR_playlists.Where(d => d.deleted == false), "id", "title"); } }
       
    }
}