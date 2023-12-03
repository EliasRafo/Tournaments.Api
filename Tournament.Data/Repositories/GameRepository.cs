using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly TournamentApiContext _context;
        public GameRepository(TournamentApiContext context)
        {
            _context = context;
        }
        public async void Add(Game game)
        {
            _context.Game.Add(game);
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            return await _context.Game.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Game>> GetAllAsync(bool sorting = false)
        {
             return sorting ? await _context.Game.OrderBy(g => g.Title).ToListAsync()
                                    : await _context.Game.ToListAsync();
        }

        public async Task<Game> GetAsync(Guid id)
        {
            return await _context.Game.FindAsync(id);
        }

        public async Task<Game> GetByTitleAsync(string title)
        {
            return await _context.Game.Where(g => g.Title == title).FirstOrDefaultAsync();
        }

        public async void Remove(Game game)
        {
            _context.Game.Remove(game);
        }

        public async void Update(Game game)
        {
            _context.Game.Update(game);
        }
    }
}
