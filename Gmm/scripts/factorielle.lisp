(%% 
    (function fact n
        (if (== 1  (ret n))
            1
            (* (ret n) (fact (- (ret n) 1)))))
    
    (print (fact 5)))