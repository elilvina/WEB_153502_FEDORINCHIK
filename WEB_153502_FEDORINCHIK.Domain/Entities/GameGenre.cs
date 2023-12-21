using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_153502_FEDORINCHIK.Domain.Entities
{
    public class GameGenre
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string NormalizedName { get; set; } = null!;
    }
}
