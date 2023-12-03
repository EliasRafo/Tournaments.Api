using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public record GameDto
    {
        public Guid Id { get; init; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public Guid TournamentId { get; set; }
    }
        
    public abstract record GameForManipulationDto()
    {
        [Required]
        [MaxLength(30)]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public Guid TournamentId { get; set; }
    }

    public record GameForUpdateDto() : GameForManipulationDto
    {
        public Guid GameId { get; set; }
    }
}
