using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NFCrowlString.Models
{
    public class Ctimer
    {   [Required]
        public string id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime _start {get;set;}
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime _end { get; set; }

        [Display(Name="снимать")]
        public bool _isEnd { get; set; }
        [Display(Name = "ежедневно")]
        [Required]
        public bool _daily { get; set; }

        public Ctimer()
        {

        }
        public Ctimer(NFCrowlString.tSTR_timer l)
        {
            id = l.id;
            _start = l.dateStart;
            _end = l.deteEnd;
            _isEnd = l.isEnd;
            _daily = l.isDaily;

        }
        public static string getPassiveTimerStatus(string itemId){
            using(var dc= new MainDataClassesDataContext())
            {
                return ((bool)dc.fSTR_getPassiveTimers(itemId)) ? "Yes" : "No";
            }
        }
        public static string getActiveTimerStatus(string itemId)
        {
            using (var dc = new MainDataClassesDataContext())
            {
                return ((bool)dc.fSTR_getActiveTimers(itemId)) ? "Yes" : "No";
            }
        }


    }
}