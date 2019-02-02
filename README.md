# Pseudo-lisp-interpreter
Trying to craft an interpreter using s-exp but with a C-like way of things (aka, writing grammars is hard when not s-exp)

# Sample program (Won't work as-is in the REPL, as it reads line by line for now)
```
(%% 
    (print "This is a test program") 
    (print "%% is for declaring a statement group which is similar to a progn")
    (print (+ 41 1 )) 
    
    (%%  
        (def a (read) 
            (%%  
                (when (eq (ret a) 12) (print "a"))  
                (when (not (eq (ret a) 12)) (print "b")))))
    
    (%%  
        (def a (read)
            (%% 
                (when (eq (ret a) 12) (print "a"))  
                (when (not (eq (ret a) 12)) (print "b"))))
         (print "This works !")
     
     (%% 
        (function yes/no/loop
            (def answer (read)
                (if (eq (ret answer) yes)
                    (print "Fine !" "")
                    (%% 
                        (print "That's not quite what I want to hear ...")
                        (yes/no/loop)))))
                        
        (function yes/no
                (def answer (read)
                    (if (eq (ret answer) yes)
                        t
                        ())))
    
        (print "Welcome to my world !")
        (print "First of all, what is your name ?")
        (def name (read)
            (%% 
                (print "Ok so your name is " (ret name) " !")
                (print "I hope you're not a faint of hearth, my little " (ret name) " !")
                (yes/no/loop)
                (print "Which reminds me... Do you like cookies ?")
                (def cookie (yes/no) 
                    (if (eq t (ret cookie)) 
                        (print "You're a fine chap !")
                        (print "We'll deal with this later...")))))))
 
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
