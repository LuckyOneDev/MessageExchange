import axios from "axios";
import { useRef, useState } from "react";

export function SendMessage() {
    const inputRef = useRef<HTMLInputElement>(null);
    const [index, setIndex] = useState(0);
    return <div>
        <h1>Send message</h1>
        <p>Index: {index}</p>
        <input type="text" ref={inputRef} />
        <button onClick={() => {
            axios.post('/api/MessageExchange/SendMessage', {
                Index: index,
                Text: inputRef.current?.value
            }).then(() => {
                setIndex(index + 1);
            });
        }}>Send</button>
    </div>;
}