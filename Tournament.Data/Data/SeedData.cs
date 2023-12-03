using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Tournament.Data.Data
{
    public class SeedData
    {
        private static TournamentApiContext db = null!;
        public static async Task InitAsync(TournamentApiContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));

            if (await db.Tournament.AnyAsync()) return;

            var Tournaments = GenerateTournaments(5);
            await db.AddRangeAsync(Tournaments);
            await db.SaveChangesAsync();
        }

        private static IEnumerable<Tournament.Core.Entities.Tournament> GenerateTournaments(int nrOfTournaments)
        {
            var faker = new Faker<Tournament.Core.Entities.Tournament>("sv").Rules((f, c) =>
            {
                c.Title = f.Company.CatchPhrase();
                c.StartDate = f.Date.Between(DateTime.Now, DateTime.Now.AddMonths(8));
                c.Games = GenerateGames(f.Random.Int(min: 2, max: 10));
            });

            return faker.Generate(nrOfTournaments);
        }

        private static ICollection<Game> GenerateGames(int nrOfGame)
        {
            var faker = new Faker<Game>("sv").Rules((f, e) =>
            {
                e.Title = f.Company.CatchPhrase();
                e.Time = f.Date.Between(DateTime.Now, DateTime.Now.AddMonths(8));
            });

            return faker.Generate(nrOfGame);
        }
    }
}
