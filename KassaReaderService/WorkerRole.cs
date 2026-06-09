using Confluent.Kafka;

namespace CassaReaderService
{
    public class WorkerRole
    {


        public async Task RunAsync(string kafkaTopic, string groupId) 
        {
            //var consumer = new Consumer<UUID, string>(kafka-topic = "kassavik_events", group_id = groupId);  
            // читаем Kafka topic и пишем в Postgres  
        }
    }
}