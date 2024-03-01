import "./index.css";
import { useState } from "react";
import "@chatscope/chat-ui-kit-styles/dist/default/styles.min.css";
import {
  MainContainer,
  ChatContainer,
  MessageList,
  Message,
  MessageInput,
} from "@chatscope/chat-ui-kit-react";

type ChatMessageContent = {
  message: string;
  role: string;
};

function App() {
  const [chatHistory, setChatHistory] = useState<ChatMessageContent[]>([]);
  const [isSending, setIsSending] = useState(false);

  const handleSend = async (value: string) => {
    setIsSending(true);
    setChatHistory([...chatHistory, { message: value, role: "user" }]);

    const response = await fetch("https://localhost:7285/api/Chat/message", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ sessionId: "my-session", message: value }),
    });

    if (!response.ok) {
      setIsSending(false);
      throw new Error("Network response was not ok");
    }

    const content: ChatMessageContent = await response.json();

    setChatHistory([
      ...chatHistory,
      { message: value, role: "user" },
      { message: content.message, role: "assistant" },
    ]);
    setIsSending(false);
  };

  return (
    <div className="px-8 pt-4">
      <h1 className="text-3xl pb-2">Barber Booking Agent</h1>
      <MainContainer>
        <ChatContainer>
          <MessageList>
            {chatHistory.map((chatMessage) => (
              <Message
                model={{
                  message: chatMessage.message,
                  sentTime: "just now",
                  sender: chatMessage.role === "user" ? "me" : "them",
                  direction:
                    chatMessage.role === "user" ? "outgoing" : "incoming",
                  position: "normal",
                }}
              />
            ))}
            {isSending && <div>Typing...</div>}
          </MessageList>
          <MessageInput placeholder="Type message here" onSend={handleSend} />
        </ChatContainer>
      </MainContainer>
    </div>
  );
}

export default App;
