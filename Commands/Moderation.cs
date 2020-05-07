using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Vinex_Bot.CommandExtensions.EmbedShortcut;


namespace Vinex_Bot.Commands
{
    class Moderation : BaseCommandModule
    {
        #region Role Management
        [Command("giverole")]
        [Aliases("gr", "grantrole")]
        [Description("Gives a role to someone")]
        public async Task GiveRole(CommandContext ctx, DiscordMember user, [RemainingText] DiscordRole role)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                if (user.Roles.Contains(role))
                {
                    var embed = Vembed(user.DisplayName + "#" + user.Discriminator + " already has the " + role.Name + " role.");
                    await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                }
                else
                {
                    await user.GrantRoleAsync(role).ConfigureAwait(false);

                    var embed = Vembed("Granted Role " + role.Name + " to " + user.DisplayName + "#" + user.Discriminator + ".");
                    await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                }
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command."); 
        }

        [Command("removerole")]
        [Aliases("rr", "revokerole")]
        [Description("Removes a role from someone")]
        public async Task Remove(CommandContext ctx, DiscordMember user, [RemainingText] DiscordRole role)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                if (user.Roles.Contains(role))
                {
                    await user.RevokeRoleAsync(role).ConfigureAwait(false);

                    var embed = Vembed("Removed Role " + role.Name + " from " + user.DisplayName + "#" + user.Discriminator + ".");
                    await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                }
                else
                {
                    var embed = Vembed(user.DisplayName + "#" + user.Discriminator + " does not have the " + role.Name + " role.");
                    await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                }
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }
        #endregion

        #region Kicking
        [Command("kick")]
        [Aliases("k")]
        [Description("Kicks someone from the server")]
        public async Task Kick(CommandContext ctx, DiscordMember user)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                if (ctx.Guild.Members.Values.Contains(user))
                {
                    await user.RemoveAsync().ConfigureAwait(false);

                    var embed = Vembed("Successfully kicked " + user.DisplayName + "#" + user.Discriminator + ".");
                    await ctx.Channel.SendMessageAsync(embed: embed);
                }
                else
                    await ctx.Channel.SendMessageAsync("User does not exist in this server.");
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }
        #endregion

        #region Banning and Unbanning

        [Command("getbans")]
        [Aliases("banned", "bans", "allbans")]
        [Description("Gets all the banned users from this server")]
        public async Task GetBans(CommandContext ctx)
        {
            var bans = await ctx.Guild.GetBansAsync().ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync(embed: Vembed($"{string.Join("\n", bans.Select(x => x.User))}"));
        }

        [Command("ban")]
        [Aliases("b")]
        [Description("Bans someone from the server")]
        public async Task Ban(CommandContext ctx, DiscordMember user)
        {
            bool isHere = false;
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                foreach (var member in ctx.Guild.Members)
                {
                    if (member.Value == user)
                    {
                        isHere = true;
                        await user.BanAsync().ConfigureAwait(false);

                        var embed = Vembed("Successfully Banned " + user.DisplayName + "#" + user.Discriminator + ".");
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        break;
                    }
                }
                if (!isHere)
                    await ctx.Channel.SendMessageAsync("User does not exist in this server.");
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }

        [Command("ban")]
        [Description("Bans someone from the server")]
        public async Task Ban(CommandContext ctx, DiscordMember user, [RemainingText] string reason)
        {
            bool isHere = false;
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                foreach (var member in ctx.Guild.Members)
                {
                    if (member.Value == user)
                    {
                        isHere = true;
                        await user.BanAsync(0, reason).ConfigureAwait(false);

                        var embed = Vembed("Successfully Banned " + user.DisplayName + "#" + user.Discriminator + ".");
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        break;
                    }
                }
                if (!isHere)
                    await ctx.Channel.SendMessageAsync("User does not exist in this server.");
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }

        [Command("unban")]
        [Aliases("ub", "unb")]
        [Description("Unbans someone from the server")]
        public async Task Unban(CommandContext ctx, ulong id)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                await ctx.Guild.UnbanMemberAsync(id).ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync(embed: Vembed("Successfully unbanned " + id));
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }

        [Command("unban")]
        [Description("Unbans someone from the server")]
        public async Task Unban(CommandContext ctx, DiscordMember member)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                await member.UnbanAsync().ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync(embed: Vembed("Successfully unbanned " + member));
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }

        [Command("unban")]
        [Description("Unbans someone from the server")]
        public async Task Unban(CommandContext ctx, DiscordUser user)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                await ctx.Channel.SendMessageAsync(embed: Vembed("Successfully unbanned " + user.Username));
                await user.UnbanAsync(ctx.Guild).ConfigureAwait(false);
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }
        #endregion

        #region Muting

        [Command("mutetext")]
        [Aliases("mt", "mute", "mutet")]
        [Description("Revokes sending messages from a user")]
        public async Task MuteText(CommandContext ctx, DiscordMember member)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                await member.GrantRoleAsync(ctx.Guild.GetRole(693710042075234335)).ConfigureAwait(false);
                var embed = Vembed("Successfully text muted " + member.DisplayName);
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }

        [Command("mutevoice")]
        [Aliases("mv", "mutev")]
        [Description("Revokes talking abilities from a user")]
        public async Task MuteVoice(CommandContext ctx, DiscordMember member)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                await member.GrantRoleAsync(ctx.Guild.GetRole(695947782216745010)).ConfigureAwait(false);
                var embed = Vembed("Successfully voice muted " + member.DisplayName);
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }
        #endregion

        #region Prune

        [Command("prune")]
        [Aliases("delete", "p", "d")]
        [Description("Deletes x messages before. If no number is set, 20 messages will be removed")]
        public async Task Prune(CommandContext ctx)
        {
            var msgs = ctx.Channel.GetMessagesAsync(20);
            await ctx.Channel.DeleteMessagesAsync(msgs.Result).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync(embed: Vembed("Successfully deleted 20 messages."));
        }

        [Command("prune")]
        [Description("Deletes x messages before. If no number is set, 20 messages will be removed")]
        public async Task Prune(CommandContext ctx, int num)
        {
            var msgs = ctx.Channel.GetMessagesAsync(num);
            await ctx.Channel.DeleteMessagesAsync(msgs.Result).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync(embed: Vembed("Successfully deleted " + num + " messages."));
        }

        [Command("prune")]
        [Description("Deletes x messages before. If no number is set, 20 messages will be removed")]
        public async Task Prune(CommandContext ctx, DiscordMember member)
        {
            var msgs = ctx.Channel.GetMessagesAsync(200);
            int i = 0;

            if (ctx.Member == member)
                i--;

            foreach (var msg in msgs.Result)
            {

                if (msg.Author == member && i < 2)
                {
                    await msg.DeleteAsync().ConfigureAwait(false);
                    i++;
                }
                else if (i >= 3)
                    break;                 
            }
            await ctx.Channel.SendMessageAsync(embed: Vembed
                ("Successfully deleted 2 messages from " + member.DisplayName + "."));

        }

        [Command("prune")]
        [Description("Deletes x messages before. If no number is set, 20 messages will be removed")]
        public async Task Prune(CommandContext ctx, DiscordMember member, int num)
        {
            var msgs = ctx.Channel.GetMessagesAsync(200);
            int i = 0;

            if (ctx.Member == member)
                i--;

            foreach (var msg in msgs.Result)
            {

                if (msg.Author == member && i < num)
                {
                    await msg.DeleteAsync().ConfigureAwait(false);
                    i++;
                }
                else if (i >= num)
                    break;
            }
            await ctx.Channel.SendMessageAsync(embed: Vembed
                ("Successfully deleted 2 messages from " + member.DisplayName + "."));
        }

        #endregion

        #region Add Emoji

        [Command("addemoji")]
        [Aliases("emojiadd")]
        //Adds an emoji to the server
        [Description("work in progress")]
        public async Task AddEmoji(CommandContext ctx)
        {
            WebClient webclient = new WebClient();
            //webclient.DownloadFileCompleted += 
            var interactivity = ctx.Client.GetInteractivity();
            string fullPath = "";

            await ctx.Channel.SendMessageAsync(embed: Vembed("Please send the emoji you would like to add:"));
            var pic = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel
                                                                && x.Author == ctx.User);
            var attachments = pic.Result.Attachments;

            foreach (var attachment in attachments)
            {
                string url = attachment.Url;
                if (!string.IsNullOrEmpty(url))
                {
                    Thread thread = new Thread(() =>
                    {
                        Uri uri = new Uri(url);
                        string fileName = System.IO.Path.GetFileName(uri.AbsolutePath);
                        fullPath = @"Z:\Self Study\Programming\C#\Discord Bots\Vinex Bot\Vinex Bot\Docs" + "\\" + fileName;
                        webclient.DownloadFileAsync(uri, fullPath);
                    });
                    thread.Start();
                }
                else
                {
                    await ctx.Channel.SendMessageAsync("You didn't add a picture");
                }
            }
            await ctx.Channel.SendMessageAsync("Please fix this because this is so scuffed, I swear to god you don't know anything about C# why are you even programming this bot what are you " +
                "doing with your life, you should be doing something else, but guess what? you're here programming, you idiot. now if you're still insisting on not getting a life, why don't you" +
                "fucking learn how to program events you piece of shit? you're really gonna just leave this fucking tAsK.DeLaY(2000) here and just move on with your life? FUCK NO, NIGGER. please" +
                "learn how to program, you sick fuck. god.");
            await Task.Delay(2000);
            await ctx.Channel.SendFileAsync(fullPath);

        }
        #endregion

        #region Change Nickname

        [Command("changenickname")]
        [Aliases("changenick")]
        [Description("Changes the nickname of a user")]
        public async Task ChangeNickname(CommandContext ctx, DiscordMember member, [RemainingText] string name)
        {
            if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
            {
                await member.ModifyAsync(x => x.Nickname = name);
                await ctx.Channel.SendMessageAsync($"Successfully changed {member.Username}'s name to {name}");
            }
            else
                await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");
        }

        [Command("changenickname")]
        [Description("Changes the nickname of a user")]
        public async Task ChangeNickname(CommandContext ctx, [RemainingText] string name)
        {
            await ctx.Member.ModifyAsync(x => x.Nickname = name);
            await ctx.Channel.SendMessageAsync($"Successfully changed {ctx.User.Username}'s name to {name}");
        }
        #endregion
    }
}
// code for perms

//if (ctx.Member.PermissionsIn(ctx.Channel) == DSharpPlus.Permissions.All)
//
//else
//await ctx.Channel.SendMessageAsync("You do not have sufficient permissions to perform this command.");