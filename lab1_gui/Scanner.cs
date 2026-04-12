using lab1_gui;

public class Scanner
{
    public List<Token> Analyze(string text)
    {
        var tokens = new List<Token>();
        int pos = 0, line = 1, col = 1;

        while (pos < text.Length)
        {
            int startCol = col;
            int startPos = pos;

            if (text[pos] == '\n')
            {
                line++;
                col = 1;
                pos++;
                continue;
            }
            if (text[pos] == ' ' || text[pos] == '\t' || text[pos] == '\r')
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
            if (char.IsLetter(text[pos]) || text[pos] == '_')
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
                        type = TokenType.Const;
                        break;
                    case "integer":
                        type = TokenType.Integer;
                        break;
                    default:
                        type = TokenType.Id;
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
            if (char.IsDigit(text[pos]))
            {
                string lexeme = "";
                while (pos < text.Length && char.IsDigit(text[pos]))
                {
                    lexeme += text[pos];
                    col++;
                    pos++;
                }

                tokens.Add(new Token
                {
                    Type = TokenType.IntDigit,
                    Value = lexeme,
                    Line = line,
                    StartPos = startCol,
                    EndPos = col - 1,
                    AbsoluteIndex = startPos
                });
                continue;
            }
            switch (text[pos])
            {
                case '=':
                    tokens.Add(new Token
                    {
                        Type = TokenType.Equal,
                        Value = "=",
                        Line = line,
                        StartPos = startCol,
                        EndPos = col,
                        AbsoluteIndex = startPos
                    });
                    col++;
                    pos++;
                    continue;

                case ':':
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

                case ';':
                    tokens.Add(new Token
                    {
                        Type = TokenType.EndOperator,
                        Value = ";",
                        Line = line,
                        StartPos = startCol,
                        EndPos = col,
                        AbsoluteIndex = startPos
                    });
                    col++;
                    pos++;
                    continue;

                case '-':
                    tokens.Add(new Token
                    {
                        Type = TokenType.Minus,
                        Value = "-",
                        Line = line,
                        StartPos = startCol,
                        EndPos = col,
                        AbsoluteIndex = startPos
                    });
                    col++;
                    pos++;
                    continue;

                case '+':
                    tokens.Add(new Token
                    {
                        Type = TokenType.Plus,
                        Value = "+",
                        Line = line,
                        StartPos = startCol,
                        EndPos = col,
                        AbsoluteIndex = startPos
                    });
                    col++;
                    pos++;
                    continue;

                default:
                    tokens.Add(new Token
                    {
                        Type = TokenType.Error,
                        Value = text[pos].ToString(),
                        Line = line,
                        StartPos = startCol,
                        EndPos = col,
                        AbsoluteIndex = startPos
                    });
                    pos++;
                    col++;
                    continue;
            }
        }

        return tokens;
    }

    public void Run(DataGridView d, RichTextBox r)
    {
        var tokens = Analyze(r.Text);
        d.DataSource = tokens;

        if (d.Columns.Contains("Code"))
            d.Columns["Code"].HeaderText = "Условный код";
        if (d.Columns.Contains("Value"))
            d.Columns["Value"].HeaderText = "Лексема";
        if (d.Columns.Contains("TypeName"))
            d.Columns["TypeName"].HeaderText = "Тип лексемы";
        if (d.Columns.Contains("Location"))
            d.Columns["Location"].HeaderText = "Местоположение";

        d.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }
}