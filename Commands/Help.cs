using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace Vinex_Bot.Commands
{
    public class Help : BaseHelpFormatter
    {
        private DiscordEmbedBuilder Embed { get; }

        public Help(CommandContext ctx)
            : base(ctx)
        {
            this.Embed = new DiscordEmbedBuilder();
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            this.Embed.Title = (command.Description ?? "No description provided.\n\n");

            if (command.Aliases?.Any() == true)
            {
                this.Embed.Description = "Other Names: "
                    + string.Join(", ", command.Aliases) + "\n\n";
            }

            if (command.Overloads?.Any() == true)
            {
                var embed = new DiscordEmbedBuilder();

                foreach (var ovl in command.Overloads.OrderByDescending(x => x.Priority))
                {
                    this.Embed.Title = command.QualifiedName;

                    this.Embed.Description += "Arguments:\n";
                    
                    foreach (var arg in ovl.Arguments)
                        this.Embed.Description += arg.IsOptional || arg.IsCatchAll ? " [" : " <" + arg.Name + (arg.IsCatchAll ? "..." : "") + (arg.IsOptional || arg.IsCatchAll ? ']' : '>');

                    this.Embed.Description += '\n';

                    foreach (var arg in ovl.Arguments)
                        this.Embed.Description += arg.Name + " (" + this.CommandsNext.GetUserFriendlyTypeName(arg.Type) + "): " + (arg.Description ?? "") + '\n';

                    this.Embed.Description += '\n';
                }
            }

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            if (this.Embed.Title == "")
                this.Embed.Title += "Displaying all available commands.\n\n";
            else
                this.Embed.Title += "Here are the available commands! \n";

            //this is where all the commands are shown. come here and edit this in case you want to edit in the future
            if (subcommands?.Any() == true)
            {
                //var ml = subcommands.Max(xc => xc.Name.Length);
                foreach (var xc in subcommands)
                {
                    if (xc.Name == "joinelement" || xc.Name == "givemoney" || xc.Name == "showinventory" || xc.Name == "showstats" || xc.Name == "coindrop")
                        continue;
                    else
                        this.Embed.Description += $"`{xc.Name}`" + ": " + (string.IsNullOrWhiteSpace(xc.Description) ? "" : xc.Description) + "\n";
                }                
            }

            return this;
        }
        public override CommandHelpMessage Build()
            => new CommandHelpMessage("", Embed);
    }
}
