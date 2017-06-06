const utils = (function () {
    const addEventHandler = (id, handler) => {
        var el = document.getElementById(id),
            elClone = el.cloneNode(true);
        el.parentNode.replaceChild(elClone, el);
        document.getElementById(id).addEventListener('click', handler, false);
    }

    const renderOptions= (rootId, className, list, getText, getClickHandler = () => '') => {
        document.getElementById(rootId).innerHTML = '';
        for (var item in list) {
            document.getElementById(rootId).innerHTML +=
                `<div class="${className}" id="${rootId}-${item}" onclick="${getClickHandler(item)}">
                    ${getText(item)}
                </div>`;
        }
    }

    return {
        addEventHandler,
        renderOptions
    }
})();
