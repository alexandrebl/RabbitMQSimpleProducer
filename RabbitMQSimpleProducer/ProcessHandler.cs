using System;

namespace RabbitMQSimpleProducer
{
    public static class ProcessHandler
    {
        public static void Retry(Action method, ref short numberOfTries)
        {
            try
            {
                method();
            }
            catch (Exception ex)
            {
                while (numberOfTries > 0)
                {
                    --numberOfTries;
                    Retry(method, ref numberOfTries);
                }
            }
        }
    }
}
