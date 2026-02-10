using System.Windows.Forms;

namespace lab1_gui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void FileNew()
        {
            richTextBox1.Clear();
        }
        private void FileOpen()
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && openFileDialog1.FileName.Contains(".txt"))
            {
                string open = File.ReadAllText(openFileDialog1.FileName);
                richTextBox1.Text = open;
            }
            else
            {
                MessageBox.Show("Объект не является текстовым файлом.");
            }
        }
        private void FileUndo()
        {
            
        }
        private void FileSave()
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string name = saveFileDialog1.FileName + ".txt";
                File.WriteAllText(name, richTextBox1.Text);
            }
        }
        private void созданиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileNew();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FileNew();
        }

        private void открытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileOpen();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FileOpen();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            FileSave();
        }

        private void сохранениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileSave();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FileUndo();
        }
    }
}
