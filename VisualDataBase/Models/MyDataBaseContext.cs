using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDataBase.Models
{
    public static class MyDataBaseContext
    {
        public static KHLContext db;

        static MyDataBaseContext()
        {
            db = new KHLContext();
        }

        public static void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
