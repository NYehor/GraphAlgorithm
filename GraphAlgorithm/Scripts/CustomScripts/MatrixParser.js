function ParseTextInput(matrix, separator) {

    let text = document.getElementById(matrix).value;
    let strArr = text.split('\n');

    var outputMatrix = [strArr.length];

    for (var i = 0; i < strArr.length; i++) {

        outputMatrix[i] = strArr[i].trim().split(separator);
    }

    return outputMatrix;
}