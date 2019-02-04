(%% 
    (function recursion count 
        (%% 
             (when (< (ret count) 100)
                (%%  
                     (print (ret count))
                     (recursion (+ 1 (ret count)))
                     (print (ret count)))))) 
                     ;; Is broken, as for now each time you have a recursion, 
                        the parameters are overriden...;;
    (recursion 0))         