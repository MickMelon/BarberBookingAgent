using BarberBookingAgent.WebApi.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace BarberBookingAgent.WebApi.Controllers;

public record ChatMessageRequest
{
    public string SessionId { get; init; } = null!;
    public string Message { get; init; } = null!;
}

public record ChatMessageResponse
{
    public string Message { get; init; } = null!;
}

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatHistoryStorage _chatHistoryStorage;
    private readonly Kernel _kernel;

    public ChatController(
        ChatHistoryStorage chatHistoryStorage,
        Kernel kernel)
    {
        _chatHistoryStorage = chatHistoryStorage;
        _kernel = kernel;
    }

    [HttpPost("message")]
    public async Task<ActionResult<ChatMessageResponse>> Message(
        [FromBody] ChatMessageRequest request)
    {
        ChatHistory chatHistory = _chatHistoryStorage.GetOrCreateChatHistory(request.SessionId);

        chatHistory.AddUserMessage(request.Message);

        OpenAIPromptExecutionSettings openAiPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        IChatCompletionService chatCompletionService = _kernel.Services.GetRequiredService<IChatCompletionService>();

        IReadOnlyList<ChatMessageContent> result = await chatCompletionService.GetChatMessageContentsAsync(
            chatHistory,
            executionSettings: openAiPromptExecutionSettings,
            kernel: _kernel);

        string agentMessage = result[0].Content!;

        chatHistory.AddAssistantMessage(agentMessage);

        return Ok(new ChatMessageResponse
        {
            Message = agentMessage
        });
    }
}
