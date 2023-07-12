using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    internal class Ammo
    {
        public int type;
        public int amount;

        public Ammo(int type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }
}
