using System;

namespace org.amimchik.NyxLangInterpreter.Source.Interpreter.Parsing.Lexing;

public class Token(TokenType type, string lexeme)
{
    public TokenType Type { get; set; } = type;
    public string Lexeme { get; set; } = lexeme;
    public override string ToString()
    {
        return $"{Type}:'{Lexeme}'";
    }
}
