(function () {
    votingApi.get().then(render);
    votingApi.getStats().then(renderStats);
    votingApi.subscribe(renderStats);

    function render(state) {
        state.error ? (document.getElementById('errors').innerHTML = state.error)
            : renderMain(state);

        function renderMain(state) {
            utils.addEventHandler('start', () =>
                votingApi.start(document.getElementById('topics').value).then(render));
            utils.addEventHandler('finish', () =>
                votingApi.finish(state.votingId).then(render));

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
