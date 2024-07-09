using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PruebaUnitaria.Models;

public partial class DbPracticaContext : DbContext
{
    public DbPracticaContext()
    {
    }

    public DbPracticaContext(DbContextOptions<DbPracticaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Proveedore> Proveedores { get; set; }
    public virtual DbSet<Producto> Productos { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=DB_Practica;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
     

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
