const utils = (function () {
    const addEventHandler = (id, handler) => {
        var el = document.getElementById(id),
            elClone = el.cloneNode(true);
        el.parentNode.replaceChild(elClone, el);
        document.getElementById(id).addEventListener('click', handler, false);
    }

    const renderOptions = (rootId, list, getClass, getText, clickHandler) => {
        document.getElementById(rootId).innerHTML = '';
        
        for (var item in list) {
            document.getElementById(rootId).innerHTML +=
                `<div class="${getClass(item)}" id="${rootId}-${item}"">
                    ${getText(item)}
                </div>`;
        }

        if (clickHandler) {
            for (let item in list) {
                addEventHandler(`${rootId}-${item}`, () => clickHandler(item));
            }
        }
    }

    return {
        addEventHandler,
        renderOptions
    }
})();
