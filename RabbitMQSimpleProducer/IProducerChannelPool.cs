using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQSimpleProducer {
    public interface IProducerChannelPool
    {
        void Publish<T>(T obj, string exchange = null, string routingKey = null,
            IBasicProperties basicProperties = null);
    }
}
