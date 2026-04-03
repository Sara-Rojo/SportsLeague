using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface ITournamentRepository : IGenericRepository<Tournament>
{
    Task<IEnumerable<Tournament>> GetByStatusAsync(TournamentStatus status);
    Task<Tournament?> GetByIdWithTeamsAsync(int id);
    Task<Tournament?> GetByIdWithSponsorsAsync(int id);
    Task<IEnumerable<TournamentSponsor>> GetSponsorsAsync(int tournamentId);
}
