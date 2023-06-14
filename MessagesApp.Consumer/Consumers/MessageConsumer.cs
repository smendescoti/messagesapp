using MessagesApp.Consumer.Logs.Collections;
using MessagesApp.Consumer.Logs.Persistence;
using MessagesApp.Consumer.Models.Messages;
using MessagesApp.Consumer.Models.Services.Requests;
using MessagesApp.Consumer.Services;
using MessagesApp.Consumer.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessagesApp.Consumer.Consumers
{
    public class MessageConsumer : BackgroundService
    {
        private readonly IServiceProvider? _serviceProvider;
        private readonly AuthService? _authService;
        private readonly MessagesService? _messagesService;
        private readonly LogMensagensPersistence? _logMensagensPersistence;
        private readonly RabbitMQSettings? _rabbitMQSettings;

        private IConnection? _connection;
        private IModel? _model;

        public MessageConsumer(IServiceProvider? serviceProvider, AuthService? authService, MessagesService? messagesService, LogMensagensPersistence? logMensagensPersistence, IOptions<RabbitMQSettings>? rabbitMQSettings)
        {
            _serviceProvider = serviceProvider;
            _authService = authService;
            _messagesService = messagesService;
            _logMensagensPersistence = logMensagensPersistence;
            _rabbitMQSettings = rabbitMQSettings.Value;

            var factory = new ConnectionFactory { Uri = new Uri(_rabbitMQSettings?.Host) };
            _connection = factory.CreateConnection();
            _model = _connection.CreateModel();

            _model.QueueDeclare(
                queue: _rabbitMQSettings?.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //objeto através do qual podemos ler o conteudo da fila
            var consumer = new EventingBasicConsumer(_model);

            //programando a leitura da fila
            consumer.Received += async (sender, args) =>
            {
                var contentString = Encoding.UTF8.GetString(args.Body.ToArray());
                var messageModel = JsonConvert.DeserializeObject<MessageModel>(contentString);

                //processando o conteudo
                using (var scope = _serviceProvider?.CreateScope())
                {
                    switch(messageModel.Tipo)
                    {
                        case 1:
                            var model = new MessagesRequestModel
                            {
                                MailTo = messageModel.EmailUsuario,
                                Subject = "Conta de usuário criada com sucesso! Sistema Infis.",
                                Body = $"Parabéns {messageModel.NomeUsuario}, sua conta foi criada com sucesso",
                                IsBodyHtml = true
                            };

                            //enviando para a API de entrega de emails
                            var authResponse = await _authService?.Create();
                            var messageResponse = await _messagesService?.Post(model, authResponse.Token);

                            //gravar o log
                            _logMensagensPersistence.Add(new LogMensagens
                            {
                                Id = Guid.NewGuid(),
                                DataHoraEnvio = DateTime.Now,
                                EmailPara = model.MailTo,
                                Assunto = model.Subject,
                                Conteudo = model.Body
                            });

                            //retirar o item da fila
                            _model.BasicAck(args.DeliveryTag, false);

                            break;
                    }
                }
            };

            //executando a leitura da fila
            _model.BasicConsume(_rabbitMQSettings.Queue, false, consumer);
        }
    }
}
