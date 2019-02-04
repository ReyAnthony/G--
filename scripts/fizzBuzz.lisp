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