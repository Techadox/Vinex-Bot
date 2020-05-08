using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vinex_Bot.Kingdom.Inventory;
using static Vinex_Bot.CommandExtensions.EmbedShortcut;
using static Vinex_Bot.Kingdom.Player;

namespace Vinex_Bot.Kingdom
{
    class Commands : BaseCommandModule
    {
        public static List<Player> players = new List<Player>(); 

        #region Join Element
        [Command("joinelement")]
        [Description("Work in progress")]
        [Aliases("joine", "joinclass", "joinc")]
        public async Task JoinClass(CommandContext ctx)
        {
            bool hasAccount = false;
            
            var interactivity = ctx.Client.GetInteractivity();
            DiscordMessage ask;

            //emojis for the user to choose from
            var fire = DiscordEmoji.FromName(ctx.Client, ":fire:");
            var water = DiscordEmoji.FromName(ctx.Client, ":ocean:");
            var earth = DiscordEmoji.FromName(ctx.Client, ":earth_americas:");
            var wind = DiscordEmoji.FromName(ctx.Client, ":wind_blowing_face:");

            //checks if player already exists in the list 
            foreach (var player in players)
            {
                if (player.User == ctx.User)
                    hasAccount = true;

                else
                    hasAccount = false;
            }

            if (hasAccount)
            {
                var thumbsUp = DiscordEmoji.FromName(ctx.Client, ":thumbsup:");
                var thumbsDown = DiscordEmoji.FromName(ctx.Client, ":thumbsdown:");

                var msg = await ctx.Channel.SendMessageAsync("You already have an account. Do you wish to reset everything?");
                await msg.CreateReactionAsync(thumbsUp);
                await msg.CreateReactionAsync(thumbsDown);
                var emoji = interactivity.WaitForReactionAsync(x => x.User == ctx.User);

                if (emoji.Result.Result.Emoji == thumbsUp)
                    foreach (var player in players)
                        if (player.User == ctx.User)
                        {
                            players.Remove(player);
                            await ctx.Channel.SendMessageAsync("Your account was successfully reset.");
                        }

                        else
                            await ctx.Channel.SendMessageAsync("Your account was not reset.");
            }   
            else
            {
                if (ctx.User.Id == 208911047854260226)
                {
                    Player player = new Player(ctx.User, Player.Element.Fire);
                    players.Add(player);
                    player.GiveMoney(9999999 - 500);
                    await ctx.Channel.SendMessageAsync("Welcome to the Fire Clan! Here are your stats");
                    await ctx.Channel.SendMessageAsync(embed: player.GetStats());
                }
                else
                {
                    ask = await ctx.Channel.SendMessageAsync(embed: Vembed("What would you like to join? Fire, Water, Wind, Earth?"));
                    await ask.CreateReactionAsync(fire);
                    await ask.CreateReactionAsync(water);
                    await ask.CreateReactionAsync(earth);
                    await ask.CreateReactionAsync(wind);

                    //waiting for the user's reaction
                    var reaction = await interactivity.WaitForReactionAsync(x => x.User == ctx.User && x.Channel == ctx.Channel);

                    if (reaction.Result.Emoji == fire)
                    {
                        Player player = new Player(ctx.User, Player.Element.Fire);
                        players.Add(player);
                        await ctx.Channel.SendMessageAsync("Welcome to the Fire Clan! Here are your stats");
                        await ctx.Channel.SendMessageAsync(embed: player.GetStats());
                    }
                    else if (reaction.Result.Emoji == water)
                    {
                        Player player = new Player(ctx.User, Player.Element.Water);
                        players.Add(player);
                        await ctx.Channel.SendMessageAsync("Welcome to the Water Clan! Here are your stats");
                        await ctx.Channel.SendMessageAsync(embed: player.GetStats());
                    }
                    else if (reaction.Result.Emoji == earth)
                    {
                        Player player = new Player(ctx.User, Player.Element.Earth);
                        players.Add(player);
                        await ctx.Channel.SendMessageAsync("Welcome to the Earth Clan! Here are your stats");
                        await ctx.Channel.SendMessageAsync(embed: player.GetStats());
                    }
                    else if (reaction.Result.Emoji == wind)
                    {
                        Player player = new Player(ctx.User, Player.Element.Wind);
                        players.Add(player);
                        await ctx.Channel.SendMessageAsync("Welcome to the Wind Clan! Here are your stats");
                        await ctx.Channel.SendMessageAsync(embed: player.GetStats());
                    }
                    else
                    {
                        await ctx.Channel.SendMessageAsync("Wrong emoji... What the hell, dude?");
                    }
                    await ask.DeleteAsync();
                }
            }   
        }
        #endregion

