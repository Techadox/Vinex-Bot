using System;
using System.Collections.Generic;
using System.Text;

namespace Vinex_Bot.Kingdom.Inventory
{
    public class Armor
    {
        string name;
        int defense;
        int durability;
        int cost;

        public string Name { get => name; set => name = value; }
        public int Defense { get => defense; set => defense = value; }
        public int Durability { get => durability; set => durability = value; }
        public int Cost { get => cost; set => cost = value; }
    }
}
