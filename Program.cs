using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MeuIFMG_DiscordBot;
using MeuIFMG_DiscordBot.modules;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    // starting variables
    private static DiscordSocketClient _client;
    private static CommandService _commands;
    private static IServiceProvider _services;

    private static Task LogTask(LogMessage msg) // logging part
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    } 

    public static async Task Main() { // starting the bot and setting some variables
        _client = new DiscordSocketClient(new DiscordSocketConfig {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        });
        _commands = new CommandService();
        _services = new ServiceCollection().BuildServiceProvider();

        _client.Log += LogTask;

        var token = "Token aqui!! XD";

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await RegisterCommandsAsync();

        await Task.Delay(-1);
    }

    private static async Task RegisterCommandsAsync()
    {
        _client.MessageReceived += HandleCommandsAsync;
        _client.ButtonExecuted += interactionHandler.HandleButtonAsync;
        

        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }

    private static async Task HandleCommandsAsync(SocketMessage messageParam)
    {
        var message = messageParam as SocketUserMessage;
        var context = new SocketCommandContext(_client, message);

        if (message.Author.IsBot) return;
        if (message.Channel is IDMChannel) {
            //Console.WriteLine("Message DM.");
        }
        else { 
            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
            }
        }
    }
}