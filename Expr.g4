grammar Expr;		
prog:  expr;	
expr:  LPAR NAME args* RPAR;
args:  INT | NAME | STRING | EMPTY_LIST | expr;

LPAR: '(';
RPAR: ')';
NAME: [a-zA-Z+-/%*]+;
COMMENTS: ';;'.*? ';;' -> skip;
STRING: '"' .*? '"';
EMPTY_LIST: LPAR ' '*? RPAR;
INT: [0-9]+;
WS : [ \t\r\n]+ -> skip ;