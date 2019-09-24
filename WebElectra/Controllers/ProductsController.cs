using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebElectra.Entities;
using WebElectra.Helpers;
using WebElectra.ViewModels;

namespace WebElectra.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly EFDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;

        public ProductsController(IHostingEnvironment env,
            IConfiguration configuration, 
            EFDbContext context)
        {
            _configuration = configuration;
            _env = env;
            _context = context;
        }
        [HttpGet]
        public IActionResult ProductList()
        {
            var model = _context.Products.Select(
                p => new ProductVM
                {
                    Id=p.Id,
                    Name=p.Name,
                    Price=p.Price
                }).ToList();
            Thread.Sleep(5000);
            return Ok(model);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public IActionResult Create([FromBody]ProductAddVM model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var fileDestDir = _env.ContentRootPath;
            string dirName = _configuration.GetValue<string>("ImagesPath");
            //Папка де зберігаються фотки
            string dirPathSave = Path.Combine(fileDestDir, dirName);
            if (!Directory.Exists(dirPathSave))
            {
                Directory.CreateDirectory(dirPathSave);
            }
            var bmp = model.Image.FromBase64StringToImage();
            var imageName = Path.GetRandomFileName() + ".jpg";
            string fileSave = Path.Combine(dirPathSave, $"{imageName}");

            bmp.Save(fileSave, ImageFormat.Jpeg);

            DbProduct p = new DbProduct
            {
                Name = model.Name,
                Price = model.Price,
                DateCreate = DateTime.Now
            };
            _context.Products.Add(p);
            _context.SaveChanges();
            return Ok(p.Id);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]ProductDeleteVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var prod = _context.Products.SingleOrDefault(p=>p.Id==model.Id);
            if (prod != null)
            {
                _context.Products.Remove(prod);
                _context.SaveChanges();
            }
            return BadRequest();
        }
    }
}