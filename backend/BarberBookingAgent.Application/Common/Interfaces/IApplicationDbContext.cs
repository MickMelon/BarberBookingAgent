using BarberBookingAgent.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBookingAgent.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AppointmentEntity> Appointments { get; }
    DbSet<ChatMessageEntity> ChatMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
