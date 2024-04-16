using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeuIFMG_DiscordBot.models;

namespace MeuIFMG_DiscordBot.modules
{
    public class syncModule
    {
        // forces user synchronization
        public static async Task UserSync(string param, string searchParam) {
            Console.WriteLine($"[DEBUG] -> UserSync {param}: {searchParam}"); 
            if (param == "RA") {
                var existingUser = await databaseJSON.ReadRAEntryAsync(searchParam) ?? throw new FindingUserError("Usuário não encontrado.");
                List<Etapa> notas = await PythonCaller.RequestNotas(existingUser.RA, existingUser.Password);
                existingUser.Notas = notas;
                Console.WriteLine($"[DEBUG] -> Notas de {existingUser.RA} adquiridas.");
                await databaseJSON.UpdateEntry(existingUser);
            } else if (param == "ID") {
                var existingUser = await databaseJSON.ReadIDEntryAsync(ulong.Parse(searchParam)) ?? throw new FindingUserError("Usuário não encontrado.");
                List<Etapa> notas = await PythonCaller.RequestNotas(existingUser.RA, existingUser.Password);
                existingUser.Notas = notas;
                await databaseJSON.UpdateEntry(existingUser);
            } else {
                throw new Exception("Valor inválido.");
            }
        }
    }
}