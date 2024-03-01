using BarberBookingAgent.Application.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace BarberBookingAgent.Application.Chat;

public record ChatMessageRequest
{
    public string SessionId { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}

public record ChatMessageResponse
{
    public string Message { get; init; } = string.Empty;
}

public class ChatMessageHandler
{
    private readonly Kernel _kernel;
    private readonly ChatHistoryStorage _chatHistoryStorage;

    public ChatMessageHandler(Kernel kernel, ChatHistoryStorage chatHistoryStorage)
    {
        _kernel = kernel;
        _chatHistoryStorage = chatHistoryStorage;
    }

    public async Task<ChatMessageResponse> Handle(ChatMessageRequest request)
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

        return new ChatMessageResponse { Message = agentMessage };
    }
}
