using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vinex_Bot.CommandExtensions
{
    public static class EmbedShortcut
    {
        public static DiscordEmbed Vembed()
        {
            var testEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Aquamarine
            };

            return testEmbed;
        }

        public static DiscordEmbed Vembed(string description)
        {
            var testEmbed = new DiscordEmbedBuilder
            {
                Description = description,
                Color = DiscordColor.Aquamarine
            };

            return testEmbed;
        }

        public static DiscordEmbed Vembed(string title, string description)
        {
            var testEmbed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = DiscordColor.Aquamarine
            };

            return testEmbed;
        }

        public static DiscordEmbed Vembed(string title, string description, string thumbURL)
        {
            var testEmbed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                ThumbnailUrl = thumbURL,
                Color = DiscordColor.Aquamarine
            };

            return testEmbed;
        }

        public static DiscordEmbed Vembed(string title, string description, string thumbURL, string imgURL)
        {
            var testEmbed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                ThumbnailUrl = thumbURL,
                ImageUrl = imgURL,
                Color = DiscordColor.Aquamarine
            };

            return testEmbed;
        }
    }
}
