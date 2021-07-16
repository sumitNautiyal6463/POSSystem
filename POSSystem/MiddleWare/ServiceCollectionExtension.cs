using Microsoft.Extensions.DependencyInjection;
using POSSystem.Interfaces;
using POSSystem.Repositories;

namespace POSSystem.MiddleWare
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLibrary(this IServiceCollection services)
        {
            services.AddScoped<ILogin, LoginRepository>();
            services.AddScoped<IProduct, ProductRepository>();
            services.AddScoped<IInvoice, InvoiceRepository>();
            return services;
        }
    }
}
