using ControlGastos.Application.Interfaces;
using ControlGastos.Application.Services;
using ControlGastos.Application.Validators;
using ControlGastos.Domain.Interfaces;
using ControlGastos.Domain.Services;
using ControlGastos.Infrastructure.ExternalServices;
using ControlGastos.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ControlGastos.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Repositories
            services.AddSingleton<IPresupuestoRepository, PresupuestoRepository>();
            services.AddSingleton<IOrdenCompraRepository, OrdenCompraRepository>();

            // Domain Services
            services.AddSingleton<ValidadorOrdenCompraService>();

            // Application Services
            services.AddScoped<IPresupuestoService, PresupuestoService>();
            services.AddScoped<IOrdenCompraService, OrdenCompraService>();

            // External Services
            services.AddHttpClient<OrdenCompraApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://api.example.com/");
            });

            services.AddHttpClient<PresupuestoApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://api.example.com/");
            });

            // Validators
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CrearOrdenCompraValidator>();

            return services;
        }
    }
}
