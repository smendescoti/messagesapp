using MessagesApp.Consumer.Logs.Collections;
using MessagesApp.Consumer.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MessagesApp.Consumer.Logs.Contexts
{
    public class MongoDBContext
    {
        private readonly MongoDBSettings? _mongoDBSettings;
        private IMongoDatabase? _mongoDatabase;

        public MongoDBContext(IOptions<MongoDBSettings>? mongoDBSettings)
        {
            _mongoDBSettings = mongoDBSettings.Value;

            #region Conexão com o banco de dados

            var client = MongoClientSettings.FromUrl(new MongoUrl(_mongoDBSettings.Host));
            if (_mongoDBSettings.isSSL)
                client.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                };

            _mongoDatabase = new MongoClient(client)
                .GetDatabase(_mongoDBSettings.Name);

            #endregion
        }

        public IMongoCollection<LogMensagens> LogMensagens
            => _mongoDatabase.GetCollection<LogMensagens>("logmensagens");
    }
}
