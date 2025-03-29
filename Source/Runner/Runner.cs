using org.amimchik.NyxLangInterpreter.Source.Interpreter.Parsing.Lexing;

namespace org.amimchik.NyxLangInterpreter.Source.Runner;

public class Runner
{
    public static void Main(string[] args)
    {
        Tokenizer lexer = new("""
        class MyClass
        {
            pub def _init(let val: int)
            {
                myField = val
            }
            pub prop MyProperty: int { get { return myField } set { myField = _value } }
            priv field myField: int

            pub def printInfo(): void
            {
                printf("val: {}", val)
            }
        }

        let myClass: MyClass = new MyClass(4)
        
        myClass.printInfo()

        myCLass.MyProperty = 5

        myClass.printInfo()
        """);
        var tokens = lexer.Tokenize();
        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}