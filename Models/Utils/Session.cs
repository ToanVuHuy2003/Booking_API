using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlutterCinemaAPI.Models.Utils
{
    public class Session
    {
        private static Dictionary<string, SessionItem> data = new Dictionary<string, SessionItem>();

        public static void Add(string key, object value, TimeSpan? expiration = null)
        {
            var expirationTime = expiration.HasValue ? DateTime.Now.Add(expiration.Value) : (DateTime?)null;
            data[key] = new SessionItem { Value = value, Expiration = expirationTime };
        }

        public static object Get(string key)
        {
            if (data.ContainsKey(key))
            {
                var item = data[key];
                if (item.Expiration == null || item.Expiration > DateTime.Now)
                {
                    return item.Value;
                }
                else
                {
                    data.Remove(key); // Remove expired item
                }
            }
            return null;
        }

        public static void Remove(string key)
        {
            data.Remove(key);
        }

        private class SessionItem
        {
            public object Value { get; set; }
            public DateTime? Expiration { get; set; }
        }
    }
}