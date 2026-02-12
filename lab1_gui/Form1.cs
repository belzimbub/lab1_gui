using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lab1_gui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIsClosing);
        }
        private int CommitChanges()
        {
            if (richTextBox1.Text.Length > 0)
            {
                DialogResult dlg = MessageBox.Show("Сохранить изменения?", "Предупреждение", MessageBoxButtons.YesNo);
                if (dlg == DialogResult.Yes)
                {
                    FileSave();
                    return 1;
                }
                else
                {
                    return 1;
                }
            }
            else 
            {
                return 1;
            }
        }
        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            if (CommitChanges() == 1)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void FileNew()
        {
            if (CommitChanges() == 1)
            {
                richTextBox1.Clear();
            }
        }
        private void FileOpen()
        {
            if (CommitChanges() == 1)
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && openFileDialog1.FileName.Contains(".txt"))
                {
                    string open = File.ReadAllText(openFileDialog1.FileName);
                    richTextBox1.Text = open;
                }
            }
        }
        private void FileSave()
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string name = saveFileDialog1.FileName + ".txt";
                File.WriteAllText(name, richTextBox1.Text);
            }
        }
        private void FileUndo()
        {
            if (richTextBox1.CanUndo)
            {
                richTextBox1.Undo();
            }
        }
        private void FileRedo()
        {
            if (richTextBox1.CanRedo)
            {
                if (richTextBox1.RedoActionName != "Delete")
                    richTextBox1.Redo();
            }
        }
        private void FileCut()
        {
            if (richTextBox1.SelectedText.Length > 0) richTextBox1.Cut();
        }
        private void FileCopy()
        {
            if (richTextBox1.SelectedText.Length > 0) richTextBox1.Copy();
        }
        private void FilePaste()
        {
            richTextBox1.Paste();
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

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            FileRedo();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            FileCut();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            FileCopy();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            FilePaste();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
