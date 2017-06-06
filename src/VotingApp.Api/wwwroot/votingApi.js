const votingApi = (function () {
    const baseUrl = `${window.location.href}api/`;
    const apiUrl = `${baseUrl}voting/`;

    const client = (url, method, body) =>
        fetch(url, { method, headers: { Accept: 'application/json', 'Content-Type': 'application/json' }, body });
    
    const get = (url = apiUrl) =>
        client(url, 'GET')
        .then(r => r.json());
    
    const getStats = get.bind(null, `${baseUrl}votingstats/`);

    const start = (topics) =>
        client(apiUrl, 'POST', JSON.stringify(topics && topics.split(',')))
        .then(r => r.json());

    const finish = (votingId) =>
        client(`${apiUrl}${votingId}`, 'DELETE')
        .then(r => r.json());

    const vote = (votingId, topic) =>
        client(`${apiUrl}${votingId}`, 'PUT', `"${topic}"`)
        .then(r => r.json());

    const subscribe = (action) => {
        const webSocket = new WebSocket(`ws://${window.location.host}/ws`);
        webSocket.onmessage = msg => action(JSON.parse(msg.data))
    }

    return {
        get,
        getStats,
        start,
        finish,
        vote,
        subscribe
    }
})();
