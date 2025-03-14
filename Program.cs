using System.ClientModel;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using OpenAI;

var endpoint = "http://localhost:1234";
var openaiClient = new OpenAIClient(new ApiKeyCredential("api-key"), new OpenAIClientOptions
{
    Endpoint = new Uri(endpoint),
});

var lmAgent = new OpenAIChatAgent(
    chatClient: openaiClient.GetChatClient("<does-not-matter>"),
    name: "assistant")
    .RegisterMessageConnector()
    .RegisterPrintMessage();

string? UserQuery;
while (true)
{
    Console.WriteLine("Enter your prompt Below:");
    UserQuery=Console.ReadLine();

    if ("quit,exit,close".Contains(UserQuery.ToLower()))
        break;

    await lmAgent.SendAsync(UserQuery);
}
