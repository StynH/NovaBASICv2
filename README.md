# NovaBASICv2
![novabasic](https://github.com/StynVanDeHaterd/NovaBASIC/assets/9077578/80dc5727-aeb1-4a8d-b800-6cc74a2b202f)

Second implementation of my own programming language called NovaBASIC. Made in C# .NET 8, using Blazor as front-end.

Example of Bubble Sort in NovaBasic.
```
FUNC printArray(arr)
    FOR LET i = 0 TO COUNT(arr)
        PRINT arr[i]
    ENDFOR
ENDFUNC

FUNC bubbleSort(arr)
    LET n = COUNT(arr)
    LET swapped = true
    WHILE swapped
        swapped = false
        FOR LET i = 0 TO n - 1
            IF arr[i] > arr[i + 1] THEN
                LET temp = arr[i]
                arr[i] = arr[i + 1]
                arr[i + 1] = temp
                swapped = true
            ENDIF
        ENDFOR
        n = n - 1
    ENDWHILE
ENDFUNC

LET arr = NEW [10]
FOR LET i = 0 TO COUNT(arr)
    arr[i] = RAND(1, 50)
ENDFOR

PRINT "-----[BEFORE SORT]-----"
printArray(arr)

PRINT "-----[AFTER SORT]-----"
bubbleSort(REF arr)
printArray(arr)
```
