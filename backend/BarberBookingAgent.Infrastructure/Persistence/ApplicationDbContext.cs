using BarberBookingAgent.Application.Common.Interfaces;
using BarberBookingAgent.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBookingAgent.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<AppointmentEntity> Appointments { get; set; } = null!;
    public DbSet<ChatMessageEntity> ChatMessages { get; set; } = null!;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
