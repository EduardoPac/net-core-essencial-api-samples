using APICatalogue.Context;
using APICatalogue.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogue.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    public CategoriesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Category>> Get()
    {
        try
        {
            var Categories = _dbContext.Categories.AsNoTracking().ToList(); //AsNoTracking melhora o desenpenho não guardando em cache

            if (Categories == null || !Categories.Any())
                return NotFound("Categories not found...");

            return Categories;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                "Ocorreu um problema ao tratar a sua solicitação");
        }
        
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        var Categories = _dbContext.Categories.AsNoTracking().Include(p => p.Products).ToList();

        if (Categories == null || !Categories.Any())
            return NotFound("Categories not found...");

        return Categories;
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public ActionResult<Category> Get(int id)
    {
        var Category = _dbContext.Categories.AsNoTracking().FirstOrDefault(p => p.CategoryId == id);

        if (Category == null)
            return NotFound("Category not found...");

        return Category;
    }

    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
            return BadRequest("Category is null...");

        _dbContext.Categories.Add(category);
        _dbContext.SaveChanges();

        return new CreatedAtRouteResult("GetCategory", new { id = category.CategoryId }, category);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
            return BadRequest("Category not found...");

        _dbContext.Entry(category).State = EntityState.Modified;
        _dbContext.SaveChanges();

        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var Category = _dbContext.Categories.FirstOrDefault(p => p.CategoryId == id);

        if (Category is null)
            return NotFound("Category not found...");

        _dbContext.Categories.Remove(Category);
        _dbContext.SaveChanges();

        return Ok();
    }
}
