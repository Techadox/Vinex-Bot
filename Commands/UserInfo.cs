using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Vinex_Bot.CommandExtensions.EmbedShortcut;

namespace Vinex_Bot.Commands
{
    public class UserInfo : BaseCommandModule
    {
        #region Avatar
        [Command("avatar")]
        [Description("Gets the avatar of the member")]
        [Aliases("pic", "picture")]

        public async Task Avatar(CommandContext ctx)
        {
            var pic = ctx.User.AvatarUrl;
            await ctx.Channel.SendMessageAsync(embed: Vembed(ctx.User.Username + "\'s avatar is:", "", "", pic)).ConfigureAwait(false);
        }

        [Command("avatar")]
        [Description("Gets the avatar of the member")]

        public async Task Avatar(CommandContext ctx, DiscordMember member)
        {
            var pic = member.AvatarUrl;
            await ctx.Channel.SendMessageAsync(embed: Vembed(member.Username + "\'s avatar is:", "", "", pic)).ConfigureAwait(false);
        }
        #endregion

        #region Name

        [Command("name")]
        [Description("Gets the name of the member")]
        [Aliases("user", "username")]

        public async Task Name(CommandContext ctx)
        {
            var name = ctx.User.Username + "#" + ctx.User.Discriminator;
            await ctx.Channel.SendMessageAsync(embed: Vembed(ctx.User.Username + "\'s name is:", name)).ConfigureAwait(false);
        }

        [Command("name")]
        [Description("Gets the name of the member")]

        public async Task Name(CommandContext ctx, DiscordMember member)
        {
            var name = member.Username + "#" + member.Discriminator;
            await ctx.Channel.SendMessageAsync(embed: Vembed(member.Nickname + "\'s name is:", name)).ConfigureAwait(false);
        }

        #endregion

        #region Nickname

        [Command("nickname")]
        [Description("Gets the nickname of the member")]
        [Aliases("nick")]

        public async Task Nickname(CommandContext ctx)
        {
            var name = (ctx.Member.Nickname == null) ? ctx.Member.DisplayName : ctx.Member.Nickname;
            await ctx.Channel.SendMessageAsync(embed: Vembed(ctx.User.Username + "\'s nickname is:", name)).ConfigureAwait(false);
        }

        [Command("nickname")]
        [Description("Gets the nickname of the member")]

        public async Task Nickname(CommandContext ctx, DiscordMember member)
        {
            var name = (member.Nickname == null) ? member.DisplayName : member.Nickname;
            await ctx.Channel.SendMessageAsync(embed: Vembed(member.Username + "\'s nickname is:", name)).ConfigureAwait(false);
        }

        #endregion

        #region ID

        [Command("ID")]
        [Description("Gets the ID of the member")]

        public async Task ID(CommandContext ctx)
        {
            var ID = ctx.Member.Id;
            var name = (ctx.Member.Nickname == null) ? ctx.Member.DisplayName : ctx.Member.Nickname;

            await ctx.Channel.SendMessageAsync(embed: Vembed(name + "\'s ID is:", ID.ToString())).ConfigureAwait(false);
        }

        [Command("ID")]
        [Description("Gets the ID of the member")]

        public async Task ID(CommandContext ctx, DiscordMember member)
        {
            var ID = member.Id;
            var name = (member.Nickname == null) ? member.DisplayName : member.Nickname;

            await ctx.Channel.SendMessageAsync(embed: Vembed(name + "\'s ID is:", ID.ToString())).ConfigureAwait(false);
        }

        #endregion

        #region Joined Server

        [Command("joined")]
        [Aliases("joinedat", "joinedserver", "serverjoined", "serverjoin")]
        [Description("Gets the join server date")]

        public async Task JoinServer(CommandContext ctx)
        {
            var joinDate = ctx.Member.JoinedAt;
            var name = (ctx.Member.Nickname == null) ? ctx.Member.DisplayName : ctx.Member.Nickname;

            await ctx.Channel.SendMessageAsync(embed: Vembed(name + " joined the server at:", joinDate.ToString())).ConfigureAwait(false);
        }

        [Command("joined")]
        [Description("Gets the join server date")]

        public async Task JoinServer(CommandContext ctx, DiscordMember member)
        {
            var joinDate = member.JoinedAt;
            var name = (member.Nickname == null) ? member.DisplayName : member.Nickname;

            await ctx.Channel.SendMessageAsync(embed: Vembed(name + " joined the server at:", joinDate.ToString())).ConfigureAwait(false);
        }

        #endregion

