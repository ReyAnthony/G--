(%% 
    (set i 0)
    
    (function while c b
        (if (== no (apply (ret c)))
            (%%
                (apply (ret b))
                (while (ret c) (ret b)))
            no))   
                   
    (function body 
        (%%
            (print (ret i))
            (set i (+ 1 (ret i)))))  
                         
    (function cond 
        (< 100 (ret i)))   

    (while cond body)) 