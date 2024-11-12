grammar MiniScript;

// =====================
// Lexer Rules
// =====================

// Keywords
IF          : 'if';
ELSE        : 'else';
LOOP        : 'loop';
FOR         : 'for';
VAR         : 'var';
BREAK       : 'break';
CONTINUE    : 'continue';
NOT         : 'not';            // Added for 'not' operator
EXCLAMATION : '!';              // Added for '!' operator

// Operators and Separators
ASSIGN      : '=';
PLUS        : '+';
MINUS       : '-';
MUL         : '*';
DIV         : '/';
MOD         : '%';
POW         : '^';
LPAREN      : '(';
RPAREN      : ')';
LBRACE      : '{';
RBRACE      : '}';
COMMA       : ',';
SEMICOLON   : ';';
DOT         : '.';
LBRACKET    : '[';
RBRACKET    : ']';
COLON       : ':';

// Literals
STRING      : '\'' ( ~['\\\r\n] | '\\' . )* '\'';
FLOAT       : [0-9]+ '.' [0-9]* | '.' [0-9]+;
NUMBER      : [0-9]+;
BOOLEAN     : 'true' | 'false';

// Identifiers
IDENTIFIER  : [a-zA-Z_][a-zA-Z_0-9]*;

// Whitespace and Comments
WS          : [ \t\r\n]+ -> skip;
COMMENT     : '//' ~[\r\n]* ;
MULTILINE_COMMENT
            : '/*' .*? '*/' 
            ;

// =====================
// Parser Rules
// =====================

program
    : statement* EOF
    ;

statement
    : variableDeclaration
    | assignment
    | ifStatement
    | loopStatement
    | forStatement
    | breakStatement
    | continueStatement
    | expressionStatement
    | functionCallStatement
    | comment
    | multilineComment
    ;

variableDeclaration
    : VAR IDENTIFIER (ASSIGN expression)? SEMICOLON
    ;

assignment
    : (IDENTIFIER | arrayAccess) ASSIGN expression SEMICOLON
    ;

ifStatement
    : IF LPAREN expression RPAREN block (ELSE block)?
    ;

loopStatement
    : LOOP LPAREN expression RPAREN block
    ;

forStatement
    : FOR LPAREN (variableDeclaration | assignment)? SEMICOLON expression? SEMICOLON assignment? RPAREN block
    ;

breakStatement
    : BREAK SEMICOLON
    ;

continueStatement
    : CONTINUE SEMICOLON
    ;

expressionStatement
    : expression SEMICOLON
    ;

functionCallStatement
    : functionCall SEMICOLON
    ;

block
    : LBRACE statement* RBRACE
    ;

expression
    : comparisonExpression
    ;

comparisonExpression
    : additiveExpression (('>' | '<' | '>=' | '<=' | '==' | '!=') additiveExpression)?
    ;

additiveExpression
    : multiplicativeExpression ( (PLUS | MINUS) multiplicativeExpression )*
    ;

multiplicativeExpression
    : powerExpression ( (MUL | DIV | MOD) powerExpression )*
    ;

powerExpression
    : unaryExpression (POW powerExpression)?
    ;

unaryExpression
    : (PLUS | MINUS | NOT | EXCLAMATION) unaryExpression
    | primaryExpression
    ;

primaryExpression
    : NUMBER
    | FLOAT
    | BOOLEAN
    | STRING
    | functionCall
    | arrayAccess
    | IDENTIFIER
    | LPAREN expression RPAREN
    | arrayLiteral
    ;

functionCall
    : qualifiedIdentifier LPAREN (expression (COMMA expression)*)? RPAREN
    ;

qualifiedIdentifier
    : IDENTIFIER (DOT IDENTIFIER)*
    ;

arrayLiteral
    : LBRACKET (expression (COMMA expression)*)? RBRACKET
    ;

arrayAccess
    : IDENTIFIER LBRACKET expression RBRACKET
    ;

comment : COMMENT ;
multilineComment : MULTILINE_COMMENT ;

// Error handling
error : . ; // Catch-all for any unexpected characters