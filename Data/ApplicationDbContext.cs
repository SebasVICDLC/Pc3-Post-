﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ImplementaPost.Models;

namespace ImplementaPost.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

     public DbSet<Contacto> DataContactos { get; set; }
    public DbSet<Producto> DataProductos { get; set; }
    public DbSet<Proforma> DataCarrito { get; set; }
    public DbSet<Pago> DataPago { get; set; }
    public DbSet<Pedido> DataPedido { get; set; }
    public DbSet<DetallePedido> DataDetallePedido { get; set; }
}