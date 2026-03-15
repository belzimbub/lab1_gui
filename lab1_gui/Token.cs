using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1_gui
{
    public class Token
    {
        public int Code
        {
            get
            {
                return (int)Type;
            }
        }
        public string Value { get; set; }
        [Browsable(false)]
        public int Line { get; set; }
        [Browsable(false)]
        public int StartPos { get; set; }
        [Browsable(false)]
        public int EndPos { get; set; }
        [Browsable(false)]
        public int AbsoluteIndex { get; set; }
        [Browsable(false)]
        public TokenType Type { get; set; }
        public string TypeName => Type switch
        {
            TokenType.Id => "Идентификатор",
            TokenType.Colon => "Оператор объявления",
            TokenType.IntDigit => "Порядковое число",
            TokenType.WhiteSpace => "Разделитель (пробел)",
            TokenType.End_operator => "Конец оператора",
            TokenType.Equal => "Оператор присваивания",
            TokenType.Minus => "Арифметический оператор (-)",
            TokenType.Plus => "Арифметический оператор (+)",
            TokenType.Error => "Лексическая ошибка",
            TokenType.Const => "Ключевое слово const",
            TokenType.Integer => "Ключевое слово integer",
            _ => "Неизвестная лексема"
        };

        public string Location => $"Стр: {Line}, Поз: {StartPos}-{EndPos}";
    }
}
