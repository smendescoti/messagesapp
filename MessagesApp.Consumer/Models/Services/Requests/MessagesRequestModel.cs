namespace MessagesApp.Consumer.Models.Services.Requests
{
    public class MessagesRequestModel
    {
        public string? Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public string? MailTo { get; set; }
        public string? Subject { get; set; }
    }
}
