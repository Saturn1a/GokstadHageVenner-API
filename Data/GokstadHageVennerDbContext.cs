using GokstadHageVennerAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GokstadHageVennerAPI.Data;

public class GokstadHageVennerDbContext : DbContext
{
   
    public GokstadHageVennerDbContext(DbContextOptions<GokstadHageVennerDbContext> options) : base(options)
    {

    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Registration> Registration { get; set; }

}