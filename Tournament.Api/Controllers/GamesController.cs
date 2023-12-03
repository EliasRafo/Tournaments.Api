using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;
using Tournament.Core.Entities;
using Tournament.Data.Repositories;
using AutoMapper;
using Tournament.Core.Repositories;
using Tournament.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly TournamentApiContext _context;
        private readonly IMapper mapper;
        private readonly IUoW _uoW;

        public GamesController(TournamentApiContext context, IMapper mapper, IUoW uoW)
        {
            _context = context;
            this.mapper = mapper;
            _uoW = uoW;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame(bool sorting = false, string filter = "")
        {
            var games = await _uoW.GameRepository.GetAllAsync(sorting);

            if (!string.IsNullOrEmpty(filter))
            {
                games = games.Where(d => d.Title.Contains(filter));
            }

            return Ok(mapper.Map<IEnumerable<GameDto>>(games));
        }

        // GET: api/Games/
        [HttpGet("{title}")]
        public async Task<ActionResult<GameDto>> GetGame(string title)
        {
            var game = mapper.Map<GameDto>(await _uoW.GameRepository.GetByTitleAsync(title));

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(Guid id, GameForUpdateDto game)
        {
            if (id != game.GameId)
            {
                return BadRequest();
            }

            var existingGame = await _uoW.GameRepository.GetAsync(id);

            if (existingGame == null) return NotFound();

            mapper.Map(game, existingGame);

            _uoW.GameRepository.Update(existingGame);

            await _uoW.CompleteAsync();

            return Ok(mapper.Map<GameForUpdateDto>(existingGame));
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(GameDto game)
        {
            var g = mapper.Map<Game>(game);
            _uoW.GameRepository.Add(g);
            await _uoW.CompleteAsync();

            var GameToReturn = mapper.Map<GameDto>(g);

            return CreatedAtAction("GetGame", new { title = g.Title }, GameToReturn);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            var game = await _uoW.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            _uoW.GameRepository.Remove(game);

            await _uoW.CompleteAsync();

            return NoContent();
        }

        [HttpPatch("{gameId}")]
        public async Task<ActionResult> PatchGame(
            Guid gameId,
            JsonPatchDocument<GameForUpdateDto> patchDoc)
        {
            var gameToPatch = await _uoW.GameRepository.GetAsync(gameId);

            if (gameToPatch is null) return NotFound();

            var dto = mapper.Map<GameForUpdateDto>(gameToPatch);

            patchDoc.ApplyTo(dto, ModelState);

            await TryUpdateModelAsync(dto);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            mapper.Map(dto, gameToPatch);
            _uoW.GameRepository.Update(gameToPatch);
            await _uoW.CompleteAsync();

            return NoContent();
        }

    }
}
