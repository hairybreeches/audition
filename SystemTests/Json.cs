using Newtonsoft.Json;

namespace SystemTests
{
    public static class Json
    {
        public static string MungeJson(string value)
        {           
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(value));           
        }
    }
}