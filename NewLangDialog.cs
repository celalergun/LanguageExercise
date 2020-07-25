using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LangExercise
{
    public partial class NewLangDialog : Form
    {
        public NewLangDialog()
        {
            InitializeComponent();
        }

        public string LangName { get { return textBoxName.Text; } }
        public string LangCode { get { return textBoxCode.Text; } }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxCode.Text) || String.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Name and Code fields cannot be blank!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
