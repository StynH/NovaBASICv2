# NovaBASICv2
![novabasic](https://github.com/StynVanDeHaterd/NovaBASIC/assets/9077578/80dc5727-aeb1-4a8d-b800-6cc74a2b202f)

Second implementation of my own programming language called NovaBASIC. Made in C# .NET 8, using Blazor as front-end.

## Examples
Standard operations in NovaBASIC:

### Variables
```
LET i = 50
IMMUTABLE j = 100
LET myArray = NEW[10]
```

### Loops
```
FOR LET i = 0 TO 10
    PRINT i
END

FOR LET i = 0 TO 10 STEP 2
    PRINT i
END

LET j = 0
WHILE j < 10
    PRINT j
    j = j + 1
ENDWHILE

LET h = 10
REPEAT
    PRINT h
    h = h + 1
UNTIL h > 20
```

### Functions & References
```
FUNC myFunc(arrRef, index)
    arrRef[index] = 20
ENDFUNC

LET myArray = NEW[5]
myFunc(REF myArray, 4)
```

### If/Else
```
LET i = 6

IF i % 2 THEN
    PRINT "Value " + i + " is dividable by 2."
ELSEIF i % 3 THEN
    PRINT "Value " + i + " is dividable by 3."
ENDIF
```

### Guard
```
FUNC myFunc(param)
    GUARD param < 20 ELSE
        PRINT "Too much!"
        RETURN null
    ENDGUARD

    RETURN param * 50
ENDFUNC

LET i = 10
LET j = 20
myFunc(i)
myFunc(j)
```

### Array Slicing
```
LET arr = NEW[10]
LET firstTwo = arr[:3]
LET lastSeven = arr[3:]
LET between = arr[2:5]
LET inTwo = arr[1:10:2]
```
