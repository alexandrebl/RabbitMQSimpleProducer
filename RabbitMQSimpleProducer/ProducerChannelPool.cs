using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using RabbitMQSimpleConnectionFactory.Entity;
using RabbitMQSimpleConnectionFactory.Library;
using System.Text;

namespace RabbitMQSimpleProducer {
    public class ProducerChannelPool : IProducerChannelPool {

        private readonly ConnectionPool _connectionPool;
        public ProducerChannelPool(int poolSize, ConnectionSetting connectionSetting) {
            _connectionPool = new ConnectionPool(poolSize, connectionSetting);
        }

        public void Publish<T>(T obj, string exchange = null, string routingKey = null, IBasicProperties basicProperties = null) {
            var data = JsonConvert.SerializeObject(obj);
            var buffer = Encoding.UTF8.GetBytes(data);
            var channel = _connectionPool.GetChannel();

            //basicProperties = basicProperties ?? new BasicProperties { DeliveryMode = 2, Persistent = true };

            channel.BasicPublish(exchange: exchange ?? "", routingKey: routingKey, basicProperties: basicProperties, body: buffer);
        }
    }
}
