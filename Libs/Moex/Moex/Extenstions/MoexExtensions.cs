using Microsoft.Extensions.DependencyInjection;
using Moex.Abstractions;

namespace Moex.Extenstions;

public static class MoexExtensions
{
    public static IServiceCollection AddMoex(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMoexApi, MoexApi>();

        return serviceCollection;
    }
}