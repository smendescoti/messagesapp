using MessagesApp.Consumer.Helpers;
using MessagesApp.Consumer.Models.Services.Requests;
using MessagesApp.Consumer.Models.Services.Responses;
using MessagesApp.Consumer.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace MessagesApp.Consumer.Services
{
    public class MessagesService
    {
        private readonly EmailMessageApiSettings? _emailMessageApiSettings;

        public MessagesService(IOptions<EmailMessageApiSettings>? emailMessageApiSettings)
        {
            _emailMessageApiSettings = emailMessageApiSettings?.Value;
        }

        public async Task<MessagesResponseModel> Post(MessagesRequestModel model, string accessToken)
        {
            var client = HttpClientHelper.CreateClient;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PostAsync(_emailMessageApiSettings?.BaseUrl + "/messages",
                HttpClientHelper.CreateContent(model));

            return HttpClientHelper.ReadResponse<MessagesResponseModel>(response);
        }
    }
}
