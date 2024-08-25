import dayjs from "dayjs";
import { ApiMessage } from "./ApiMessage";

export function MessageRenderer(props: { messages: ApiMessage[] }) {
    return <table style={{ width: '70vw' }}>
        <thead>
            <tr>
                <th>Index</th>
                <th>Date</th>
                <th>Text</th>
            </tr>
        </thead>
        <tbody>
            {props.messages.map((message, i) => <tr key={i}>
                <th>{message.index}</th>
                <th>{dayjs(message.date).format('YYYY-MM-DD HH:mm:ss')}</th>
                <th style={{ whiteSpace: "balance", wordBreak: "break-word" }}>{message.text}</th>
            </tr>)}
        </tbody>
    </table>
}