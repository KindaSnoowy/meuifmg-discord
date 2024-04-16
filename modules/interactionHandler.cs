using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using MeuIFMG_DiscordBot.models;
using MeuIFMG_DiscordBot.modules;

namespace MeuIFMG_DiscordBot
{
    public class interactionHandler
    {
        // Objeto para registrar interações
        public class InteractionRegistration {
            public string CommandIdentifier {get; set;} // <- Identificador do comando
            public object Payload {get; set;} // <- Payload (como objeto de usuário) para exibição em menus.
        }

        // ||    <- Guarda todas as mensagens com os comandos que existem interações  
        // VV
        private static Dictionary<ulong, InteractionRegistration> commandInteractionMap = new();
        
        // Método para registrar interações
        public static void RegisterCommandInteractions(ulong messageId, InteractionRegistration objIRegister) {
            commandInteractionMap[messageId] = objIRegister;
        }
        
        // menu interaction handler dictionary;
        // dicionário para guardar estados de menus 
        private static Dictionary<ulong, Dictionary<string, int>> _menuStates = new(); 

        public static async Task HandleButtonAsync(SocketMessageComponent componentInteraction) {
            ulong msgId = componentInteraction.Message.Id;

            //      ||    <- Verifica se a interação já existe para essa mensagem
            //      VV                                  (ou seja, já foi registrada)
            if (commandInteractionMap.TryGetValue(msgId, out InteractionRegistration objIRegister)) {
                switch (objIRegister.CommandIdentifier) {

                    // [Command(!notas)]:
                    case "notasMenu":
                        if (!_menuStates.ContainsKey(msgId)) {
                            _menuStates[msgId] = new Dictionary<string, int>(); // <- Dicionário para guardar estados de menu.
                        }

                        User user = (User)objIRegister.Payload;
                        Matéria matériaAtual = new();
                        switch (componentInteraction.Data.CustomId) {
                            // Primeira parte do menu -> Seleciona etapas
                            case "primeira-etapa-bttn":
                                _menuStates[msgId]["stateEtapa"] = 0;
                                _menuStates[msgId]["stateMatéria"] = -1;
                                await componentInteraction.UpdateAsync(x => {
                                    x.Embed = Embeds.GetMatériasEmbed()
                                                .WithFooter($"Matéria 0/{user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias.Count}").Build();
                                    x.Components = Components.GetMatériasButtons().Build();
                                });
                                break;

                            case "primeira-etapar-bttn":
                                _menuStates[msgId]["stateEtapa"] = 1;
                                _menuStates[msgId]["stateMatéria"] = -1;
                                await componentInteraction.UpdateAsync(x => {
                                    x.Embed = Embeds.GetMatériasEmbed()
                                                .WithFooter($"Matéria 0/{user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias.Count}").Build();
                                    x.Components = Components.GetMatériasButtons().Build();
                                });
                                break;
                            
                            case "segunda-etapa-bttn":
                                _menuStates[msgId]["stateEtapa"] = 2;
                                _menuStates[msgId]["stateMatéria"] = -1;
                                await componentInteraction.UpdateAsync(x => {
                                    x.Embed = Embeds.GetMatériasEmbed()
                                                .WithFooter($"Matéria 0/{user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias.Count}").Build();
                                    x.Components = Components.GetMatériasButtons().Build();
                                });
                                break;

                            case "segunda-etapar-bttn":
                                _menuStates[msgId]["stateEtapa"] = 3;
                                _menuStates[msgId]["stateMatéria"] = -1;
                                await componentInteraction.UpdateAsync(x => {
                                    x.Embed = Embeds.GetMatériasEmbed()
                                                .WithFooter($"Matéria 0/{user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias.Count}").Build();
                                    x.Components = Components.GetMatériasButtons().Build();
                                });
                                break;

                            case "terceira-etapa-bttn":
                                _menuStates[msgId]["stateEtapa"] = 4;
                                _menuStates[msgId]["stateMatéria"] = -1;
                                await componentInteraction.UpdateAsync(x => {
                                    x.Embed = Embeds.GetMatériasEmbed()
                                                .WithFooter($"Matéria 0/{user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias.Count}").Build();
                                    x.Components = Components.GetMatériasButtons().Build();
                                });
                                break;

                            case "exame-final-bttn":
                                _menuStates[msgId]["stateEtapa"] = 5;
                                _menuStates[msgId]["stateMatéria"] = -1;
                                await componentInteraction.UpdateAsync(x => {
                                    x.Embed = Embeds.GetMatériasEmbed()
                                                .WithFooter($"Matéria 0/{user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias.Count}").Build();
                                    x.Components = Components.GetMatériasButtons().Build();
                                });
                                break;

                            // Segunda parte do menu -> Navega nas matérias
                            
                            case "home-bttn":
                                await componentInteraction.UpdateAsync(x => {
                                    x.Embed = Embeds.GetEtapasEmbed().Build();
                                    x.Components = Components.GetEtapasButtons().Build();
                                });
                                break;

                            case "next-bttn":
                                if (_menuStates[msgId]["stateMatéria"] >= user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias.Count - 1) {
                                    _menuStates[msgId]["stateMatéria"] = 0;
                                } else {
                                    _menuStates[msgId]["stateMatéria"]++;
                                }

                                matériaAtual = user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias[_menuStates[msgId]["stateMatéria"]];
                                    
                                await componentInteraction.UpdateAsync(x => {
                                    x.Embed = new EmbedBuilder()
                                                .WithTitle(matériaAtual.Nome)
                                                .WithDescription($"Situação: {matériaAtual.Situação}\nAvaliações: {(matériaAtual.Avaliações.Count == 0 ? "Nenhuma avaliação cadastrada" : "Clique em \"Ver Avaliações\"")}\nSomatório: {matériaAtual.SomatórioObtido}/{matériaAtual.SomatórioTotal}")
                                                .WithFooter($"{_menuStates[msgId]["stateMatéria"] + 1}/{user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias.Count}")
                                                .Build();

                                    //                ||    check if there is any avaliações
                                    //                VV   
                                    x.Components = (matériaAtual.Avaliações.Count == 0) ? Components.GetMatériasButtons().Build()
                                                    : Components.GetMatériasButtons()
                                                        .WithButton("Ver Avaliações", "see-av-bttn") // <- this one has "ver avaliações"
                                                        .Build();
                                });

                                break;
                            
                            case "back-bttn":
                                if (_menuStates[msgId]["stateMatéria"] == 0) {
                                    _menuStates[msgId]["stateMatéria"] = user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias.Count - 1;
                                } else {
                                    _menuStates[msgId]["stateMatéria"]--; 
                                }

                                matériaAtual = user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias[_menuStates[msgId]["stateMatéria"]];

                                await componentInteraction.UpdateAsync(x => {
                                    x.Embed = new EmbedBuilder()
                                                .WithTitle(matériaAtual.Nome)
                                                .WithDescription($"Situação: {matériaAtual.Situação}\nAvaliações: {(matériaAtual.Avaliações.Count == 0 ? "Nenhuma avaliação cadastrada" : "Clique em \"Ver Avaliações\"")}\nSomatório: {matériaAtual.SomatórioObtido}/{matériaAtual.SomatórioTotal}")
                                                .WithFooter($"{_menuStates[msgId]["stateMatéria"] + 1}/{user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias.Count}")
                                                .Build();

                                            //            ||    Verifica se existem avaliações.
                                            //            VV   
                                    x.Components = (matériaAtual.Avaliações.Count == 0) ? Components.GetMatériasButtons().Build()
                                                    : Components.GetMatériasButtons()
                                                        .WithButton("Ver Avaliações", "see-av-bttn") // <- this one has "ver avaliações"
                                                        .Build();
                                });
                                break;

                            case "see-av-bttn":
                                matériaAtual = user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias[_menuStates[msgId]["stateMatéria"]];
                                List<Avaliação> avaliaçõesAtual = matériaAtual.Avaliações;
                                string description = "";
                                foreach (Avaliação avaliação in avaliaçõesAtual) {
                                    description += $"**Título**: {avaliação.Nome}";
                                    description += "\n";
                                    description += $"**Data**: {(string.IsNullOrWhiteSpace(avaliação.Data) ? "Não cadastrada" : avaliação.Data)}";
                                    description += "\n";
                                    description += $"**Data de Devolução**: {(string.IsNullOrWhiteSpace(avaliação.DataDevolução) ? "Não cadastrada" : avaliação.DataDevolução)}";
                                    description += "\n";
                                    description += $"**Nota**: {avaliação.Nota}/{avaliação.Valor}";
                                    description += "\n";
                                    description += "\n";
                                }

                                await componentInteraction.UpdateAsync(x => {
                                    x.Embed = new EmbedBuilder()
                                                .WithTitle($"Avaliações de {matériaAtual.Nome}")
                                                .WithDescription(description)
                                                .Build();
                                    x.Components = new ComponentBuilder()
                                                    .WithButton("Início", "home-bttn")
                                                    .WithButton("Voltar", "back-atv-bttn")
                                                    .Build();
                                });
                                break;

                            case "back-atv-bttn":
                                matériaAtual = user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias[_menuStates[msgId]["stateMatéria"]];
                                await componentInteraction.UpdateAsync(x => {
                                    x.Embed = new EmbedBuilder()
                                                .WithTitle(matériaAtual.Nome)
                                                .WithDescription($"Situação: {matériaAtual.Situação}\nAvaliações: {(matériaAtual.Avaliações.Count == 0 ? "Nenhuma avaliação cadastrada" : "Clique em \"Ver Avaliações\"")}\nSomatório: {matériaAtual.SomatórioObtido}/{matériaAtual.SomatórioTotal}")
                                                .WithFooter($"{_menuStates[msgId]["stateMatéria"] + 1}/{user.Notas[_menuStates[msgId]["stateEtapa"]].Matérias.Count}")
                                                .Build();
                                    x.Components = (matériaAtual.Avaliações.Count == 0) ? Components.GetMatériasButtons().Build()
                                                    : Components.GetMatériasButtons()
                                                        .WithButton("Ver Avaliações", "see-av-bttn") // <- this one has "ver avaliações"
                                                        .Build();
                                });
                                break;
                        }
                        break;           
                }
            }

            
        }
    }
}