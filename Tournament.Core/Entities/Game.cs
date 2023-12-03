using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Entities
{
    public class Game
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the {0} is {1} characters.")]
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public Guid TournamentId { get; set; }
        public Tournament Tournament { get; set; }
    }
}
