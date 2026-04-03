using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class TournamentRepository : GenericRepository<Tournament>, ITournamentRepository
{
    public TournamentRepository(LeagueDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Tournament>> GetByStatusAsync(TournamentStatus status)
    {
        return await _dbSet
        .Where(t => t.Status == status)
        .ToListAsync();
    }

    public async Task<Tournament?> GetByIdWithTeamsAsync(int id)
    { 
        return await _dbSet
        .Include(t => t.TournamentTeams)
        .ThenInclude(tt => tt.Team)
        .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<Tournament?> GetByIdWithSponsorsAsync(int id)
    {
        return await _dbSet
            .Include(t => t.TournamentSponsors)
            .ThenInclude(ts => ts.Sponsor)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<IEnumerable<TournamentSponsor>> GetSponsorsAsync(int tournamentId)
    {
        return await _dbSet
            .Where(t => t.Id == tournamentId)
            .SelectMany(t => t.TournamentSponsors)
            .Include(ts => ts.Sponsor)
            .ToListAsync();
    }
}
