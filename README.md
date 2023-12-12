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

IF i % 2 == 0 THEN
    PRINT "Value " + i + " is dividable by 2."
ELSEIF i % 3 == 0 THEN
    PRINT "Value " + i + " is dividable by 3."
ENDIF
```

### Switch
```
LET dayOfWeek = "Friday"

SWITCH dayOfWeek
    CASE "Monday":
        PRINT "Start of the workweek"
        BREAK
    CASE "Friday":
        PRINT "End of the workweek"
        BREAK
    DEFAULT:
        PRINT "Middle of the workweek"
        BREAK
ENDSWITCH
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

### Type Checking
```
STRUCT Person
    Name
ENDSTRUCT

FUNC GetNameLength(p)
    GUARD p IS Person ELSE
        PRINT "Parameter must be a 'Person'!"
        RETURN -1
    ENDGUARD

    RETURN COUNT(p.Name)
ENDFUNC

PRINT GetNameLength(NEW Person("Bob Ross"))
PRINT GetNameLength(25)
```

### Array Slicing
```
LET arr = NEW[10]
LET firstTwo = arr[:3]
LET lastSeven = arr[3:]
LET between = arr[2:5]
LET inTwo = arr[1:10:2]
```

### Import/Export (Desktop Only)
```
//File1.nova
IMMUTABLE myConst = 3.14

STRUCT myStruct
	Val
ENDSTRUCT

EXPORT
	myConst,
	myStruct
ENDEXPORT

//File2.nova
FUNC myFunc(var1)
	PRINT var1
ENDFUNC

EXPORT
	myFunc
ENDEXPORT

//File3.nova
IMPORT FROM "File1"
IMPORT "MyFunc" FROM "File2"

LET i = NEW myStruct(myConst)
myFunc(i)
```
