namespace MessagesApp.Consumer.Models.Messages
{
    public class MessageModel
    {
        public int? Tipo { get; set; }
        public DateTime? DataHora { get; set; }
        public string? IdUsuario { get; set; }
        public string? NomeUsuario { get; set; }
        public string? EmailUsuario { get; set; }
    }
}
