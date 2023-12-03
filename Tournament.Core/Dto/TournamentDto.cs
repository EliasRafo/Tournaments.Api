using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Dto
{
    public record TournamentDto
    {
        public Guid Id { get; init; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate => StartDate.AddMonths(3);

        // add list of game
        public IEnumerable<GameDto> Games { get; set; }
    }

    public abstract record TournamentForManipulationDto()
    {
        [Required]
        [MaxLength(30)]
        public string? Title { get; init; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate => StartDate.AddMonths(3);
    }

    public record TournamentForUpdateDto() : TournamentForManipulationDto
    {
        public Guid TournamentId { get; set; }
    }
}
