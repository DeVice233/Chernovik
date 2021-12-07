using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chernovik.db
{
    public static class DBInstance
    {
        static ChernovikDbEntities connection;
        static object objectLock = new object();
        public static ChernovikDbEntities Get()
        {
            lock (objectLock)
            {
                if (connection == null)
                    connection = new ChernovikDbEntities();
                return connection;
            }
        }
    }
}
