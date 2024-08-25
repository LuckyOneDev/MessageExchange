import './App.css'
import { Route, Routes, Link } from "react-router-dom";
import { RealtimeMessages } from './RealtimeMessages';
import { LastMessages } from './LastMessages';
import { SendMessage } from './SendMessage';

function Navbar() {
    return <div style={{
        display: 'flex',
        flexDirection: 'row',
        justifyContent: 'space-evenly',
        position: 'absolute',
        top: "10px",
        left: 0,
        right: 0,
        zIndex: 1,
    }}>
        <Link to="/">Last messages</Link>
        <Link to="/send">Send message</Link>
        <Link to="/realtime">Realtime messages</Link>
    </div>;
}

function App() {
    return <>
        <Navbar />
        <Routes>

            <Route path="/" element={<LastMessages />} />
            <Route path="/send" element={<SendMessage />} />
            <Route path="/realtime" element={<RealtimeMessages />} />
        </Routes>
    </>;
}

export default App
