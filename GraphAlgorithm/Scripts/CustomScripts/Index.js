var amountOfNode = 0;
var amountOfEdge = 0;
var isOrientedGraph = true;
var isWeightedGraph = false;

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
                'curve-style': 'bezier',
                'target-arrow-color': '#369',
                'target-arrow-shape': 'triangle',
                'label': '',
                'font-size': '14px',
                'color': '#777'
            }
        }
    ],
    wheelSensitivity: 0.2
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

        var message = document.getElementById('message');
        while (message.firstChild)
            message.removeChild(message.firstChild);
        message.textContent = "Виділіть і переміщуйте об'єкти.";
    });

    $("#deleteBtn").on('click', function (eventObject) {

        supplayFunc("deleteBtn");

        var message = document.getElementById('message');
        while (message.firstChild)
            message.removeChild(message.firstChild);
        message.textContent = "Клацніть по об'єкту, який хочете видалити";

        cy.on('tap', function (evt) {
            var node = evt.target;

            if (node != evt.cy)
                cy.remove(node);
        });

    });

    $("#addBtn").on('click', function (eventObject) {

        supplayFunc("addBtn");
        message.textContent = "Натисніть на робочу область, щоб додати вершину.";
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
        var message = document.getElementById('message');
        while (message.firstChild)
            message.removeChild(message.firstChild);
        message.textContent = "Виділіть першу вершину для створення дуги";

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
            directed: isOrientedGraph,
            weight: val
        }
    });
}

function isOriented(matrix) {
    var flag = false;

    for (var i = 0; i < matrix.length; i++)
        for (var j = 0; j < matrix.length; j++) {
            if (matrix[i][j] != matrix[j][i]) {
                flag = true;
                return flag;
            }
        }

    return flag
}

function isWeighted(matrix) {
    var flag = false;

    for (var i = 0; i < matrix.length; i++)
        for (var j = 0; j < matrix.length; j++) {
            if (matrix[i][j] != 1)
                if (matrix[i][j] != 0) {
                    flag = true;
                    return flag;
                }
        }

    return flag
}

