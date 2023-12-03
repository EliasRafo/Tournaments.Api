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
using Tournament.Core.Repositories;
using AutoMapper;
using Tournament.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Bogus.DataSets;
using Humanizer;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly TournamentApiContext _context;
        private readonly IMapper mapper;
        private readonly IUoW _uoW;

        public TournamentsController(TournamentApiContext context, IMapper mapper, IUoW uoW)
        {
            _context = context;
            this.mapper = mapper;
            _uoW = uoW;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournament(bool includeGames = false, bool sorting = false, string filter = "")
        {
            var dtos = includeGames ? mapper.Map<IEnumerable<TournamentDto>>(await _uoW.TournamentRepository.GetAllAsync(includeGames: true, sorting)) :
                                          mapper.Map<IEnumerable<TournamentDto>>(await _uoW.TournamentRepository.GetAllAsync(includeGames: false, sorting));

            if (!string.IsNullOrEmpty(filter))
            {
                dtos = dtos.Where(d => d.Title.Contains(filter));
            }

            return Ok(dtos);
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournament(Guid id)
        {
            var tournament = mapper.Map<TournamentDto>(await _uoW.TournamentRepository.GetAsync(id));

            if (tournament == null)
            {
                return NotFound();
            }

            return tournament;
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(Guid id, TournamentForUpdateDto tournament)
        {
            if (id != tournament.TournamentId)
            {
                return BadRequest();
            }

            var existingTou = await _uoW.TournamentRepository.GetAsync(id);

            if (existingTou == null) return NotFound();

            mapper.Map(tournament, existingTou);

            _uoW.TournamentRepository.Update(existingTou);

            await _uoW.CompleteAsync();

            return Ok(mapper.Map<TournamentForUpdateDto>(existingTou));
        }

        // POST: api/Tournaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tournament.Core.Entities.Tournament>> PostTournament(TournamentDto tournament)
        {
            var tou = mapper.Map<Tournament.Core.Entities.Tournament>(tournament);
            _uoW.TournamentRepository.Add(tou);

            await _uoW.CompleteAsync();

            var touToReturn = mapper.Map<TournamentDto>(tou);

            return CreatedAtAction("GetTournament", new { id = tou.Id }, touToReturn);
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(Guid id)
        {
            var tournament = await _uoW.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            _uoW.TournamentRepository.Remove(tournament);

            await _uoW.CompleteAsync();

            return NoContent();
        }

        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult> PatchTournament(
            Guid tournamentId,
            JsonPatchDocument<TournamentForUpdateDto> patchDoc)
        {
            var touToPatch = await _uoW.TournamentRepository.GetAsync(tournamentId);

            if (touToPatch is null) return NotFound();

            var dto = mapper.Map<TournamentForUpdateDto>(touToPatch);

            patchDoc.ApplyTo(dto, ModelState);

            await TryUpdateModelAsync(dto);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            mapper.Map(dto, touToPatch);
            _uoW.TournamentRepository.Update(touToPatch);
            await _uoW.CompleteAsync();

            return NoContent();
        }

    }
}
