using AutoMapper;
using Microsoft.AspNetCore.Mvc.Formatters;
using WebApi.MinimalApi.Domain;
using WebApi.MinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5001;http://localhost:5000");
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

builder.Services.AddControllers(options =>
    {
        options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
        options.ReturnHttpNotAcceptable = true;
        options.RespectBrowserAcceptHeader = true;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressMapClientErrors = true;
    });


builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<UserEntity, UserDto>().ForMember(dest=>dest.FullName,
        act=>act.MapFrom(src=>
            $"{src.LastName} {src.FirstName}"));

    cfg.CreateMap<UserToCreateDTO, UserEntity>()
        .ForMember(dest=>dest.Login,
        act=>act.MapFrom(src=>src.Login))
        .ForMember(dest=>dest.FirstName,
            act=>act.MapFrom(src=>src.FirstName))
        .ForMember(dest=>dest.LastName,
            act=>act.MapFrom(src=>src.LastName))
        .ForMember(dest=>dest.Id,
            act=>act.MapFrom((src)=>new Guid()));
}, new System.Reflection.Assembly[0]);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();


app.Run();