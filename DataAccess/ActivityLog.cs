using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ActivityLog
    {
        public int ID { get; set; }
        public DateTime? Timestamp { get; set; }
        public String Activity { get; set; }

    }
}
