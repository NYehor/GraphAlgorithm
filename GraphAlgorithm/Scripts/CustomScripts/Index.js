var amountOfNode = 0;
var amountOfEdge = 0;

let cy = cytoscape({

    container: document.getElementById('cy'), 

    elements: [],

    style: [ 
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
    ]
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
    amountOfEdge++;
    var val = document.getElementById('valueOfEdge').value;
    if (val == '')
        val = '1';

    cy.add({
        group: 'edges', data: {
            id: 'e' + amountOfEdge,
            source: document.getElementById('firstSelected').value,
            target: document.getElementById('secondSelected').value,
            directed: false,
            label: val,
        }
    });
}

function loadNewGraph(matrix, adjacency) {

    if (!adjacency)
        matrix = incidenceToAdjacency(matrix);

    cy.elements().remove();
    cy.add(convertToElements(matrix));

    cy.layout({
        name: 'circle',
        radius: 120,
    }).run();

    amountOfNode = cy.nodes().length;
    amountOfEdge = cy.edges().length;
}

function getAdjacencyMatrix() {

    var elementLength = cy.elements().length;
    if (elementLength == 0)
        return '';

    var matrix = [];
    matrix.length = cy.nodes().length;
    for (var i = 0; i < matrix.length; i++) {
        matrix[i] = [];
        matrix[i].length = matrix.length;
        for (var j = 0; j < matrix.length; j++) {
            matrix[i][j] = 0;
        }
    }
    var dict = {};
    for (var i = 0; i < matrix.length; i++) {
        dict[cy.nodes()[i].data('id')] = i;
    }

    var len = parseInt(cy.edges().length);

    if (len == 0)
        return matrix;

    for (var i = 0; i < len; i++) {

        var source = cy.edges()[i].data('source');
        var target = cy.edges()[i].data('target');

        matrix[dict[source]][dict[target]] = parseInt(cy.elements().edges()[i].data('label'));
    }

    var condition = cy.edges()[0].data('directed');
    if (!condition) {
        for (var i = 0; i < matrix.length; i++) {
            for (var j = 0; j < i; j++) {

                if (i == j) continue;

                if (matrix[j][i] == 0)
                    matrix[j][i] = matrix[i][j];
                else
                    matrix[i][j] = matrix[j][i];
            }
        }
    }

    return matrix;
}



