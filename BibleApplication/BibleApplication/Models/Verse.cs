using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleApplication.Models
{
    public class Verse
    {
        public VerseDetails prev { get; set; }
        public VerseDetails curr { get; set; }
        public VerseDetails next { get; set; }
    }
}
