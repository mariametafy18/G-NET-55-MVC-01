using GymManagementSystem.BLL;
using GymManagementSystem.BLL.Services.Members.Classes;
using GymManagementSystem.BLL.Services.Members.Interfaces;
using GymManagementSystem.BLL.Services.Sessions.Classes;
using GymManagementSystem.BLL.Services.Sessions.Interfaces;
using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Dependency Container
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));

            //builder.Services.AddScoped<IRepository<HealthRecord>, Repository<HealthRecord>>();
            //builder.Services.AddScoped<IRepository<Membership>, Repository<Membership>>();
            //builder.Services.AddScoped<IRepository<Plan>, Repository<Plan>>();
            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
