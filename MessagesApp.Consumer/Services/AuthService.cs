using MessagesApp.Consumer.Helpers;
using MessagesApp.Consumer.Models.Services.Requests;
using MessagesApp.Consumer.Models.Services.Responses;
using MessagesApp.Consumer.Settings;
using Microsoft.Extensions.Options;

namespace MessagesApp.Consumer.Services
{
    public class AuthService
    {
        private readonly EmailMessageApiSettings? _emailMessageApiSettings;

        public AuthService(IOptions<EmailMessageApiSettings>? emailMessageApiSettings)
        {
            _emailMessageApiSettings = emailMessageApiSettings?.Value;
        }

        public async Task<AuthResponseModel> Create()
        {
            var client = HttpClientHelper.CreateClient;

            var model = new AuthRequestModel
            {
                Key = _emailMessageApiSettings?.User,
                Pass = _emailMessageApiSettings?.Pass
            };

            var response = await client.PostAsync(_emailMessageApiSettings?.BaseUrl + "/auth",
                HttpClientHelper.CreateContent(model));

            return HttpClientHelper.ReadResponse<AuthResponseModel>(response);
        }
    }
}
