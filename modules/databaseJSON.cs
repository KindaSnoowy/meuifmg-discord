using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using MeuIFMG_DiscordBot.models;
using Newtonsoft.Json;

namespace MeuIFMG_DiscordBot.modules
{
    public class databaseJSON
    {
        readonly static string filePath = @"database\users.json";
        
        // CreateEntry -> Cria a entrada no banco de dados
        public static async Task CreateEntry(User usuario)
        {
            string fileContents = await File.ReadAllTextAsync(filePath);
            List<User> usersJSON = JsonConvert.DeserializeObject<List<User>>(fileContents) ?? new List<User>();
            usersJSON.Add(usuario);

            string updatedJSON = JsonConvert.SerializeObject(usersJSON, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, updatedJSON);
        }

        // UpdateEntry -> Atualiza a entrada com um objeto Usuário
        public static async Task UpdateEntry(User usuarioUpdated) {
            Console.WriteLine($"[DEBUG] -> Atualizando {usuarioUpdated.RA}");
            string fileContents = await File.ReadAllTextAsync(filePath);
            List<User> userList = JsonConvert.DeserializeObject<List<User>>(fileContents);

            User existingUser = userList.FirstOrDefault(u => u.RA == usuarioUpdated.RA);
            if (existingUser != null)
            {
                existingUser.RA = usuarioUpdated.RA;
                existingUser.Password = usuarioUpdated.Password;
                existingUser.DiscordID = usuarioUpdated.DiscordID;
                existingUser.Notas = usuarioUpdated.Notas;

                string updatedJSON = JsonConvert.SerializeObject(userList, Formatting.Indented);

                await File.WriteAllTextAsync(filePath, updatedJSON);
                Console.WriteLine($"[DEBUG] -> Usuário {usuarioUpdated.RA} atualizado!");
                //throw new FindingUserError("Usuário não encontrado.");
            }
            else
            {
                throw new FindingUserError("Usuário não encontrado.");
            }
        }

        // -> Deleta Entrada no banco de dados
        public static async Task DeleteEntry(string param, string searchParam)
        {
            string fileContents = await File.ReadAllTextAsync(filePath);
            List<User> userList = JsonConvert.DeserializeObject<List<User>>(fileContents);

            if (param == "RA")
            {
                var existingUser = await ReadRAEntryAsync(searchParam);

                if (existingUser != null)
                {
                    userList.RemoveAll(s => s.RA == searchParam); 
                    string updatedJSON = JsonConvert.SerializeObject(userList, Formatting.Indented);
                    await File.WriteAllTextAsync(filePath, updatedJSON);
                    Console.WriteLine($"Usuário com RA {searchParam} deletado com sucesso.");
                }
                else
                {
                    Console.WriteLine($"Usuário com RA {searchParam} não encontrado.");
                }
            }
            else if (param == "ID")
            {
                var existingUser = await ReadIDEntryAsync(ulong.Parse(searchParam));

                if (existingUser != null)
                {
                    userList.RemoveAll(s => s.DiscordID == ulong.Parse(searchParam)); 
                    string updatedJSON = JsonConvert.SerializeObject(userList, Formatting.Indented);
                    await File.WriteAllTextAsync(filePath, updatedJSON);
                    Console.WriteLine($"Usuário com ID {searchParam} deletado com sucesso.");
                }
                else
                {
                    Console.WriteLine($"Usuário com ID {searchParam} não encontrado.");
                }
            }
            else
            {
                throw new Exception("Parâmetro inválido.");
            }
        }

        // ==== Métodos de leitura ==== 

        //readRA e readID -> retorna User
        public static async Task<User> ReadRAEntryAsync(string RA)
        {
            Console.WriteLine($"[DEBUG] -> ReadRAEntryAsync: {RA}");
            string fileContents = await File.ReadAllTextAsync(filePath);
            List<User> userList = JsonConvert.DeserializeObject<List<User>>(fileContents);

            foreach (var user in userList)
            {
                if (RA == user.RA)
                {
                    Console.WriteLine("[DEBUG] -> Usuário encontrado.");
                    return user;
                }
            }
            return null;
        }

        public static async Task<User> ReadIDEntryAsync(ulong DiscordID)
        {
            Console.WriteLine($"[DEBUG] -> ReadIDEntryAsync: {DiscordID}");
            string fileContents = await File.ReadAllTextAsync(filePath);
            List<User> userList = JsonConvert.DeserializeObject<List<User>>(fileContents);

            foreach (var user in userList)
            {  
                if (DiscordID == user.DiscordID)
                {
                    Console.WriteLine("[DEBUG] -> Usuário encontrado.");
                    return user;
                }
            }
            return null;
        }
    }
}