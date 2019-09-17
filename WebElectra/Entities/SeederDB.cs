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
        public static void SeedData(IServiceProvider services, IHostingEnvironment env, 
            IConfiguration config)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EFDbContext>();

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
