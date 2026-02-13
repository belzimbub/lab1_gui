using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lab1_gui
{
    public partial class Form1 : Form
    {
        private string FileName = string.Empty;
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIsClosing);
        }
        private int CommitChanges()
        {
            if (richTextBox1.Text.Length > 0)
            {
                DialogResult dlg = MessageBox.Show("Ñîõğàíèòü èçìåíåíèÿ?", "Ïğåäóïğåæäåíèå", MessageBoxButtons.YesNo);
                if (dlg == DialogResult.Yes)
                {
                    FileSave();
                }
            }
            return 1;
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
                FileName = string.Empty;
                richTextBox1.Clear();
            }
        }
        private void FileOpen()
        {
            if (CommitChanges() == 1)
            {
                OpenFileDialog open = new OpenFileDialog();

                open.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
                open.Title = "Open File";
                open.FileName = "";

                if (open.ShowDialog() == DialogResult.OK)
                {
                    // save the opened FileName in our variable
                    this.FileName = open.FileName;
                    this.Text = string.Format("{0}", Path.GetFileNameWithoutExtension(open.FileName));
                    StreamReader reader = new StreamReader(open.FileName);
                    richTextBox1.Text = reader.ReadToEnd();
                    reader.Close();
                }
            }
        }
        private void FileSave()
        {
            if (string.IsNullOrEmpty(this.FileName))
            {
                SaveFileDialog saving = new SaveFileDialog();

                saving.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                saving.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
                saving.Title = "Save As";
                saving.FileName = "Untitled";

                if (saving.ShowDialog() == DialogResult.OK)
                {
                    FileName = saving.FileName;
                    StreamWriter writing = new StreamWriter(saving.FileName);
                    writing.Write(richTextBox1.Text);
                    writing.Close();
                }
            }
            else
            {
                StreamWriter writer = new StreamWriter(this.FileName);
                writer.Write(richTextBox1.Text);
                writer.Close();
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
        private void ñîçäàíèåToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileNew();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FileNew();
        }

        private void îòêğûòèåToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void ñîõğàíåíèåToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileSave();
        }
        private void ñîõğàíåíèåÊàêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileName = string.Empty;
            FileSave();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FileUndo();
        }
        private void îòìåíèòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUndo();
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            FileRedo();
        }
        private void ïîâòîğèòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileRedo();
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            FileCut();
        }
        private void âûğåçàòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileCut();
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            FileCopy();
        }
        private void êîïèğîâàòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileCopy();
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            FilePaste();
        }
        private void âñòàâèòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilePaste();
        }
        private void âûõîäToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void óäàëèòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void âûäåëèòüÂñåToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }
    }
}
