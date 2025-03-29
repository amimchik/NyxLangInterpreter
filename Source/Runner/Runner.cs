using org.amimchik.NyxLangInterpreter.Source.Interpreter.Parsing.Lexing;

namespace org.amimchik.NyxLangInterpreter.Source.Runner;

public class Runner
{
    public static void Main(string[] args)
    {
        Tokenizer lexer = new("""
        def fact(let n: int): int
        {
            if (n == 0)
                ret 1
            else
                ret n * fact(n - 1)
        }

        def main(): void
        {
            let x: int = int(input("Enter a number: "))
            printf("{}! = {}", x, fact(x))
        }

        if (_name == "main")
            main()
        """);
        var tokens = lexer.Tokenize();
        foreach (var token in tokens)
        {
            Console.Write($"[{token}]");
        }
        Console.WriteLine();
    }
}