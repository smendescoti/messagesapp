using Newtonsoft.Json;
using System.Text;

namespace MessagesApp.Consumer.Helpers
{
    public static class HttpClientHelper
    {
        public static HttpClient CreateClient
            => new HttpClient();

        public static StringContent CreateContent(object request)
            => new StringContent(JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json"); 

        public static TResponse ReadResponse<TResponse>(HttpResponseMessage response)
        {
            var sb = new StringBuilder();
            using (var item = response.Content)
            {
                var task = item.ReadAsStringAsync();
                sb.Append(task.Result);    
            }

            return JsonConvert.DeserializeObject<TResponse>(sb.ToString());
        }
    }
}
