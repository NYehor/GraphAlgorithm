function parseTextInput(text, separator) {

    var strArr = text.split('\n');

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
            if (outputMatrix[i][j] == '') 
                outputMatrix[i].splice(j, 1);
            else
                j++;          
        }

        if (outputMatrix[i] == '') 
            outputMatrix.splice(i, 1);
        else 
            i++;
    }

    return outputMatrix;
}

function convertToElements(matrix) {

    var elements = [];

    for (var i = 0; i < matrix.length; i++) {

        elements.push({ group: 'nodes', data: { id: 'n' + (i+1) } });
    }

    var state = isDirection(matrix);
    var index = 0;
    for (var i = 0; i < matrix.length; i++) {
        for (var j = 0; j < i; j++) {
            if (matrix[i][j] != 0) {
                elements.push({
                    group: 'edges',
                    data: { id: 'e' + index, source: 'n' + (i + 1), target: 'n' + (j + 1), label: matrix[i][j], directed: state }
                });
                index++;
            }
            else
                if (matrix[j][i] != 0) {
                    elements.push({
                        group: 'edges',
                        data: { id: 'e' + index, source: 'n' + (j + 1), target: 'n' + (i + 1), label: matrix[j][i], directed: state }
                    });
                    index++;
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

function adjacencyToIncidence(matrix) {

    var incidence = [];
    incidence.length = matrix.length;
    for (var i = 0; i < incidence.length; i++)
        incidence[i] = [];

    var edge = 0;
    var cols = matrix.length;
    var rows = matrix.length;

    for (var col = 0; col < cols; col++) {
        for (var row = 0; row < col; row++) {
            if (matrix[col][row] != 0 || matrix[row][col] != 0) {
                for (var i = 0; i < incidence.length; i++) {
                    incidence[i].push(0);
                }
                
                if (matrix[col][row] == matrix[row][col]) {
                    incidence[row][edge] = matrix[col][row];
                    incidence[col][edge] = matrix[col][row];
                }
                else
                    if (matrix[col][row] != 0) {
                        incidence[row][edge] = - matrix[col][row];
                        incidence[col][edge] = matrix[col][row];
                    }
                    else {
                        incidence[row][edge] = matrix[row][col];
                        incidence[col][edge] = - matrix[row][col];
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
            adjacency[i].push(0);
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
            if (matrix[col][edge] < 0) 
                adjacency[row][col] = matrix[row][edge];
            else
                adjacency[col][row] = -matrix[row][edge];
        }
        else {
            adjacency[col][row] = adjacency[row][col] = matrix[row][edge]; 
        }
    }

    return adjacency;
}
