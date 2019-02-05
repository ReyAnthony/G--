(%% 
    (function recursion count 
        (%% 
             (when (< (ret count) 100)
                (%%  
                     (print (ret count))
                     (recursion (+ 1 (ret count)))
                     (print (ret count)))))) 
    (recursion 0))       