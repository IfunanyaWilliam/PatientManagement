
namespace PatientManagement.Common.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Common.DependencyInjection;


    public static class DependencyInjection
    {
        public static IServiceCollection AddDependenciesFromCommon(this IServiceCollection services)
        {
            services.AddCqrs(cqrs => cqrs.AddHandlers());

            return services;
        }
    }
}
