using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/match/{matchId}/lineup")]
public class MatchLineupController : ControllerBase
{
    private readonly IMatchLineupService _lineupService;
    private readonly IMapper _mapper;

    public MatchLineupController(
        IMatchLineupService lineupService,
        IMapper mapper)
    {
        _lineupService = lineupService;
        _mapper = mapper;
    }

    // POST
    [HttpPost]
    public async Task<ActionResult<MatchLineupResponseDTO>> AddPlayer(
        int matchId,
        MatchLineupRequestDTO dto)
    {
        var lineup = _mapper.Map<MatchLineup>(dto);

        var created = await _lineupService.AddPlayerAsync(
            matchId,
            lineup);

        var response = _mapper.Map<MatchLineupResponseDTO>(
            created);

        return Ok(response);
    }

    // GET ALL
    [HttpGet]
    public async Task<ActionResult<
        IEnumerable<MatchLineupResponseDTO>>> GetLineup(
        int matchId)
    {
        var lineup = await _lineupService.GetLineupAsync(
            matchId);

        var response = _mapper.Map<
            IEnumerable<MatchLineupResponseDTO>>(lineup);

        return Ok(response);
    }

    // GET BY TEAM
    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<
        IEnumerable<MatchLineupResponseDTO>>> GetByTeam(
        int matchId,
        int teamId)
    {
        var lineup = await _lineupService.GetLineupByTeamAsync(
            matchId,
            teamId);

        var response = _mapper.Map<
            IEnumerable<MatchLineupResponseDTO>>(lineup);

        return Ok(response);
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int matchId,
        int id)
    {
        await _lineupService.DeleteAsync(
            matchId,
            id);

        return NoContent();
    }
}
