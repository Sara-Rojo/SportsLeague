using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services;

public interface IMatchLineupService
{
    Task<MatchLineup> AddPlayerAsync(int matchId, MatchLineup lineup);

    Task<IEnumerable<MatchLineup>> GetLineupAsync(int matchId);

    Task<IEnumerable<MatchLineup>> GetLineupByTeamAsync(
        int matchId,
        int teamId);

    Task DeleteAsync(int matchId, int lineupId);
}
