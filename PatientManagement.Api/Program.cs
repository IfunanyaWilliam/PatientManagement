
using PatientManagement.Api.Extensions;
using Serilog;


public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddSerilog();

            builder.Services.AddServices(builder.Configuration);
            
            var app = builder.Build();

            // Seed data and configure the request pipeline
            await app.ConfigureRequestPipeline();
            app.Run();

        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, ex.Source ?? string.Empty, ex.InnerException, ex.Message, ex.ToString());
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
