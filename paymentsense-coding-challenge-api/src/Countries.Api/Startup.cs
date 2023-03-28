using Countries.Api.Extensions;
using Countries.Application.Mappers;
using Countries.Application.Mappers.Interfaces;
using Countries.Application.Repositories;
using Countries.Application.Repositories.Decorators;
using Countries.Domain.Repositories.Interfaces;
using Countries.Infrastructure.Handlers;
using Countries.Infrastructure.HttpClients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly.Extensions.Http;
using Polly;
using System;
using System.Net.Http;

namespace Countries.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            services.AddCors(options =>
            {
                options.AddPolicy("PaymentsenseCodingChallengeOriginPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddHttpClient<RestCountriesHttpClient>((sp, httpClient) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                httpClient.BaseAddress = new Uri(configuration["RestCountriesApiUrl"]);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddHttpMessageHandler<RestCountriesOfflineDelegatingHandler>();

            services.AddMemoryCache();

            services.AddTransient<ICountriesRepository, CountriesRepository>();
            services.Decorate<ICountriesRepository, CountriesRepositoryWithCache>();

            services.AddSingleton<ICountryMapper, CountryMapper>();
            services.AddTransient<RestCountriesOfflineDelegatingHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("PaymentsenseCodingChallengeOriginPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseCustomExceptionHandlingMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(1));
        }
    }
}
