using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_modellezes.Entities
{
    public class DeathProbability
    {
        public Gender Gender { get; set; }
        public int BirthYear { get; set; }
        public double Probability { get; set; }
    }
}