function loadNewGraph(matrix, adjacency) {

    if (!adjacency)
        matrix = incidenceToAdjacency(matrix);

    if (isOriented(matrix))
        if (isWeighted(matrix))
            createOrientedWeightedGraph();
        else
            createDisorientedUnweightedGraph();
    else
        if (isWeighted(matrix))
            createDisorientedWeightedGraph();
        else
            createDisorientedUnweightedGraph();

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

        matrix[dict[source]][dict[target]] = parseInt(cy.elements().edges()[i].data('weight'));
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

function removeElement() {
    amountOfNode = 0;
    amountOfEdge = 0;
    cy.elements().remove();
}

function download() {
    var filename = document.getElementById('fileName').value;
    var matrix = getAdjacencyMatrix();
    var text = matrixToString(matrix);

    var element = document.createElement('a');
    element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
    element.setAttribute('download', filename);

    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}

function showIncidence() {
    $('#showIncidenceMatrix').modal();
    var matrix = getAdjacencyMatrix();
    matrix = adjacencyToIncidence(matrix);
    document.getElementById('textIncidenceMatrix').value = matrixToString(matrix);
}

function showAdjacency() {
    $('#showAdjacencyMatrix').modal();
    var matrix = getAdjacencyMatrix();
    document.getElementById('textAdjacencyMatrix').value = matrixToString(matrix);
}

function matrixToString(matrix) {
    var strValue = '';
    for (var i = 0; i < matrix.length; i++) {
        for (var j = 0; j < matrix[0].length; j++) {
            strValue += matrix[i][j].toString() + ", ";
        }
        strValue += '\n';
    }
    return strValue;
}

function adjacencySaveChanges() {
    var text = document.getElementById('textAdjacencyMatrix').value;
    var matrix = parseTextInput(text, ',');
    loadNewGraph(matrix, true)
}

function incidenceSaveChanges() {
    var text = document.getElementById('textIncidenceMatrix').value;
    var matrix = parseTextInput(text, ',');
    loadNewGraph(matrix, false)
}

function createOrientedUnweightedGraph() {
    removeElement();
    isOrientedGraph = true;
    isWeightedGraph = false;

    cy.style().selector('edge')
        .style({
            'target-arrow-color': '#369',
            'target-arrow-shape': 'triangle',
            'label': '',
        }).update();
}

function createDisorientedUnweightedGraph() {
    removeElement();
    isOrientedGraph = false;
    isWeightedGraph = false;

    cy.style().selector('edge')
        .style({
            'target-arrow-shape': 'none',
            'label': '',
        }).update();
}

function createOrientedWeightedGraph() {
    removeElement();
    isOrientedGraph = true;
    isWeightedGraph = true;

    cy.style().selector('edge')
        .style({
            'target-arrow-color': '#369',
            'target-arrow-shape': 'triangle',
            'label': 'data(weight)',
        }).update();
}

function createDisorientedWeightedGraph() {
    removeElement();
    isOrientedGraph = false;
    isWeightedGraph = true;

    cy.style().selector('edge')
        .style({
            'target-arrow-shape': 'none',
            'label': 'data(weight)',
        }).update();
}

function toStr(matrix) {
    var str = '';

    for (var i = 0; i < matrix.length; i++) {
        for (var j = 0; j < matrix[i].length; j++) {
            str += matrix[i][j] + ', ';
        }
        str += '\n';
    }

    return str;
}

function chooseNode(func) {

    $("#defaultBtn").click();

    var message = document.getElementById('message');
    while (message.firstChild)
        message.removeChild(message.firstChild);

    message.textContent = "Виберіть вершину.";

    cy.on('tap', function (evt) {
        var node = evt.target;

        try {
            func(cy.nodes().indexOf(node));
            cy.removeListener('tap');
        }
        catch (error) {
            console.log(error)
        }
    });
}

function showMessage(text) {
    var message = document.getElementById('message');
    while (message.firstChild)
        message.removeChild(message.firstChild);
    message.textContent = text;
}

function dialogShowMatrix(text, matrix) {

    var str = toStr(matrix);

    var message = document.getElementById('message');
    while (message.firstChild)
        message.removeChild(message.firstChild);

    var button = document.createElement('button');
    button.type = "button";
    button.className = "btn btn-primary";
    button.textContent = "Показати матрицю";
    button.style = "float:right";
    button.addEventListener('click', function () {
        $('#dialogModalShowMatrix').modal();
        document.getElementById('textMatrix').value = str;
    })
    message.textContent = text;
    message.appendChild(button);
}

document.getElementById('kruskalAlgorithmId').addEventListener('click', function () {
    var graphMatrix = getAdjacencyMatrix();
    var matrix = JSON.stringify(graphMatrix);

    if (graphMatrix.length > 0) {

        $.get('/Home/KruskalAlgorithm', $.param({ data: matrix }, true), function (data) {

            if (data.exception == "") {
                var message = "Мінімальна вага: " + data.minimalCost;
                dialogShowMatrix(message, data.matrix);
            }
            else
                showMessage(data.exception);
        });
    }
})

document.getElementById('primAlgorithmId').addEventListener('click', function () {
    var graphMatrix = getAdjacencyMatrix();
    var matrix = JSON.stringify(graphMatrix);

    if (graphMatrix.length > 0) {

        $.get('/Home/PrimAlgorithm', $.param({ data: matrix }, true), function (data) {

            if (data.exception == "") {
                var message = "Мінімальна вага: " + data.minimalCost;
                dialogShowMatrix(message, data.matrix);
            }
            else
                showMessage(data.exception);
        });
    }
})

document.getElementById('hamiltonianCycleAlgorithmId').addEventListener('click', function () {
    var graphMatrix = getAdjacencyMatrix();
    var matrix = JSON.stringify(graphMatrix);

    if (graphMatrix.length > 0) {

        $.get('/Home/HamiltonianCycleAlgorithm', $.param({ data: matrix }, true), function (data) {

            if (data.exception == "") {
                var message = "Гамільтонів цикл: ";
                for (var i = 0; i < data.path.length; i++)
                    message += cy.nodes()[data.path[i]].data('id') + '->';
                message += cy.nodes()[data.path[0]].data('id');

                dialogShowMatrix(message, data.matrix);
            }
            else
                showMessage(data.exception);
        });
    }
})

document.getElementById('floydWarshallSecondAlgorithmId').addEventListener('click', function () {
    var graphMatrix = getAdjacencyMatrix();
    var matrix = JSON.stringify(graphMatrix);

    if (graphMatrix.length > 0) {

        $.get('/Home/FloydWarshallSecondAlgorithm', $.param({ data: matrix }, true), function (data) {

            if (data.exception == "") {
                dialogShowMatrix("", data.matrix);
            }
            else
                showMessage(data.exception);
        });
    }
})

document.getElementById('floydWarshallFirstAlgorithmId').addEventListener('click', function () {
    var graphMatrix = getAdjacencyMatrix();
    var matrix = JSON.stringify(graphMatrix);

    if (graphMatrix.length > 0) {

        $.get('/Home/FloydWarshallFirstAlgorithm', $.param({ data: matrix }, true), function (data) {

            if (data.exception == "") {
                dialogShowMatrix("", data.matrix);
            }
            else
                showMessage(data.exception);
        });
    }
})

document.getElementById('maxMatchesAlgorithmId').addEventListener('click', function () {
    var graphMatrix = getAdjacencyMatrix();
    var matrix = JSON.stringify(graphMatrix);

    if (graphMatrix.length > 0) {

        $.get('/Home/MaxMatchesAlgorithm', $.param({ data: matrix }, true), function (data) {

            if (data.exception == "") {
                dialogShowMatrix("", data.matrix);
            }
            else
                showMessage(data.exception);
        });
    }
})

document.getElementById('dijkstraAlgorithmId').addEventListener('click', function () {
    var graphMatrix = getAdjacencyMatrix();
    var matrix = JSON.stringify(graphMatrix);

    if (graphMatrix.length > 0) {

        var func = function (id) {
            id++;
            $.get('/Home/DijkstraAlgorithm', $.param({ data: matrix, start: id }, true), function (responseData) {

                if (responseData.exception == "") {

                    
                    showMessage("");
                    var message = document.getElementById('message');
                    for (var i = 0; i < responseData.matrix.length; i++) {
                        var path = responseData.matrix[i].Path;
                        var stringPath = '[ ';
                        for (var j = 0; j < path.length; j++) {
                            stringPath += cy.nodes()[path[j] - 1].data('id');
                            stringPath += j < path.length - 1 ? ", " : " ]";
                        }
                        var index = responseData.matrix[i].VerticeNumber;
                        var vertex = cy.nodes()[index - 1].data('id');

                        var div = document.createElement('div');
                        div.textContent = "Вершина: " + vertex + " - Длина: " + responseData.matrix[i].PathLength + " - Путь: " + stringPath;
                        div.style = "float:left";
                        message.appendChild(div);
                        message.appendChild(document.createElement('br'));
                    }
                }
                else
                    showMessage(data.exception);
            });
        }

        chooseNode(func);
    }
})

document.getElementById('wideSearchTreeAlgorithmId').addEventListener('click', function () {
    var graphMatrix = getAdjacencyMatrix();
    var matrix = JSON.stringify(graphMatrix);

    if (graphMatrix.length > 0) {

        var func = function (id) {
            $.get('/Home/WideSearchTreeAlgorithm', $.param({ data: matrix, start: id }, true), function (data) {

                if (data.exception == "") {
                    var message = "Порядок обходу: ";
                    for (var i = 0; i < data.matrix.length - 1; i++)
                        message += cy.nodes()[data.matrix[i]].data('id') + '->';
                    message += cy.nodes()[data.matrix[data.matrix.length - 1]].data('id');
                    showMessage(message);
                }
                else
                    showMessage(data.exception);
            });
        }

        chooseNode(func);
    }
})

document.getElementById('deepSearchTreeAlgorithmId').addEventListener('click', function () {
    var graphMatrix = getAdjacencyMatrix();
    var matrix = JSON.stringify(graphMatrix);

    if (graphMatrix.length > 0) {

        var func = function (id) {
            $.get('/Home/DeepSearchTreeAlgorithm', $.param({ data: matrix, start: id }, true), function (data) {
                if (data.exception == "") {
                    var message = "Порядок обходу: ";              
                    for (var i = 0; i < data.matrix.length - 1; i++)
                        message += cy.nodes()[data.matrix[i]].data('id') + '->';
                    message += cy.nodes()[data.matrix[data.matrix.length - 1]].data('id');
                    showMessage(message);
                }
                else
                    showMessage(data.exception);
            });
        }

        chooseNode(func);
    }
})