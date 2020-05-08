function ParseTextInput(text, separator) {

    let strArr = text.split('\n');

    var tmp;
    if (separator == ',')
        tmp = /\s/gi;
    else
        tmp = /,/gi;

    var outputMatrix = [strArr.length];

    for (var i = 0; i < strArr.length; i++) {
        
        outputMatrix[i] = strArr[i].replace(tmp,'').split(separator);
    }

    var i = 0;
    while (i < outputMatrix.length) {

        var j = 0;
        while (j < outputMatrix[i].length) {
            if (outputMatrix[i][j] == '') {
                outputMatrix[i].splice(j, 1);
            } else {
                ++j;
            }
        }

        if (outputMatrix[i] == '') {
            outputMatrix.splice(i, 1);
        } else {
            ++i;
        }
    }

    return outputMatrix;
}

function convertToElements(matrix) {

    var elements = [];

    for (var i = 0; i < matrix.length; i++) {

        elements.push({ group: 'nodes', data: { id: 'n' + i } });
    }

    var state = isDirection(matrix);
    if (!state) {
        var index = 0;
        for (var i = 0; i < matrix.length; i++) {
            for (var j = 0; j < i; j++) {

                if (matrix[i][j] != 0) {

                    elements.push({
                        group: 'edges',
                        data: { id: 'e' + index, source: 'n' + i, target: 'n' + j, label: matrix[i][j], directed: 'false' }
                    });
                    index++;
                }
            }
        }
    }
    else {

        var index = 0;
        for (var i = 0; i < matrix.length; i++) {
            for (var j = 0; j < matrix.length; j++) {

                if (matrix[i][j] != 0) {

                    elements.push({
                        group: 'edges',
                        data: { id: 'e' + index, source: 'n' + i, target: 'n' + j, label: matrix[i][j], directed:'true' }
                    });
                    index++;
                }
            }
        }

    }

    return elements;
}

function isDirection(matrix) {

    var flag = true;

    for (var i = 0; i < matrix.length; i++) {
        for (var j = 0; j < i; j++) {

            if (matrix[i][j] == matrix[j][i] && i != j && matrix[j][i] != 0)
                flag = false;
        }
    }

    return flag;
}

function getAdjacencyMatrix(cy) {

    var elementLength = cy.elements().length;
    if (elementLength == 0)
        return '';
    
    var matrix = [];
    matrix.length = cy.elements().nodes().length;
    for (var i = 0; i < matrix.length; i++) {
        matrix[i] = [];
        matrix[i].length = matrix.length;
        for (var j = 0; j < matrix.length; j++) {
            matrix[i][j] = "0";
        }
    }
    var dict = {};
    for (var i = 0; i < matrix.length; i++) {
        dict[cy.elements().nodes()[i].data('id')] = i;
    }

    var length = cy.elements().edges().length;
    for (var i = 0; i < length; i++) {

        var source = cy.elements().edges()[i].data('source');
        var target = cy.elements().edges()[i].data('target');

        matrix[dict[source]][dict[target]] = cy.elements().edges()[i].data('label');
    }

    var condition = cy.elements().edges()[0].data('directed');
    if (condition == "false") {
        for (var i = 0; i < matrix.length; i++) {
            for (var j = 0; j < i; j++) {

                if (i == j) continue;
                matrix[j][i] = matrix[i][j];
            }
        }
    }
    
    return matrix;
}

function adjacencyToIncidence(matrix) {

    var incidence = [];
    incidence.length = matrix.length;
    for (var i = 0; i < incidence.length; i++)
        incidence[i] = [];

    var edge = 0;
    var cols = matrix.length;
    var rows = matrix[0].length;

    for (var col = 0; col < cols; col++) {
        for (var row = 0; row < col; row++) {
            if (matrix[col][row] != 0) {
                for (var i = 0; i < incidence.length; i++) {
                    incidence[i].push('0');
                }
                
                if (matrix[col][row] == matrix[row][col]) {
                    incidence[row][edge] = matrix[col][row];
                    incidence[col][edge] = matrix[col][row];
                }
                else {
                    incidence[row][edge] = - matrix[col][row];
                    incidence[col][edge] = matrix[col][row];
                }

                edge++;
            }
        }
    }


    return incidence;
}

function incidenceToAdjacency(matrix) {

    var edges = matrix[0].length;
    var vertices = matrix.length;

    var adjacency = [];
    adjacency.length = matrix.length;
    for (var i = 0; i < adjacency.length; i++) {
        adjacency[i] = [];
        for (var j = 0; j < adjacency.length; j++)
            adjacency[i].push('0');
    };

    for (var edge = 0; edge < edges; edge++) {
        var vertex = 0;
        var row = Infinity;
        var col = Infinity;
        for (; vertex < vertices; vertex++)
            if (matrix[vertex][edge] != 0) {
                row = vertex;
                break;
            }
        vertex++;
        for (; vertex < vertices; vertex++)
            if (matrix[vertex][edge] != 0) {
                col = vertex;
                break;
            }

        if (matrix[col][edge] < 0 || matrix[row][edge] < 0) {
            adjacency[col][row] = - matrix[row][edge];
        }
        else {
            adjacency[col][row] = adjacency[row][col] = matrix[row][edge]; 
        }
    }

    return adjacency;
}