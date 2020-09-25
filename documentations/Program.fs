let rec Fib  n =
    match n with
    | 1 | 2 -> 1
    | n -> Fib(n-1) + Fib(n-2)

let x = Fib(24)
printf "%i" x
