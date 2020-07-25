using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LangExercise
{
    public partial class MainForm : Form
    {
        private LanguageData _languageData = null;
        private List<string> _recentFiles = new List<string>();
        string _lastLangFile;

        public MainForm()
        {
            InitializeComponent();
            _lastLangFile = System.Configuration.ConfigurationManager.AppSettings["LastLanguageFile"];
            if (File.Exists(_lastLangFile))
                LoadLanguage(_lastLangFile);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenALanguageFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Language file (*.lang)|*.lang|All files|*.*";
                ofd.CheckFileExists = true;
                var res = ofd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    string fileName = ofd.FileName;
                    LoadLanguage(fileName);
                    bool isIn = _recentFiles.Any(x => x.Equals(fileName, StringComparison.OrdinalIgnoreCase));
                    if (!isIn)
                    {
                        _recentFiles.Insert(0, fileName);
                    }

                    while (_recentFiles.Count > 5)
                        _recentFiles.RemoveAt(_recentFiles.Count - 1);
                }
            }
        }

        private void LoadLanguage(string fileName)
        {
            _languageData = LanguageData.LoadFromFile(fileName);
            SetTitle();
        }

        private void SetTitle()
        {
            Text = $"Language Exercise - {_languageData.Name} ({_languageData.Code})";
        }

        private void SaveLanguageFile(string fileName)
        {
            _languageData.SaveToFile(fileName);
        }

        private void SaveFileAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Language file (*.lang)|*.lang|All files|*.*";
                sfd.DefaultExt = ".lang";
                var res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    SaveLanguageFile(sfd.FileName);
                }
            }
        }

        private void NewLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewLangDialog newLangDialog = new NewLangDialog())
            {
                var res = newLangDialog.ShowDialog();
                if (res == DialogResult.OK)
                {
                    _languageData = new LanguageData(newLangDialog.LangName, newLangDialog.LangCode);
                    SetTitle();
                }
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A basic language exercise utility to memorize words", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EditLanguageFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_languageData == null)
            {
                MessageBox.Show("You need to create a new language file or open an existing file to save changes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_languageData.IsModified)
                return;

            SaveLanguageFile(_lastLangFile);
        }
    }
}