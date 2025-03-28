
# NyxLangInterpreter

  

NyxLang is a simple and modern programming language designed for readability and ease of use. It draws inspiration from languages like C# and Java but focuses on simplicity and efficiency, with a custom runtime and interpreter. NyxLang supports object-oriented programming (OOP), with features such as classes, methods, fields, and basic control structures.

  

## Features

-   **Static Typing**: NyxLang uses statically-typed variables, ensuring type safety at compile-time.
    
-   **Object-Oriented Programming (OOP)**: Classes, inheritance, and methods provide the building blocks for creating powerful and reusable code.
    
-   **Custom Interpreter**: NyxLang does not rely on external runtimes like .NET or JVM. Instead, it uses its own interpreter designed for the language's syntax and features.

-   **Simple syntax**: NyxLang syntax designed to be readable and understandable

## Syntax
### Hello World in NyxLang
```nyx
print("Hello, World!")
```

### Variables and Types
```nyx
let num: int = 34
let str: string = "Hello"
```

### Functions
Use `def` keyword to init function in NyxLang
```nyx
def sum(let a: int, let b: int): int {
	return a + b
}

print(sum(2, 5))
```

### Classes
```nyx
class MyClass {
	pub def MyMethod(): void {
		print(f"val is {myField}")
	}
	pub def _init() {
		myField = 2
	}
	pub property MyProperty {
		get {
			return myField
		}
		set {
			myField = _val
		}
	}
	priv field myField;
}

let myClass: MyClass = new MyClass()
myClass.MyMethod()
myClass.MyProperty = 4
myClass.MyMethod()
```