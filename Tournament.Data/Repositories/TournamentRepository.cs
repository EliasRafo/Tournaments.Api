using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentApiContext _context;
        public TournamentRepository(TournamentApiContext context)
        {
            _context = context;
        }
        public async void Add(Core.Entities.Tournament tournament)
        {
            _context.Tournament.Add(tournament);
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            return await _context.Tournament.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Core.Entities.Tournament>> GetAllAsync(bool includeGames = false, bool sorting = false)
        {
            var tournament = includeGames ? await _context.Tournament.Include(c => c.Games).ToListAsync()
                                        : await _context.Tournament.ToListAsync();

            return sorting ? tournament.OrderBy(t => t.Title) : tournament;
        }

        public async Task<Core.Entities.Tournament> GetAsync(Guid id)
        {
            var tournament = await _context.Tournament.FindAsync(id);
                        
            return tournament;
        }

        public void Remove(Core.Entities.Tournament tournament)
        {
            _context.Tournament.Remove(tournament);
        }

        public async void Update(Core.Entities.Tournament tournament)
        {
            _context.Tournament.Update(tournament);
        }
    }
}
