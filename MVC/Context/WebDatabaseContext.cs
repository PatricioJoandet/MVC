using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;
using System.Collections.Generic;

namespace MVC.Context
{
    public class WebDatabaseContext : DbContext
    {
        public WebDatabaseContext(DbContextOptions<WebDatabaseContext> options) : base(options)
        {
        }
            public DbSet<Usuario> Usuarios { get; set; }
            public DbSet<Tablero> Tableros { get; set; }
            public DbSet<Tarea> Tareas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tablero>()
                .HasOne(t => t.Usuario)
                .WithMany()
                .HasForeignKey(t => t.userId);

            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.Tablero)
                .WithMany()
                .HasForeignKey(t => t.tableroId);
        }

    }
}
