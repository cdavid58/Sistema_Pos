using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class OpenAIClient
{
    private readonly string apiKey;
    private readonly HttpClient httpClient;

    public OpenAIClient(string apiKey)
    {
        this.apiKey = apiKey;
        this.httpClient = new HttpClient();
        this.httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<string> GenerateChatCompletion(string model, string description, string conversation)
    {
        var requestBody = new
        {
            model = model,
            messages = new List<object>
            {
                new { role = "system", content = description },
                new { role = "user", content = conversation }
            }
        };

        var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
        var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", httpContent);
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Analizar y extraer la respuesta generada del JSON de respuesta
        var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResponse);
        var generatedText = responseObject.choices[0].message.content.Value;

        return generatedText;
    }
}
class Program
{
    static async Task Main(string[] args)
    {
        var apiKey = "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";
        var model = "gpt-3.5-turbo";
        var description = @"
            Eres una niña llamada Evangelí
            Te gusta jugar a la pelota y conocer gente nueva para jugar.
            Te gusta ir al parque pero no te gusta ir de noche porque te da miedo.
        ";
        var conversation = @"
            David, el vecino que vive enfrente, inicia una conversación contigo.
            Solo debes dar una respuesta a la vez, y esperar a que David te responda.

            A continuacion aparece la conversación hasta el punto actual.
            Agrega solo una respuesta a la vez.
            David: 
        ";

        var openaiClient = new OpenAIClient(apiKey);
        var generatedResponse = await openaiClient.GenerateChatCompletion(model, description, conversation);

        Console.WriteLine(generatedResponse);
    }
}
