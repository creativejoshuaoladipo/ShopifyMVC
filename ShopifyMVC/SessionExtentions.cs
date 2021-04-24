using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC
{
    public static class SessionExtentions
    {
        /*
         * Creating a Set Method that can help to store Object/List of Object to a Session
         */
        public static void Set<T>(this ISession session, string key, T value)
        {

            session.SetString(key, JsonSerializer.Serialize(value));
        }


        public static T Get<T>(this ISession session, string key)
        {
            //We will first Check the Session Object to be sure that there is an object value in it

            var value = session.GetString(key);
            //return value == null ? default : JsonSerializer.Deserialize<T>(value);

            if (value == null)
            {
                return default;
            }
            else

                return JsonSerializer.Deserialize<T>(value);
        }
    }
}
