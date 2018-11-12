using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
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
            var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

            ProcessHandler.Retry(() =>
            {
                if (_channel == null || _channel.IsClosed)
                    _channel = ChannelFactory.Create(_connectionSetting);

                basicProperties = basicProperties ?? new BasicProperties { DeliveryMode = 2, Persistent = true };

                _channel.BasicPublish(exchange: exchange ?? "", routingKey: routingKey, basicProperties: basicProperties, body: buffer);

                numberOfTries = 0;

            }, ref numberOfTries);
        }

        public void BatchPublish<T>(IEnumerable<T> data, string exchange = null, string routingKey = null, bool mandatory = false, IBasicProperties basicProperties = null)
        {
            var publisher = _channel.CreateBasicPublishBatch();

            foreach (var item in data)
            {
                var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(item));

                basicProperties = basicProperties ?? new BasicProperties { DeliveryMode = 2, Persistent = true };

                publisher.Add(exchange, routingKey, mandatory, basicProperties, buffer);
            }

            publisher.Publish();
        }

        public void BatchPublish(IEnumerable<byte[]> data, string exchange = null, string routingKey = null, bool mandatory = false, IBasicProperties basicProperties = null)
        {
            var publisher = _channel.CreateBasicPublishBatch();

            foreach (var item in data)
            {
                basicProperties = basicProperties ?? new BasicProperties { DeliveryMode = 2, Persistent = true };

                publisher.Add(exchange, routingKey, mandatory, basicProperties, item);
            }

            publisher.Publish();
        }

        public void BatchPublish<T>(List<MessageSet<T>> messageBatch)
        {
            var publisher = _channel.CreateBasicPublishBatch();

            foreach (var message in messageBatch)
            {
                var body = Encoding.UTF8.GetBytes(message.ToString());

                message.BasicProperties = message.BasicProperties ?? new BasicProperties { DeliveryMode = 2, Persistent = true };

                publisher.Add(message.Exchange, message.RoutingKey, message.Mandatory, message.BasicProperties, body);
            }

            publisher.Publish();
        }
    }
}