        #region Joined Discord

        [Command("joineddiscord")]
        [Aliases("joineddiscordat", "discordjoin", "discordjoined")]
        [Description("Gets the join server date")]

        public async Task JoinDiscord(CommandContext ctx)
        {
            var joinDate = ctx.User.CreationTimestamp;
            var name = (ctx.Member.Username == null) ? ctx.Member.DisplayName : ctx.Member.Username;

            await ctx.Channel.SendMessageAsync(embed: Vembed(name + " joined discord at:", joinDate.ToString())).ConfigureAwait(false);
        }

        [Command("joineddiscord")]
        [Description("Gets the join server date")]

        public async Task JoinDiscord(CommandContext ctx, DiscordUser user)
        {
            var joinDate = user.CreationTimestamp;
            await ctx.Channel.SendMessageAsync(embed: Vembed(user.Username + " joined discord at:", joinDate.ToString())).ConfigureAwait(false);
        }

        #endregion

        #region Roles

        [Command("roles")]
        [Aliases("getroles", "role")]
        [Description("Gets the roles of the member")]
        public async Task Roles(CommandContext ctx)
        {
            var roles = ctx.Member.Roles;
            var name = (ctx.Member.Username == null) ? ctx.Member.DisplayName : ctx.Member.Username;
            string stringRoles = "";

            foreach (var role in roles)
            {
                stringRoles = stringRoles + role.Name + "\n";
            }

            await ctx.Channel.SendMessageAsync(embed: Vembed(name + "\'s roles are:", stringRoles)).ConfigureAwait(false);
        }

        [Command("roles")]
        [Description("Gets the roles of the member")]
        public async Task Roles(CommandContext ctx, DiscordMember member)
        {
            var roles = member.Roles;
            var name = (member.Username == null) ? member.DisplayName : member.Username;
            string stringRoles = "";

            foreach (var role in roles)
            {
                stringRoles = stringRoles + role.Name + "\n";
            }

            await ctx.Channel.SendMessageAsync(embed: Vembed(name + "\'s roles are:", stringRoles)).ConfigureAwait(false);
        }

        #endregion

        #region UserInformation

        [Command("userinfo")]
        [Aliases("info", "userdetails")]
        [Description("Gets all the information possible from a member")]

        public async Task Info(CommandContext ctx)
        {
            var pic = ctx.User.AvatarUrl;
            var name = ctx.User.Username + "#" + ctx.User.Discriminator;
            var nick = (ctx.Member.Username == null) ? ctx.Member.DisplayName : ctx.Member.Username;
            var ID = ctx.Member.Id;
            var joinDate = ctx.Member.JoinedAt;
            var joinDiscord = ctx.User.CreationTimestamp;
            var roles = ctx.Member.Roles;
            string stringRoles = "";

            foreach (var role in roles)
            {
                stringRoles = stringRoles + role.Name + "\n";
            }

            var info = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Aquamarine,
                ThumbnailUrl = pic
            };

            info.AddField("Name:", name, true);
            info.AddField("Nickname:", nick, true);
            info.AddField("ID:", ID.ToString(), true);
            info.AddField("Joined Server:", joinDate.ToString(), true);
            info.AddField("Joined Discord:", joinDiscord.ToString(), true);
            info.AddField("Roles:", stringRoles, true);

            await ctx.Channel.SendMessageAsync(embed: info).ConfigureAwait(false);
        }

        [Command("userinfo")]
        [Description("Gets all the information possible from a member")]

        public async Task Info(CommandContext ctx, DiscordMember member)
        {
            var pic = member.AvatarUrl;
            var name = member.Username + "#" + ctx.User.Discriminator;
            var nick = (member.Username == null) ? member.DisplayName : member.Username;
            var ID = member.Id;
            var joinDate = member.JoinedAt;
            var joinDiscord = member.CreationTimestamp;
            var roles = member.Roles;
            string stringRoles = "";

            foreach (var role in roles)
            {
                stringRoles = stringRoles + role.Name + "\n";
            }

            var info = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Aquamarine,
                ThumbnailUrl = pic
            };

            info.AddField("Name:", name, true);
            info.AddField("Nickname:", nick, true);
            info.AddField("ID:", ID.ToString(), true);
            info.AddField("Joined Server:", joinDate.ToString(), true);
            info.AddField("Joined Discord:", joinDiscord.ToString(), true);
            info.AddField("Roles:", stringRoles, true);

            await ctx.Channel.SendMessageAsync(embed: info).ConfigureAwait(false);
        }

        #endregion
    }

}
