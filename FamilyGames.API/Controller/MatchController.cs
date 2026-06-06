namespace FamilyGames.API.Controllers;

using FamilyGames.Application.DTOs;
using FamilyGames.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly IMatchService _matchService;

    public MatchesController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    // GET api/matches
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var matches = await _matchService.GetAllMatchesAsync();
        return Ok(matches);
    }

    // GET api/matches/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var match = await _matchService.GetMatchByIdAsync(id);
        return match is null ? NotFound() : Ok(match);
    }

    // GET api/matches/player/3 – alla matcher för en specifik spelare
    [HttpGet("player/{playerId}")]
    public async Task<IActionResult> GetByPlayer(int playerId)
    {
        var matches = await _matchService.GetMatchesByPlayerAsync(playerId);
        return Ok(matches);
    }

    // POST api/matches
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMatchDto dto)
    {
        try
        {
            var created = await _matchService.CreateMatchAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            // Spelaren hittades inte – returnera 404
            return NotFound(ex.Message);
        }
    }

    // PUT api/matches/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMatchDto dto)
    {
        try
        {
            var updated = await _matchService.UpdateMatchAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE api/matches/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _matchService.DeleteMatchAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}