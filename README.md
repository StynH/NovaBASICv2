# NovaBASICv2
![novabasic](https://github.com/StynVanDeHaterd/NovaBASIC/assets/9077578/80dc5727-aeb1-4a8d-b800-6cc74a2b202f)

Second implementation of my own programming language called NovaBASIC. Made in C# .NET 8, using Blazor as front-end.

Example of Quick Sort in NovaBasic.
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
