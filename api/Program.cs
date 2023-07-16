using api.Helper;
using api.Middelware;
using Core.Entities;
using Core.Interface;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
/*builder.Services.AddDbContext<Context>(optionbuilder =>
optionbuilder.UseSqlite(builder.Configuration.GetConnectionString("default"),
op => op.MigrationsAssembly(typeof(Context).Assembly.FullName)));*/
builder.Services.AddDbContext<Context>(optionbuilder =>
optionbuilder.UseSqlServer(builder.Configuration.GetConnectionString("cs"),
op => op.MigrationsAssembly(typeof(Context).Assembly.FullName)));
builder.Services.AddIdentity<AddressBook, IdentityRole>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddSwaggerGen();
FileHelper.Initialize(builder.Environment);
var app = builder.Build();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddelware>();
app.UseStatusCodePagesWithReExecute("/errors/0");//use custome statuse code
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using var scope=app.Services.CreateScope();
var services = scope.ServiceProvider;
var context =services.GetRequiredService<Context>();
var User = services.GetRequiredService<UserManager<AddressBook>>();
//var logger= services.GetRequiredService<ILogger>();
try { 
await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
    await StoreContextSeed.SeedUserAsync(User);
}
catch {
   // logger.LogError("Error While Migrate");
}
app.Run();
