using System.Text.Json;
using Confluent.Kafka;

public class JsonSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context) =>
        JsonSerializer.SerializeToUtf8Bytes(data);
}
