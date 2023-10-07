using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_utkast_SKALA_GENERATOR
{
    public class Tuning
    {
        public string Name { get; set; }
        public MusicNotes[] Notes { get; set; } = new MusicNotes[6];
    }
}