        #region Show Stats
        [Command("showstats")]
        [Description("Work in progress")]
        [Aliases("stats")]
        public async Task ShowStats(CommandContext ctx)
        {
            if (players.Count == 0)
                await ctx.Channel.SendMessageAsync("You haven't registered yet. Use the v.`joinclass` to choose an element!");
            else
            {
                foreach (var player in players)
                {
                    if (ctx.User == player.User)
                    {
                        var embed = player.GetStats();
                        await ctx.Channel.SendMessageAsync(embed: embed);
                    }
                }
            }            
        }

        [Command("showstats")]
        public async Task ShowStats(CommandContext ctx, DiscordUser _user)
        {
            if (players.Count == 0)
                await ctx.Channel.SendMessageAsync("You haven't registered yet. Use the v.`joinclass` to choose an element!");
            else
            {
                foreach (var player in players)
                {
                    if (_user == player.User)
                    {
                        var embed = player.GetStats();
                        await ctx.Channel.SendMessageAsync(embed: embed);
                    }
                }
            }
        }

        [Command("money")]
        [Description("Work in progress")]
        [Aliases("showmoney", "cash")]
        public async Task ShowMoney(CommandContext ctx)
        {
            foreach (var player in players)
            {
                if (player.User == ctx.User)
                {
                    await ctx.Channel.SendMessageAsync(embed: Vembed("Money:", player.Money + "𝕍"));
                    break;
                }
            }
        }
        #endregion

        #region Show Inventory

        [Command("showweapons")]
        [Description("Work in progress")]
        [Aliases("showweapon", "weapons")]
        public async Task ShowWeapons(CommandContext ctx)
        {
            foreach (var player in players)
            {
                if (player.User == ctx.User)
                {
                    var embed = player.ShowWeapons();

                    await ctx.Channel.SendMessageAsync(embed: embed);
                    break;
                }
            }
        }

        [Command("showinventory")]
        [Description("Work in progress")]
        [Aliases("showinv", "inventory", "inv", "bag")]
        public async Task ShowInventory(CommandContext ctx)
        {
            foreach (var player in players)
            {
                if (player.User == ctx.User)
                {
                    var pages = player.ShowInventory();
                    PaginationEmojis emojis = new PaginationEmojis();

                    await ctx.Channel.SendPaginatedMessageAsync(ctx.User, pages.ToArray(), emojis);
                    break;
                }
            }
        }

        #endregion

        #region Give Money
        [Command("givemoney")]
        [Description("Work in progress")]
        [Aliases("give")]
        public async Task GiveMoney(CommandContext ctx, DiscordUser _user, int _money)
        {
            bool showError = true;

            if (players.Count == 0)
                await ctx.Channel.SendMessageAsync("You haven't registered yet. Use the v.`joinclass` to choose an element!");

            foreach (var player in players)
            {
                if (ctx.User == player.User)
                {
                    showError = false;
                    var embed = player.GiveMoneyToPlayer(_user, _money);
                    await ctx.Channel.SendMessageAsync(embed: embed);
                }
            }
            if (showError)
                await ctx.Channel.SendMessageAsync("You haven't registered yet. Use the v.`joinclass` to choose an element!");
        }
        #endregion
        
