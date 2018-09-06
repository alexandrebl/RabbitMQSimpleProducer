using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQSimpleConnectionFactory.Entity;
using RabbitMQSimpleConnectionFactory.Library;

namespace RabbitMQSimpleProducer {
    public class ProducerChannelPool
    {

        private readonly ConnectionPool _connectionPool;
        public ProducerChannelPool(int poolSize, ConnectionSetting connectionSetting)
        {
            _connectionPool = new ConnectionPool(poolSize, connectionSetting);
        }

        public void Publish<T>(T obj, string exchange = null, string routingKey = null, IBasicProperties basicProperties = null) {
            var data = JsonConvert.SerializeObject(obj);
            var buffer = Encoding.UTF8.GetBytes(data);
            var channel = _connectionPool.GetChannel();

            channel.BasicPublish(exchange: exchange ?? "", routingKey: routingKey, basicProperties: basicProperties, body: buffer);
        }
    }
}
