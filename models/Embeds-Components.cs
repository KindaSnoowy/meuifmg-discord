using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;

// store all embeds n components for easy access
namespace MeuIFMG_DiscordBot
{
    public class Embeds
    {
        // !notas command embeds:
        public static EmbedBuilder GetEtapasEmbed()
        {
            string description = "";
            description += "Menu para exibição de notas.\n";
            description += "Selecione uma etapa para a exibição\n";
            return new EmbedBuilder()
                .WithTitle("Menu Notas")
                .WithDescription(description)
                .WithColor(Color.Magenta);
        }

        public static EmbedBuilder GetMatériasEmbed()
        {
            return new EmbedBuilder()
                .WithTitle("Menu Notas")
                .WithDescription($"Pressione \"Voltar\" para voltar.\nPressione \"Próximo\" para prosseguir\nPressione \"Início\" para voltar à seleção de etapas.")
                .WithColor(Color.Magenta);
        }

    }

    public class Components {
        // !notas commmand components:
        public static ComponentBuilder GetEtapasButtons()
        {
            return new ComponentBuilder()
                .WithButton("1ª", "primeira-etapa-bttn")
                .WithButton("1ª (R)", "primeira-etapar-bttn")
                .WithButton("2ª", "segunda-etapa-bttn")
                .WithButton("2ª (R)", "segunda-etapar-bttn")
                .WithButton("3ª", "terceira-etapa-bttn")
                .WithButton("Exame Final", "exame-final-bttn");
        }

        public static ComponentBuilder GetMatériasButtons()
        {
            return new ComponentBuilder()
                .WithButton("Início", "home-bttn")
                .WithButton("Voltar", "back-bttn")
                .WithButton("Próximo", "next-bttn");
        }

        
    }
}