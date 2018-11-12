using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQSimpleProducer
{
    public class MessageSet<T>
    {
        public MessageSet(T message, string exchange, string routingKey, bool mandatory)
        {
            Message = message;
            Exchange = exchange;
            RoutingKey = routingKey;
            Mandatory = mandatory;
        }

        public T Message { get; set; }

        //public U Destination { get; set; }

        public string Exchange { get; set; }
        public bool Mandatory { get; set; }
        public string RoutingKey { get; set; }
        public IBasicProperties BasicProperties { get; set; }


        public string ToString()
        {
            return JsonConvert.SerializeObject(this.Message);
        }
    }



    class Destination
    {
        public string Exchange { get; set; }
        public bool Mandatory { get; set; }
        public string RouteKey { get; set; }

    }
}
