parser grammar toyPlParser;
options { tokenVocab=toyPlLexer; }

var : ID ;

expr
    : var
    | INT
    | '(' expr int_op expr ')'
    ;

int_op
    : '+'
    | '-'
    | '*'
    | '/'
    | '%'
    ;

cond_int_op
    : '='
    | '/='
    | '>'
    | '<'
    | '>='
    | '<='
    ;

cond_bool_op
    : '&&'
    | '||'
    ;

cond
    : '(' expr cond_int_op expr ')'
    | '(' '!' cond ')'
    | '(' cond cond_bool_op cond ')'
    ;
    
statement
    : var ':=' expr
    | cond '?'
    | '(' statement ';' statement ')'
    | '(' statement 'U' statement ')'
    | statement '*'
    | '(' statement ')'
    ;

program 
    : statement 
    ;