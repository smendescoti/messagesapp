namespace MessagesApp.Consumer.Models.Services.Responses
{
    public class AuthResponseModel
    {
        public string? Message { get; set; }
        public string? Client { get; set; }
        public DateTime? Expiration { get; set; }
        public string? Token { get; set; }
    }
}
