using Confluent.Kafka;

namespace B_Q01.Kafka
{
    /// <summary>
    ///     Leverages the injected KafkaClientHandle instance to allow
    ///     Confluent.Kafka.Message{K,V}s to be produced to Kafka.
    /// </summary>
    public class KafkaDependentProducer//<K,V>
    {
        IProducer<string, string> kafkaHandle;

        public KafkaDependentProducer(KafkaClientHandle handle)
        {
            kafkaHandle = new DependentProducerBuilder<string, string>(handle.Handle).Build();
        }

        /// <summary>
        ///     Asychronously produce a message and expose delivery information
        ///     via the returned Task. Use this method of producing if you would
        ///     like to await the result before flow of execution continues.
        /// <summary>
        public Task<DeliveryResult<string, string>> ProduceAsync(string topic, Message<string, string> message)
            => this.kafkaHandle.ProduceAsync(topic, message);

        /// <summary>
        ///     Asynchronously produce a message and expose delivery information
        ///     via the provided callback function. Use this method of producing
        ///     if you would like flow of execution to continue immediately, and
        ///     handle delivery information out-of-band.
        /// </summary>
        public void Produce(string topic, Message<string, string> message, Action<DeliveryReport<string, string>> deliveryHandler = null)
            => this.kafkaHandle.Produce(topic, message, deliveryHandler);

        public void Flush(TimeSpan timeout)
            => this.kafkaHandle.Flush(timeout);
    }
}
