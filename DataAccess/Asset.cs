using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [StringLength(250)]
        [Index(IsUnique = true)]
        public String Tag { get; set; }


        public String Vendor { get; set; }
        public String Location { get; set; }
        public DateTime? AttendedOn { get; set; }
        [NotMapped]
        public DateTime? AttentionDue { get; set; }
        public int? AttentionInterval { get; set; }
        public int? AlertInterval { get; set; }
        public String Department { get; set; }

        [NotMapped]
        public bool Selected { get; set; }


        public Asset()
        {

            if (AttendedOn != null)
                AttentionDue = AttendedOn.Value.AddDays((double)AttentionInterval);
            Selected = false;
        }

        public void Update()
        {
            if (AttendedOn != null)
                AttentionDue = AttendedOn.Value.AddDays((double)AttentionInterval);
            Selected = false;
        }

    }
}
