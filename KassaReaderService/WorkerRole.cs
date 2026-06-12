// using Confluent.Kafka;
// using ClickHouse.Client;
// using Microsoft.Extensions.Hosting;
// using System.Text.Json;
// using Microsoft.Extensions.Configuration;
// using ClickHouse.Client.ADO;
// using Microsoft.Extensions.Logging;

// namespace KassaReaderService
// {
//     public class KafkaBackgroundService : BackgroundService
//     {
//         private readonly ILogger<KafkaBackgroundService> _logger;
//         private readonly IConfiguration _configuration;
//         private IConsumer<Ignore, string> _consumer;
//         private ClickHouseConnection? _clickhouseConnection;

//         public KafkaBackgroundService(ILogger<KafkaBackgroundService> logger, IConfiguration configuration)
//         {
//             _logger = logger;
//             _configuration = configuration;
//         }

//         protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//         {
//             // Configure Kafka consumer
//             var config = new ConsumerConfig
//             {
//                 BootstrapServers = _configuration["Kafka:Broker"] ?? "localhost:9092",
//                 GroupId = _configuration["Kafka:GroupId"] ?? "kassa-reader-group",
//                 AutoOffsetReset = AutoOffsetReset.Earliest,
//                 EnableAutoCommit = true
//             };

//             _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            
//             // Configure ClickHouse connection - default to localhost for now
//             var clickhouseConnectionString = _configuration["ClickHouse:ConnectionString"] ?? 
//                 "Host=localhost;Port=9000;Database=default;Username=default;";
//             _clickhouseConnection = new ClickHouseConnection(clickhouseConnectionString);

//             // Subscribe to Kafka topic - using the configured topic from appsettings.json
//             var kafkaTopic = _configuration["Kafka:Topic"] ?? "kassavik_events";
//             _consumer.Subscribe(kafkaTopic);

//             _logger.LogInformation("Kafka Background Service started. Listening for events on topic {Topic}", kafkaTopic);
            
//             try
//             {
//                 while (!stoppingToken.IsCancellationRequested)
//                 {
//                     try
//                     {
//                         var result = _consumer.Consume(stoppingToken);
                        
//                         if (result != null && !string.IsNullOrEmpty(result.Message.Value))
//                         {
//                             await ProcessKafkaMessage(result.Message.Value, stoppingToken);
//                         }
//                     }
//                     catch (ConsumeException ex)
//                     {
//                         _logger.LogError(ex, "Error consuming Kafka message");
//                     }
//                 }
//             }
//             finally
//             {
//                 _consumer.Close();
//                 _consumer.Dispose();
//             }
//         }

//         private async Task ProcessKafkaMessage(string kafkaMessage, CancellationToken cancellationToken)
//         {
//             try
//             {
//                 // Parse the Kafka message (assuming JSON format for now)
//                 var eventPayload = JsonSerializer.Deserialize<Dictionary<string, object>>(kafkaMessage);
                
//                 if (eventPayload == null)
//                 {
//                     _logger.LogWarning("Received empty or invalid Kafka message");
//                     return;
//                 }

//                 // Prepare data to insert into ClickHouse
//                 var tableName = "kassa_events"; // We can make this configurable later
//                 var columns = string.Join(", ", eventPayload.Keys);
//                 var values = string.Join(", ", eventPayload.Values.Select(v => 
//                     v is string s ? $"'{s.Replace("'", "''")}'" : v?.ToString() ?? "NULL"));
                
//                 // Insert into ClickHouse
//                 using var command = _clickhouseConnection.CreateCommand();
//                 command.CommandText = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
//                 await command.ExecuteNonQueryAsync(cancellationToken);
                
//                 _logger.LogInformation("Successfully processed Kafka event: {EventPayload}", kafkaMessage);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error processing Kafka message: {Message}", kafkaMessage);
//             }
//         }

//         public override async Task StopAsync(CancellationToken cancellationToken)
//         {
//             await base.StopAsync(cancellationToken);
            
//             if (_consumer != null)
//             {
//                 _consumer.Unsubscribe();
//                 _consumer.Close();
//                 _consumer.Dispose();
//             }
            
//             if (_clickhouseConnection != null)
//             {
//                 await _clickhouseConnection.CloseAsync();
//                 _clickhouseConnection.Dispose();
//             }
//         }
//     }
// }