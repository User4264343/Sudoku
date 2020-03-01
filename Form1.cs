using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FunkDiv = DiverseFunksjoner.Class1; 

namespace Sudoku
{

    public class FeilIBrettException : ApplicationException
    {
        public FeilIBrettException(string feil) : base(feil) { }
        public FeilIBrettException(string feil, Exception innerException) : base(feil, innerException) { }

    }

    class Størrelse
    {
        public readonly double faktor;
        public readonly double størrelseRute;
        public readonly int størrelseRuteInt;
        public readonly double størrelseKnappX;
        public readonly double størrelseKnappY;
        public readonly int størrelseKnappXInt;
        public readonly int størrelseKnappYInt;
        public readonly double størrelseRuteKladdKort;
        public readonly int størrelseRuteKladdKortInt;
        public readonly int størrelseFont;
        public readonly int størrelseFontLiten;
        public readonly double plasseringKnappX;
        public readonly double plasseringLangVenstreOpp;
        public readonly double plasseringKortVenstreOpp;
        public readonly double plasseringLangHøyreNed;
        public readonly double plasseringKortHøyreNed;
        public readonly int plasseringKnappXInt;
        public readonly int plasseringLangVenstreOppInt;
        public readonly int plasseringKortVenstreOppInt;
        public readonly int plasseringLangHøyreNedInt;
        public readonly int plasseringKortHøyreNedInt;

        public Størrelse(double faktor, int nRot)
        {
            int n = nRot * nRot;
            this.faktor = faktor;
            this.størrelseRute = 60 * faktor;
            this.størrelseRuteInt = Convert.ToInt32(størrelseRute);
            this.størrelseKnappX = 115 * faktor;
            this.størrelseKnappY = 37 * faktor;
            this.størrelseKnappXInt = Convert.ToInt32(størrelseKnappX);
            this.størrelseKnappYInt = Convert.ToInt32(størrelseKnappY);
            this.størrelseRuteKladdKort = størrelseRute / 3;
            this.størrelseRuteKladdKortInt = Convert.ToInt32(størrelseRuteKladdKort);
            this.størrelseFont = Convert.ToInt32(størrelseRute / 2 * 1.1);
            this.størrelseFontLiten = Convert.ToInt32(størrelseRute / 6 * 1.1);
            this.plasseringKnappX = 4 * størrelseRuteKladdKort * 1.5 + (n + 1) * størrelseRute + (nRot - 1) + 10 * faktor;
            this.plasseringLangVenstreOpp = 5 * faktor + størrelseRuteKladdKort * 1.5;
            this.plasseringKortVenstreOpp = 5 * faktor;
            this.plasseringLangHøyreNed = 3 * størrelseRuteKladdKort * 1.5 + (n + 1) * størrelseRute + (nRot - 1);
            this.plasseringKortHøyreNed = 2 * størrelseRuteKladdKort * 1.5 + (n + 1) * størrelseRute + (nRot - 1);
            this.plasseringKnappXInt = Convert.ToInt32(plasseringKnappX);
            this.plasseringLangVenstreOppInt = Convert.ToInt32(plasseringLangVenstreOpp);
            this.plasseringKortVenstreOppInt = Convert.ToInt32(plasseringKortVenstreOpp);
            this.plasseringLangHøyreNedInt = Convert.ToInt32(plasseringLangHøyreNed);
            this.plasseringKortHøyreNedInt = Convert.ToInt32(plasseringKortHøyreNed);
        }
    }

    public partial class Form1 : Form
    {
        private int nRot;
        private int n;
        private static Størrelse størrelse;
        private static Brett brett;
        private bool lettModus = false;
        private bool visKladdModus = false;
        private bool kladdModus = false;
        /// <summary>
        /// Bool that shall prevent the textN_LostFocus event from being called too often.
        /// </summary>
        private bool focus = true;
        private KeyValuePair<int, int> hintRute = new KeyValuePair<int, int>();
         
