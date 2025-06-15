namespace Talabat.Extensions
{
    public static class SwaggerServices
    {
        public static IServiceCollection ApplySwaggerServices(this IServiceCollection Services)
        {
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();
            return Services;
        }
        public static WebApplication ApplySwaggerServices(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
