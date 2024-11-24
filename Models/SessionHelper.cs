using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace Student_Crud_Operation_Using_Session_State_In_Asp.net_Core_MVC.Models
{
    public static class SessionHelper
    {
        // Save an object to session as JSON
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            var jsonData = JsonConvert.SerializeObject(value);
            session.SetString(key, jsonData);
        }

        // Retrieve an object from session using a key
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var jsonData = session.GetString(key);
            if (jsonData == null)
                return default;

            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
