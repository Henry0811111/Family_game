namespace FamilyGames.API.Controllers;

using FamilyGames.Application.DTOs;
using FamilyGames.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;

 
    public PlayersController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var players = await _playerService.GetAllPlayersAsync();
        return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var player = await _playerService.GetPlayerByIdAsync(id);
        return player is null ? NotFound() : Ok(player);
    }

    // POST api/players
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePlayerDto dto)
    {
        try
        {
            var created = await _playerService.CreatePlayerAsync(dto);
            // 201 Created med Location-header som pekar på den nya resursen
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePlayerDto dto)
    {
        try
        {
            var updated = await _playerService.UpdatePlayerAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _playerService.DeletePlayerAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}