        #region Shop
        [Command("shop")]
        [Description("Work in progress")]
        [Aliases("market", "buy")]
        public async Task Shop(CommandContext ctx)
        {
            //first one reads all text from json, second parses and turns it into a C# list class called WeaponCollection, which
            //will later be used inside a foreach to access each member of the list
            var weaponJson = File.ReadAllText(@"Z:\Self Study\Programming\C#\Discord Bots\Vinex Bot\Vinex Bot\Docs\Weapon.json");
            var weapons = JsonConvert.DeserializeObject<WeaponCollection>(weaponJson);

            var armorJson = File.ReadAllText(@"Z:\Self Study\Programming\C#\Discord Bots\Vinex Bot\Vinex Bot\Docs\Armor.json");
            var armors = JsonConvert.DeserializeObject<ArmorCollection>(armorJson);

            //embed for GUI
            var mainEmbed = new DiscordEmbedBuilder
            {
                Title = "Shop",
                Color = DiscordColor.Aquamarine,
                Description = "Please make this so that a user says v.buy weapons or v.buy items and this embed just shows the categories " +
                "that the user can choose. Add choices for filtering (v.buy cost>500) oh also," +
                " try adding a description to each item in the future. That'll make for some good lore!"
            };
            await ctx.Channel.SendMessageAsync(embed: mainEmbed);

            var weaponsEmbed = new DiscordEmbedBuilder
            {
                Title = "Weapons",
                Color = DiscordColor.Aquamarine
            };
            foreach (var weapon in weapons.Weapons)
            {
                weaponsEmbed.AddField(weapon.Name,
                    "Damage: " + weapon.Damage + "\n" +
                    "Durability: " + weapon.Durability + "\n" +
                    "Cost: " + weapon.Cost + "𝕍", true);
            }

            var armorsEmbed = new DiscordEmbedBuilder
            {
                Title = "Armors",
                Color = DiscordColor.Aquamarine
            };
            foreach (var armor in armors.Armors)
            {
                armorsEmbed.AddField(armor.Name, 
                    "Defense: " + armor.Defense + "\n" +
                    "Durability: " + armor.Durability + "\n" +
                    "Cost: " + armor.Cost + "𝕍", true);
            }

            //This is basically how paginated messages work and i still don't know how they work
            PaginationEmojis emojis = new PaginationEmojis();
            List<Page> pages = new List<Page>();
            Page weaponsPage = new Page("", weaponsEmbed);
            Page armorsPage = new Page("", armorsEmbed);
            pages.Add(weaponsPage);
            pages.Add(armorsPage);


            await ctx.Channel.SendPaginatedMessageAsync(ctx.User, pages.ToArray(), emojis);
        }

        [Command("shop")]
        public async Task Shop(CommandContext ctx, [RemainingText] string item)
        {
            bool wasFound = false;
            string name = string.Empty;
            int cost = 0;

            string w = File.ReadAllText(@"Z:\Self Study\Programming\C#\Discord Bots\Vinex Bot\Vinex Bot\Docs\Weapon.json");
            var weapons = JsonConvert.DeserializeObject<WeaponCollection>(w);

            string a = File.ReadAllText(@"Z:\Self Study\Programming\C#\Discord Bots\Vinex Bot\Vinex Bot\Docs\Armor.json");
            var armors = JsonConvert.DeserializeObject<ArmorCollection>(a);

            foreach (var weapon in weapons.Weapons)
            {
                if (weapon.Name.ToLower() == item.ToLower())
                {
                    wasFound = true;
                    cost = weapon.Cost;
                    name = weapon.Name;

                    foreach (var player in players)
                    {
                        if (player.User == ctx.User)
                        {
                            if (player.Money >= weapon.Cost)
                            {
                                player.BuyItem(weapon);

                                if (player.weaponInv.Count == 1)
                                    player.SetWeapon(weapon);

                                await ctx.Channel.SendMessageAsync("Successfully bought " + name + " for " + cost + "𝕍.");
                                break;
                            }
                                
                            else
                                await ctx.Channel.SendMessageAsync($"Not enough money to buy to buy {weapon.Name}.");
                        }
                    }
                }
            }

            foreach (var armor in armors.Armors)
            {
                if (armor.Name.ToLower() == item.ToLower())
                {
                    wasFound = true;
                    cost = armor.Cost;
                    name = armor.Name;

                    foreach (var player in players)
                    {
                        if (player.User == ctx.User)
                            player.BuyItem(armor);
                    }
                    await ctx.Channel.SendMessageAsync("Successfully bought " + name + "for" + cost + "𝕍");
                    break;
                }
            }

            if (!wasFound)
                await ctx.Channel.SendMessageAsync("Sorry, we couldn't find that item.");

        }
        #endregion Shop

