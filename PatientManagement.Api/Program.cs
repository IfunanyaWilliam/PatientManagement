using PatientManagement.Api.Extensions;
using PatientManagement.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args).AddSerilog();

// Add services to the container.


builder.Services.AddSwaggerGen();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
