using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_153502_FEDORINCHIK.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public GameGenre? Genre { get; set; }
        public int GenreId { get; set; }
        public decimal Price { get; set; }
        public string? Path { get; set; }
    }
}
