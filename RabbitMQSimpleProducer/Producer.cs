using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;
using RabbitMQSimpleConnectionFactory.Library;

namespace RabbitMQSimpleProducer
{
    public class Producer : IProducer
    {
        private IModel _channel;
        private readonly ConnectionSetting _connectionSetting;

        public Producer(ConnectionSetting connectionSetting)
        {
            _connectionSetting = connectionSetting;
            _channel = ChannelFactory.Create(_connectionSetting);
        }

        public Producer(IModel channel)
        {
            _channel = channel;
        }

        public Producer(IConnection connection)
        {
            _channel = connection.CreateModel();
        }

        /// <summary>
        /// Publica a mensagem na fila
        /// </summary>
        /// <param name="obj"></param>
        public void Publish<T>(T obj, string exchange = null, string routingKey = null, IBasicProperties basicProperties = null, short numberOfTries = 0)
        {
            var data = JsonConvert.SerializeObject(obj);
            var buffer = Encoding.UTF8.GetBytes(data);

            ProcessHandler.Retry(() =>
            {
                if (_channel == null || _channel.IsClosed)
                _channel = ChannelFactory.Create(_connectionSetting);

                _channel.BasicPublish(exchange: exchange ?? "", routingKey: routingKey, basicProperties: basicProperties, body: buffer);
            }, ref numberOfTries);
        }
    }
}
