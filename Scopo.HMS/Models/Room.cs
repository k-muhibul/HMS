using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string Name { get; set; }
        public decimal Rent { get; set; }

        public bool IsOccupied { get; set; }
    }
}
