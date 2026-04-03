using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class TournamentSponsorRepository
    : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
{
    public TournamentSponsorRepository(LeagueDbContext context) : base(context)
    {
    }

    public async Task<TournamentSponsor?> GetRelation(int tournamentId, int sponsorId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.TournamentId == tournamentId && x.SponsorId == sponsorId);
    }

    public async Task<IEnumerable<TournamentSponsor>> GetByTournament(int tournamentId)
    {
        return await _dbSet
            .Where(x => x.TournamentId == tournamentId)
            .Include(x => x.Sponsor)
            .ToListAsync();
    }

    public async Task<IEnumerable<TournamentSponsor>> GetBySponsor(int sponsorId)
    {
        return await _dbSet
            .Where(x => x.SponsorId == sponsorId)
            .Include(x => x.Tournament)
            .ToListAsync();
    }
}
