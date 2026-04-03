using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
{
    Task<TournamentSponsor?> GetRelation(int tournamentId, int sponsorId);

    Task<IEnumerable<TournamentSponsor>> GetByTournament(int tournamentId);

    Task<IEnumerable<TournamentSponsor>> GetBySponsor(int sponsorId);
}