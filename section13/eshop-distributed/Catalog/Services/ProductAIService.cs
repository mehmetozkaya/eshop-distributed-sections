using Microsoft.Extensions.AI;

namespace Catalog.Services;

public class ProductAIService(IChatClient chatClient)
{
    public async Task<string> SupportAsync(string query)
    {
        var systemPrompt = """
        You are a useful assistant. 
        You always reply with a short and funny message. 
        If you do not know an answer, you say 'I don't know that.' 
        You only answer questions related to outdoor camping products. 
        For any other type of questions, explain to the user that you only answer outdoor camping products questions.
        At the end, Offer one of our products: Hiking Poles-$24, Outdoor Rain Jacket-$12, Outdoor Backpack-$32, Camping Tent-$22
        Do not store memory of the chat conversation.
        """;

        var chatHistory = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, systemPrompt),
            new ChatMessage(ChatRole.User, query)
        };
        
        var resultPrompt = await chatClient.CompleteAsync(chatHistory);
        return resultPrompt.Message.Contents[0].ToString()!;
    }
}
