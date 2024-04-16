using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeuIFMG_DiscordBot.modules;

namespace MeuIFMG_DiscordBot.models
{
    public class User
    {
        public string RA { get; set; }
        public string Password { get; set; }
        public ulong DiscordID { get; set; }
        public List<Etapa> Notas { get; set; }

        public User() { }

        public static async Task<User> CreateUserAsync(string ra, string password, ulong discordid)
        {
            Console.WriteLine("Criando usuário.");
            var notas = await PythonCaller.RequestNotas(ra.ToString(), password);
            
            return new User
            {
                RA = ra,
                Password = password,
                DiscordID = discordid,
                Notas = notas
            };
        }
    }
}