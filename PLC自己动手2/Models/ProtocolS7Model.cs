using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC自己动手2.Models
{
    public class ProtocolS7Model
    {
        public string IP { get; set; }
        public int Port { get; set; } = 102;

        public int Rock { get; set; } = 0;

        public int Slot { get; set; } = 1;
    }
}
