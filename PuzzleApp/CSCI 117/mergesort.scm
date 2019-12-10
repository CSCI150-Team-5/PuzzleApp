;A program to sort a list from smallest to largest
;Utilizes the mergesort algorithm which recursively breaks lists in half, then puts then back together
;Input, one unsorted list of integers 
;Output, one sorted list of integers

#lang sicp ;Required for Dr. Racket Interpreter

;split a given list in half by every 2nd element
(define(split x)
(cond((null? x)(cons'()'())) ;If given empty list return 2 empty lists
     ((null?(cdr x))(cons x '())) ;If given list with one element return it and an empty list
     (else(cons(cons(car x)(car(split(cddr x))))(cons(cadr x)(cdr(split(cddr x)))))) ;Otherwise split the list
)
)

;merge two sorted lists into one sorted list by element value
(define(merge x y)
  (cond((null? x) y) ;if first list is empy, return the second list
       ((null? y) x) ;if second list is empty, return the first one
       (else
         (cond((<= (car x) (car y)) (cons(car x) (merge(cdr x) y))) ;if first elem of 1st list is <= first elem of 2nd list, construct list with 1st of 1st and rest of 1st merged with 2nd
         (else(cons(car y) (merge x (cdr y)))) ;else construct list with 1st of 2nd and 1st merged with rest of 2nd
         )
       )
  )
)

;sort a given list by value
(define(mergesort x)
  (cond((null? x) x) ;if list is empty, return it
     ((null?(cdr x)) x) ;if list has only one element, return it
       (else
         (merge(mergesort(car(split x))) (mergesort(cdr(split x)))) ;split x, mergesort both the first element and the rest, then merge those together
       )
  )
)


