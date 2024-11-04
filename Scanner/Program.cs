using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Scanner
{
    private Regex identifiers;// تبدأ بحرف أو -
    private Regex numericConst;//الأرقام
    private Regex charConst;//'اي حرف '

    private Dictionary<string, Tuple<string, string>> tokensClasses;//بسرش باستخدام key

    public Scanner()
    {
        identifiers = new Regex(@"[a-z|A-Z|_][a-z|A-Z|0-9|_]*");//هنا يا اما الحروف capital او small او _
        numericConst = new Regex(@"(-)?(0|1|2|3|4|5|6|7|8|9)+( (\.) (0|1|2|3|4|5|6|7|8|9)*)?");
        charConst = new Regex(@"\'[a-zA-Z]\'");//حرف capital او small او 

        tokensClasses = new Dictionary<string, Tuple<string, string>>()
        {
            {"int", Tuple.Create("keyword", "integer")}, //key>>int,value>>"keyword" and "integer"
    {"float", Tuple.Create("keyword", "floating ")},
    {"double", Tuple.Create("keyword", "double ")},
    {"char", Tuple.Create("keyword", "character")},
    {"string", Tuple.Create("keyword", "string")},
    {"if", Tuple.Create("keyword", "if statement")},
    {"else", Tuple.Create("keyword", "else statement")},
    {"for", Tuple.Create("keyword", "for loop")},
    {"const", Tuple.Create("keyword", "constant")},
    {"bool", Tuple.Create("keyword", "boolean")},
    {"return", Tuple.Create("keyword", "return statement")},
    {"break", Tuple.Create("keyword", "break statement")},
    {"case", Tuple.Create("keyword", "case statement")},
    {"continue", Tuple.Create("keyword", "continue statement")},
    {"default", Tuple.Create("keyword", "default case")},
    {"do", Tuple.Create("keyword", "do statement")},
    {"goto", Tuple.Create("keyword", "goto statement")},
    {"short", Tuple.Create("keyword", "short integer")},
    {"signed", Tuple.Create("keyword", "signed integer")},
    {"sizeof", Tuple.Create("keyword", "size of")},
    {"static", Tuple.Create("keyword", "static")},
    {"struct", Tuple.Create("keyword", "structure")},
    {"switch", Tuple.Create("keyword", "switch statement")},
    {"typedef", Tuple.Create("keyword", "type definition")},
    {"unsigned", Tuple.Create("keyword", "unsigned integer")},
    {"void", Tuple.Create("keyword", "void")},
    {"while", Tuple.Create("keyword", "while loop")},
    {"=", Tuple.Create("operator", "assignment")},
    {"+", Tuple.Create("operator", "addition")},
    {"-", Tuple.Create("operator", "subtraction")},
    {"*", Tuple.Create("operator", "multiplication")},
    {"/", Tuple.Create("operator", "division")},
    {"++", Tuple.Create("operator", "increment")},
    {"--", Tuple.Create("operator", "decrement")},
    {"==", Tuple.Create("operator", "equality")},
    {"<=", Tuple.Create("operator", "less than or equal to")},
    {">=", Tuple.Create("operator", "greater than or equal to")},
    {"!=", Tuple.Create("operator", "not equal to")},
    {"<", Tuple.Create("operator", "less than")},
    {">", Tuple.Create("operator", "greater than")},
    {"||", Tuple.Create("operator", "logical or")},
    {"!", Tuple.Create("operator", "logical not")},
    {"&&", Tuple.Create("operator", "logical and")},
    {"(", Tuple.Create("specialChar", "left parenthesis")},
    {")", Tuple.Create("specialChar", "right parenthesis")},
    {"{", Tuple.Create("specialChar", "left curly brace")},
    {"}", Tuple.Create("specialChar", "right curly brace")},
    {";", Tuple.Create("specialChar", "semicolon")},
    {"@", Tuple.Create("specialChar", "at sign")},
    {"#", Tuple.Create("specialChar", "hash sign")},
    {"%", Tuple.Create("specialChar", "percent sign")},
    {"^", Tuple.Create("specialChar", "caret")},
    {"~", Tuple.Create("specialChar", "tilde")},
    {"&", Tuple.Create("specialChar", "ampersand")},
    {"$", Tuple.Create("specialChar", "dollar sign")},
    {"`", Tuple.Create("specialChar", "backtick")},
     
        };
    }

    public List<Tuple<string, int>> ExtractLexems(string input)
    {
        List<Tuple<string, int>> lexems = new List<Tuple<string, int>>();
        string pattern = @"//.*|[();,{}[\]]|[+\-*/=<>!&|]|(?<!\d)[\d]+(?:\.[\d]+)?|[\d]+(?:\.[\d]+)?(?!\d)|[a-zA-Z_][a-zA-Z0-9_]*|@";
        int pos = 0;
        int commentIndex = input.IndexOf("//");
        if (commentIndex >= 0)
        {
            input = input.Substring(0, commentIndex); // بتجاهل اي كومنت 
        }

        foreach (Match match in Regex.Matches(input, pattern))
        {
            lexems.Add(Tuple.Create(match.Value, pos));

            pos += match.Length;
        }

        return lexems;
    }

    public Tuple<string, string> CheckLexem(Tuple<string, int> lexem, int line)
    {
        if (tokensClasses.ContainsKey(lexem.Item1))
        {
            return tokensClasses[lexem.Item1];
        }
        else if (identifiers.IsMatch(lexem.Item1))
        {
            return Tuple.Create("Identifier", "ID");
        }
        else if (charConst.IsMatch(lexem.Item1))
        {
            return Tuple.Create("CharacterConst", "CharConst");
        }
        else if (numericConst.IsMatch(lexem.Item1))
        {
            return Tuple.Create("NumericConst", "NumConst");
        }
        else
        {
            return Tuple.Create("Error", $"Error in line {line}: '{lexem.Item1}' is not defined.");
        }
    }
}

public class Program
{
    public static void Main()
    {
        
        Scanner scanner = new Scanner();
        string input = "float a =2*(3+7) ; ";
        string input2 = "ahmed ! 1234";

        List<Tuple<string, int>> lexemes = scanner.ExtractLexems(input);
        List<Tuple<string, int>> lexemes2 = scanner.ExtractLexems(input2);

        int line = 1;


        //foreach (var lexeme in lexemes)
        //{
        //    var tokenClass = scanner.CheckLexem(lexeme, line);
        //    Console.WriteLine($"Lexeme: {lexeme.Item1}, Type: {tokenClass.Item1}, Class: {tokenClass.Item2}");
        //}
        //Console.WriteLine("------------------------------");
        //foreach (var lexeme in lexemes2)
        //{
        //    var tokenClass = scanner.CheckLexem(lexeme, line);
        //    Console.WriteLine($"Lexeme:{lexeme.Item1} , Type: {tokenClass.Item1} , Class: {tokenClass.Item2}");

        //}

        //for user 

        Console.WriteLine("Please enter the  input :");
        string input3 = Console.ReadLine();
        List<Tuple<string, int>> lexemes3 = scanner.ExtractLexems(input3);



        Console.WriteLine("------------------------------");
        foreach (var lexeme in lexemes3)
        {
            var tokenClass = scanner.CheckLexem(lexeme, line);
            Console.WriteLine($"Lexeme: {lexeme.Item1}, Type: {tokenClass.Item1}, Class: {tokenClass.Item2}");
        }


    }
}
