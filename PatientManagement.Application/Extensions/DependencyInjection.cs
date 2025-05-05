
namespace PatientManagement.Application.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Application.Extensions.Configurators;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddCqrs(cqrs => cqrs.AddHandlers());

            return services;
        } 
    }
}
