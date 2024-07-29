var builder = WebApplication.CreateBuilder(args);
{
builder.Services.AddControllers();
builder.Services.AddScoped<IBreakfastService,breakfastService>();
}

var app = builder.Build();
{
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
}