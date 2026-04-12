using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1_gui
{
    public class ParseError : Exception
    {
        private int _idx;
        public int Idx
        {
            get
            {
                return _idx;
            }
        }
        private String incorrStr;

        public String IncorrStr
        {
            get
            {
                return incorrStr;
            }
        }

        public ParseError(String msg, String rem, int index) : base(msg)
        {
            incorrStr = rem;
            _idx = index;
        }
    }

    public class Parser
    {
        private int state;
        private List<Token> tokens;
        private int currentTokenIndex;

        private List<ParseError> errors;

        public List<ParseError> GetErrors()
        {
            return errors;
        }

        public bool Parse(List<Token> tokenList)
        {
            tokens = tokenList;
            state = 1;
            currentTokenIndex = 0;

            errors = new List<ParseError>();

            while (state != 10 && currentTokenIndex < tokens.Count)
            {
                SkipWhiteSpace();
                switch (state)
                {
                    case 1:
                        state1();
                        break;
                    case 2:
                        state2();
                        break;
                    case 3:
                        state3();
                        break;
                    case 4:
                        state4();
                        break;
                    case 5:
                        state5();
                        break;
                    case 6:
                        state6();
                        break;
                    case 7:
                        state7();
                        break;
                    case 8:
                        state8();
                        break;
                }
            }
            if (errors.Count == 0)
            {
                ValidateEndOfInput();
            }
            return errors.Count == 0;
        }

        private Token GetCurrentToken()
        {
            if (currentTokenIndex < tokens.Count)
                return tokens[currentTokenIndex];
            return null;
        }

        private void SkipWhiteSpace()
        {
            while (currentTokenIndex < tokens.Count &&
                   tokens[currentTokenIndex].Type == TokenType.WhiteSpace)
            {
                currentTokenIndex++;
            }
        }

        private void handleError(string eMess, string removed, Token token)
        {
            errors.Add(new ParseError(eMess, removed, token != null ? token.StartPos : -1));
        }

        private void state1()
        {
            SkipWhiteSpace();
            Token token = GetCurrentToken();
            if (token != null && token.Type == TokenType.Const)
            {
                currentTokenIndex++;
                state = 2;
            }
            else
            {
                handleError("Ожидается ключевое слово 'const'.", token != null ? token.Value : null, token);
                while (currentTokenIndex < tokens.Count)
                {
                    if (token != null && token.Type == TokenType.EndOperator)
                    {
                        state = 8;
                        return;
                    }
                    currentTokenIndex++;
                    token = GetCurrentToken();
                    if (token != null && token.Type == TokenType.Id || token.Type == TokenType.IntDigit)
                    {
                        state = 2;
                        return;
                    }
                }
            }
        }

        private void state2()
        {
            SkipWhiteSpace();
            Token token = GetCurrentToken();
            if (token != null && token.Type == TokenType.Id)
            {
                currentTokenIndex++;
                state = 3;
            }
            else
            {
                handleError("Ожидается идентификатор.", token != null ? token.Value : null, token);
                while (currentTokenIndex < tokens.Count)
                {
                    if (token != null && token.Type == TokenType.Id)
                    {
                        state = 2;
                        return;
                    }
                    if (token != null && token.Type == TokenType.Colon)
                    {
                        state = 3;
                        return;
                    }
                    if (token != null && token.Type == TokenType.EndOperator)
                    {
                        state = 8;
                        return;
                    }
                    currentTokenIndex++;
                    token = GetCurrentToken();
                }
            }
        }
        private void state3()
        {
            SkipWhiteSpace();
            Token token = GetCurrentToken();
            if (token != null && token.Type == TokenType.Colon)
            {
                currentTokenIndex++;
                state = 4;
            }
            else
            {
                handleError("Ожидается двоеточие ':'.", token != null ? token.Value : null, token);
                while (currentTokenIndex < tokens.Count)
                {
                    if (token != null && token.Type == TokenType.Integer)
                    {
                        state = 4;
                        return;
                    }
                    if (token!= null && token.Type == TokenType.Id)
                    {
                        state = 4;
                        return;
                    }
                    if (token != null && token.Type == TokenType.EndOperator)
                    {
                        state = 8;
                        return;
                    }
                    currentTokenIndex++;
                    token = GetCurrentToken();
                }
            }
        }

        private void state4()
        {
            SkipWhiteSpace();
            Token token = GetCurrentToken();

            if (token != null && token.Type == TokenType.Integer)
            {
                currentTokenIndex++;
                state = 5;
            }
            else
            {
                handleError("Ожидается ключевое слово 'integer'.", token != null ? token.Value : null, token);
                while (currentTokenIndex < tokens.Count)
                {
                    if (token != null && token.Type == TokenType.Equal)
                    {
                        state = 5;
                        return;
                    }
                    if (token != null && token.Type == TokenType.EndOperator)
                    {
                        state = 8;
                        return;
                    }
                    currentTokenIndex++;
                    token = GetCurrentToken();
                }
            }
        }

        private void state5()
        {
            SkipWhiteSpace();
            Token token = GetCurrentToken();
            if (token != null && token.Type == TokenType.Equal)
            {
                currentTokenIndex++;
                state = 6;
            }
            else
            {
                handleError("Ожидается знак '='.", token != null ? token.Value : null, token);
                while (currentTokenIndex < tokens.Count)
                {
                    if (token != null && (token.Type == TokenType.Minus || token.Type == TokenType.Plus))
                    {
                        state = 6;
                        return;
                    }
                    if (token != null && token.Type == TokenType.IntDigit)
                    {
                        state = 7;
                        return;
                    }
                    if (token != null && token.Type == TokenType.EndOperator)
                    {
                        state = 8;
                        return;
                    }
                    currentTokenIndex++;
                    token = GetCurrentToken();
                    if (token != null && token.Type == TokenType.Id)
                    {
                        state = 6;
                        return;
                    }
                }
            }
        }
        private void state6()
        {
            SkipWhiteSpace();
            Token token = GetCurrentToken();
            if (token != null && (token.Type == TokenType.Minus || token.Type == TokenType.Plus))
            {
                currentTokenIndex++;
                state = 7;
            }
            else
            {
                state = 7;
            }
        }
        private void state7()
        {
            SkipWhiteSpace();
            Token token = GetCurrentToken();
            if (token != null && (token.Type == TokenType.IntDigit))
            {
                currentTokenIndex++;
                state = 8;
            }
            else
            {
                handleError("Ожидается целое число.", token != null ? token.Value : null, token);
                while (currentTokenIndex < tokens.Count)
                {
                    if (token != null && token.Type == TokenType.EndOperator)
                    {
                        state = 8;
                        return;
                    }
                    currentTokenIndex++;
                    token = GetCurrentToken();
                }
            }
        }
        private void state8()
        {
            SkipWhiteSpace();
            Token token = GetCurrentToken();
            if (token != null && token.Type == TokenType.EndOperator)
            {
                currentTokenIndex++;
                if (currentTokenIndex >= tokens.Count) state = 9;
                else state = 1;
            }
            else
            {
                handleError("Ожидается знак ';'.", tokens[currentTokenIndex - 1].Value, tokens[currentTokenIndex - 1]);
                while (currentTokenIndex < tokens.Count)
                {
                    if (token != null && (token.Type == TokenType.Const|| token.Type == TokenType.Id))
                    {
                        state = 1;
                        return;
                    }
                    currentTokenIndex++;
                    token = GetCurrentToken();
                }
            }
        }
        private void ValidateEndOfInput()
        {
            SkipWhiteSpace();
            if (state!=9)
            {
                handleError("Ожидается знак ';'.",tokens[currentTokenIndex - 1].Value, tokens[currentTokenIndex - 1]);
            }
        }
        public void Display(DataGridView grid, Label label)
        {
            grid.Columns.Clear();
            grid.Rows.Clear();
            label.Text = "";
            if (errors.Count > 0)
            {
                grid.AllowUserToAddRows = false;
                grid.AllowUserToDeleteRows = false;
                grid.ReadOnly = true;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.MultiSelect = false;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grid.RowHeadersVisible = false;

                grid.Columns.Add("ErrorFragment", "Неверный фрагмент");
                grid.Columns.Add("Location", "Местоположение");
                grid.Columns.Add("Description", "Описание ошибки");

                grid.Columns["ErrorFragment"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                grid.Columns["Location"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                grid.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                foreach (ParseError error in errors)
                {
                    string location = GetLocationFromIndex(error.Idx);
                    grid.Rows.Add(error.IncorrStr ?? "", location, error.Message);
                }
                label.Text = "Всего ошибок: "+errors.Count.ToString();
            }
            else
            {
                MessageBox.Show("Ошибок нет!", "Результат", MessageBoxButtons.YesNo);
            }
        }
        private string GetLocationFromIndex(int index)
        {
            if (tokens == null || tokens.Count == 0)
                return "Неизвестно";
            foreach (Token token in tokens)
            {
                if (token.AbsoluteIndex <= index && index <= token.EndPos)
                {
                    return $"строка: {token.Line}, позиция: {token.StartPos}-{token.EndPos}";
                }
            }
            return "Неизвестно";
        }
    }
}
