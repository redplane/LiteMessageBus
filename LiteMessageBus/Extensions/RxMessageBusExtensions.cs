using LiteMessageBus.Services.Implementations;
using LiteMessageBus.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LiteMessageBus.Extensions
{
    // ReSharper disable once UnusedMember.Global
    public static class RxMessageBusExtensions
    {
        #region Methods

        /// <summary>
        /// Add in memory message bus into the system.
        /// </summary>
        /// <param name="services"></param>
        public static void AddInMemoryRxMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<ILiteMessageBusService, InMemoryLiteMessageBusService>();
        }

        #endregion
    }
}