using SchoolOfDevs.Helpers;
using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Services;
using SchoolOfDevs.Middleware;
using System.Text.Json.Serialization;
using SchoolOfDevs.Profiles;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); //Evitar estouro de mem�ria ignorando os ciclos


builder.Services.AddDbContext<DataContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<ICourseService, CourseService>();

var mapperConfig = MapperConfig.GetMapperConfig();
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