        public Form1(int nRot, double faktor)
        {
            this.nRot = nRot;
            n = nRot * nRot;

            størrelse = new Størrelse(faktor, nRot);
            brett = new Brett(nRot);
            InitializeComponent();
        }

        private void OppdaterDgValue()
        {
            if (brett.låst) buttonLås.Text = "Lås opp brett";
            else buttonLås.Text = "Lås brett";
            lettModus = checkBoxLett.Checked;
            visKladdModus = checkBoxVisKladd.Checked;
            kladdModus = checkBoxKladd.Checked;
            StørrelseKorrigering();
            foreach (Celle c in brett)
            {
                if (visKladdModus && c.verdi == ' ') dg[c.kol, c.rad].Value = brett.kladd.brett[c.rad, c.kol];
                else dg[c.kol, c.rad].Value = c.verdi;
                if (c.låst) dg[c.kol, c.rad].Style.ForeColor = System.Drawing.Color.Red;
                else dg[c.kol, c.rad].Style.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void LåsBrett()
        {
            brett.LåsBrett();
            brett.VisLøstBrett(out string løsning);
            labelTekst.Text = løsning;
            foreach (Celle c in brett) if (c.låst) dg[c.kol, c.rad].Style.ForeColor = System.Drawing.Color.Red;
            buttonLås.Text = "Lås opp brett";
        }

        private void LåsOppBrett()
        {
            brett.LåsOppBrett();
            foreach (Celle c in brett) dg[c.kol, c.rad].Style.ForeColor = System.Drawing.Color.Black;
            buttonLås.Text = "Lås brett";
        }

        void TekstN_LostFocus(object sender, EventArgs e)
        {
            if (focus)
            {
                if (Int32.TryParse(tekstN.Text, out int nRotTemp) && nRotTemp > 1 && nRotTemp < 7)
                {
                    if (nRot == nRotTemp) return;
                    focus = false;
                    bool fortsett = true;
                    if (brett.gjenværendeCeller < n * n)
                    {
                        JaNeiVindu.Class1 spørreVindu = new JaNeiVindu.Class1();
                        fortsett = spørreVindu.JaNei("Hvis du fortsetter, så vil du miste det nåværendet spillet.\nVil du endre brettstørrelsen?");
                    }
                    if (fortsett)
                    {
                        RemoveObjects();
                        nRot = nRotTemp;
                        n = nRot * nRot;
                        double faktor = 1;
                        if (nRot == 3) faktor = 0.9;
                        else if (nRot == 4) faktor = 0.7;
                        else if (nRot == 5) faktor = 0.5;
                        størrelse = new Størrelse(faktor, nRot);
                        CreateObjects();
                        Construct();
                        LayoutObjects();
                        this.Refresh();
                        brett = new Brett(nRot);
                        OppdaterDgValue();
                        NullstillKladd();
                        BindKladd();
                    }
                    focus = true;
                }
                else labelTekst.Text = "Kunne ikke lese verdien. Venligst oppgi verdien som et heltall (f. eks. 4).";
            }
        }

        void TekstStørrelse_LostFocus(object sender, EventArgs e)
        {
            if (Double.TryParse(tekstStørrelse.Text, out double faktor))
            {
                if (størrelse.faktor == faktor) return;
                størrelse = new Størrelse(faktor, nRot);
                Placement();
                this.Refresh();
                StørrelseKorrigering();
            }
            else labelTekst.Text = "Kunne ikke lese verdien. Venligst oppgi verdien som et tall (f. eks. 0.7).";
        }

        private void StørrelseKorrigering()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (visKladdModus && brett[i, j].verdi == ' ') dg[j, i].Style.Font = new System.Drawing.Font("", størrelse.størrelseFontLiten);
                    else dg[j, i].Style.Font = new System.Drawing.Font("", størrelse.størrelseFont);
                }
            }
        }

