using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1_gui
{
    public enum TokenType
    {
       
        // КЛЮЧЕВЫЕ СЛОВА
        Const = 1,
        Integer = 2,
        // ИНДЕТИФИКАТОРЫ
        Id = 3,
        WhiteSpace = 4,
        Colon = 5,
        IntDigit = 6,
        End_operator = 7,
        // МАТЕМАТИЧЕСКИЕ ЗНАКИ
        Equal = 8,
        Plus = 9,
        Minus = 10,
        // ОШИБКИ
        Error = 0
    }
}
