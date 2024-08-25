import { useState, useEffect, useRef } from "react";
import { MessageRenderer } from "./MessageRenderer";
import { ApiMessage } from "./ApiMessage";

export function RealtimeMessages() {
    const socket = useRef<WebSocket>();
    const [messages, setMessages] = useState<ApiMessage[]>([]);

    useEffect(() => {
        if (!socket.current) {
            socket.current = new WebSocket("/api/MessageExchange/Connect");
            socket.current.onopen = () => console.log("Socket connected");
            socket.current.onmessage = (event) => {
                setMessages((messages) => [...messages, JSON.parse(event.data) as ApiMessage]);
            };
        }
    }, []);

    return <div>
        <h1>Realtime messages</h1>
        <MessageRenderer messages={messages} />
    </div>;
}
