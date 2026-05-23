using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class MatchLineupService : IMatchLineupService
{
    private readonly IMatchLineupRepository _lineupRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerRepository _playerRepository;

    public MatchLineupService(
        IMatchLineupRepository lineupRepository,
        IMatchRepository matchRepository,
        IPlayerRepository playerRepository)
    {
        _lineupRepository = lineupRepository;
        _matchRepository = matchRepository;
        _playerRepository = playerRepository;
    }

    public async Task<MatchLineup> AddPlayerAsync(
        int matchId,
        MatchLineup lineup)
    {
        // Verificar que el partido exista
        var match = await _matchRepository.GetByIdAsync(matchId);

        if (match == null)
        {
            throw new KeyNotFoundException(
                $"No se encontró el partido con ID {matchId}");
        }

        // El partido debe estar Scheduled
        if (match.Status != MatchStatus.Scheduled)
        {
            throw new InvalidOperationException(
                "Solo se pueden registrar alineaciones en partidos Scheduled");
        }

        // Verificar que el jugador exista
        var player = await _playerRepository.GetByIdAsync(
            lineup.PlayerId);

        if (player == null)
        {
            throw new KeyNotFoundException(
                $"No se encontró el jugador con ID {lineup.PlayerId}");
        }

        // El jugador debe pertenecer a uno de los equipos del partido
        if (player.TeamId != match.HomeTeamId &&
            player.TeamId != match.AwayTeamId)
        {
            throw new InvalidOperationException(
                "El jugador no pertenece a ninguno de los equipos del partido");
        }

        // El jugador no puede estar repetido
        var exists = await _lineupRepository.ExistsAsync(
            matchId,
            lineup.PlayerId);

        if (exists)
        {
            throw new InvalidOperationException(
                "El jugador ya está registrado en la alineación de este partido");
        }

        // Máximo 11 titulares por equipo
        if (lineup.IsStarter)
        {
            var starters = await _lineupRepository.CountStartersAsync(
                matchId,
                player.TeamId);

            if (starters >= 11)
            {
                throw new InvalidOperationException(
                    "El equipo ya tiene 11 titulares registrados en este partido");
            }
        }


        lineup.MatchId = matchId;

        await _lineupRepository.AddAsync(lineup);
        await _lineupRepository.SaveChangesAsync();
        return lineup;
    }

    public async Task<IEnumerable<MatchLineup>> GetLineupAsync(
        int matchId)
    {
        var lineup = await _lineupRepository.GetByMatchAsync(
            matchId);

        return lineup;
    }

    public async Task<IEnumerable<MatchLineup>> GetLineupByTeamAsync(
        int matchId,
        int teamId)
    {
        var lineup = await _lineupRepository.GetByMatchAndTeamAsync(
            matchId,
            teamId);

        return lineup;
    }

    public async Task DeleteAsync(
        int matchId,
        int lineupId)
    {
        var lineup = await _lineupRepository.GetByIdAsync(
            lineupId);

        if (lineup == null || lineup.MatchId != matchId)
        {
            throw new KeyNotFoundException(
                "No se encontró el registro de alineación");
        }

        _lineupRepository.Delete(lineup);

        await _lineupRepository.SaveChangesAsync();
    }
}
