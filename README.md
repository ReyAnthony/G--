# Pseudo LISP-like interpreter
Trying to craft an interpreter using s-exp but with a C-like way of things (aka, writing grammars is hard when not using s-exp)

Features : 
- Lexically scoped functions and variables 
- Function with parameters
- Recursion (with and without parameters) (beware of the stackoverflow...)
- Print to repl
- Read repl 
- Boolean logic
- Control flow
- Type predicates

# Sample program (Won't work as-is in the REPL, as it reads line by line for now)
```
(%% 
    ;; This is a comment it will be ignored by the interpreter ;;
	(function with-params a b c
	    (print (ret a) " " (ret b) " " (ret c)))
 
	(with-params 1 2 3)
	
	(function recur-with-params a
	    (%% 
	        (print (ret a))
	        (if (== (ret a) 100)
	           no 
               (recur-with-params (+ 1 (ret a))))))
	        
	(recur-with-params 1)
	    
	(function yes/no/loop
        (def answer (read)
            (if (== (ret answer) yes)
                (print "Fine !" "")
                (%% 
                    (print "That's not quite what I want to hear ...")
                    (yes/no/loop)))))
                    
    (function yes/no
            (def answer (read)
                (if (== (ret answer) yes)
                    t
                    no)))

	(print "Welcome to my world !")
	(print "First of all, what is your name ?")
	(def name (read)
        (%% 
            (print "Ok so your name is " (ret name) " !")
            (print "I hope you're not a faint of hearth, my little " (ret name) " !")
            (yes/no/loop)
            (print "Which reminds me... Do you like cookies ?")
            (def cookie (yes/no) 
                (if (== t (ret cookie)) 
                    (print "You're a fine chap !")
                    (print "We'll deal with this later...")))))) 
                
```  
# In the REPL you can declare anything

```
(+ 1 2 3)
6
(- 34 2)
32
(print (+ 1 2))
3
```
