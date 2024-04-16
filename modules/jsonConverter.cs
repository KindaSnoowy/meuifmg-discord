using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MeuIFMG_DiscordBot.modules
{
    public class jsonConverter
    {
        // -> Recebe a string em json e retorna uma lista de Etapas -> Notas.
        public static async Task<List<Etapa>> JsonToObjectAsync(string json)
        {
            Console.WriteLine($"[DEBUG] -> pythonJsonToObject");
            
            var jsonObject = await Task.Run(() => JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json));

            List<Etapa> Notas = new();
            foreach (var etapa in jsonObject)
            {
                Etapa etapaAtual = new() { Nome = etapa.Key };

                List<Matéria> listMatérias = new();
                foreach (var matéria in etapa.Value)
                {
                    JObject matériaContent = (JObject)matéria.Value;

                    Matéria matériaAtual = new()
                    {
                        Nome = matéria.Key,
                        Situação = (string)matériaContent["Situação"],
                        Avaliações = new(),
                    };

                    if (matériaContent["Somatórios"] != null)
                    {
                        matériaAtual.SomatórioObtido = (float)matériaContent["Somatórios"]["SomatórioAtual"];
                        matériaAtual.SomatórioTotal = (float)matériaContent["Somatórios"]["SomatórioTotal"];
                    }

                    if (matériaContent["Avaliações"].GetType() != typeof(JArray))
                    {
                        // -> Se não tem avaliação, a lista de avaliações é simplesmente nula.
                        // Console.WriteLine("Nenhuma avaliação encontrada.");
                    }
                    else
                    {
                        foreach (JObject avaliação in matériaContent["Avaliações"].Cast<JObject>())
                        {
                            Avaliação avaliaçãoAtual = new()
                            {
                                Nome = (string)avaliação["Avaliação"],
                                Data = (string)avaliação["Data da Avaliação"],
                                DataDevolução = (string)avaliação["Data de Devolução"],
                                Valor = string.IsNullOrEmpty((string)avaliação["Valor da Avaliação"]) ? 0 : float.Parse((string)avaliação["Valor da Avaliação"]),
                                Nota = string.IsNullOrEmpty((string)avaliação["Nota Obtida"]) ? 0 : float.Parse((string)avaliação["Nota Obtida"])
                            };

                            matériaAtual.Avaliações.Add(avaliaçãoAtual);
                        }
                    }

                    listMatérias.Add(matériaAtual);
                }
                etapaAtual.Matérias = listMatérias;
                Notas.Add(etapaAtual);
            }

            return Notas;
        }
    }
}