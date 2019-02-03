grammar Expr;		
prog:  expr;	
expr:  LPAR NAME args* RPAR;
args:  INT | FLOATING | NAME | STRING | FALSY | expr;

LPAR: '(';
RPAR: ')';
FALSY: 'no';
NAME: [a-zA-Z+-/%*><=]+;
COMMENTS: ';;'.*? ';;' -> skip;
STRING: '"' .*? '"';
INT: [0-9]+;
FLOATING: [0-9]+'.'[0-9]+;
WS : [ \t\r\n]+ -> skip ;