using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinex_Bot.Kingdom;
using static Vinex_Bot.CommandExtensions.EmbedShortcut;

namespace Vinex_Bot.Commands
{
    public class Interactive : BaseCommandModule
    {
        #region Emotes
        [Command("monkaW")]
        [Description("types \"monkaW\"")]
        public async Task monkaW(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("<:monkaW:683291628605276160>").ConfigureAwait(false);
        }
        #endregion

        #region Set Game
        [Command("setgame")]
        [Aliases("sg", "setg")]
        [Description("sets the game of the bot")]
        public async Task SetGame(CommandContext ctx, [RemainingText] string game)
        {
            DiscordActivity activity = new DiscordActivity(game);
            await ctx.Client.UpdateStatusAsync(activity).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(embed: 
                Vembed("Successfully changed game to " + game + ".")).ConfigureAwait(false);
        }
        #endregion Set Game

        #region Timer
        [Command("Timer")]
        [Aliases("settimer", "reminder", "remind")]
        [Description("Sets a timer at a specific time")]
        public async Task Timer(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity(); //Readies the interactivity functions

            //asking for input and parsing to usable time variables
            await ctx.Channel.SendMessageAsync("Set time in hours:");
            var hours = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel);
            int hrs = int.Parse(hours.Result.Content);
            
            await ctx.Channel.SendMessageAsync("Set time in minutes:");
            var minutes = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel);
            int mins = int.Parse(minutes.Result.Content);
            
