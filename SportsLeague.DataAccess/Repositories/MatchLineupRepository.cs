using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class MatchLineupRepository : IMatchLineupRepository
{
    private readonly LeagueDbContext _context;

    public MatchLineupRepository(LeagueDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId)
    {
        return await _context.MatchLineups
            .Include(ml => ml.Player)
            .Where(ml => ml.MatchId == matchId)
            .OrderByDescending(ml => ml.IsStarter)
            .ThenBy(ml => ml.Player.Number)
            .ToListAsync();
    }

    public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(
        int matchId,
        int teamId)
    {
        return await _context.MatchLineups
            .Include(ml => ml.Player)
            .Where(ml => ml.MatchId == matchId &&
                         ml.Player.TeamId == teamId)
            .OrderByDescending(ml => ml.IsStarter)
            .ThenBy(ml => ml.Player.Number)
            .ToListAsync();
    }

    public async Task<MatchLineup?> GetByIdAsync(int id)
    {
        return await _context.MatchLineups
            .Include(ml => ml.Player)
            .FirstOrDefaultAsync(ml => ml.Id == id);
    }

    public async Task AddAsync(MatchLineup lineup)
    {
        await _context.MatchLineups.AddAsync(lineup);
    }

    public void Delete(MatchLineup lineup)
    {
        _context.MatchLineups.Remove(lineup);
    }

    public async Task<bool> ExistsAsync(int matchId, int playerId)
    {
        return await _context.MatchLineups
            .AnyAsync(ml => ml.MatchId == matchId &&
                            ml.PlayerId == playerId);
    }

    public async Task<int> CountStartersAsync(int matchId, int teamId)
    {
        return await _context.MatchLineups
            .Include(ml => ml.Player)
            .CountAsync(ml => ml.MatchId == matchId &&
                              ml.Player.TeamId == teamId &&
                              ml.IsStarter);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
