(%%    
    (function recursion count max 
        (%% 
             (when (< (ret count) (ret max))
                (%%  
                     (print (ret count))
                     (recursion (+ 1 (ret count)) (ret max))
                     (print (ret count))))))
    (recursion 0 100))