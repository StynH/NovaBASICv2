# NovaBASICv2
![novabasic](https://github.com/StynVanDeHaterd/NovaBASIC/assets/9077578/80dc5727-aeb1-4a8d-b800-6cc74a2b202f)

Second implementation of my own programming language called NovaBASIC. Made in C# .NET 8, using Blazor as front-end.

## Examples
Example of Fibonacci in NovaBASIC.
```
FUNC fibonacci(n)
    IF n <= 1 THEN
        RETURN n
    ENDIF
    RETURN fibonacci(n - 1) + fibonacci(n - 2)
ENDFUNC

LET num = 10
PRINT "Fibonacci sequence up to " + num + ":"

FOR LET i = 0 TO num + 1
    PRINT fibonacci(i)
ENDFOR
```

Example of Sieve of Eratosthenes in NovaBASIC.
```
FUNC sieveOfEratosthenes(limit)
    LET prime = NEW [limit + 1]
    FOR LET i = 2 TO limit
        prime[i] = true
    ENDFOR

    FOR LET p = 2 TO limit
        IF prime[p] THEN
            FOR LET i = p * p TO limit STEP p
                prime[i] = false
            ENDFOR
        ENDIF
    ENDFOR

    PRINT "Prime numbers up to " + limit + ":"
    FOR LET i = 2 TO limit
        IF prime[i] THEN
            PRINT i
        ENDIF
    ENDFOR
ENDFUNC

LET n = 30
sieveOfEratosthenes(n)
```

Example of Structs in NovaBASIC:
```
STRUCT Person
    Name,
    Age,
    Height
ENDSTRUCT

FUNC EnableToJoinTallClub(person)
    RETURN person.Height >= 190
ENDFUNC

FUNC EnableToJoinJoeClub(person)
    RETURN person.Name MATCHES "^Joe\s+[A-Za-z]+$"
ENDFUNC

LET joe = NEW Person("Joe Doe", 25, 185)
PRINT "Member of the Tall Club: " + EnableToJoinTallClub(joe)
PRINT "Member of the Joe Club: " + EnableToJoinJoeClub(joe)
```

Example of Quick Sort in NovaBASIC.
```
FUNC printArray(arrRef)
    FOR LET i = 0 TO COUNT(arrRef)
        PRINT arrRef[i]
    ENDFOR
ENDFUNC

FUNC randomizeArray(arrRef)
    FOR LET i = 0 TO COUNT(arrRef)
        arrRef[i] = RAND(1, 100)
    ENDFOR
ENDFUNC

FUNC swap(arrRef, i, j)
    LET temp = arrRef[i]
    arrRef[i] = arrRef[j]
    arrRef[j] = temp
ENDFUNC

FUNC partition(arrRef, low, high)
    LET pivot = arrRef[high]
    LET i = low - 1
    FOR LET j = low TO high
        IF arrRef[j] < pivot THEN
            i = i + 1
            swap(arrRef, i, j)
        ENDIF   
    ENDFOR
    
    swap(arrRef, i + 1, high)
    RETURN i + 1
ENDFUNC

FUNC quickSort(arrRef, low, high)
    IF low < high THEN
        LET pi = partition(arrRef, low, high)
        quickSort(arrRef, low, pi - 1)
        quickSort(arrRef, pi + 1, high)     
    ENDIF
ENDFUNC

LET arr = NEW [10]
randomizeArray(REF arr)

PRINT "-----[BEFORE SORT]-----"
printArray(arr)

PRINT "-----[AFTER SORT]-----"
quickSort(REF arr, 0, COUNT(arr) - 1)
printArray(arr)
```
