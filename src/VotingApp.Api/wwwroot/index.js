(function () {
    votingApi.subscribe(setState);
    votingApi.getStats().then(setState);

    let state = {};
    function setState(newState)
    {
        state = Object.assign(state, newState); 
        render(state);
    }

    function render(state) {
        const isAdmin = window.location.search.indexOf('admin') !== -1;
        if (isAdmin){
            document.getElementById('voting').style.display = 'none';
            renderCommands(state)
        }
        else{
            document.getElementById('admin').style.display = 'none';
            renderVoting(state);
        }

        renderStats(state);
    }

    function renderCommands({votingId, error}) {
        const startHandler = _ => votingApi.start(document.getElementById('topics').value).then(setState);
        const finishHandler = _ => votingApi.finish(votingId).then(setState)

        utils.addEventHandler('startBtn', startHandler);

        document.getElementById('errors').innerHTML = '';
        error ? document.getElementById('errors').innerHTML = error 
        : utils.addEventHandler('finishBtn', finishHandler);
    }

    function renderVoting(state) {
        const voteHandler = (vote) => 
            votingApi.vote(state.votingId, vote).then(() => setState({vote}));

        utils.renderOptions('votingTopics',
            state.votes,
            (vote) => `option ${state.vote == vote ? 'selected' : 'active'}`,
            (vote) => vote,
            (vote) => voteHandler(vote));
    }

    function renderStats(state) {
        utils.renderOptions('votingStats',
            state.votes,
            _ => 'result',
            (vote) => `${vote} ${state.votes[vote].item1}%`);

        state.winner && (document.getElementById(`votingStats-${state.winner}`).className = 'result selected');
    }
})();
