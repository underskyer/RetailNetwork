using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KassaEventsDataBase;

public static class KassaEventsDbRegistrator
{
    public static IServiceCollection AddKassaEventsDb(
        this IServiceCollection services,
        string connectionString
    ) =>
        services.AddDbContext<KassaEventsDbContext>(op => op.UseClickHouse(connectionString));

}