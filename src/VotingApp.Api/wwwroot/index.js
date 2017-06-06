(function () {
    votingApi.subscribe(renderStats);
    votingApi.get().then(render);
    votingApi.getStats().then(renderStats);

    utils.addEventHandler('startBtn',
        () => votingApi.start(document.getElementById('topics').value).then(render));

    function render(state) {
        if (state.error) {
            document.getElementById('errors').innerHTML = state.error;
        }
        else {
            document.getElementById('errors').innerHTML = '';
            utils.addEventHandler('finishBtn', () => votingApi.finish(state.votingId).then(render));
            utils.renderOptions('votingTopics', 'option active',
                state.topics,
                (topic) => topic,
                (topic) => `votingApi.vote('${state.votingId}', '${topic}')`);
        }
    }

    function renderStats(state) {
        utils.renderOptions('votingStats', 'result',
            state.votes,
            (vote) => `${vote} ${state.votes[vote].item1}%`);

        state.winner && (document.getElementById(`votingStats-${state.winner}`).className = 'result selected');
    }
})();
