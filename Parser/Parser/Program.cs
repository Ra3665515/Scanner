using System;
using System.Collections.Generic;
using System.IO;

namespace Parsing
{
    class Program
    {
        static void Main(string[] args)
        {
            var tokens = new List<string>();
            var tokensType = new List<string>();
            var lines = new List<int>();

            using (var reader = new StreamReader("D:\\TASKS .NET\\Parser\\Parser\\output.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    tokens.Add(parts[0]);
                    tokensType.Add(parts[2]);
                    lines.Add(int.Parse(parts[3]));
                }
            }

            var parser = new Parser
            {
                Tokens = tokens,
                TokensType = tokensType,
                Lines = lines
            };

            parser.Init();
            parser.Statement(tokens[0], tokensType[0]);

            if (parser.flag)
                Console.WriteLine("Accept");
        }
    }
}