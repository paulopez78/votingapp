(function () {
    votingApi.subscribe(renderStats);
    votingApi.get().then(render);
    votingApi.getStats().then(renderStats);

    function render(state) {
        const isAdmin = window.location.search.indexOf('admin') !== -1;
        isAdmin ? renderCommands(state) : document.getElementById('admin').style.display = 'none';

        !state.error && renderVoting(state);
    }

    function renderCommands(state) {
        utils.addEventHandler('startBtn',
            () => votingApi.start(document.getElementById('topics').value).then(render));
        
        document.getElementById('errors').innerHTML = '';
        state.error 
            ? document.getElementById('errors').innerHTML = state.error
            : utils.addEventHandler('finishBtn', () => votingApi.finish(state.votingId).then(render));
    }

    function renderVoting(state) {
        utils.renderOptions('votingTopics', 'option active',
            state.topics,
            (topic) => topic,
            (topic) => `votingApi.vote('${state.votingId}', '${topic}')`);
    }

    function renderStats(state) {
        utils.renderOptions('votingStats', 'result',
            state.votes,
            (vote) => `${vote} ${state.votes[vote].item1}%`);

        state.winner && (document.getElementById(`votingStats-${state.winner}`).className = 'result selected');
    }
})();
