using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services;

public interface ISponsorService
{

    Task<IEnumerable<Sponsor>> GetAllAsync();

    Task<Sponsor?> GetByIdAsync(int id);

    Task<Sponsor> CreateAsync(Sponsor sponsor);

    Task UpdateAsync(int id, Sponsor sponsor);

    Task DeleteAsync(int id);


    // Vincular sponsor a torneo
    Task RegisterToTournament(int sponsorId, int tournamentId, decimal contractAmount);

    // Listar torneos de un sponsor
    Task<IEnumerable<TournamentSponsor>> GetTournamentsAsync(int sponsorId);

    // Desvincular sponsor de torneo
    Task RemoveFromTournament(int sponsorId, int tournamentId);
}
