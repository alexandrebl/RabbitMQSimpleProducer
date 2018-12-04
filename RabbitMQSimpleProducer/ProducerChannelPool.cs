using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;
using RabbitMQSimpleConnectionFactory.Library;

namespace RabbitMQSimpleProducer
{
    public class ProducerChannelPool : IProducerChannelPool
    {

        private readonly ConnectionPool _connectionPool;
        public ProducerChannelPool(int poolSize, ConnectionSetting connectionSetting, string clientProvidedName = null)
        {

            var channelFactory = new ChannelFactory(connectionSetting, clientProvidedName);
            _connectionPool = new ConnectionPool(poolSize, channelFactory);
        }

        public void Publish<T>(T obj, string exchange = null, string routingKey = null, IBasicProperties basicProperties = null)
        {
            var data = JsonConvert.SerializeObject(obj);
            var buffer = Encoding.UTF8.GetBytes(data);
            var channel = _connectionPool.GetChannel();

            //basicProperties = basicProperties ?? new BasicProperties { DeliveryMode = 2, Persistent = true };

            channel.BasicPublish(exchange: exchange ?? "", routingKey: routingKey, basicProperties: basicProperties, body: buffer);
        }
    }
}
