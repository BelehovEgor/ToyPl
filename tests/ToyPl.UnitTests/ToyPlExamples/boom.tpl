(   
    (
        (a := (a + 1))
        U
        (a := (a + b))
    )
    U
    (
        (b := (b * 2))
        ;
        (c := (c + b))
    )
)*