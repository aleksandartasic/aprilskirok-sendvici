using Microsoft.EntityFrameworkCore;
using Template.Models;

namespace Template.Models
{
    public class IspitDbContext : DbContext
    {
        public DbSet<Prodavnica> Prodavnica{get;set;}
        public DbSet<Proizvod> Proizvod{get;set;}
        public DbSet<Sastojak> Sastojak {get;set;}

        public DbSet<ProizvodSastojak> proizvodSastojak {get;set;}
       
// DbSet...

        public IspitDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
