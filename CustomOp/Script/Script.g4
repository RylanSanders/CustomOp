
grammar Script;
/*
 * Parser Rules
 */


                   

operation: WORD;
arg_part : WORD'='WORD;
args: arg_part(','arg_part)*;
subTerm : (NOT_SEMICOLON | ANGLE_BRACKET);
term : (WORD | EXPANDED_CHARS)+;
line : START_LINE operation START_ARG args END_ARG term END_LINE;
process: START_PROCESS WORD START_PROCESS_OP line+ END_PROCESS;
processes: process+ EOF;

/*
 * Lexer Rules
 */

fragment A:[aA];
fragment B:[bB];
fragment C:[cC];
fragment D:[dD];
fragment E:[eE];
fragment F:[fF];
fragment G:[gG];
fragment H:[hH];
fragment I:[iI];
fragment J:[jJ];
fragment K:[kK];
fragment L:[lL];
fragment M:[mM];
fragment N:[nN];
fragment O:[oO];
fragment P:[pP];
fragment Q:[qQ];
fragment R:[rR];
fragment S:[sS];
fragment T:[tT];
fragment U:[uU];
fragment V:[vV];
fragment W:[wW];
fragment X:[xX];
fragment Y:[yY];
fragment Z:[zZ];





//WHITESPACE		: (' ' | '\t')+ -> skip ;

INDEX_START		: '[';
INDEX_END		: ']';
WORD    : ('a'..'z' | 'A'..'Z' | [0-9])+;

//EXPANDED_WORD: ('a'..'z' | 'A'..'Z' | [0-9] | '<' | '>' | '/' | '=')+;
EXPANDED_CHARS: ([0-9] | '<' | '>' | '/' | '=' | '"' | ' ');
//EXPANDED_WORD: (~[a-zA-Z]* EXPANDED_CHARS+ ~[a-zA-Z]*);
ENDLINE : ';';
START_ARG:'(';
END_ARG:')';
START_PROCESS_OP: '{';
END_PROCESS:'}';
//ANGLE_BRACKET:  '<' | '>';
//NOT_SEMICOLON: ~(';') ;

START_LINE: '#';
START_PROCESS: '$';