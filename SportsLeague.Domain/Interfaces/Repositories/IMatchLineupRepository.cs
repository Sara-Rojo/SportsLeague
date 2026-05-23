using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface IMatchLineupRepository
{
    Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);

    Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(
        int matchId,
        int teamId);

    Task<MatchLineup?> GetByIdAsync(int id);

    Task AddAsync(MatchLineup lineup);

    void Delete(MatchLineup lineup);

    Task<bool> ExistsAsync(int matchId, int playerId);

    Task<int> CountStartersAsync(int matchId, int teamId);

    Task SaveChangesAsync();
}
