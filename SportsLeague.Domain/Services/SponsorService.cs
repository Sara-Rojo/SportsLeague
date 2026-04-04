using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class SponsorService : ISponsorService
{
    private readonly ISponsorRepository _sponsorRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITournamentSponsorRepository _tsRepository;
    private readonly ILogger<SponsorService> _logger;

    public SponsorService(
        ISponsorRepository sponsorRepository,
        ITournamentRepository tournamentRepository,
        ITournamentSponsorRepository tsRepository,
        ILogger<SponsorService> logger)
    {
        _sponsorRepository = sponsorRepository;
        _tournamentRepository = tournamentRepository;
        _tsRepository = tsRepository;
        _logger = logger;
    }


    // CRUD

    public async Task<IEnumerable<Sponsor>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all sponsors");
        return await _sponsorRepository.GetAllAsync();
    }

    public async Task<Sponsor?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving sponsor with ID: {Id}", id);
        return await _sponsorRepository.GetByIdAsync(id);
    }

    public async Task<Sponsor> CreateAsync(Sponsor sponsor)
    {
        
        if (await _sponsorRepository.ExistsByNameAsync(sponsor.Name))
        {
            _logger.LogWarning("Sponsor with name {Name} already exists", sponsor.Name);
            throw new InvalidOperationException("Ya existe un sponsor con ese nombre");
        }


        if (!sponsor.ContactEmail.Contains("@"))
        {
            throw new InvalidOperationException("Email inválido");
        }

        _logger.LogInformation("Creating sponsor: {Name}", sponsor.Name);

        return await _sponsorRepository.CreateAsync(sponsor);
    }

    public async Task UpdateAsync(int id, Sponsor sponsor)
    {
        var existing = await _sponsorRepository.GetByIdAsync(id);

        if (existing == null)
            throw new KeyNotFoundException($"Sponsor con ID {id} no encontrado");

        existing.Name = sponsor.Name;
        existing.ContactEmail = sponsor.ContactEmail;
        existing.Phone = sponsor.Phone;
        existing.WebsiteUrl = sponsor.WebsiteUrl;
        existing.Category = sponsor.Category;

        _logger.LogInformation("Updating sponsor with ID: {Id}", id);

        await _sponsorRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _sponsorRepository.ExistsAsync(id);

        if (!exists)
            throw new KeyNotFoundException($"Sponsor con ID {id} no encontrado");

        _logger.LogInformation("Deleting sponsor with ID: {Id}", id);

        await _sponsorRepository.DeleteAsync(id);
    }


    // RELACIÓN N:M

    public async Task RegisterToTournament(int sponsorId, int tournamentId, decimal contractAmount)
    {
 
        if (contractAmount <= 0)
            throw new InvalidOperationException("El monto debe ser mayor a 0");

        var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
        if (sponsor == null)
            throw new KeyNotFoundException("Sponsor no encontrado");

        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
        if (tournament == null)
            throw new KeyNotFoundException("Tournament no encontrado");

        var exists = await _tsRepository.GetRelation(tournamentId, sponsorId);
        if (exists != null)
            throw new InvalidOperationException("El sponsor ya está vinculado a este torneo");

        _logger.LogInformation("Linking sponsor {SponsorId} to tournament {TournamentId}", sponsorId, tournamentId);

        await _tsRepository.CreateAsync(new TournamentSponsor
        {
            SponsorId = sponsorId,
            TournamentId = tournamentId,
            ContractAmount = contractAmount
        });
    }

    public async Task<IEnumerable<TournamentSponsor>> GetTournamentsAsync(int sponsorId)
    {
        var exists = await _sponsorRepository.ExistsAsync(sponsorId);

        if (!exists)
            throw new KeyNotFoundException("Sponsor no encontrado");

        return await _tsRepository.GetBySponsor(sponsorId);
    }

    public async Task RemoveFromTournament(int sponsorId, int tournamentId)
    {
        var relation = await _tsRepository.GetRelation(tournamentId, sponsorId);

        if (relation == null)
            throw new KeyNotFoundException("Relación no encontrada");

        await _tsRepository.DeleteAsync(relation.Id);
    }
}
