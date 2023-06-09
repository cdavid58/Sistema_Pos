﻿using System;
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

        if (response.IsSuccessStatusCode)
        {
            var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            var generatedText = responseObject.choices[0].message.content.Value;

            return generatedText;
        }
        else
        {
            throw new Exception("La solicitud a OpenAI no fue exitosa.");
        }
    }
}
class Program
{
    public static async Task RunOpenAI()
    {
        var apiKey = "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";
        var model = "gpt-3.5-turbo";
        var description = @"te llamas antonio reyes y eres desarrollador de software y ambisioso";
        var conversation = @"Hola antonio cuentame que quieres hacer hoy?";
        var openaiClient = new OpenAIClient(apiKey);
        var generatedResponse = await openaiClient.GenerateChatCompletion(model, description, conversation);
        Console.WriteLine(generatedResponse);
    }
}
