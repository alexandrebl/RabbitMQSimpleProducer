using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;
using RabbitMQSimpleConnectionFactory.Library;

namespace RabbitMQSimpleProducer {
    public class Producer : IProducer {
        private readonly IModel _channel;
        public Producer(ConnectionSetting connectionSetting) {
            _channel = ChannelFactory.Create(connectionSetting);
        }

        public Producer(IModel channel) {
            _channel = channel;
        }

        public Producer(IConnection connection) {
            _channel = connection.CreateModel();
        }

        /// <summary>
        /// Publica a mensagem na fila
        /// </summary>
        /// <param name="obj"></param>
        public void Publish<T>(T obj, string exchange = null, string routingKey = null, IBasicProperties basicProperties = null) {
            var data = JsonConvert.SerializeObject(obj);
            var buffer = Encoding.UTF8.GetBytes(data);

            _channel.BasicPublish(exchange: exchange ?? "", routingKey: routingKey, basicProperties: basicProperties, body: buffer);
        }
    }
}
