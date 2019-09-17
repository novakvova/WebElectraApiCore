using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebElectra.Entities;
using WebElectra.ViewModels;

namespace WebElectra.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly EFDbContext _context;
        public ProductsController(EFDbContext context)
        {
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
        [HttpPost]
        public IActionResult Create([FromBody]ProductAddVM model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
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