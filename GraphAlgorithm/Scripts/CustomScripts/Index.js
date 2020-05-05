let cy = cytoscape({

    container: document.getElementById('cy'), // container to render in

    elements: [],

    style: [ // the stylesheet for the graph
        {
            selector: 'node',
            style: {
                'background-color': '#69e',
                'label': 'data(id)',
            }
        },

        {
            selector: 'edge',
            style: {
                'width': 1,
                'line-color': '#369',
                'curve-style': 'bezier',
                'target-arrow-color': '#369',
                'target-arrow-shape': 'triangle',
                'label': 'data(label)',
                'font-size': '14px',
                'color': '#777'
            }
        }
    ],

    layout: {
        name: 'grid',
        rows: 1
    }

});

cy.add([
    { group: 'nodes', data: { id: 'n1' }, position: { x: 50, y: 200 } },
    { group: 'nodes', data: { id: 'n2' }, position: { x: 131, y: 226 } },
    { group: 'nodes', data: { id: 'n3' }, position: { x: 128, y: 143 } },
    { group: 'nodes', data: { id: 'n4' }, position: { x: 249, y: 142 } },
    { group: 'nodes', data: { id: 'n5' }, position: { x: 191, y: 62 } },
    { group: 'nodes', data: { id: 'n6' }, position: { x: 66, y: 83 } },
    { group: 'edges', data: { id: 'e0', source: 'n1', target: 'n2', label: 7 } },
    { group: 'edges', data: { id: 'e1', source: 'n2', target: 'n3', label: 10 } },
    { group: 'edges', data: { id: 'e2', source: 'n1', target: 'n6', label: 14 } },
    { group: 'edges', data: { id: 'e3', source: 'n1', target: 'n3', label: 9 } },
    { group: 'edges', data: { id: 'e4', source: 'n2', target: 'n4', label: 15 } },
    { group: 'edges', data: { id: 'e5', source: 'n3', target: 'n4', label: 11 } },
    { group: 'edges', data: { id: 'e6', source: 'n3', target: 'n6', label: 2 } },
    { group: 'edges', data: { id: 'e7', source: 'n6', target: 'n5', label: 9 } },
    { group: 'edges', data: { id: 'e8', source: 'n5', target: 'n4', label: 6 } },
]);

function addEdges() {

    var amountOfEdge = cy.edges().length;
    cy.add({
        group: 'edges', data: {
            id: 'e' + amountOfEdge,
            source: document.getElementById('firstSelected').value,
            target: document.getElementById('secondSelected').value,
            label: document.getElementById('valueOfEdge').value,
        }
    });
}

window.onload = function () {

    var element = document.getElementById("addBtn");;
    var className = "btn btn-primary margin-right";
    var amountOfNode = cy.nodes().length;

    function supplayFunc(idElement) {
        element.className = className;

        element = document.getElementById(idElement);
        element.className = "btn btn-secondary margin-right";
        cy.removeListener('tap');
    }

    cy.on('tap', function (evt) {
        amountOfNode++;
        cy.add({
            group: 'nodes', data: { id: 'n' + amountOfNode },
            renderedPosition: { x: evt.renderedPosition.x, y: evt.renderedPosition.y }
        });
    });

    $("#deleteBtn").on('click', function (eventObject) {

        supplayFunc("deleteBtn");

        cy.on('tap', function (evt) {

            var node = evt.target;

            if (node != evt.cy)
                cy.remove(node);
        });

    });

    $("#addBtn").on('click', function (eventObject) {

        supplayFunc("addBtn");

        cy.on('tap', function (evt) {
            amountOfNode++;
            cy.add({
                group: 'nodes', data: { id: 'n' + amountOfNode },
                renderedPosition: { x: evt.renderedPosition.x, y: evt.renderedPosition.y }
            });
        });
    });

    $("#connectBtn").on('click', function (eventObject) {

        supplayFunc("connectBtn");

        cy.on('tap', 'node', function (evt) {
            $('#connectDialog').modal();

            var selectedId = evt.target.data('id');
            document.getElementById('firstSelected').value = selectedId;
            document.getElementById('secondSelected').value = '';
            document.getElementById('valueOfEdge').value = '';
            var element = document.getElementById('browsers');

            while (element.hasChildNodes()) {
                element.removeChild(element.firstChild);
            }

            for (var i = 0, length = cy.nodes().length; i < length; i++) {

                var nodeId = cy.nodes()[i].data('id');
                var child = document.createElement('option');
                child.value = nodeId;
                element.appendChild(child);
            }
        });
    });
}