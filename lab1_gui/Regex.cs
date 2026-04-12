using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace lab_compilator
{
    public class RegexClass
    {
        private RichTextBox editorTextBox;
        private DataGridView resultsGridView;
        private ComboBox searchTypeComboBox;
        private Label countLabel;
        private class SearchResult
        {
            public string MatchText { get; set; }
            public int StartIndex { get; set; }
            public int Length { get; set; }
            public int LineNumber { get; set; }
            public int ColumnNumber { get; set; }
        }
        public enum SearchType
        {
            WordsNotEndingWithS,
            NumbersStartingWith9,
            ComplexNumbers
        }
        private readonly Dictionary<SearchType, string> searchPatterns = new Dictionary<SearchType, string>
        {
            { SearchType.WordsNotEndingWithS, @"\b\w*[^sS\s]\b" },
            { SearchType.NumbersStartingWith9, @"\b9\d*\b" },
            { SearchType.ComplexNumbers, @"[+-]?(((\d+\.\d*|\d*\.\d+|\d+)[+-])?((\d+\.\d*|\d*\.\d+|\d+)i|i(\d+\.\d*|\d*\.\d+|\d+)|i)|(\d+\.\d*|\d*\.\d+|\d+)?e\^(\([+-]?|[+-]?\()((\d+\.\d*|\d*\.\d+|\d+)i|i(\d+\.\d*|\d*\.\d+|\d+)|i)\))" }
        };

        private readonly Dictionary<SearchType, string> searchDescriptions = new Dictionary<SearchType, string>
        {
            { SearchType.WordsNotEndingWithS, "Слова, не заканчивающиеся на 's' или 'S'" },
            { SearchType.NumbersStartingWith9, "Числа, начинающиеся на 9" },
            { SearchType.ComplexNumbers, "Комплексные числа" }
        };

        private List<SearchResult> currentResults = new List<SearchResult>();

        public RegexClass(RichTextBox editor, DataGridView resultsGrid,
                               ComboBox searchCombo, Label countLabel)
        {
            this.editorTextBox = editor;
            this.resultsGridView = resultsGrid;
            this.searchTypeComboBox = searchCombo;
            this.countLabel = countLabel;

            InitializeResultsGridView();
            InitializeSearchComboBox();
            SubscribeEvents();
        }

        private void InitializeResultsGridView()
        {
            if (resultsGridView == null) return;

            resultsGridView.AutoGenerateColumns = false;
            resultsGridView.Columns.Clear();

            resultsGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MatchText",
                HeaderText = "Найденная подстрока",
                Width = 200
            });

            resultsGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Position",
                HeaderText = "Позиция (строка, символ)",
                Width = 150
            });

            resultsGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Length",
                HeaderText = "Длина",
                Width = 80
            });

            resultsGridView.AllowUserToAddRows = false;
            resultsGridView.ReadOnly = true;
            resultsGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void InitializeSearchComboBox()
        {
            if (searchTypeComboBox == null) return;

            searchTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            searchTypeComboBox.Items.Clear();

            foreach (var description in searchDescriptions)
            {
                searchTypeComboBox.Items.Add(description.Value);
            }

            if (searchTypeComboBox.Items.Count > 0)
                searchTypeComboBox.SelectedIndex = 0;
        }

        private void SubscribeEvents()
        {
            if (resultsGridView != null)
            {
                resultsGridView.SelectionChanged += OnResultSelected;
            }
        }

        public void PerformSearch()
        {
            try
            {
                ClearResults();
                if (string.IsNullOrWhiteSpace(editorTextBox.Text))
                {
                    MessageBox.Show("Нет данных для поиска. Введите текст в редакторе.",
                                  "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateCountDisplay(0);
                    return;
                }
                SearchType selectedType = GetSelectedSearchType();
                string pattern = searchPatterns[selectedType];
                currentResults = FindMatches(pattern, editorTextBox.Text);
                DisplayResults(currentResults);
                UpdateCountDisplay(currentResults.Count);

                if (currentResults.Count == 0)
                {
                    MessageBox.Show($"Совпадений для типа поиска '{searchDescriptions[selectedType]}' не найдено.",
                                  "Результаты поиска", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}",
                              "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<SearchResult> FindMatches(string pattern, string text)
        {
            var results = new List<SearchResult>();

            try
            {
                RegexOptions options = RegexOptions.Multiline;
                Regex regex = new Regex(pattern, options);
                MatchCollection matches = regex.Matches(text);

                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        var position = GetLineAndColumn(text, match.Index);

                        results.Add(new SearchResult
                        {
                            MatchText = match.Value,
                            StartIndex = match.Index,
                            Length = match.Length,
                            LineNumber = position.LineNumber,
                            ColumnNumber = position.ColumnNumber
                        });
                    }
                }
            }
            catch (RegexParseException ex)
            {
                throw new Exception($"Ошибка в синтаксисе регулярного выражения: {ex.Message}");
            }

            return results;
        }
        private (int LineNumber, int ColumnNumber) GetLineAndColumn(string text, int index)
        {
            if (index < 0 || index > text.Length)
                return (1, 1);

            int lineNumber = 1;
            int columnNumber = 1;

            for (int i = 0; i < index && i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    lineNumber++;
                    columnNumber = 1;
                }
                else if (text[i] != '\r')
                {
                    columnNumber++;
                }
            }

            return (lineNumber, columnNumber);
        }

        private void DisplayResults(List<SearchResult> results)
        {
            if (resultsGridView == null) return;

            resultsGridView.Rows.Clear();

            foreach (var result in results)
            {
                resultsGridView.Rows.Add(
                    result.MatchText,
                    $"строка {result.LineNumber}, символ {result.ColumnNumber}",
                    result.Length
                );
            }
        }
        private void OnResultSelected(object sender, EventArgs e)
        {
            if (resultsGridView.SelectedRows.Count == 0 || editorTextBox == null)
                return;

            int selectedIndex = resultsGridView.SelectedRows[0].Index;
            if (selectedIndex >= 0 && selectedIndex < currentResults.Count)
            {
                var selectedResult = currentResults[selectedIndex];
                HighlightTextInEditor(selectedResult.StartIndex, selectedResult.Length);
            }
        }
        private void HighlightTextInEditor(int startIndex, int length)
        {
            if (editorTextBox == null) return;

            try
            {
                editorTextBox.Focus();
                editorTextBox.Select(startIndex, length);
                editorTextBox.ScrollToCaret();
                editorTextBox.SelectionBackColor = Color.Yellow;
                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 2000;
                timer.Tick += (s, args) =>
                {
                    editorTextBox.SelectionBackColor = editorTextBox.BackColor;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при подсветке текста: {ex.Message}");
            }
        }
        public void ClearResults()
        {
            if (resultsGridView != null)
                resultsGridView.Rows.Clear();

            currentResults.Clear();

            if (editorTextBox != null)
            {
                editorTextBox.SelectAll();
                editorTextBox.SelectionBackColor = editorTextBox.BackColor;
                editorTextBox.Select(0, 0);
            }

            UpdateCountDisplay(0);
        }
        private void UpdateCountDisplay(int count)
        {
            if (countLabel != null)
            {
                countLabel.Text = $"Найдено совпадений: {count}";
                countLabel.ForeColor = count > 0 ? Color.Green : Color.Red;
            }
        }

        private SearchType GetSelectedSearchType()
        {
            if (searchTypeComboBox?.SelectedIndex >= 0)
            {
                string selectedDescription = searchTypeComboBox.SelectedItem.ToString();
                return searchDescriptions.First(x => x.Value == selectedDescription).Key;
            }

            return SearchType.WordsNotEndingWithS;
        }
    }
}