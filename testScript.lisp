(%% 
	(function yes/no/loop
        (def answer (read)
            (if (eq (ret answer) yes)
                (print "Fine !" "")
                (%% 
                    (print "That's not quite what I want to hear ...")
                    (yes/no/loop)))))
                    
    (function yes/no
            (def answer (read)
                (if (eq (ret answer) yes)
                    t
                    ())))

	(print "Welcome to my world !")
	(print "First of all, what is your name ?")
	(def name (read)
        (%% 
            (print "Ok so your name is " (ret name) " !")
            (print "I hope you're not a faint of hearth, my little " (ret name) " !")
            (yes/no/loop)
            (print "Which reminds me... Do you like cookies ?")
            (def cookie (yes/no) 
                (if (eq t (ret cookie)) 
                    (print "You're a fine chap !")
                    (print "We'll deal with this later...")))))) 
        