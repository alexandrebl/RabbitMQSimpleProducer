# RabbitMQSimpleProducer

## How to use
Install-Package RabbitMQSimpleProducer -Version 1.0.0

```csharp
using System;
using RabbitMQSimpleConnectionFactory.Entity;

namespace ConsoleApp10 {
    class Program {
        static void Main(string[] args) {

            var producar = new RabbitMQSimpleProducer.Producer(
                new ConnectionSetting {
                    HostName = "xxxxx",
                    Password = "xxxxx",
                    UserName = "xxxxx",
                    Port = 5672,
                    VirtualHost = "xxxxx"
                }
            );
            Console.WriteLine($"{DateTime.UtcNow:o}");
            for (var index = 0; index < 1000; index++)
                producar.Publish($"{DateTime.UtcNow:o} - Hi!", "xxxxx", "xxxxx");

            Console.WriteLine("end");
            Console.WriteLine($"{DateTime.UtcNow:o}");
            Console.ReadLine();
        }
    }
}
```
