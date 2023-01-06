using APICatalogue.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogue.Context;

public class AppDbContext : DbContext //Classe de configuração da conexão com o banco de dados
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{}

	public DbSet<Category>? Categories { get; set; }
	public DbSet<Product>? Products { get; set; }
}
