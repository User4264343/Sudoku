using System;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class FormVisLøsning : Form
    {
        public FormVisLøsning(string tekst)
        {
            InitializeComponent();
            label.Text = tekst;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

