import { useCallback, useEffect, useState } from "react";
import { ApiMessage } from "./ApiMessage";
import dayjs from "dayjs";
import axios from "axios";
import { MessageRenderer } from "./MessageRenderer";

export function LastMessages() {
    const [messages, setMessages] = useState<ApiMessage[]>([]);

    const getData = useCallback(() => {
        axios.get('/api/MessageExchange/GetMessagesByDate', {
            params: {
                StartDate: dayjs().subtract(10, 'm').toJSON(),
                EndDate: dayjs().toJSON()
            }
        }).then((response) => {
            setMessages(response.data);
        });
    }, []);

    useEffect(() => {
        getData();
    }, [getData]);

    return <div>
        <h1>
            Messages from last 10 minutes
        </h1>
        <MessageRenderer messages={messages} />
    </div>;
}