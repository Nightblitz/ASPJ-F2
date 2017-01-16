using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPJ_F2
{
    public class session
    {
        private static string sessionid;
        public static string SName
        {
            get
            {
                return sessionid;
            }
            set
            {
                sessionid = value;
            }
        }
    }
}