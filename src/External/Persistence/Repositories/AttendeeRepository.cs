using Domain.Entities;
using Domain.Repositories;
using Persistence.Data;

namespace Persistence.Repositories;

internal sealed class AttendeeRepository(ApplicationDbContext dbContext) : IAttendeeRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void Add(Attendee attendee) => _dbContext.Set<Attendee>().Add(attendee);
}