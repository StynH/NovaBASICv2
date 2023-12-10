window.NovaBasic = {
    // Set defaultToken to invalid to see what you do not tokenize yet
    defaultToken: 'invalid',

    // The main tokenizer for our languages
    tokenizer: {
        root: [
            // keywords and guards
            [/\b(?:LET|IMMUTABLE|STRUCT|ENDSTRUCT|ENDFUNC|GUARD|MATCHES|ELSE|ENDGUARD|IF|THEN|ELSEIF|ENDIF|SWITCH|ENDSWITCH|CASE|DEFAULT|RETURN|BREAK|FOR|TO|STEP|ENDFOR|REPEAT|UNTIL|WHILE|ENDWHILE|PRINT|COUNT|TO_ARRAY|REF|FUNC|ENDFUNC|NEW|NOT|null|true|false)\b/, 'keyword'],

            // numbers
            [/\b\d+\b/, 'number'],

            // strings
            [/"/, { token: 'string.quote', bracket: '@open', next: '@string' }],

            // whitespace
            { include: '@whitespace' },

            // delimiters and operators
            [/[{}()\[\]]/, '@brackets'],
            [/@symbols/, 'delimiter'],

            [/\d*\.\d+([eE][\-+]?\d+)?/, 'number.float'],
            [/\d+/, 'number'],
        ],

        whitespace: [
            [/[ \t\r\n]+/, 'white'],
            [/\/\/.*$/, 'comment'],
        ],

        string: [
            [/[^"]+/, 'string'],
            [/"/, { token: 'string.quote', bracket: '@close', next: '@pop' }],
        ],
    },

    // we include these common regular expressions
    symbols: /[=><!~?:&|+\-*\/\^%]+/,

    // The operators and delimiters
    operators: [
        '=', '>', '<', '!', '~', '?', ':',
        '&&', '||', '++', '--', '+', '-', '*', '/',
        '&', '|', '^', '%', '<<', '>>', '>>>',
        '+=', '-=', '*=', '/=', '&=', '|=', '^=', '%=',
        '<<=', '>>=', '>>>='
    ],

    // define our own brackets as '<' and '>' do not match in javascript
    brackets: [
        ['{', '}', 'delimiter.curly'],
        ['[', ']', 'delimiter.square'],
        ['(', ')', 'delimiter.parenthesis'],
        ['<', '>', 'delimiter.angle']
    ]
};