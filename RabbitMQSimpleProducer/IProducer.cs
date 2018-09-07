using RabbitMQ.Client;

namespace RabbitMQSimpleProducer {
    public interface IProducer
    {
        void Publish<T>(T obj, string exchange = null, string routingKey = null,
            IBasicProperties basicProperties = null);
    }
}
