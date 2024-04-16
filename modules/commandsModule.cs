using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using MeuIFMG_DiscordBot.models;
using MeuIFMG_DiscordBot.modules;
using static MeuIFMG_DiscordBot.interactionHandler;

namespace MeuIFMG_DiscordBot.modules
{
    public class CommandsModule : ModuleBase<SocketCommandContext>
    {
        private DiscordSocketClient _socketClient;

        // !register -> Registra usuário pela DM.
        [Command("register", RunMode = Discord.Commands.RunMode.Async)]
        public async Task RegisterCommand()
        {
            if (await databaseJSON.ReadIDEntryAsync(Context.User.Id) != null) { 
                await ReplyAsync("O ID do seu usuário já possui uma conta já associada.");
            } else {
                await ReplyAsync("Por favor responda a mensagem enviada na sua DM!");
                IDMChannel _dmChannel = await Context.User.CreateDMChannelAsync();
                await _dmChannel.SendMessageAsync("Responda com somente uma mensagem que deva seguir o seguinte formato:\n{Usuário do MeuIFMG}${Senha do MeuIFMG}\nExemplo: ``0068078$senha123*``");

                _socketClient = Context.Client;
                _socketClient.MessageReceived += DMRegisterHandler;
            }
        }

        // Handler pra mensagem na DM de !notas
        // (por algum motivo dá aviso dizendo que tem um MessageReceived Handler bloqueando a tarefa de gateway
        //   mas até então não encontrei nenhum problema mesmo com o aviso)
        private async Task DMRegisterHandler(SocketMessage msg)
        {
            if (msg.Author.Id == Context.User.Id && msg.Channel is IDMChannel)
            {
                IDMChannel _dmChannel = await Context.User.CreateDMChannelAsync();
                var userParams = msg.Content.Split("$");
                try
    	        {
                    User usuarioDBEntry = await User.CreateUserAsync(userParams[0], userParams[1], Context.User.Id);
                    await databaseJSON.CreateEntry(usuarioDBEntry);
                    await Context.User.SendMessageAsync("Usuário criado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[GENERAL ERROR] -> {ex.Message}");
                    await _dmChannel.SendMessageAsync($"[GENERAL ERROR] -> **Erro no Login**\nVocê digitou o seu usuário e senha corretamente? Por favor, tente novamente.");
                }
                finally 
                {
                    _socketClient.MessageReceived -= DMRegisterHandler;
                }
            }
        }

        // !unregister -> Deleta o usuário do banco de dados.
        [Command("unregister")]
        public async Task UnregisterUser()
        {
            var m = await ReplyAsync("Deletando entrada...");
            await databaseJSON.DeleteEntry("ID", Context.User.Id.ToString());
            await m.ModifyAsync(m => m.Content = "Entrada deletada com sucesso!");
        }

        // !forcesync -> Sincroniza as notas do usuário no banco de dados com as do MeuIFMG
        [Command("forcesync", RunMode = Discord.Commands.RunMode.Async)]
        public async Task ForceUpdateNotas()
        {
            User user = await databaseJSON.ReadIDEntryAsync(Context.User.Id);
            var m = await ReplyAsync("Sincronização solicitada...");
            await syncModule.UserSync("RA", user.RA);
            await m.ModifyAsync(m => m.Content = "Sincronização feita com sucesso!");
        }

        // !notas -> Exibe menu de notas
        [Command("notas")]
        public async Task GetNotasTest()
        {
            User user = await databaseJSON.ReadIDEntryAsync(Context.User.Id);

            var message = await ReplyAsync(embed: Embeds.GetEtapasEmbed()
                                                    .WithFooter($"Notas do usuário com RA {user.RA}").Build(), 
                                            components: Components.GetEtapasButtons().Build());
            
            InteractionRegistration interactionRegister = new() {
                CommandIdentifier = "notasMenu",
                Payload = user
            };
            // ||   <- Registra a mensagem em interactionHandler.cs para a exibição do menu de forma correta.
            // VV   
            RegisterCommandInteractions(message.Id, interactionRegister);
        }
    }    
}