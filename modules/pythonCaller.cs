using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using MeuIFMG_DiscordBot.models;
using MeuIFMG_DiscordBot.modules;

// Executa o processo do python pelo c#, o script em python printa todo o json em string
// e lê-mos por aqui.

public static class PythonCaller {
   private static async Task<string> PythonRun(string file, string args)
    {
        Console.WriteLine("[DEBUG] -> Running Python");
        ProcessStartInfo start = new()
        {
            FileName = "python",
            Arguments = $"{file} {args}",
            UseShellExecute = false,
            RedirectStandardOutput = true
        };

        using Process process = Process.Start(start);
        using StreamReader reader = process.StandardOutput;
        string result = await reader.ReadToEndAsync();
        return result;
    }

    // Roda o python, pega a string, transforma em objeto e retorna.
    public static async Task<List<Etapa>> RequestNotas(string user, string password)
    {
        Console.WriteLine($"[DEBUG] -> Request notas de {user}.");
        string pythonResponse = await PythonRun("pythonScripts/maincall.py", $"{user} {password}");
        Console.WriteLine($"[DEBUG] -> pythonResponse");

        List<Etapa> response = await jsonConverter.JsonToObjectAsync(pythonResponse);
        return response;
    }
}
