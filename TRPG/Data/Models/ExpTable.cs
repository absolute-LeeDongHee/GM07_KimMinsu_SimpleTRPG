using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Data.Models
{
    public class ExpTable
    {
        public static int GetExpForLevel(int level)
        {
            return (int)(10 * Math.Pow(level, 1.5));
        }
    }
}
