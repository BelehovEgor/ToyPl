(
    (
        (
            a := 2
            ;
            b := 4
        )
        ;
        z := 0
    )
    ;
    (
        ( 
            ( 
                ( 
                    ( (b > 0)? ) 
                    ; 
                    (
                        ( 
                            ( 
                                ( (b % 2) = 1 ) ? 
                                ; 
                                z := (z + a)
                            ) 
                            U 
                            ( 
                                ( !( (b % 2) = 1 ) ) ?
                            )
                        )
                        ;
                        (
                            a := (a * 2)
                            ;
                            b := (b / 2)
                        )

                    ) 
                )*
            ) 
            ; 
            ( ( !(b > 0) )? ) 
        )
        U
        ( ( !(b > 0) )? )
    )
)