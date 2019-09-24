using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebElectra.Entities
{
    public class SeederDB
    {
        public static void SeedUsers(UserManager<DbUser> userManager,
            RoleManager<DbRole> roleManager)
        {
            string roleName = "Admin";
            var role = roleManager.FindByNameAsync(roleName).Result;
            if(role==null)
            {
                role = new DbRole
                {
                    Name = roleName
                };
                var addRoleResult=roleManager.CreateAsync(role).Result;
            }
            var userEmail = "bomba@gmail.com";
            var user = userManager.FindByEmailAsync(userEmail).Result;
            if(user==null)
            {
                user = new DbUser
                {
                    Email=userEmail,
                    UserName=userEmail
                };
                var result = userManager.CreateAsync(user, "Qwerty1-").Result;
                if(result.Succeeded)
                {
                    result = userManager.AddToRoleAsync(user, roleName).Result;
                }

            }
        }
        public static void SeedData(IServiceProvider services, IHostingEnvironment env, 
            IConfiguration config)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EFDbContext>();
                var managerUser = scope.ServiceProvider.GetRequiredService<UserManager<DbUser>>();
                var managerRole = scope.ServiceProvider.GetRequiredService<RoleManager<DbRole>>();

                SeedUsers(managerUser,managerRole);

                string name = "i7-7700K";
                var prod = context.Products.SingleOrDefault(p=>p.Name==name);
                if(prod==null)
                {
                    prod = new DbProduct
                    {
                        Name = name,
                        Price = 10000,
                        DateCreate = DateTime.Now
                    };
                    context.Products.Add(prod);
                    context.SaveChanges();
                }
            }
        }
    }
}
