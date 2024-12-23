using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsing
{
   

public class Parser
    {
        private struct CurrentToken
        {
            public string Value;
            public string Type;
            public int Line;
        }

        private CurrentToken currentToken;
        private int tokenId = 0;
        public bool flag = true;

        public List<string> Tokens { get; set; } = new List<string>();
        public List<string> TokensType { get; set; } = new List<string>();
        public List<int> Lines { get; set; } = new List<int>();

        public void Init()
        {
            currentToken = new CurrentToken
            {
                Value = Tokens[0],
                Type = TokensType[0],
                Line = Lines[0]
            };
        }

        public void Statement(string token, string tokenType)
        {
            if (!flag || tokenId >= Tokens.Count)
                return;

            if (tokenType == "IF")
            {
                Match("IF", currentToken.Type);
                Match("LPAREN", currentToken.Type);
                BExpression(currentToken.Type);
                Match("RPAREN", currentToken.Type);

                if (currentToken.Type == "LQURLY")
                    Block();
                else
                    Statement(currentToken.Value, currentToken.Type);
            }
            else if (tokenType == "WHILE")
            {
                Match("WHILE", currentToken.Type);
                Match("LPAREN", currentToken.Type);
                BExpression(currentToken.Type);
                Match("RPAREN", currentToken.Type);

                if (currentToken.Type == "LQURLY")
                    Block();
                else
                    Statement(currentToken.Value, currentToken.Type);
            }
            else if (tokenType == "ID")
            {
                Assignment(currentToken.Type);
                if (GetNextToken())
                {
                    Statement(currentToken.Value, currentToken.Type);
                }
            }
            else
            {
                flag = false;
            }
        }

        private void Match(string expectedToken, string currentTokenType)
        {
            if (!flag) return;

            if (expectedToken == currentTokenType)
                GetNextToken();
            else
            {
                Console.WriteLine($"Error: Expected {expectedToken} but found {currentTokenType} on line {currentToken.Line}");
                flag = false;
            }
        }

        private void Assignment(string currentTokenType)
        {
            GetNextToken();
            if (currentToken.Type == "ASSIGN")
                GetNextToken();
            else
            {
                Console.WriteLine($"Error: Expected assignment operator but found {currentTokenType} on line {currentToken.Line}");
                flag = false;
                return;
            }

            Expression(currentToken.Type);

            if (currentToken.Type == "SEMICOLON")
            {
                // Valid statement
            }
            else
            {
                Console.WriteLine($"Error: Expected semicolon but found {currentToken.Type} on line {currentToken.Line}");
                flag = false;
            }
        }

        private void Expression(string currentTokenType)
        {
            Term(currentTokenType);
            if (currentToken.Type == "PLUS" || currentToken.Type == "MINUS")
            {
                GetNextToken();
                Term(currentToken.Type);
            }
        }

        private void Term(string currentTokenType)
        {
            Factor(currentTokenType);
            if (currentToken.Type == "MULT" || currentToken.Type == "DIV")
            {
                GetNextToken();
                Factor(currentToken.Type);
            }
        }

        private void Factor(string currentTokenType)
        {
            if (currentTokenType == "ID" || currentTokenType == "NUMCONST")
            {
                GetNextToken();
            }
            else if (currentTokenType == "LPAREN")
            {
                Match("LPAREN", currentTokenType);
                Expression(currentToken.Type);
                Match("RPAREN", currentToken.Type);
            }
            else
            {
                Console.WriteLine($"{currentTokenType} not expected on line {currentToken.Line}");
                flag = false;
            }
        }

        private void BExpression(string currentTokenType)
        {
            Expression(currentTokenType);
            if (new[] { "EQ", "NEQ", ">=", "<=", ">", "<" }.Contains(currentToken.Type))
            {
                GetNextToken();
                Expression(currentToken.Type);
            }
        }

        private void Block()
        {
            Match("LQURLY", currentToken.Type);
            StatementList(currentToken.Value, currentToken.Type);
            Match("RQURLY", currentToken.Type);
        }

        private void StatementList(string token, string tokenType)
        {
            Statement(token, tokenType);
            RestList(currentToken.Value, currentToken.Type);
        }

        private void RestList(string token, string tokenType)
        {
            if (tokenType == "SEMICOLON")
            {
                Match("SEMICOLON", tokenType);
                StatementList(currentToken.Value, currentToken.Type);
            }
        }

        private bool GetNextToken()
        {
            if (tokenId >= Tokens.Count) return false;

            currentToken = new CurrentToken
            {
                Type = TokensType[tokenId],
                Value = Tokens[tokenId],
                Line = Lines[tokenId]
            };
            tokenId++;
            return true;
        }
    }

}

