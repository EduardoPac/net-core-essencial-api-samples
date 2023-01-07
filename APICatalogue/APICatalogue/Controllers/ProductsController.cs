using APICatalogue.Context;
using APICatalogue.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogue.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    public ProductsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _dbContext.Products.AsNoTracking().ToList(); //AsNoTracking melhora o desenpenho não guardando em cache

        if(products == null || !products.Any())
            return NotFound("Products not found...");

        return products;
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
    public ActionResult<Product> Get(int id)
    {
        var product = _dbContext.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);

        if (product == null)
            return NotFound("Product not found...");

        return product;
    }

    [HttpPost]
    public ActionResult Post(Product product)
    {
        if (product is null)
            return BadRequest("Product is null...");

        _dbContext.Products.Add(product);
        _dbContext.SaveChanges();

        return new CreatedAtRouteResult("GetProduct", new { id = product.ProductId }, product);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Product product)
    {
        if(id != product.ProductId)
            return BadRequest("Product not found...");

        _dbContext.Entry(product).State = EntityState.Modified;
        _dbContext.SaveChanges();

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var product = _dbContext.Products.FirstOrDefault(p =>p.ProductId == id);

        if(product is null)
            return NotFound("Product not found...");

        _dbContext.Products.Remove(product);
        _dbContext.SaveChanges();

        return Ok();
    }
}
