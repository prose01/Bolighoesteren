using FilterLibrary;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;

namespace Bolighoesteren
{
    public class Context : DbContext
    {
        private string _connectionString;

        public DbSet<Ejendom> Ejendomme { get; set; }

        public Context()
        {
            string json = File.ReadAllText("appsettings.json");
            var settings = JsonConvert.DeserializeObject<Appsettings>(json);
            _connectionString = settings.ConnectionString ?? "Data Source=BolighoesterenDB";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ejendom>()
                .HasIndex(p => p.Postnummer);

            modelBuilder.Entity<Ejendom>()
                .HasIndex(p => p.Adresse);
        }
    }
}
