using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFCrowlString.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {          
            return View(new MainDataClassesDataContext().tSTR_playlists.Where(p=>p.deleted==false));
        }
        
        public PartialViewResult playList(string id)
        {
            var ret = new NFCrowlString.Models.CstrItems();
            using(MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                var rec = dc.tSTR_items.Where(i => i.playlistId == id && i.deleted == false).OrderByDescending(ii => ii.dateAdd);
                ret._activeItems = rec.Where(a => a.isActive == true).ToList();
                ret._passiveItems = rec.Where(a => a.isActive == false).ToList();
            }

            return PartialView(ret);
        }
        [HttpPost]
        public PartialViewResult strAdd(string text, string playlistId, string clientId)
        {
            var ret = new NFCrowlString.Models.CstrItems();
            var rec = new tSTR_item()
            {
                id = Guid.NewGuid().ToString(),
                dateAdd = DateTime.Now,
                dateModify = DateTime.Now,
                deleted = false,
                isActive = false,
                isAlert = false,
                playlistId = playlistId,
                sortOrder = 0,
                text = text
            };
            using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                
                dc.tSTR_items.InsertOnSubmit(rec);
                dc.SubmitChanges();

               // var rec = dc.tSTR_items.Where(i => i.playlistId == playlistId && i.deleted == false).OrderByDescending(ii => ii.dateAdd);
               // ret._activeItems = rec.Where(a => a.isActive == true).OrderBy(a=>a.sortOrder).ToList();
               // ret._passiveItems = rec.Where(a => a.isActive == false).ToList();
            }
            return PartialView("_playListPassiveItem", rec);
        }
        [HttpPost]
        public PartialViewResult PassiveItemDelete(string itemId, string clientId)
        {
            List<tSTR_item> ret;
            using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                var rec=dc.tSTR_items.Single(e => e.id == itemId);
                rec.deleted = true;
                dc.SubmitChanges();
                ret = dc.tSTR_items.Where(e => e.deleted == false && e.isActive == false && e.playlistId == rec.playlistId).ToList();
            }
            return PartialView("playListPassive", ret);
        }

        [HttpPost]
        public PartialViewResult PassiveItemActivate(string itemId, string clientId)
        {
            tSTR_item ret;
            using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                var rec = dc.tSTR_items.Single(e => e.id == itemId);
                rec.isActive = true;
                rec.sortOrder = 0;
                dc.SubmitChanges();
                resortPlaylistItems(rec.playlistId);
                ret = rec;// dc.tSTR_items.Where(e => e.deleted == false && e.isActive == true && e.playlistId == rec.playlistId).ToList();
            }
            return PartialView("_playListActiveItem", ret);
        }
        [HttpPost]
        public PartialViewResult ActiveItemToPassive(string itemId, string clientId)
        {
            tSTR_item ret;
            using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                var rec = dc.tSTR_items.Single(e => e.id == itemId);
                rec.isActive = false;
                dc.SubmitChanges();
                resortPlaylistItems(rec.playlistId);
                ret = rec; dc.tSTR_items.Where(e => e.deleted == false && e.isActive == false && e.playlistId == rec.playlistId).ToList();
            }
            return PartialView("_playListPassiveItem", ret);
        }
        private void resortPlaylistItems(string playlistId){
            using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                 var rec = dc.tSTR_items.Where(e => e.deleted == false && e.isActive == true && e.playlistId ==playlistId).OrderBy(ee=>ee.sortOrder);
                 var i = 0;//rec.Count();
                 foreach (var item in rec)
                 {
                     i++;
                     item.sortOrder = i * 10;
                 }
                 dc.SubmitChanges();
     
            }
        }

        [HttpPost]
        public string itemChangeText(string itemId, string text,  string clientId)
        {
            using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                var rec = dc.tSTR_items.Single(e =>  e.id == itemId);
                rec.text = text;
                rec.dateModify = DateTime.Now;
                dc.SubmitChanges();

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, id = itemId });
        }
        [HttpPost]
        public PartialViewResult itemClone(string itemId, string clientId)
        {
            using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                var rec = dc.tSTR_items.Single(e => e.id == itemId);
                var id = Guid.NewGuid().ToString();
                var newItem = new tSTR_item() { 
                id=id,
                dateAdd=DateTime.Now,
                dateModify=DateTime.Now,
                deleted=false,
                isActive=false,
                isAlert=false,
                playlistId=rec.playlistId,
                sortOrder=0,
                text=rec.text
                };
                dc.tSTR_items.InsertOnSubmit(newItem);
                dc.SubmitChanges();
                return PartialView("_playListPassiveItem", rec);
            }
        }
        [HttpPost]
        public string activeItemUp(string itemId,  string clientId){
             using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                var rec = dc.tSTR_items.Single(e =>  e.id == itemId);
                rec.sortOrder-=15;
                dc.SubmitChanges();
                resortPlaylistItems(rec.playlistId);
            }
             return Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, id = itemId });
        }
        [HttpPost]
        public string activeItemDown(string itemId, string clientId)
        {
            using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                var rec = dc.tSTR_items.Single(e => e.id == itemId);
                rec.sortOrder += 15;
                dc.SubmitChanges();
                resortPlaylistItems(rec.playlistId);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, id = itemId });
        }
        [HttpGet]
        public  PartialViewResult passiveTimer(string id)
        {
            var ret = new List<NFCrowlString.Models.Ctimer>();
            using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                dc.tSTR_timers.Where(t=>t.itemId==id && t.deleted==false).OrderBy(tt=>tt.dateInsert).ToList().ForEach(l=>{

                    ret.Add(new NFCrowlString.Models.Ctimer(l));
                });  
            }
            return PartialView(ret);
        }

        [HttpPost]
        public PartialViewResult timerItemAdd(string itemId, string clientId)
        {
            using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                var rec = new tSTR_timer()
                {
                    id = Guid.NewGuid().ToString(),
                    dateInsert = DateTime.Now,
                    dateStart = DateTime.Now.AddHours(1),
                    deteEnd = DateTime.Now.AddHours(2),
                    deleted = false,
                    isDaily = false,
                    isEnd = true,
                    itemId = itemId
                };
                dc.tSTR_timers.InsertOnSubmit(rec);
                dc.SubmitChanges();

                var ret = new NFCrowlString.Models.Ctimer(rec);
                return PartialView("_passiveTimerItem", ret);
            }
        }
        [HttpPost]
        public string timerItemDelete(string timerId, string clientId){
             using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                dc.tSTR_timers.Single(t => t.id == timerId).deleted = true;
                dc.SubmitChanges();
             }
             return Newtonsoft.Json.JsonConvert.SerializeObject(new { status=1, timerId=timerId});
        }
        [HttpPost]
        public PartialViewResult timerItemChange(NFCrowlString.Models.Ctimer model){
            if (ModelState.IsValid)
            {
                using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
                {
                    var rec=dc.tSTR_timers.Single(t => t.id == model.id);
                    rec.dateStart = model._start;
                    rec.deteEnd = model._end;
                    rec.isDaily = model._daily;
                    rec.isEnd = model._isEnd;
                    dc.SubmitChanges();
                }
            }
            return PartialView("_passiveTimerItem", model);

        }

    }
}
