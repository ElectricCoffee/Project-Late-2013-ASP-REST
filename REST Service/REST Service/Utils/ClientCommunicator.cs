using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REST_Service.Utils
{
    public static class ClientCommunicator
    {
        public static string ConvertToString(this byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}