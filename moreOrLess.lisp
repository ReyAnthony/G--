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