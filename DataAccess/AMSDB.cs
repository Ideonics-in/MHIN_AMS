using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AMSDB :DbContext
    {
        public AMSDB() : base("name=AMSDBConnectionString")
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<ActivityLog> Log { get; set; }
    }
}