        #region Set Main Weapon

        [Command("setweapon")]
        [Aliases("setmainweapon")]
        [Description("Chooses the main weapon from the list of weapons from a player's inventory")]
        public async Task SetMainWeapon(CommandContext ctx)
        {
            Weapon mainWeapon = new Weapon();
            var interactivity = ctx.Client.GetInteractivity();
            var embed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Aquamarine
            };

            foreach (var player in players)
            {
                foreach (var weapon in player.weaponInv)
                {
                    embed.AddField(weapon.Name,
                        "Damage: " + weapon.Damage + "\n" +
                    "Durability: " + weapon.Durability);
                    mainWeapon = weapon;
                }
                await ctx.Channel.SendMessageAsync("Type the name of the weapon you would like to choose");
                await ctx.Channel.SendMessageAsync(embed: embed);

                var weaponName = interactivity.WaitForMessageAsync(x => x.Author == ctx.User);

                bool wasSuccess = player.SetWeapon(weaponName.Result.Result.Content);
                if (wasSuccess)
                    await ctx.Channel.SendMessageAsync($"Successfully set Main Weapon to {mainWeapon.Name}");
                else
                    await ctx.Channel.SendMessageAsync($"You don't have {weaponName} in your inventory.");
                break;
            }
        }

        [Command("setweapon")]
        public async Task SetMainWeapon(CommandContext ctx, [RemainingText] string weaponName)
        {
            bool wasSuccess = false;

            var embed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Aquamarine
            };

