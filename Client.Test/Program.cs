using Confluent.Kafka;

var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "foo",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
{
    consumer.Subscribe("departures");
    try
    {
        while (true)
        {
            var result = consumer.Consume();
            Console.WriteLine(result.Message.Value);
        }
    }
    finally
    {
        consumer.Close();
    }
}