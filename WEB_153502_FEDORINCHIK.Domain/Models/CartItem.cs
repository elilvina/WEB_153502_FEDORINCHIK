using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153502_FEDORINCHIK.Domain.Entities;

namespace WEB_153502_FEDORINCHIK.Domain.Models
{
    public class CartItem
    {
        public Game Game { get; set; } = null!;
        public int Count { get; set; }
    }
}
