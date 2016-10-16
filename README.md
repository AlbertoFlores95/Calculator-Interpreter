# Calculator-Interpreter

Matemáticas Computacionales
Prof. Carlos Blanco
22 de septiembre de 2016 - 20 de octubre de 2016

Resumen
En esta asignación el alumno escribirá un interprete para un lenguaje de programación. El
alumno también definirá una gramática independiente de contexto para dicho lenguaje.

1. Diferencia entre interpretes y compiladores
Un interprete es un programa que es capaz de analizar y ejecutar otros programas escritos en
un lenguaje de alto nivel. Los interpretes solo realizan la traducción a medida que es necesaria y
normalmente no almacenan el resultado de dicha traducción. Algunos ejemplos de lenguajes que
comúnmente son interpretados son Python, JavaScript y Perl.
Un compilador es un programa informático que traduce un programa escrito en un lenguaje
de programación a otro lenguaje de programación, comúnmente este es un lenguaje de bajo nivel
e incluso el lenguaje maquina, aunque en ocasiones este lenguaje puede ser también un lenguaje
intermedio como el bytecode. Algunos ejemplos de lenguajes que comúnmente son compilados son C,
Pascal y Java.

2. Asignación
El alumno diseñara una gramática independiente de contexto que describa el lenguaje para una
calculadora. El alumno también escribirá un interprete para este dicho lenguaje. El lenguaje debe
cumplir con las siguientes características: (1 - 4 son el requerimiento mínimo; 15 pts)

    1. Poder ejecutar las operaciones de suma (+), resta (-), multiplicación (*), división (/), potencia
    (ˆ), modulo ( %) y la agrupación por medio de paréntesis.
    2. Tener la habilidad de declarar variables y asignarles el resultado de una expresión mediante el
    símbolo de igualdad (=). 26 variables como mínimo. Cada letra del alfabeto ingles.
    3. El lenguaje debe poseer dos variables definidas por defecto; pi = 3.141592653589793 y e =
    2.718281828459045. (Pueden tomar el lugar de dos variables de las 26 mínimas)
    4. La evaluación de cada sentencia debe ser impresa como salida, a menos que la sentencia sea
    una asignación.
    5. Poder utilizar sentencias if-then-else, while y for. (5 pts)
    6. Habilidad para definir funciones en el lenguaje. (5 pts)
