# G--
Craft an interpreter using s-exp but with a C-like way of things (aka, writing grammars is hard when not using s-exp)
(Please don't insult me if I don't respect lisp principles, that's not the goal.)

Features : 

- Lexically scoped functions and variables (let)
- Globally scoped variables (set)
- Function (with and without parameters)
- Passing function (parameterless) as parameters
- Recursion (with and without parameters) (beware of the stackoverflow...) (+ avoid using them if not tail recursive rn..)
- Print to repl
- Read repl 
- Boolean logic
- Control flow
- Type predicates
- Random number generation

# A game of more or less 
```
(%% 
    (function get-random-number min max 
        (random (ret min) (ret max)))
        
    (function game tries number min max
        (%%
            (print "Please choose a number beetwen " (ret min) " and " (ret max))
            (let choice (read)
                (%%
                    (if (and (== (ret choice) (ret number)) (not (== tries 0)))
                        (print "Congratulation ! You won")
                        (%%
                            (when (< (ret choice) (ret number)) (print "It's bigger !"))
                            (when (> (ret choice) (ret number)) (print "It's smaller !"))
                            (print (- (ret tries) 1) " left !")
                            (if (== 0 (- (ret tries) 1))   
                                (print "No more tries.. You loose !")
                                (game (- (ret tries) 1) (ret number) (ret min) (ret max)))))))))
                            
    (let min 1
        (let max 100
            (game 10 (get-random-number (ret min) (ret max)) (ret min) (ret max)))))

```

# Broken recursion 

```
;; Right now this is BROKEN ;;
(%% 
    (function recursion count 
        (%% 
             (when (< (ret count) 100)
                (%%  
                     (print (ret count))
                     (recursion (+ 1 (ret count)))
                     (print (ret count))))))  ;; HERE, count is affected by call of the recursion because of a design flaw ;;
    (recursion 0))    

```

# The meanest fizzBuzz test ever (you have to implement an interpreter THEN solve fizzBuzz lmao) :
```
(%%   
    (function isFizz num 
        (== 0 (% (ret num) 3)))
        
    (function isBuzz num 
        (== 0 (% (ret num) 5)))
        
   (function isNeither num 
        (not (or (isFizz (ret num)) (isBuzz (ret num)))))     

   (function letsgo counter max 
        (when (< (ret counter) (+ (ret max) 1))
            (%%
                (print 
                    (when (isFizz (ret counter)) "Fizz")
                    (when (isBuzz (ret counter)) "Buzz")
                    (when (isNeither (ret counter)) (ret counter)))
                (letsgo (+ (ret counter) 1) (ret max)))))
            
   (letsgo 1 100))
```

# Sample program with random stuff
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
        (let answer (read)
            (if (== (ret answer) yes)
                (print "Fine !" "")
                (%% 
                    (print "That's not quite what I want to hear ...")
                    (yes/no/loop)))))
                    
    (function yes/no
            (let answer (read)
                (if (== (ret answer) yes)
                    yes
                    no)))

	(print "Welcome to my world !")
	(print "First of all, what is your name ?")
	(let name (read)
        (%% 
            (print "Ok so your name is " (ret name) " !")
            (print "I hope you're not a faint of hearth, my little " (ret name) " !")
            (yes/no/loop)
            (print "Which reminds me... Do you like cookies ?")
            (let cookie (yes/no) 
                (if (== yes (ret cookie)) 
                    (print "You're a fine chap !")
                    (print "We'll deal with this later...")))))) 
                
```  

# Weird stuff with scopes : 

```
(%% 
    (set i 0)
    (%%
        (function while cond body
            (if (== no (apply (ret cond)))
                (%%
                    (apply body)
                    (while (ret cond) (ret body)))
                no))          
        (function body 
            (%%
                (print (ret i))
                (set i (+ 1 (ret i)))))               
        (function cond 
            (< 100 (ret i)))   

        (while cond body)))  

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
