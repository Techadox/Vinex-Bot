using DSharpPlus.Entities;
using static Vinex_Bot.Kingdom.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Vinex_Bot.Kingdom.Inventory;
using DSharpPlus.Interactivity;

namespace Vinex_Bot.Kingdom
{
    public class Player
    {
        public enum Element { Fire, Water, Earth, Wind };
        DiscordUser user;
        int level;
        int exp;
        int hp;
        int atk;
        int def;
        int stamina;
        int heal;
        double money;
        Weapon weapon;


        public Element element { get; }
        public DiscordUser User { get => user; private set => user = value; }
        public int Level { get => this.level; private set => this.level = value; }
        public int Exp { get => exp; private set => exp = value; }
        public int Hp { get => hp; private set => hp = value; }
        public int Atk { get => atk; private set => atk = value; }
        public int Def { get => def; private set => def = value; }
        public int Stamina { get => stamina; private set => stamina = value; }
        public int Heal { get => heal; private set => heal = value; }
        public double Money { get => money; private set => money = value; }
        public Weapon MainWeapon { get => weapon; private set => weapon = value; }
        public List<Weapon> weaponInv { get; }
        public List<Armor> armorInv { get; }

        public Player(DiscordUser _user, Element _element)
        {
            User = _user;

            weaponInv = new List<Weapon>();
            armorInv = new List<Armor>();

            MainWeapon = new Weapon();

            Money = 500;
            level = 1;

            if (_element == Element.Earth)
            {
                Hp = 65;
                Atk = 95;
                Def = 55;
                Stamina = 40;
                Heal = 0;
                element = Element.Earth;
            }
            else if (_element == Element.Fire)
            {
                Hp = 80;
                Atk = 120;
                Def = 70;
                Stamina = 70;
                Heal = 0;
                element = Element.Fire;

            }
            else if (_element == Element.Water)
            {
                Hp = 80;
                Atk = 65;
                Def = 80;
                Stamina = 80;
                Heal = 100;
                element = Element.Water;
            }
            else if (_element == Element.Wind)
            {
                Hp = 80;
                Atk = 95;
                Def = 55;
                Stamina = 40;
                Heal = 0;
                element = Element.Wind;
            }
        }

        public DiscordEmbedBuilder GetStats()
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = this.User.Username,
                ThumbnailUrl = this.User.AvatarUrl,
                Color = DiscordColor.Aquamarine
            };

            embed.AddField(
            "Level: " + this.level.ToString(),

            "Element: " + this.element + " \n " +
            "HP: " + this.Hp + " \n " +
            "Attack: " + this.Atk + " \n " +
            "Defense: " + this.Def + " \n " +
            "Stamina: " + this.Stamina + " \n " +
            "Healing: " + this.Heal,
            true);

