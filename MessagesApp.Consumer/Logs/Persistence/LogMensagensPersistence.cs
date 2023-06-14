using MessagesApp.Consumer.Logs.Collections;
using MessagesApp.Consumer.Logs.Contexts;
using MongoDB.Driver;

namespace MessagesApp.Consumer.Logs.Persistence
{
    public class LogMensagensPersistence
    {
        private readonly MongoDBContext? _mongoDBContext;

        public LogMensagensPersistence(MongoDBContext? mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }

        public void Add(LogMensagens log)
        {
            _mongoDBContext.LogMensagens
                .InsertOne(log);
        }

        public List<LogMensagens> GetAll()
        {
            var filter = Builders<LogMensagens>.Filter.Where(l => true);
            return _mongoDBContext.LogMensagens
                .Find(filter)
                .SortByDescending(l => l.DataHoraEnvio)
                .ToList();
        }
    }
}
