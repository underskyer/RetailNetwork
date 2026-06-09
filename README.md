# RetailNetwork Event Simulator

## Описание
Проект `RetailNetwork` представляет собой систему симуляции событий кассовых операций, предназначенную для тестирования и отладки сервисов розничной сети. Система генерирует асинхронные события продажи (sales events) в формате JSON-сообщений и отправляет их в брокер сообщений Apache Kafka.

## Основные компоненты

### 1. KassaEventSimulator
Основной сервис, отвечающий за генерацию кассовых событий:
- Использует `IAsyncEnumerable<KassaMessage>` для потоковой передачи событий.
- Поддерживает настройку задержки между событиями.
- Отправляет сообщения в Kafka через Confluent.Kafka.

### 2. KassaMessage
Класс, описывающий структуру кассовых событий:
```csharp
public record KassaMessage(
    string Operation,
    string Terminal,
    DateTime Timestamp,
    decimal Amount,
    string Currency
);
```

### 3. Kafka Integration
- Использует Confluent.Kafka для отправки сообщений в брокер.
- Поддерживает конфигурацию через `appsettings.json`.
- Гарантирует сериализацию объектов с помощью `JsonSerializer`.

## Настройка

### appsettings.json
```json
{
  "EventProducer": {
    "BootstrapServers": "localhost:9092",
    "Topic": "kassa-events",
    "Delay": 1000
  }
}
```

## Запуск

### Сборка и запуск:
```bash
 dotnet build
 dotnet run
```

### Docker-развертывание:
```bash
 docker-compose up --build
```

## Логика работы
1. Сервис `BackgroundEventProducer` генерирует события с заданной периодичностью.
2. События сериализуются в JSON и отправляются в Kafka на тему "kassa-events".
3. Другие сервисы могут потреблять эти события для анализа или обработки.

## Ошибки и отладка
- Убедитесь, что Kafka запущен и доступен по адресу `localhost:9092`.
- Проверьте настройку темы в `appsettings.json`.
- Используйте логи для отслеживания ошибок отправки сообщений.

## Лицензия
Проект распространяется под лицензией MIT.