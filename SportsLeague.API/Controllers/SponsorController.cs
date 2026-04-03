using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorController : ControllerBase
{
    private readonly ISponsorService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<SponsorController> _logger;

    public SponsorController(
        ISponsorService service,
        IMapper mapper,
        ILogger<SponsorController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    // GET ALL
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()
    {
        var sponsors = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors));
    }

    // GET BY ID
    [HttpGet("{id}")]
    public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)
    {
        var sponsor = await _service.GetByIdAsync(id);

        if (sponsor == null)
            return NotFound(new { message = $"Sponsor con ID {id} no encontrado" });

        return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));
    }

    // CREATE
    [HttpPost]
    public async Task<ActionResult<SponsorResponseDTO>> Create(SponsorRequestDTO dto)
    {
        try
        {
            var entity = _mapper.Map<Sponsor>(dto);
            var created = await _service.CreateAsync(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                _mapper.Map<SponsorResponseDTO>(created));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, SponsorRequestDTO dto)
    {
        try
        {
            var entity = _mapper.Map<Sponsor>(dto);
            await _service.UpdateAsync(id, entity);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // VINCULAR UN SPONSOR A UN TORNEO
    [HttpPost("{id}/tournaments")]
    public async Task<ActionResult> Register(int id, TournamentSponsorRequestDTO dto)
    {
        try
        {
            await _service.RegisterToTournament(id, dto.TournamentId, dto.ContractAmount);
            return Ok(new { message = "Sponsor vinculado correctamente" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // LISTAR TORNEOS DE SPONSOR
    [HttpGet("{id}/tournaments")]
    public async Task<ActionResult<IEnumerable<TournamentSponsorResponseDTO>>> GetTournaments(int id)
    {
        try
        {
            var data = await _service.GetTournamentsAsync(id);

            return Ok(_mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(data));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

   
    // DESVINCULAR
  
    [HttpDelete("{id}/tournaments/{tournamentId}")]
    public async Task<ActionResult> Remove(int id, int tournamentId)
    {
        try
        {
            await _service.RemoveFromTournament(id, tournamentId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