        private void LeggTegnTilKladd(ref string kladdVerdi, char tegn)
        {
            if (kladdVerdi.Length > 0)
            {
                if (kladdVerdi.Length <= nRot)
                {
                    if (kladdVerdi.Length % nRot == 0) kladdVerdi += '\n';
                }
                else if ((kladdVerdi.Length - nRot) % (nRot + 1) == 0) kladdVerdi += '\n';
            }
            kladdVerdi += tegn;
        }

        private void FjernTegnFraKladd(int rad, int kol, char verdi)
        {
            int blokk = brett[rad, kol].blokk;
            foreach (Celle c in brett) if (c.verdi == ' ' && (c.rad == rad || c.kol == kol || c.blokk == blokk)) FjernTegnFraKladdRute(c.rad, c.kol, verdi);
        }

        private void FjernTegnFraKladdRute(int rad, int kol, char verdi)
        {
            string temp = brett.kladd.brett[rad, kol].Replace(verdi.ToString(), "");
            temp = temp.Replace("\n", "");
            brett.kladd.brett[rad, kol] = "";
            foreach (char c in temp) LeggTegnTilKladd(ref brett.kladd.brett[rad, kol], c); //Jeg gjør dette så \n skal komme ut riktig.
            if (visKladdModus) dg[kol, rad].Value = brett.kladd.brett[rad, kol];
        }

        private void TastTastet(Object sender, KeyPressEventArgs tast)
        {
            if ('\b' == tast.KeyChar) return;
            int rad = dg.CurrentCell.RowIndex;
            int kol = dg.CurrentCell.ColumnIndex;
            if (brett[rad, kol].låst) return;
            if (kladdModus)
            {
                if (brett[rad, kol].verdi == ' ')
                {
                    LeggTegnTilKladd(ref brett.kladd.brett[rad, kol], tast.KeyChar);
                    dg.CurrentCell.Value = brett.kladd.brett[rad, kol];
                }
            }
            else
            {
                if (Celle.MuligeVerdier(n).Contains(tast.KeyChar.ToString().ToUpperInvariant()))
                {
                    char verdi = tast.KeyChar.ToString().ToUpperInvariant()[0];
                    if (brett[rad, kol].verdi == verdi) return;
                    if (lettModus)
                    {
                        if (brett[rad, kol].verdi == ' ')
                        {
                            if (!brett[rad, kol].tillatteVerdier.Contains(verdi)) return;
                        }
                        else
                        {
                            char verdiGammel = brett[rad, kol].verdi;
                            brett.SettVerdi(rad, kol, ' ');
                            if (!brett[rad, kol].tillatteVerdier.Contains(verdi))
                            {
                                brett.SettVerdi(rad, kol, verdiGammel);
                                return;
                            }
                        }
                    }
                    brett.SettVerdi(rad, kol, verdi);
                    brett.kladd.brett[rad, kol] = "";
                    dg.CurrentCell.Value = verdi;
                    if (visKladdModus) dg.CurrentCell.Style.Font = new System.Drawing.Font("", størrelse.størrelseFont);

                    if (brett.gjenværendeCeller == 0)
                    {
                        if (!brett.løst && !brett.feil) brett.feil = !brett.SjekkBrettLøst();
                        if (!brett.feil)
                        {
                            labelTekst.Text = "Gratulerer!\nBrett løst.";
                            MessageBox.Show("Gratulerer!\nBrett løst.");
                        }
                        else MessageBox.Show("Det er dessverre en feil på brettet :(");
                    }
                    else FjernTegnFraKladd(rad, kol, verdi);
                }
            }
        }

