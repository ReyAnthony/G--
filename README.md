# Pseudo-lisp-interpreter
Trying to craft an interpreter using s-exp but with a C-like way of things (aka, writing grammars is hard when not s-exp)

# Sample program (Won't work as-is in the REPL, as it reads line by line for now)
```
(%% 
  (print "This is a test program") 
  (print "%% is for declaring a statement group (similar to a progn)")
  (print (+ 41 1 ))) 
  
OR 

(%%  
    (def a (read) 
        (%%  
            (when (eq (ret a) 12) (print "a"))  
            (when (not (eq (ret a) 12)) (print "b")))))
13
b
"b"

(%%  
    (def a (read)
        (%% 
            (when (eq (ret a) 12) (print "a"))  
            (when (not (eq (ret a) 12)) (print "b"))))
     (print "This works :)")
12 
()
  
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
