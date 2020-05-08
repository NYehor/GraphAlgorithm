let cy = cytoscape({

    container: document.getElementById('cy'), // container to render in

    elements: [],

    style: [ // the stylesheet for the graph
        {
            selector: 'node',
            style: {
                'background-color': '#69e',
                'text-valign': 'center',
                'label': 'data(id)',
            }
        },

        {
            selector: 'edge',
            style: {
                'width': 2,
                'line-color': '#369',
                //'curve-style': 'bezier',
                'target-arrow-color': '#369',
                'target-arrow-shape': 'triangle',
                'label': 'data(label)',
                'font-size': '14px',
                'color': '#777'
            }
        }
    ],

    layout: {
        name: 'circle',
        radius: 120,
    }

});

window.onload = function () {

    var element = document.getElementById("defaultBtn");
    var className = "btn btn-primary margin-right";

    function supplayFunc(idElement) {
        element.className = className;

        element = document.getElementById(idElement);
        element.className = "btn btn-secondary margin-right";
        cy.removeListener('tap');
    }

    $("#defaultBtn").on('click', function (eventObject) {

        supplayFunc("defaultBtn");
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
            var amountOfNode = cy.nodes().length;
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
