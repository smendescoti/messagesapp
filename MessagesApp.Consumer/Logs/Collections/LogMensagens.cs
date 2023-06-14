namespace MessagesApp.Consumer.Logs.Collections
{
    public class LogMensagens
    {
        public Guid? Id { get; set; }
        public DateTime? DataHoraEnvio { get; set; }
        public string? EmailPara { get; set; }
        public string? Assunto { get; set; }
        public string? Conteudo { get; set; }
    }
}
