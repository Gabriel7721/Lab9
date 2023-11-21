using Lab9Lib;
using Lab9WebAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab9WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DatabaseContext _db;
        public ProductsController(DatabaseContext db)
        {
            _db=db;
        }
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _db.Product.ToListAsync();
        }
        [HttpPost]
        public async Task<bool> PostProduct(Product newProduct)
        {
            await _db.Product.AddAsync(newProduct);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
