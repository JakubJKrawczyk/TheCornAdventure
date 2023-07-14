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
        public int Maxamount;

        public Ammo(int type, int amount, int maxamount)
        {
            this.type = type;
            this.amount = amount;
            this.Maxamount = maxamount;
        }
    }
}