            foreach (var player in players)
            {
                foreach (var weapon in player.weaponInv)
                {
                    if (weapon.Name.ToLower() == weaponName.ToLower())
                    {
                        wasSuccess = player.SetWeapon(weapon);

                        await ctx.Channel.SendMessageAsync($"Successfully set main weapon to {weapon.Name}");
                        break;
                    }
                }
                if (wasSuccess)
                    break;
            }
        }
        #endregion

        #region Enable Coin Drop - Plz change to item drop too

        bool wasDropped = false;
        [Command("coindrop")]
        public async Task CoinDrop(CommandContext ctx)
        {
            if (wasDropped)
            {
                if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
                {
                    wasDropped = false;
                    await ctx.Channel.SendMessageAsync("Coin drops disabled.");
                }
                else
                    await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
            }

            else
                await ctx.Channel.SendMessageAsync("Please enter the maximum time in minutes\n Example: `v.coindrop 5`");
               
        }

        [Command("coindrop")]
        [Description("Enables random coin drops by a random time interval")]
        [Aliases("dropcoins")]
        public async Task CoinDrop(CommandContext ctx, int time)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                if (wasDropped)
                {
                    wasDropped = false;
                    await ctx.Channel.SendMessageAsync("Coin drops disabled.");
                }
                else
                {
                    var interactivity = ctx.Client.GetInteractivity();
                    Random r = new Random();
                    var moneyBag = DiscordEmoji.FromName(ctx.Client, ":moneybag:");

                    do
                    {
                        int wait = r.Next(time);
                        int money = r.Next(50, 100);
                        var msg = await ctx.Channel.SendMessageAsync(":moneybag: <- A bag has dropped! React to this message to claim it");
                        await msg.CreateReactionAsync(moneyBag);
                        var reaction = await interactivity.WaitForReactionAsync(x => x.Message == msg && x.User.Id != 695192284500852787);

                        if (reaction.Result.Emoji == moneyBag)
                        {
                            foreach (var player in players)
                            {
                                if (player.User == reaction.Result.User)
                                {
                                    player.GiveMoney(money);
                                    await ctx.Channel.SendMessageAsync($"Successfully given {money.ToString()} to {player.User.Username}!");
                                }
                            }

                            wasDropped = true;

                            await msg.DeleteAsync();
                            await Task.Delay(wait * 1000 * 60);
                        }

                    } while (wasDropped);
                }
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }

        [Command("coindrop")]
        public async Task CoinDrop(CommandContext ctx, int timeMin, int timeMax)
        {
            if (wasDropped)
            {
                wasDropped = false;
                await ctx.Channel.SendMessageAsync("Coin drops disabled.");
            }
            else
            {
                var interactivity = ctx.Client.GetInteractivity();
                Random r = new Random();
                var moneyBag = DiscordEmoji.FromName(ctx.Client, ":moneybag:");

                do
                {
                    int wait = r.Next(timeMin, timeMax);
                    int money = r.Next(50, 100);
                    var msg = await ctx.Channel.SendMessageAsync(":moneybag: <- A bag has dropped! React to this message to claim it");
                    await msg.CreateReactionAsync(moneyBag);
                    var reaction = await interactivity.WaitForReactionAsync(x => x.Message == msg && x.User.Id != 695192284500852787);

                    if (reaction.Result.Emoji == moneyBag)
                    {
                        foreach (var player in players)
                        {
                            if (player.User == reaction.Result.User)
                            {
                                player.GiveMoney(money);
                                await ctx.Channel.SendMessageAsync($"Successfully given {money.ToString()} to {player.User.Username}!");
                            }
                        }

                        wasDropped = true;

                        await msg.DeleteAsync();
                        await Task.Delay(wait * 1000 * 60);
                    }

                } while (wasDropped);
            }
        }
        #endregion

        #region Money Events - Plz change to item events too (implement plz)

        bool isDone = false;
        [Command("moneyevent")]
        [Aliases("coinevent")]
        [Description("Makes an event for a specific time where the user can get free money")]
        public async Task MoneyEvent(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync(embed: Vembed("Usage",
                "`v.moneyevent (money)`\nOptional: \n`v.moneyevent (channel) (money)`\n`v.moneyevent (channel) (money) (time)`"));
        }

        [Command("moneyevent")]
        public async Task MoneyEvent(CommandContext ctx, int money, [RemainingText] string text)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                if (isDone)
                {
                    await ctx.Channel.SendMessageAsync("Event disabled.");
                    isDone = false;
                }

                else
                {
                    int playerCount = 0;
                    var interactivity = ctx.Client.GetInteractivity();
                    var bag = DiscordEmoji.FromName(ctx.Client, ":moneybag:");
                    var embed = new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.Aquamarine,
                        Title = "Event has started!",
                        Description = text
                    };
                    var eventMsg = await ctx.Channel.SendMessageAsync(embed: embed);
                    await eventMsg.CreateReactionAsync(bag);

                    //await Task.Delay(24 * 60 * 60 * 1000);
                    while (!isDone && playerCount < players.Count)
                    {
                        var reactions = await interactivity.WaitForReactionAsync(x => x.Message == eventMsg);
                        var user = reactions.Result.User;

                        foreach (var player in players)
                        {
                            if (player.User == user)
                            {
                                playerCount++;
                                var msg = await ctx.Channel.SendMessageAsync(embed: player.GiveMoney(money));
                                await Task.Delay(5000);
                                await msg.DeleteAsync();
                                break;
                            }
                        }
                    }
                    await ctx.Channel.SendMessageAsync("Event has come to an end! Thanks for joining!");
                }
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");

        }

        [Command("moneyevent")]
        public async Task MoneyEvent(CommandContext ctx, DiscordChannel channel, int money, [RemainingText] string text)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                await ctx.Channel.SendMessageAsync("");
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }

        [Command("moneyevent")]
        public async Task MoneyEvent(CommandContext ctx, DiscordChannel channel, int money, int time, [RemainingText] string text)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                await ctx.Channel.SendMessageAsync("");
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }

        #endregion

        #region 1v1 IMPLEMENT

        [Command("battle")]
        [Aliases("combat", "fight")]
        [Description("Engages in a fight with another player")]
        public async Task Battle(CommandContext ctx, DiscordUser user)
        {
            Player player1;
            Player player2;

            //finds the player values for both players
            foreach (var player in players)
            {
                if (player.User == ctx.User)
                    player1 = player;

                else if (player.User == user)
                    player2 = player;
            }

        }

        #endregion
    }
}
