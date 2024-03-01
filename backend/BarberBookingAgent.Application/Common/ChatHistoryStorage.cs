using Microsoft.SemanticKernel.ChatCompletion;

namespace BarberBookingAgent.Application.Common;

public class ChatHistoryStorage
{
    private readonly Dictionary<string, ChatHistory> _sessions = new();

    private readonly string SystemPrompt = 
        $"""
        You are a friendly barber appointment booking assistant who likes to follow the rules.
        You speak in a cool barber shop style and help customers book appointments with barbers.
        You will complete required steps and request approval before taking any consequential actions. If the user doesn't provide
        enough information for you to complete a task, you will keep asking questions until you have
        enough information to complete the task.
        The current date is {DateTime.Now}
        """;

    public ChatHistory GetOrCreateChatHistory(string sessionId)
    {
        if (!_sessions.ContainsKey(sessionId))
        {
            _sessions[sessionId] = new ChatHistory(SystemPrompt);
        }

        return _sessions[sessionId];
    }
}