            embed.AddField(
            "Inventory:",

            "Money: " + this.Money + "𝕍");
            return embed;
        }

        public DiscordEmbedBuilder GiveMoneyToPlayer(DiscordUser _user, int _money)
        {
            DiscordEmbedBuilder embed;

            embed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Aquamarine,
                Description = $"Successfully given {_user.Username} {_money}𝕍"
            };
            this.Money -= _money;
            foreach (var player in Commands.players)
            {
                if (player.User == _user)
                    player.Money += _money;
            }

            return embed;
        }

        public DiscordEmbedBuilder GiveMoney(int _money)
        {
            DiscordEmbedBuilder embed;

            embed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Aquamarine,
                Description = $"Successfully given {this.user.Username} {_money}𝕍"
            };
            this.Money += _money;

            return embed;
        }

        public void BuyItem(Weapon weapon)
        {
            weaponInv.Add(weapon);
            this.Money -= weapon.Cost;
        }

        public void BuyItem(Armor armor)
        {
            armorInv.Add(armor);
            this.Money -= armor.Cost;
        }

        public DiscordEmbedBuilder ShowWeapons()
        {
            //embed to store both weapon and armor inventory items
            var weaponEmbed = new DiscordEmbedBuilder
            {
                Title = "Weapons",
                Color = DiscordColor.Aquamarine
            };

            if (weaponInv.Count == 0)
            {
                weaponEmbed.AddField("Empty", "Once you obtain weapons, it will show up here!");
            }
            else
            {
                weaponEmbed.AddField("Main Weapon",
                         "Name: " + MainWeapon.Name + "\n"
                         + "Damage: " + MainWeapon.Damage.ToString() + "\n"
                         + "Durability: " + MainWeapon.Durability.ToString() + "\n"
                         + "Damage: " + MainWeapon.Damage.ToString() + "\n", false);

                foreach (var weapon in weaponInv)
                {
                    if (weapon == MainWeapon)
                        continue;

                    else
                        weaponEmbed.AddField(weapon.Name,
                        "Damage: " + weapon.Damage.ToString() + "\n"
                        + "Durability: " + weapon.Durability.ToString() + "\n"
                        + "Damage: " + weapon.Damage.ToString() + "\n", true);
                }
            }

            return weaponEmbed;
        }
        public List<Page> ShowInventory()
        {
            //embed to store both weapon and armor inventory items
            var weaponEmbed = new DiscordEmbedBuilder
            {
                Title = "Weapons",
                Color = DiscordColor.Aquamarine
            };
            var armorEmbed = new DiscordEmbedBuilder
            {
                Title = "Armors",
                Color = DiscordColor.Aquamarine
            };

            //if there's nothing in the inventory, say empty. otherwise, loop through them to show them
            if (armorInv.Count == 0)
            {
                armorEmbed.AddField("Empty", "Once you obtain armor, it will show up here!");
            }
            else
            {
                //looping through weapon inventory
                foreach (var armor in armorInv)
                {
                    armorEmbed.AddField(armor.Name,
                    "Defense: " + armor.Defense.ToString() + "\n"
                    + "Durability: " + armor.Durability.ToString() + "\n", true);
                }
            }

            if (weaponInv.Count == 0)
            {
                weaponEmbed.AddField("Empty", "Once you obtain weapons, it will show up here!");
            }
            else
            {
                weaponEmbed.AddField("Main Weapon",
                         "Name: " + MainWeapon.Name + "\n"
                         + "Damage: " + MainWeapon.Damage.ToString() + "\n"
                         + "Durability: " + MainWeapon.Durability.ToString() + "\n"
                         + "Damage: " + MainWeapon.Damage.ToString() + "\n", false);

                foreach (var weapon in weaponInv)
                {
                    if (weapon == MainWeapon)
                        continue;

                    else
                        weaponEmbed.AddField(weapon.Name,
                        "Damage: " + weapon.Damage.ToString() + "\n"
                        + "Durability: " + weapon.Durability.ToString() + "\n"
                        + "Damage: " + weapon.Damage.ToString() + "\n", true);
                }
            }

            //making it paginated
            List<Page> pages = new List<Page>();
            Page weaponPage = new Page("", weaponEmbed);
            Page armorPage = new Page("", armorEmbed);
            pages.Add(weaponPage);
            pages.Add(armorPage);

            return pages;
        }

        public bool SetWeapon(string weaponName)
        {
            bool hasWeapon = false;
            foreach (var weapon in weaponInv)
            {
                if (weapon.Name.ToLower() == weaponName.ToLower())
                {
                    this.MainWeapon = weapon;
                    hasWeapon = true;
                }
            }
            return hasWeapon;
        }

        public bool SetWeapon(Weapon _weapon)
        {
            bool hasWeapon = false;
            foreach (var weapon in weaponInv)
            {
                if (weapon == _weapon)
                {
                    this.MainWeapon = weapon;
                    hasWeapon = true;
                }
            }
            return hasWeapon;
        }
    }  
}


