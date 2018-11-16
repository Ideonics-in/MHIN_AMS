using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Asset
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Tag{ get; set; }
        public String Vendor { get; set; }
        public String Location { get; set; }
        public DateTime? MaintainedOn { get; set; }
        [NotMapped]
        public DateTime? MaintainanceDue { get; set; }
        public int? MaintenanceInterval { get; set; }
        public int? AlertInterval_FirstLevel { get; set; }
        public int? AlertInterval_SecondLevel { get; set; }
        [NotMapped]
        public bool Selected { get; set; }


        public Asset()
        {
            if(MaintainedOn != null)
                MaintainanceDue = MaintainedOn.Value.AddDays((double)MaintenanceInterval);
            Selected = false;
        }
    }
}
