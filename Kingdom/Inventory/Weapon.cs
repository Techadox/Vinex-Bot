using System;
using System.Collections.Generic;
using System.Text;

namespace Vinex_Bot.Kingdom.Inventory
{
    public class Weapon
    {
        string name;
        int damage;
        int durability;
        int cost;

        public string Name { get => name; set => name = value; }
        public int Damage { get => damage; set => damage = value; }
        public int Durability { get => durability; set => durability = value; }
        public int Cost { get => cost; set => cost = value; }
    }
}
