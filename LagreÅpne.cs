using System;
using System.Linq;
using System.Windows.Forms;
using FunkDiv = DiverseFunksjoner.Class1;

namespace Sudoku
{
    public partial class LagreÅpne : Form
    {

        public string Fil { get; set; }
        private bool lagre;
        private bool programLukker = false;

        public LagreÅpne(string[] filer, bool lagre)
        {
            InitializeComponent();
            listBox1.Items.AddRange(filer);
            listBox1.SelectedIndex = 0;
            textBox1.Text = filer[0];
            this.lagre = lagre;
            if (lagre)
            {
                this.Text = "Lagr brett";
                this.button1.Text = "Lagre";
            }
            else
            {
                this.Text = "Åpn brett";
                this.button1.Text = "Åpne";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!lagre && !listBox1.Items.Contains(textBox1.Text))
            {
                MessageBox.Show("Kunne ikke finne filen. Vennligst sjekk at filnavnet stemmer med listen til høyre.");
                return;
            }
            if (lagre && !FilFunksjoner.Class1.SjekkGyldigFilNavn(textBox1.Text))
            {
                MessageBox.Show("Filnavn ikke gyldig. Vennligst sjekk at filnavnet er gyldig og ikke for langt.");
                return;
            }
            Fil = textBox1.Text;
            programLukker = true;
            this.Close();
        }

        private void ListBox1_MouseDoubleClick(object sender, EventArgs e)
        {
            Fil = listBox1.SelectedItem.ToString();
            programLukker = true;
            this.Close();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.SelectedItem.ToString();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Fil = null;
            this.Close();
        }

        void LagreÅpne_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            //if(e.CloseReason == CloseReason.UserClosing) fil = null; //Skjilner ikke mellom this.Close og når brukeren lukker vinduet.
            if (!programLukker) Fil = null;
        }
    }
}

//this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(listBox1_MouseDoubleClick);
//this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LagreÅpne_FormClosing);
