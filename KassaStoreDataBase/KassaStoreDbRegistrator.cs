using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KassaStoreDataBase;

public static class KassaStoreDbRegistrator
{
    public static IServiceCollection AddKassaStoreDb(
        this IServiceCollection services,
        string connectionString
    ) =>
        services.AddDbContext<KassaStoreDbContext>(op => op.UseNpgsql(connectionString));

}