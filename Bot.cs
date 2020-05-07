using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Vinex_Bot.Commands;

namespace Vinex_Bot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; } //The actual bot
        public InteractivityExtension Interactivity {get; private set; } //Class with Interactivity Methods
        public static CommandsNextExtension Commands { get; private set; } //Commands Handler
        public async Task RunAsync()
        {
            #region Using the .json file
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
            #endregion

            #region Bot Configuration 
            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            Client = new DiscordClient(config);
            #endregion

            #region Interactivity Configuration

            Client.UseInteractivity(new InteractivityConfiguration
            {
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromDays(1)
            });

            #endregion

            #region Command Configuration and Adding Them To The Available Commands
            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] {configJson.Prefix},
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = true,
                DmHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<Test>();
            Commands.RegisterCommands<Moderation>();
            Commands.RegisterCommands<Interactive>();
            Commands.RegisterCommands<UserInfo>();
            Commands.RegisterCommands<Kingdom.Commands>();
            Commands.SetHelpFormatter<Vinex_Bot.Commands.Help>();

            #endregion

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
    