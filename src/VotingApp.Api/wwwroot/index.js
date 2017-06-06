(function () {
    votingApi.subscribe(setState);
    votingApi.getStats().then(setState);

    let state = {};
    function setState(newState)
    {
        const errorMessage = newState.error || '';
        state = Object.assign(state, newState, { error: errorMessage }); 
        render(state);
    }

    function render(state) {
        const isAdmin = window.location.search.indexOf('admin') !== -1;
        isAdmin ? renderCommands(state) : renderVoting(state);
        renderStats(state);
    }

    function renderCommands({votingId, error}) {
        document.getElementById('voting').style.display = 'none';
        document.getElementById('errors').innerHTML = error || '';

        const startHandler = _ => votingApi.start(document.getElementById('topics').value).then(setState);
        utils.addEventHandler('startBtn', startHandler);

        const finishHandler = _ => votingApi.finish(votingId).then(setState)
        utils.addEventHandler('finishBtn', finishHandler);        
    }

    function renderVoting(state) {
        document.getElementById('admin').style.display = 'none';

        const voteHandler = (vote) => votingApi.vote(state.votingId, vote).then(() => setState({vote}));
        utils.renderOptions('votingTopics',
            state.votes,
            (vote) => `option ${state.vote == vote ? 'selected' : 'active'}`,
            (vote) => vote,
            'Voting will start in a moment...',
            (vote) => voteHandler(vote));
    }

    function renderStats(state) {
        utils.renderOptions('votingStats',
            state.votes,
            _ => 'result',
            (vote) => `${vote} ${state.votes[vote].item1}%`);

        state.winner 
        && (document.getElementById(`votingStats-${state.winner}`).className = 'result selected');
    }
})();
