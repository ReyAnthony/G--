(%%
    (print "Apply parameterless lambda" (apply (lambda (+ 1 2))))
    (print "generated func name : " (lambda a b (+ (ret a) (ret b))))
  
    (let l (function test a b (+ (ret a) (ret b)))
            (print "Apply parameterized function " (apply (ret l) 1 2)))
            
    (let l (lambda a b (+ (ret a) (ret b)))
            (print "Apply parameterized lambda " (apply (ret l) 1 2)))
)      