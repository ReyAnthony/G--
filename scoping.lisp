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