        void Dg_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int rad = dg.CurrentCell.RowIndex;
            int kol = dg.CurrentCell.ColumnIndex;
            if ((Keys.Delete == e.KeyData || Keys.Back == e.KeyData) && !brett[rad, kol].låst)
            {
                if (kladdModus && brett[rad, kol].verdi == ' ')
                {
                    if (Keys.Back == e.KeyData && !string.IsNullOrEmpty(brett.kladd.brett[rad, kol])) brett.kladd.brett[rad, kol] = brett.kladd.brett[rad, kol].Remove(brett.kladd.brett[rad, kol].Length - 1);
                    else if (Keys.Delete == e.KeyData) brett.kladd.brett[rad, kol] = "";
                    dg.CurrentCell.Value = brett.kladd.brett[rad, kol];
                }
                else if (!kladdModus && brett[rad, kol].verdi != ' ')
                {
                    dg.CurrentCell.Value = "";
                    brett.SettVerdi(rad, kol, ' ');
                }
                else if (visKladdModus && brett[rad, kol].verdi == ' ')
                { //Jeg vil ikke slette verdier i brett når spillet er i kladdemodus. Jeg vil derimot slette kladden når den synes.
                    dg.CurrentCell.Value = "";
                    brett.kladd.brett[rad, kol] = "";
                }
            }
        }

        void Dg_MouseClick(object sender, MouseEventArgs e)
        {
            if (lettModus)
            {
                int rad = dg.CurrentCell.RowIndex;
                int kol = dg.CurrentCell.ColumnIndex;
                string ut = "rad " + rad + " kol " + kol + "  blokk " + brett[rad, kol].blokk + " celle i blokk " + brett[rad, kol].celleIBlokk + '\n';
                foreach (char c in brett[rad, kol].tillatteVerdier) if (c != ' ') ut += c + " ";
                ut += '\n';
                ut += "Gjenværende celler: " + brett.gjenværendeCeller;
                labelTekst.Text = ut;
            }
        }

        private void CheckBoxLett_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLett.Checked) lettModus = true;
            else
            {
                lettModus = false;
                labelTekst.Text = "";
            }
        }

        private void CheckBoxVisKladd_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxVisKladd.Checked)
            {
                visKladdModus = true;
                checkBoxKladd.Visible = true;
                buttonFyllKladd.Visible = true;
                foreach (Celle c in brett)
                {
                    if (c.verdi == ' ')
                    {
                        dg[c.kol, c.rad].Value = brett.kladd.brett[c.rad, c.kol];
                        dg[c.kol, c.rad].Style.Font = new System.Drawing.Font("", størrelse.størrelseFontLiten);
                    }
                }
            }
            else
            {
                visKladdModus = false;
                checkBoxKladd.Checked = false;
                checkBoxKladd.Visible = false;
                kladdModus = false;
                buttonFyllKladd.Visible = false;
                foreach (Celle c in brett)
                {
                    if (c.verdi == ' ')
                    {
                        dg[c.kol, c.rad].Value = brett[c.rad, c.kol].verdi;
                        dg[c.kol, c.rad].Style.Font = new System.Drawing.Font("", størrelse.størrelseFont);
                    }
                }
            }
        }

        private void CheckBoxKladd_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxKladd.Checked)
            {
                kladdModus = true;
                tekstFjernKladdVerdi.Visible = true;
                buttonFjernKladdVerdi.Visible = true;
            }
            else
            {
                kladdModus = false;
                tekstFjernKladdVerdi.Visible = false;
                buttonFjernKladdVerdi.Visible = false;
            }
        }

        private void ButtonFjernKladdVerdi_Click(object sender, EventArgs e)
        {
            if (brett[dg.CurrentCell.RowIndex, dg.CurrentCell.ColumnIndex].verdi != ' ') return;
            foreach (char c in tekstFjernKladdVerdi.Text) FjernTegnFraKladdRute(dg.CurrentCell.RowIndex, dg.CurrentCell.ColumnIndex, c);
        }

        private void NullstillKladd()
        {
            for (int l = 0; l < Kladd.antallLinjer; l++) for (int i = 0; i < n; i++) tekstKladdelinjer[l, i].Text = "";
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) brett.kladd.brett[i, j] = "";
        }

        private void KopierKladdFraBrett()
        {
            for (int l = 0; l < Kladd.antallLinjer; l++) for (int i = 0; i < n; i++) tekstKladdelinjer[l, i].Text = brett.kladd.kladdelinjer[l, i].Verdi;
        }

        private void ButtonNy_Click(object sender, EventArgs e)
        {
            bool fortsett = true;
            if (brett.gjenværendeCeller < n * n)
            {
                JaNeiVindu.Class1 spørreVindu = new JaNeiVindu.Class1();
                fortsett = spørreVindu.JaNei("Hvis du fortsetter, så vil du miste det nåværendet spillet.\nVil du nullstille brettet?");
            }
            if (fortsett)
            {
                brett = new Brett(nRot);
                OppdaterDgValue();
                NullstillKladd();
                BindKladd();
            }
        }

        private void ButtonLagSpill_Click(object sender, EventArgs e)
        {
            string lenke = "";
            if (nRot == 3) lenke = "http://www.menneske.no/sudoku/random.html";
            else if (nRot == 4) lenke = "http://www.menneske.no/sudoku/4/random.html";
            else
            {
                labelTekst.Text = "Greier ikke å lage " + nRot + "x" + nRot + " brett.";
                return;
            }
            bool fortsett = true;
            if (brett.gjenværendeCeller < n * n)
            {
                JaNeiVindu.Class1 spørreVindu = new JaNeiVindu.Class1();
                fortsett = spørreVindu.JaNei("Hvis du fortsetter, så vil du miste det nåværendet spillet.\nVil du opprette nytt spill?");
            }
            if (fortsett)
            {
                brett.LagBrettFraNett(lenke, out string tekst);
                OppdaterDgValue();
                NullstillKladd();
                BindKladd();
                labelTekst.Text = tekst;
            }
        }

        private void ButtonLagre_Click(object sender, EventArgs e)
        {
            if (brett.gjenværendeCeller == n * n)
            {
                labelTekst.Text = "Tomme brett blir ikke lagret";
                return;
            }
            string mappe = Application.StartupPath + "\\Lagret_brett\\";
            string[] filer = System.IO.Directory.GetFiles(mappe, "*.dat");
            for (int i = 0; i < filer.Length; i++)
            {
                filer[i] = filer[i].Remove(0, mappe.Length);
                filer[i] = filer[i].Remove(filer[i].Length - 4); //Removes .dat ending
            }
            LagreÅpne form = new LagreÅpne(filer, true);
            form.ShowDialog();
            string fil = form.Fil;
            if (String.IsNullOrEmpty(fil)) return;
            fil = new System.IO.DirectoryInfo(Application.StartupPath).FullName + "\\Lagret_brett\\" + fil + ".dat";
            brett.Lagre(fil);
        }

        private void ButtonÅpne_Click(object sender, EventArgs e)
        {
            if (brett.gjenværendeCeller != n * n)
            {
                JaNeiVindu.Class1 spørreVindu = new JaNeiVindu.Class1();
                if (!spørreVindu.JaNei("Hvis du fortsetter, så vil du miste det nåværendet brettet.\nVil du åpne et lagret brett?")) return;
            }

            string mappe = Application.StartupPath + "\\Lagret_brett\\";
            string[] filer = System.IO.Directory.GetFiles(mappe, "*.dat");
            for (int i = 0; i < filer.Length; i++)
            {
                filer[i] = filer[i].Remove(0, mappe.Length);
                filer[i] = filer[i].Remove(filer[i].Length - 4); //Removes .dat ending
            }
            LagreÅpne form = new LagreÅpne(filer, false);
            form.ShowDialog();
            string fil = form.Fil;
            if (String.IsNullOrEmpty(fil)) return;
            fil = new System.IO.DirectoryInfo(Application.StartupPath).FullName + "\\Lagret_brett\\" + fil + ".dat";

            NullstillKladd();
            if (!brett.Åpne(fil, out string beskjed))
            {
                labelTekst.Text = beskjed;
                return;
            }
            OppdaterDgValue();
            brett.VisLøstBrett(out string løsning);
            labelTekst.Text = løsning;
            KopierKladdFraBrett();
            BindKladd();
        }

        private void ButtonLås_Click(object sender, EventArgs e)
        {
            if (brett.låst)
            {
                JaNeiVindu.Class1 spørreVindu = new JaNeiVindu.Class1();
                if (spørreVindu.JaNei("Vil du virkelig låse opp brettet?")) LåsOppBrett();
            }
            else
            {
                if (brett.feil)
                {
                    JaNeiVindu.Class1 spørreVindu = new JaNeiVindu.Class1();
                    if (spørreVindu.JaNei("Det er feil på brettet. Vil du likevel låse?")) LåsBrett();
                }
                else LåsBrett();
            }
        }

        private void ButtonSjekkFeil_Click(object sender, EventArgs e)
        {
            //if(!feil) SjekkFeil(); //Dette skal ikke være nødvendig.
            if (brett.feil) labelTekst.Text = "Feil funnet";
            else labelTekst.Text = "Ingen feil funnet";
        }

        private void ButtonSisteRette_Click(object sender, EventArgs e)
        {
            if (brett.feil)
            {
                JaNeiVindu.Class1 spørreVindu = new JaNeiVindu.Class1();
                if (spørreVindu.JaNei("Trykk ja for å åpne siste brettversjon med alle ruter riktige."))
                {
                    brett.HentSisteRetteBrett();
                    OppdaterDgValue();
                    KopierKladdFraBrett();
                    BindKladd();
                }
            }
            else labelTekst.Text = "Ingen feil på brettet.";
        }

        private void ButtonHint_Click(object sender, EventArgs e)
        {
            if (brett.feil)
            {
                labelTekst.Text = "Det er feil på brettet.";
                return;
            }
            if (brett.Hint(out string metode, out char verdi, out int rad, out int kol))
            {
                hintRute = new KeyValuePair<int, int>(rad, kol);
                dg[kol, rad].Style.BackColor = System.Drawing.Color.Red;
            }
            labelTekst.Text = metode;
        }

        void ButtonHint_Leave(object sender, System.EventArgs e)
        {
            dg[hintRute.Value, hintRute.Key].Style.BackColor = System.Drawing.Color.White;
        }

        private void ButtonVisLøsning_Click(object sender, EventArgs e)
        {
            JaNeiVindu.Class1 spørreVindu = new JaNeiVindu.Class1();
            if (spørreVindu.JaNei("Trykk ja for å vise løsning på dette brettet"))
            {
                FormVisLøsning form = new FormVisLøsning(brett.VisLøstBrett(out string løsning));
                form.ShowDialog();
                //Bruker ikke Messagebox siden den ikke har konstant spacing for hvert tegn.
                labelTekst.Text = løsning;
            }
        }

        private void ButtonAvslutt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonFyllKladd_Click(object sender, EventArgs e)
        {
            foreach (Celle c in brett)
            {
                if (c.verdi == ' ' && brett.kladd.brett[c.rad, c.kol] == "")
                {
                    foreach (char k in c.tillatteVerdier) LeggTegnTilKladd(ref brett.kladd.brett[c.rad, c.kol], k);
                    dg[c.kol, c.rad].Value = brett.kladd.brett[c.rad, c.kol];
                    dg[c.kol, c.rad].Style.Font = new System.Drawing.Font("", størrelse.størrelseFontLiten);
                }
            }
        }

        private void ButtonOptimaliserBrett_Click(object sender, EventArgs e)
        {
            int verdierFjernet = brett.OptimaliserBrett();
            if (verdierFjernet == 0) labelTekst.Text = "Kunne ikke fjerne noen verdier.";
            else
            {
                OppdaterDgValue();
                labelTekst.Text = "Fjernet " + verdierFjernet + " verdier.";
            }
        }
    }
}
