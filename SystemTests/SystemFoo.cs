using Newtonsoft.Json;

namespace SystemTests
{
    public static class SystemFoo
    {
        public static string MungeJson(string value)
        {           
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(value));           
        }
    }
}