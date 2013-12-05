using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace REST_Service.Utils
{
    public static class DataContextUtils
    {
        public static void SafeSubmitChanges(this DataLayer.ManualBookingSystemDataContext db, ref int errorCounter)
        {
            try
            {
                db.SubmitChanges();
            }

            catch (Exception e)
            {
                Debug.WriteLine("DEBUG: " + e.Message);
                errorCounter++;
            }
        }

        public static void SafeSubmitChanges(this DataLayer.ManualBookingSystemDataContext db)
        {
            try
            {
                db.SubmitChanges();
            }

            catch (Exception e)
            {
                Debug.WriteLine("DEBUG: " + e.Message);
            }
        }
    }
}