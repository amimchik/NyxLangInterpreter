namespace org.amimchik.NyxLangInterpreter.Source.Interpreter.Parsing.Lexing;

public enum TokenType
{
    // Keywords:
    Let,    // let
    Def,    // def
    While,  // while
    For,    // for
    If,     // if
    Else,   // else
    Break,  // break
    Continue,// contiue
    Int,    // int
    String, // string
    Float,  // float
    Double, // double
    Void,   // void
    Class,  // class
    Pub,    // pub
    Priv,   // priv
    Prot,   // prot
    Prop,   // prop
    Field,  // field
    Stat,   // stat
    Virt,   // virt
    Ovrd,   // ovrd
    Ret,    // ret
    Get,    // get
    Set,    // set

    // Identifiers:
    Identifier, // identifier

    // Literals:
    BooleanLiteral, // true/false
    IntegerLiteral, // 435
    FloatingLiteral,// 32.5
    StringLiteral,  // "hello"

    // Separators:
    Comma,      // ,
    Dot,        // .
    Colon,      // :
    Semicolon,  // ;
    LParen,     // (
    RParen,     // )
    LBrace,     // {
    RBrace,     // }
    LBracket,   // [
    RBracket,   // ]

    Lambda,     // =>
    Arrow,      // ->

    // Operators:
    Star,       // *
    Slash,      // /
    Plus,       // +
    Minus,      // -
    Modulo,     // %

    Assign,     // =

    AND,        // &&
    OR,         // ||
    NOT,        // !
    BAND,       // &
    BOR,        // |
    XOR,        // ^

    EQ,         // ==
    NTEQ,       // !=
    GT,         // >
    LT,         // <
    GTEQ,       // >=
    LTEQ,       // <=


    // EOF:
    EOF,        // End of file
}
