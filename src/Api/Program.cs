using Api.Utils;
using Logic.Customers;
using Logic.Movies;
using Logic.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new SessionFactory(builder.Configuration["ConnectionString"]));
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddTransient<MovieRepository>();
builder.Services.AddTransient<CustomerRepository>();

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
