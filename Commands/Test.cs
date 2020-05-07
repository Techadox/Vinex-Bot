using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Vinex_Bot.CommandExtensions.EmbedShortcut;
using System.IO;
using System.Runtime.InteropServices;

namespace Vinex_Bot.Commands
{
    public class Test : BaseCommandModule
    {
        //A way to test if the bot is online
        [Command("هلا")]
        [Aliases("شخبار")]
        [Description("يكتب لك \"كيف الحال\"")]
        public async Task SayHi(CommandContext ctx)
        {
            var ms = await ctx.Channel.SendMessageAsync("كيف الحال").ConfigureAwait(false);
        }

        [Command("هلا")]
        [Description("يكتب لك \"كيف الحال\"")]
        public async Task SayHi(CommandContext ctx, [RemainingText] string x)
        {
            string word;
            word = (x == "الخال") ? "ويش الولد" : "امك";
            await ctx.Channel.SendMessageAsync(word).ConfigureAwait(false);
        }

        [Command("screenshot")]
        [Description("Work in progress")]
        public async Task Screenshot(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("This is a work in progress. Tell Nirvana-chan to add this command to me :)");
        }

        //This code is basically how to get inventory of someone outside the player class

        //var weaponEmbed = new DiscordEmbedBuilder
        //{
        //    Color = DiscordColor.Aquamarine,
        //    Title = "Weapons"
        //};

        //var armorEmbed = new DiscordEmbedBuilder
        //{
        //    Color = DiscordColor.Aquamarine,
        //    Title = "Armor"
        //};

        //foreach (var player in players)
        //{
        //    if (ctx.User == player.user)
        //    {
        //        foreach (var weapon in player.WeaponInv.Weapons)
        //        {
        //            weaponEmbed.AddField(weapon.Name,
        //                "Damage: " + weapon.Damage.ToString() + "\n"
        //              + "Durability: " + weapon.Durability.ToString() + "\n"
        //              + "Damage: " + weapon.Damage.ToString() + "\n", true);
        //        }
        //        foreach (var armor in player.ArmorInv.Armors)
        //        {
        //            armorEmbed.AddField(armor.Name,
        //               "Defense: " + armor.Defense.ToString() + "\n"
        //             + "Durability: " + armor.Durability.ToString() + "\n", true);
        //        }

        //        List<Page> pages = new List<Page>();
        //        Page weaponPage = new Page("", weaponEmbed);
        //        Page armorPage = new Page("", armorEmbed);
        //        pages.Add(weaponPage);
        //        pages.Add(armorPage);
        //        PaginationEmojis emojis = new PaginationEmojis();

        //        await ctx.Channel.SendPaginatedMessageAsync(ctx.User, pages.ToArray(), emojis);
        //        break;
        //    }
        //}
    }
}
            