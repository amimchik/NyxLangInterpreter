using System;
using System.Runtime.InteropServices;
using System.Text;

namespace org.amimchik.NyxLangInterpreter.Source.Interpreter.Parsing.Lexing;

public class Tokenizer(string input)
{
    private readonly TextIterator it = new(input);
    private readonly List<Token> tokens = [];
    private readonly Dictionary<string, Token> keywords = new()
    {
        { "get", new(TokenType.Get, string.Empty) },
        { "if", new(TokenType.If, string.Empty) },
        { "else", new(TokenType.Else, string.Empty) },
        { "set", new(TokenType.Set, string.Empty) },
        { "prop", new(TokenType.Prop, string.Empty) },
        { "field", new(TokenType.Field, string.Empty) },
        { "let", new(TokenType.Let, string.Empty) },
        { "def", new(TokenType.Def, string.Empty) },
        { "while", new(TokenType.While, string.Empty) },
        { "for", new(TokenType.For, string.Empty) },
        { "break", new(TokenType.Break, string.Empty) },
        { "continue", new(TokenType.Continue, string.Empty) },
        { "int", new(TokenType.Int, string.Empty) },
        { "string", new(TokenType.String, string.Empty) },
        { "float", new(TokenType.Float, string.Empty) },
        { "double", new(TokenType.Double, string.Empty) },
        { "void", new(TokenType.Void, string.Empty) },
        { "class", new(TokenType.Class, string.Empty) },
        { "pub", new(TokenType.Pub, string.Empty) },
        { "priv", new(TokenType.Priv, string.Empty) },
        { "prot", new(TokenType.Prot, string.Empty) },
        { "stat", new(TokenType.Stat, string.Empty) },
        { "virt", new(TokenType.Virt, string.Empty) },
        { "ovrd", new(TokenType.Ovrd, string.Empty) },
        { "ret", new(TokenType.Ret, string.Empty) },
        { "true", new(TokenType.BooleanLiteral, "true") },
        { "false", new Token(TokenType.BooleanLiteral, "false") },
    };
    private readonly string operatorsSeparatorsChars = "=!<>+-*/%&|^,.;:{}[]()-";
    private readonly Dictionary<string, Token> operatorsSeparators = new()
    {
        { "=>", new Token(TokenType.Lambda, string.Empty) },
        { "->", new Token(TokenType.Arrow, string.Empty) },
        { ",", new Token(TokenType.Comma, string.Empty) },
        { ".", new Token(TokenType.Dot, string.Empty) },
        { ":", new Token(TokenType.Colon, string.Empty) },
        { ";", new Token(TokenType.Semicolon, string.Empty) },
        { "(", new Token(TokenType.LParen, string.Empty) },
        { ")", new Token(TokenType.RParen, string.Empty) },
        { "{", new Token(TokenType.LBrace, string.Empty) },
        { "}", new Token(TokenType.RBrace, string.Empty) },
        { "[", new Token(TokenType.LBracket, string.Empty) },
        { "]", new Token(TokenType.RBracket, string.Empty) },
        { "/", new Token(TokenType.Slash, string.Empty) },
        { "+", new Token(TokenType.Plus, string.Empty) },
        { "-", new Token(TokenType.Minus, string.Empty) },
        { "*", new Token(TokenType.Star, string.Empty) },
        { "%", new Token(TokenType.Modulo, string.Empty) },
        { "=", new Token(TokenType.Assign, string.Empty) },
        { "&&", new Token(TokenType.AND, string.Empty) },
        { "||", new Token(TokenType.OR, string.Empty) },
        { "!", new Token(TokenType.NOT, string.Empty) },
        { "&", new Token(TokenType.BAND, string.Empty) },
        { "|", new Token(TokenType.BOR, string.Empty) },
        { "^", new Token(TokenType.XOR, string.Empty) },
        { "==", new Token(TokenType.EQ, string.Empty) },
        { "!=", new Token(TokenType.NTEQ, string.Empty) },
        { ">", new Token(TokenType.GT, string.Empty) },
        { "<", new Token(TokenType.LT, string.Empty) },
        { ">=", new Token(TokenType.GTEQ, string.Empty) },
        { "<=", new Token(TokenType.LTEQ, string.Empty) },
    };
    public List<Token> Tokenize()
    {
        while (it.Peek() != '\0')
        {
            char current = it.Peek();
            if (current == '#')
            {
                TokenizePythonStyleComment();
            }
            else if (current == '/' && it.Peek(1) == '/')
            {
                TokenizeCStyleComment();
            }
            else if (current == '/' && it.Peek(1) == '*')
            {
                TokenizeCStyleMultilineComment();
            }
            else if (char.IsLetter(current) || current == '_')
            {
                TokenizeWord();
            }
            else if (current == '\'')
            {
                TokenizeSingleStringLiteral();
            }
            else if (current == '\"')
            {
                TokenizeDoubleStringLiteral();
            }
            else if (operatorsSeparatorsChars.Contains(current))
            {
                TokenizeOperatorSeparator();
            }
            else if (char.IsDigit(current))
            {
                TokenizeNumberLiteral();
            }
            else
            {
                it.Next();
            }
        }

        AddToken(TokenType.EOF);

        return tokens;
    }
    private void TokenizeDoubleStringLiteral()
    {
        it.Next();
        StringBuilder buffer = new();
        while (it.Peek() != '\"')
        {
            string c = it.Peek().ToString();
            if (it.Peek() == '\\')
            {
                char escCtrl = it.Next();
                string fnlEsc = escCtrl.ToString();
                if (escCtrl == 'u')
                {
                    for (int i = 0; i < 4; i++)
                    {
                        fnlEsc += it.Next();
                    }
                }
                else if (escCtrl == 'U')
                {
                    for (int i = 0; i < 8; i++)
                    {
                        fnlEsc += it.Next();
                    }
                }
                c = DecodeUnicodeEscapeSequence(fnlEsc);
            }
            buffer.Append(c);
            it.Next();
        }
        it.Next();
        AddToken(TokenType.StringLiteral, buffer.ToString());
    }
    private void TokenizeSingleStringLiteral()
    {
        it.Next();
        StringBuilder buffer = new();
        while (it.Peek() != '\'')
        {
            string c = it.Peek().ToString();
            if (it.Peek() == '\\')
            {
                char escCtrl = it.Next();
                string fnlEsc = escCtrl.ToString();
                if (escCtrl == 'u')
                {
                    for (int i = 0; i < 4; i++)
                    {
                        fnlEsc += it.Next();
                    }
                }
                else if (escCtrl == 'U')
                {
                    for (int i = 0; i < 8; i++)
                    {
                        fnlEsc += it.Next();
                    }
                }
                c = DecodeUnicodeEscapeSequence(fnlEsc);
            }
            buffer.Append(c);
            it.Next();
        }
        it.Next();
        AddToken(TokenType.StringLiteral, buffer.ToString());
    }
    private string DecodeUnicodeEscapeSequence(string input)
    {
        if (input.StartsWith("u") && input.Length == 5)
        {
            string hexPart = input[1..];
            int codePoint = Convert.ToInt32(hexPart, 16);
            return char.ConvertFromUtf32(codePoint);
        }
        else if (input.StartsWith("U") && input.Length == 9)
        {
            string hexPart = input[1..];
            int codePoint = Convert.ToInt32(hexPart, 16);
            return char.ConvertFromUtf32(codePoint);
        }
        else return input switch
        {
            "n" => "\n",
            "t" => "\t",
            "r" => "\r",
            "v" => "\v",
            "b" => "\b",
            "a" => "\a",
            "f" => "\f",
            "e" => "\e",
            _ => input
        };
    }
    private void TokenizeOperatorSeparator()
    {
        StringBuilder buffer = new();

        while (operatorsSeparators.ContainsKey(buffer.ToString() + it.Peek()))
        {
            buffer.Append(it.Peek());
            it.Next();
        }

        tokens.Add(operatorsSeparators[buffer.ToString()]);
    }
    private void TokenizeWord()
    {
        StringBuilder buffer = new();

        while (char.IsLetterOrDigit(it.Peek()) || it.Peek() == '_')
        {
            buffer.Append(it.Peek());
            it.Next();
        }

        if (keywords.ContainsKey(buffer.ToString()))
        {
            tokens.Add(keywords[buffer.ToString()]);
        }
        else
        {
            AddToken(TokenType.Identifier, buffer.ToString());
        }
    }
    private void TokenizeCStyleMultilineComment()
    {
        it.Next();
        it.Next();
        while (it.Peek() != '\0' && (it.Peek() != '*' || it.Peek(1) != '/'))
        {
            it.Next();
        }
        it.Next();
        it.Next();
    }
    private void TokenizeCStyleComment()
    {
        while (!"\n\0".Contains(it.Peek()))
        {
            it.Next();
        }
        it.Next();
    }
    private void TokenizePythonStyleComment()
    {
        while (!"\n\0".Contains(it.Peek()))
        {
            it.Next();
        }
        it.Next();
    }
    private void TokenizeNumberLiteral()
    {
        StringBuilder buffer = new();

        bool isFloat = false;

        while (true)
        {
            char current = it.Peek(0);
            if (char.IsDigit(current))
            {
                buffer.Append(current);
                it.Next();
                continue;
            }
            if (current == '.')
            {
                if (isFloat)
                {
                    break;
                }
                if (char.IsDigit(it.Peek(1)))
                {
                    isFloat = true;
                    buffer.Append(',');
                    it.Next();
                    continue;
                }
                else
                {
                    break;
                }
            }
            break;
        }
        AddToken(isFloat ? TokenType.FloatingLiteral : TokenType.IntegerLiteral, buffer.ToString());
    }
    private void AddToken(TokenType type)
    {
        tokens.Add(new(type, string.Empty));
    }
    private void AddToken(TokenType type, string lexeme)
    {
        tokens.Add(new(type, lexeme));
    }
    private class TextIterator(string input)
    {
        private readonly string input = input;
        private int pos = 0;
        public char Peek(int offset = 0)
        {
            return SafeGet(pos + offset);
        }
        public char Next()
        {
            if (pos < input.Length)
            {
                pos++;
            }
            return SafeGet(pos);
        }
        private char SafeGet(int index)
        {
            return index >= 0 && index < input.Length ? input[index] : '\0';
        }
    }
}