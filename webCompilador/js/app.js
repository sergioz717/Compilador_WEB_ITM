//var alphabet = ['a', 'b', 'c', 'd'];

//var er = '(a|bb*(a(b)))';//$('#er').val();
//console.log('ER validate is:', balancingGroupingSymbols(er));

function separarExpresiones(str) {
    var arr = [];
    var stack = [];

    for(var i = 0; i < str.length; i++) {
        var current = str.charAt(i);

        if (current === '(') {
            stack.push(i);
        } else if (current === ')') {
            var start = stack.pop();
            var end = i + 1;
            if (str.charAt(i + 1) === '*') {
                end = i + 2;
                i++;
            }
            var subStr = str.substring(start, end);
            arr.push(subStr);
        } else {
            if (stack.length === 0) {
                var previous = str.charAt(i-1);
                if (arr.length === 0 || previous === ')' || previous === '*') {
                    arr.push(current);
                } else {
                    arr.push(arr.pop() + current);
                }
            }
        }
    }

    return arr;
}

// Extrae un item aleatorio del array
function random(arr) {
    return arr[Math.floor(Math.random() * arr.length)];
}

// Separa la ER en un array para mejor procesamiento
function disjoin(str) {
    var arr = [];
    var current = null;
    var next = null;

    for(var i = 0; i < str.length; i++) {
        current = str.charAt(i);
        next = str.charAt(i+1);

        if (next != '*') {
            arr.push(current);
        } else {
            var substr = str.substring(i, i+2);
            arr.push(substr);
            i++;
        }
    }

    return arr
}

function generarSimbolos(str) {
    var max = 5;

    if (str.length === 1) {
        return str;
    } else if(str.length === 2 && str.charAt(1) === '*') {
        var rand = Math.floor(Math.random() * max);
        return str.charAt(0).repeat(rand);
    }

    return '';
}

function combine(arr) {
    var words = [];
    var max = 5;

    for(var i = 0; i < max; i++) {
        var word = ''
        for(var j = 0; j < arr.length; j++) {
            word += generarSimbolos(arr[j]);
        }
        words.push(word);
    }

    return words;
}

// Genera un array con max N palabras: a*
function generateWords(str) {
    var arr = [];
    var max = 10;
    var length = str.length;

    // agrupado por parentesis
    if (str.charAt(0) === '(') {
        if (str.charAt(length-1) === ')') {
            str = str.substring(1, length-1);
            arr = combine(disjoin(str));
        } else { // *
            str = str.substring(1, length-2);
            var expresiones = disjoin(str);

            // Combinar
            for(var i = 0; i < 5; i++) {
                var word = ''
                for(var j = 0; j < expresiones.length; j++) {
                    word += generarSimbolos(expresiones[j]);
                }
                if (i%2) {
                    arr.push('');
                } else {
                    arr.push(word);
                }
            }
        }

    } else { // sin parentesis
        arr = combine(disjoin(str))
    }

    return arr;
}

function combineMatrix(matrix) {
    var words = [];
    var max = 50;
    for(var i = 0; i < max; i++) {
        var word = '';
        for(var j = 0; j < matrix.length; j++) {
            word += random(matrix[j]);
        }
        words.push(word);
    }
    console.log('Combinando resultados');
    return words;
}

function removeDuplicates(arr) {
    var obj = {};
    var arr2 = [];
    for (var i = 0; i < arr.length; i++) {
        if (!(arr[i] in obj)) {
            arr2.push(arr[i]);
            obj[arr[i]] = true;
        }
    }
    console.log('Remover duplicados');
    return arr2;
}

function procesar(str) {
    var words = [];
    var matrix = [];
    var expresiones = separarExpresiones(str);
    console.log('Separando expresiones', expresiones);

    for(var i = 0; i < expresiones.length; i++) {
        matrix.push(generateWords(expresiones[i]))
    }

    words = combineMatrix(matrix);
    words = removeDuplicates(words)
    console.log(words);
    return words;
}


function isValid(str) {
    if (balancingGroupingSymbols(str) && validOrSimbol(str)) {
        return true;
    }

    return false;
}

function validOrSimbol(str) {
    var stack = [];
    var arr = str.split('');

    arr.forEach(function (element, index) {

        if (element === '(') {
            stack.push({
                start: index,
                end: null,
                value: element
            });
        } else if (element === ')') {
            stack.pop({
                start: null,
                end: index,
                value: element
            });

        }
        console.log(stack);
    })

    if (stack.length === 0) {
        return true;
    }

    return false;
}

function balancingGroupingSymbols(str) {
    var stack = [];
    var arr = str.split('');

    arr.forEach(function (element, index) {
        if (element === '(') {
            stack.push(element);
        } else if (element === ')') {
            stack.pop(element);
        }
        console.log(stack);
    })

    if (stack.length === 0) {
        return true;
    }

    return false;
}