            await ctx.Channel.SendMessageAsync("Set time in seconds");
            var seconds = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel);
            int secs = int.Parse(seconds.Result.Content);

            //asking for timer end location (DM or Mention) using reactions
            var channel = await ctx.Channel.SendMessageAsync(embed:
                Vembed("Do you want a DM or mention here?",
                       "D for DM, M for mention"));
            var d = DiscordEmoji.FromName(ctx.Client, ":regional_indicator_d:");
            var m = DiscordEmoji.FromName(ctx.Client, ":regional_indicator_m:");
            await channel.CreateReactionAsync(d);
            await channel.CreateReactionAsync(m);
            var reaction = await interactivity.WaitForReactionAsync(x => x.Message == channel && x.User == ctx.User).ConfigureAwait(false);

            //asking if the user wants an optional message to add to his timer
            string optionalMsg = string.Empty;
            var msg = await ctx.Channel.SendMessageAsync(embed: Vembed("Do you want to add a note to the timer?")).ConfigureAwait(false);
            var plusOne = DiscordEmoji.FromName(ctx.Client, ":+1:");
            var minusOne = DiscordEmoji.FromName(ctx.Client, ":-1:");
            await msg.CreateReactionAsync(plusOne);
            await msg.CreateReactionAsync(minusOne);
            var yesOrNo = interactivity.WaitForReactionAsync(x => x.Message == msg && x.User == ctx.User);
            
            if (yesOrNo.Result.Result.Emoji == plusOne)
            {
                await ctx.Channel.SendMessageAsync("Please type the note you would like to add:");
                var result = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel &&
                                                                    x.Author == ctx.User).ConfigureAwait(false);
                optionalMsg = result.Result.Content;
                await ctx.Channel.SendMessageAsync(embed:
                Vembed("Successfully set timer to " + hrs + " hours " +
                       "and " + mins + " minutes  " +
                       "and " + secs + " seconds.")).ConfigureAwait(false);
            }
            else if (yesOrNo.Result.Result.Emoji == minusOne)
                await ctx.Channel.SendMessageAsync("Successfully set timer to " + hrs + " hours " +
                           "and " + mins + " minutes  " +
                           "and " + secs + " seconds.").ConfigureAwait(false);

            //actually waits for this much time
            await Task.Delay((hrs * 60 * 60 * 1000) + (mins * 60 * 1000) + (secs * 1000));

            //checks for location of the timer ended message (+if optional message was provided)
            if (reaction.Result.Emoji == d)
            {
                DiscordChannel dm = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

                if (optionalMsg == string.Empty)
                {
                    await dm.SendMessageAsync("Timer is done, Timer is done, Timer is done!");
                }
                else
                {
                    await dm.SendMessageAsync(ctx.User.Mention);
                    await dm.SendMessageAsync(embed: Vembed(optionalMsg));
                }
            }

            else if (reaction.Result.Emoji == m)
            {
                if (optionalMsg == string.Empty)
                {
                    await ctx.Channel.SendMessageAsync("Timer is done, Timer is done, Timer is done!");
                }
                else
                {
                    await ctx.Channel.SendMessageAsync(ctx.User.Mention);
                    await ctx.Channel.SendMessageAsync(embed: Vembed(optionalMsg));
                }
            }
        }

        [Command("Timer")]
        [Description("Sets a timer at a specific time")]
        public async Task Timer(CommandContext ctx, params string[] time)
        {
            //readies the interactivity function and initiates important variables
            var interactivity = ctx.Client.GetInteractivity();
            int secs;
            int mins;
            int hours;

            //if only seconds was given
            if (time.Length == 1)
            {
                secs = int.Parse(time[0]); //parsing the string 

                //asking if the user wants a DM or a mention
                var channel = await ctx.Channel.SendMessageAsync(embed:
                    Vembed("Do you want a DM or mention here?", "D for DM, M for mention")).ConfigureAwait(false);
                var d = DiscordEmoji.FromName(ctx.Client, ":regional_indicator_d:");
                var m = DiscordEmoji.FromName(ctx.Client, ":regional_indicator_m:");
                await channel.CreateReactionAsync(d).ConfigureAwait(false);
                await channel.CreateReactionAsync(m).ConfigureAwait(false);
                var reaction = await interactivity.WaitForReactionAsync(x => x.Message == channel && x.User == ctx.User);

                //asking if the user wants a note added to the timer
                string optionalMsg = string.Empty;
                var msg = await ctx.Channel.SendMessageAsync(embed: Vembed("Do you want to add a note to the timer?")).ConfigureAwait(false);
                var plusOne = DiscordEmoji.FromName(ctx.Client, ":+1:");
                var minusOne = DiscordEmoji.FromName(ctx.Client, ":-1:");
                await msg.CreateReactionAsync(plusOne).ConfigureAwait(false);
                await msg.CreateReactionAsync(minusOne).ConfigureAwait(false);
                var yesOrNo = interactivity.WaitForReactionAsync(x => x.Message == msg && x.User == ctx.User);

                if (yesOrNo.Result.Result.Emoji == plusOne)
                {
                    await ctx.Channel.SendMessageAsync("Please type the note you would like to add:");
                    var result = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel &&
                                                                        x.Author == ctx.User).ConfigureAwait(false);
                    optionalMsg = result.Result.Content;
                    await ctx.Channel.SendMessageAsync(embed:
                    Vembed("Successfully set timer to " + secs + " seconds.")).ConfigureAwait(false);
                }
                else if (yesOrNo.Result.Result.Emoji == minusOne)
                    await ctx.Channel.SendMessageAsync(embed:
                        Vembed("Successfully set timer to " + secs + " seconds.")).ConfigureAwait(false);

                await Task.Delay(secs * 1000); //actually waits till timer ends

                //checks for location of the timer ended message (+if optional message was provided)
                if (reaction.Result.Emoji == d)
                {
                    DiscordChannel dm = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

                    if (optionalMsg == string.Empty)
                    {
                        await dm.SendMessageAsync("Timer is done, Timer is done, Timer is done!");
                    }
                    else
                    {
                        await dm.SendMessageAsync(ctx.User.Mention);
                        await dm.SendMessageAsync(embed: Vembed(optionalMsg));
                    }
                }

                else if (reaction.Result.Emoji == m)
                {
                    if (optionalMsg == string.Empty)
                    {
                        await ctx.Channel.SendMessageAsync("Timer is done, Timer is done, Timer is done!");
                    }
                    else
                    {
                        await ctx.Channel.SendMessageAsync(ctx.User.Mention);
                        await ctx.Channel.SendMessageAsync(embed: Vembed(optionalMsg));
                    }
                }
            }
            else if (time.Length == 2)
            {
                //parsing the string 
                secs = int.Parse(time[0]);
                mins = int.Parse(time[1]);

                //asking if the user wants a DM or a mention
                var channel = await ctx.Channel.SendMessageAsync(embed:
                    Vembed("Do you want a DM or mention here?", "D for DM, M for mention")).ConfigureAwait(false);
                var d = DiscordEmoji.FromName(ctx.Client, ":regional_indicator_d:");
                var m = DiscordEmoji.FromName(ctx.Client, ":regional_indicator_m:");
                await channel.CreateReactionAsync(d).ConfigureAwait(false);
                await channel.CreateReactionAsync(m).ConfigureAwait(false);
                var reaction = await interactivity.WaitForReactionAsync(x => x.Message == channel && x.User == ctx.User);

                //asking if the user wants a note added to the timer
                string optionalMsg = string.Empty;
                var msg = await ctx.Channel.SendMessageAsync(embed: Vembed("Do you want to add a note to the timer?")).ConfigureAwait(false);
                var plusOne = DiscordEmoji.FromName(ctx.Client, ":+1:"); 
                var minusOne = DiscordEmoji.FromName(ctx.Client, ":-1:");
                await msg.CreateReactionAsync(plusOne).ConfigureAwait(false);
                await msg.CreateReactionAsync(minusOne).ConfigureAwait(false);
                var yesOrNo = interactivity.WaitForReactionAsync(x => x.Message == msg && x.User == ctx.User);

                if (yesOrNo.Result.Result.Emoji == plusOne)
                {
                    await ctx.Channel.SendMessageAsync("Please type the note you would like to add:");
                    var result = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel &&
                                                                        x.Author == ctx.User).ConfigureAwait(false);
                    optionalMsg = result.Result.Content;
                    await ctx.Channel.SendMessageAsync(embed:
                    Vembed("Successfully set timer to " + mins + " minutes and " + secs + " seconds.")).ConfigureAwait(false);
                }
                else if (yesOrNo.Result.Result.Emoji == minusOne)
                    await ctx.Channel.SendMessageAsync(embed:
                    Vembed("Successfully set timer to " + mins + " minutes and " + secs + " seconds.")).ConfigureAwait(false);

                await Task.Delay((mins * 60 * 1000) + (secs * 1000)); //actually waits till timer ends

                //checks location of timer end msg
                if (reaction.Result.Emoji == d)
                {
                    DiscordChannel dm = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

                    if (optionalMsg == string.Empty)
                    {
                        await dm.SendMessageAsync("Timer is done, Timer is done, Timer is done!");
                    }
                    else
                    {
                        await dm.SendMessageAsync(ctx.User.Mention);
                        await dm.SendMessageAsync(embed: Vembed(optionalMsg));
                    }
                }

                else if (reaction.Result.Emoji == m)
                {
                    if (optionalMsg == string.Empty)
                    {
                        await ctx.Channel.SendMessageAsync("Timer is done, Timer is done, Timer is done!");
                    }
                    else
                    {
                        await ctx.Channel.SendMessageAsync(ctx.User.Mention);
                        await ctx.Channel.SendMessageAsync(embed: Vembed(optionalMsg));
                    }
                }
            }
            else if (time.Length == 3)
            {
                //tries parsing the string
                secs = int.Parse(time[0]);
                mins = int.Parse(time[1]);
                hours = int.Parse(time[2]);

                //asks where the user wants the timer to be
                var channel = await ctx.Channel.SendMessageAsync(embed:
                    Vembed("Do you want a DM or mention here?", "D for DM, M for mention")).ConfigureAwait(false);
                var d = DiscordEmoji.FromName(ctx.Client, ":regional_indicator_d:");
                var m = DiscordEmoji.FromName(ctx.Client, ":regional_indicator_m:");
                await channel.CreateReactionAsync(d).ConfigureAwait(false);
                await channel.CreateReactionAsync(m).ConfigureAwait(false);
                var reaction = await interactivity.WaitForReactionAsync(x => x.Message == channel && x.User == ctx.User);

                //checks if user wants an additional message to the timer
                string optionalMsg = string.Empty;
                var msg = await ctx.Channel.SendMessageAsync(embed: Vembed("Do you want to add a note to the timer?")).ConfigureAwait(false);
                var plusOne = DiscordEmoji.FromName(ctx.Client, ":+1:");
                var minusOne = DiscordEmoji.FromName(ctx.Client, ":-1:");
                await msg.CreateReactionAsync(plusOne).ConfigureAwait(false);
                await msg.CreateReactionAsync(minusOne).ConfigureAwait(false);
                var yesOrNo = interactivity.WaitForReactionAsync(x => x.Message == msg && x.User == ctx.User);

                if (yesOrNo.Result.Result.Emoji == plusOne)
                {
                    await ctx.Channel.SendMessageAsync("Please type the note you would like to add:");
                    var result = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel &&
                                                                        x.Author == ctx.User).ConfigureAwait(false);
                    optionalMsg = result.Result.Content;
                    await ctx.Channel.SendMessageAsync(embed:
                    Vembed("Successfully set timer to " + hours + " hours and "
                    + mins + " minutes and " + secs + " seconds.")).ConfigureAwait(false);
                }
                else if (yesOrNo.Result.Result.Emoji == minusOne)
                    await ctx.Channel.SendMessageAsync(embed:
                    Vembed("Successfully set timer to " + hours + " hours and "
                    + mins + " minutes and " + secs + " seconds.")).ConfigureAwait(false);

                //actually waits till time ends
                await Task.Delay((hours * 60 * 60 * 1000) + (mins * 60 * 1000) + (secs * 1000));

                //checks for user's timer msg location
                if (reaction.Result.Emoji == d)
                {
                    DiscordChannel dm = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

                    if (optionalMsg == string.Empty)
                    {
                        await dm.SendMessageAsync("Timer is done, Timer is done, Timer is done!");
                    }
                    else
                    {
                        await dm.SendMessageAsync(ctx.User.Mention);
                        await dm.SendMessageAsync(embed: Vembed(optionalMsg));
                    }
                }

                else if (reaction.Result.Emoji == m)
                {
                    if (optionalMsg == string.Empty)
                    {
                        await ctx.Channel.SendMessageAsync("Timer is done, Timer is done, Timer is done!");
                    }
                    else
                    {
                        await ctx.Channel.SendMessageAsync(ctx.User.Mention);
                        await ctx.Channel.SendMessageAsync(embed: Vembed(optionalMsg));
                    }
                }
            }
        }
        #endregion Timer

        #region 8ball
        [Command("8ball")]
        [Description("Ask the bot a question and see what he replies with ;)")]
        public async Task EightBall(CommandContext ctx, [RemainingText] string question)
        {
            Random r = new Random();
            int random = r.Next(0, 10);


            switch (random)
            {
                case 1:
                    await ctx.Channel.SendMessageAsync("I think you're good");
                    break;

                case 2:
                    await ctx.Channel.SendMessageAsync("You're an idiot");
                    break;

                case 3:
                    await ctx.Channel.SendMessageAsync("Bruh");
                    break;

                case 4:
                    await ctx.Channel.SendMessageAsync("نايس, والله");
                    break;

                case 5:
                    await ctx.Channel.SendMessageAsync("صدق؟");
                    break;

                case 6:
                    await ctx.Channel.SendMessageAsync("No.");
                    break;

                case 7:
                    await ctx.Channel.SendMessageAsync("Maybe?");
                    break;

                case 8:
                    await ctx.Channel.SendMessageAsync("I think that's acceptable.");
                    break;

                case 9:
                    await ctx.Channel.SendMessageAsync("That's not a good idea, bro");
                    break;

                case 10:
                    await ctx.Channel.SendMessageAsync("Hmm... Yeah, I think you should just stop");
                    break;

                default:
                    await ctx.Channel.SendMessageAsync("");
                    break;
            }
        }
        #endregion

        #region Rainbow Role

        bool isOn = false;
        [Command("rainbowrole")]
        public async Task RainbowRole(CommandContext ctx, DiscordMember member)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                var champ = ctx.Guild.GetRole(694953118563172412);
                var diamond = ctx.Guild.GetRole(694953116356837386);
                var plat = ctx.Guild.GetRole(694953113957957664);
                var gold = ctx.Guild.GetRole(694953111831183370);
                var silver = ctx.Guild.GetRole(694953104604659765);
                var bronze = ctx.Guild.GetRole(694953087567265882);
                var iron = ctx.Guild.GetRole(694953058484093018);

                if (isOn)
                {
                    isOn = false;
                    if (member.Roles.Contains(champ) || member.Roles.Contains(diamond) || member.Roles.Contains(plat)
                        || member.Roles.Contains(gold) || member.Roles.Contains(silver) || member.Roles.Contains(bronze)
                        || member.Roles.Contains(iron))
                    {
                        await member.RevokeRoleAsync(champ);
                        await member.RevokeRoleAsync(diamond);
                        await member.RevokeRoleAsync(plat);
                        await member.RevokeRoleAsync(gold);
                        await member.RevokeRoleAsync(silver);
                        await member.RevokeRoleAsync(bronze);
                        await member.RevokeRoleAsync(iron);
                    }
                    await ctx.Channel.SendMessageAsync("Disabled Rainbow Role.");
                }
                else
                {
                    isOn = true;
                    do
                    {
                        await member.GrantRoleAsync(iron);
                        await Task.Delay(1000);

                        await member.GrantRoleAsync(bronze);
                        await Task.Delay(1000);

                        await member.GrantRoleAsync(silver);
                        await Task.Delay(1000);

                        await member.GrantRoleAsync(gold);
                        await Task.Delay(1000);

                        await member.GrantRoleAsync(plat);
                        await Task.Delay(1000);

                        await member.GrantRoleAsync(diamond);
                        await Task.Delay(1000);

                        await member.GrantRoleAsync(champ);
                        await Task.Delay(1000);

                        await member.RevokeRoleAsync(champ);
                        await Task.Delay(1000);

                        await member.RevokeRoleAsync(diamond);
                        await Task.Delay(1000);

                        await member.RevokeRoleAsync(plat);
                        await Task.Delay(1000);

                        await member.RevokeRoleAsync(gold);
                        await Task.Delay(1000);

                        await member.RevokeRoleAsync(silver);
                        await Task.Delay(1000);

                        await member.RevokeRoleAsync(bronze);
                        await Task.Delay(1000);

                        await member.RevokeRoleAsync(iron);
                        await Task.Delay(1000);
                    } while (isOn);
                }
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }

        #endregion
    }
}