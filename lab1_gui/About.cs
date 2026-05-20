using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace lab1_gui
{
    public class About
    {
        public static void AboutProgram()
        {
            MessageBox.Show("Текстовый редактор, который в дальнейшем будет расширен до полноценного языкового процессора для анализа исходного кода.","О программе");
        }
    }
}
