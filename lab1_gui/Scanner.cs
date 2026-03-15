using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static lab1_gui.Token;

namespace lab1_gui
{
    public class Scanner
    {
        public List<Token> Analyze(string text)
        {
            var tokens = new List<Token>();
            int pos = 0, line = 1, col = 1;
            bool cond_colon = false, cond_keyword = true, cond_value = false;

            while (pos < text.Length)
            {
                char c = text[pos];

                int startCol = col;
                int startPos = pos;

                if (c == '\n')
                {
                    line++;
                    col = 1;
                    pos++;
                    continue;
                }
                if (c == ' ' || c == '\b' || c == '\r')
                {
                    string spaces = "";
                    while (pos < text.Length && (text[pos] == ' ' || text[pos] == '\t' || text[pos] == '\r'))
                    {
                        spaces += text[pos];
                        pos++; 
                        col++;
                    }

                    tokens.Add(new Token
                    {
                        Type = TokenType.WhiteSpace,
                        Value = "(пробел)",
                        Line = line,
                        StartPos = startCol,
                        EndPos = col - 1,
                        AbsoluteIndex = startPos
                    });
                    continue;
                }
                if (char.IsLetter(c) || c == '_')
                {
                    string lexeme = "";
                    while (pos < text.Length && (char.IsLetterOrDigit(text[pos]) || text[pos] == '_'))
                    {
                        lexeme += text[pos];
                        col++; 
                        pos++;
                    }
                    TokenType type;
                    switch (lexeme)
                        {
                            case "const":
                                if (cond_colon == false && cond_keyword == true)
                                {
                                    type = TokenType.Const;
                                    cond_keyword = false;
                                }
                                else
                                {
                                    type = TokenType.Error;
                                }
                                break;
                            case "integer":
                                if (cond_colon == true && cond_keyword == true)
                                {
                                    type = TokenType.Integer;
                                    cond_keyword = false;
                                }
                                else
                                {
                                    type = TokenType.Error;
                                }
                                break;
                            default:
                                if (cond_colon == true || cond_keyword == true || cond_value==true)
                                {
                                    type = TokenType.Error;
                                }
                                else
                                {
                                    type = TokenType.Id;
                                    cond_colon = true;
                                }
                                break;
                        }
                    tokens.Add(new Token
                    {
                        Type = type,
                        Value = lexeme,
                        Line = line,
                        StartPos = startCol,
                        EndPos = col - 1,
                        AbsoluteIndex = startPos
                    });
                    continue;
                }
                
                if (c == '=')
                {
                    TokenType type = TokenType.Error;
                    if (cond_keyword == true)
                    {
                        type = TokenType.Error;
                    }
                    else
                    {
                        
                        type = TokenType.Equal;
                        cond_value = true;
                    }
                    col++;
                    pos++;
                    tokens.Add(new Token
                    {
                        Type = type,
                        Value = char.ToString(c),
                        Line = line,
                        StartPos = startCol,
                        EndPos = col - 1,
                        AbsoluteIndex = startPos
                    });
                    continue;
                }
                if (c == ':')
                {
                    if (cond_colon == true)
                    {
                        cond_keyword = true;
                        tokens.Add(new Token
                        {
                            Type = TokenType.Colon,
                            Value = ":",
                            Line = line,
                            StartPos = startCol,
                            EndPos = col,
                            AbsoluteIndex = startPos
                        });
                        col++; 
                        pos++;
                        continue;
                    }
                }
                if (c == ';')
                {
                    cond_colon = false;
                    cond_value = false;
                    cond_keyword = true;
                    tokens.Add(new Token
                    {
                        Type = TokenType.End_operator,
                        Value = ";",
                        Line = line,
                        StartPos = startCol,
                        EndPos = col,
                        AbsoluteIndex = startPos
                    });
                    col++; 
                    pos++;
                    continue;
                }
                if (char.IsDigit(c))
                {
                    string lexeme = "";
                    while (pos < text.Length && char.IsDigit(text[pos]))
                    {
                        lexeme += text[pos];
                        col++; 
                        pos++;
                    }
                    TokenType tokenType = TokenType.IntDigit;
                    tokens.Add(new Token
                    {
                        Type = tokenType,
                        Value = lexeme,
                        Line = line,
                        StartPos = startCol,
                        EndPos = col - 1,
                        AbsoluteIndex = startPos
                    });
                    continue;
                }
                tokens.Add(new Token
                {
                    Type = TokenType.Error,
                    Value = c.ToString(),
                    Line = line,
                    StartPos = startCol,
                    EndPos = col,
                    AbsoluteIndex = startPos
                });
                pos++; 
                col++;
            }
            return tokens;
        }
        public void Run(DataGridView d, RichTextBox r)
        {
            d.DataSource = Analyze(r.Text);
        }
    }
}
