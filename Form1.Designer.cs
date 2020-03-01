using System.Windows.Forms;
using System;

namespace Sudoku
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void SetRad(ref System.Windows.Forms.DataGridViewRow rad, int teller)
        {
            if (teller % nRot == 0) rad.DividerHeight = 1;
        }

        private void SetKol(ref System.Windows.Forms.DataGridViewTextBoxColumn kol, int teller)
        {
            if (teller % nRot == 0) kol.DividerWidth = 1;
        }

        private void BindKladd()
        {
            for (int l = 0; l < Kladd.antallLinjer; l++)
            {
                for (int i = 0; i < n; i++)
                {
                    tekstKladdelinjer[l, i].DataBindings.Clear();
                    tekstKladdelinjer[l, i].DataBindings.Add(new Binding("Text", brett.kladd.kladdelinjer[l, i], "Verdi"));
                }
            }
        }

        private void Placement()
        {
            this.labelN.Location = new System.Drawing.Point(Convert.ToInt32(størrelse.plasseringKnappX - størrelse.størrelseRute * 0.3), Convert.ToInt32(størrelse.størrelseKnappY * 0.5));
            this.tekstN.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt + Convert.ToInt32(størrelse.størrelseKnappY * 1.7), Convert.ToInt32(størrelse.størrelseKnappY * 0.5));
            this.labelStørrelse.Location = new System.Drawing.Point(Convert.ToInt32(størrelse.plasseringKnappX - størrelse.størrelseRute * 1.2), Convert.ToInt32(størrelse.størrelseKnappY * 1.5));
            this.tekstStørrelse.Location = new System.Drawing.Point(Convert.ToInt32(størrelse.plasseringKnappX + størrelse.størrelseKnappY * 1.7), Convert.ToInt32(størrelse.størrelseKnappY * 1.5));
            this.checkBoxLett.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 11 * størrelse.størrelseKnappY * 1.35));
            this.checkBoxVisKladd.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 12 * størrelse.størrelseKnappY * 1.35));
            this.checkBoxKladd.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 13 * størrelse.størrelseKnappY * 1.35));
            this.buttonNy.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + størrelse.størrelseKnappY * 1.35));
            this.buttonLagSpill.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 2 * størrelse.størrelseKnappY * 1.35));
            this.buttonLagre.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 3 * størrelse.størrelseKnappY * 1.35));
            this.buttonÅpne.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 4 * størrelse.størrelseKnappY * 1.35));
            this.buttonLås.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 5 * størrelse.størrelseKnappY * 1.35));
            this.buttonSjekkFeil.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 6 * størrelse.størrelseKnappY * 1.35));
            this.buttonSisteRette.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 7 * størrelse.størrelseKnappY * 1.35));
            this.buttonHint.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 8 * størrelse.størrelseKnappY * 1.35));
            this.buttonVisLøsning.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 9 * størrelse.størrelseKnappY * 1.35));
            this.buttonAvslutt.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 10 * størrelse.størrelseKnappY * 1.35));
            this.buttonFyllKladd.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 14 * størrelse.størrelseKnappY * 1.35));
            this.tekstFjernKladdVerdi.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 15 * størrelse.størrelseKnappY * 1.35));
            this.buttonFjernKladdVerdi.Location = new System.Drawing.Point(størrelse.plasseringKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY + 16 * størrelse.størrelseKnappY * 1.35));
            this.buttonOptimaliserBrett.Location = new System.Drawing.Point(Convert.ToInt32(størrelse.plasseringKnappX + størrelse.størrelseKnappX * 1.15), Convert.ToInt32(størrelse.størrelseKnappY + størrelse.størrelseKnappY * 1.35));
            this.dg.Location = new System.Drawing.Point(Convert.ToInt32(størrelse.størrelseRuteKladdKort * 1.5 + størrelse.størrelseRute), Convert.ToInt32(størrelse.størrelseRuteKladdKort * 1.5 + størrelse.størrelseRute));
            this.labelTekst.Location = new System.Drawing.Point(Convert.ToInt32(størrelse.plasseringKnappX + størrelse.størrelseKnappX * 1.15 * 2), Convert.ToInt32(størrelse.størrelseKnappY + størrelse.størrelseKnappY * 1.35));

            this.labelN.Size = new System.Drawing.Size(Convert.ToInt32(størrelse.størrelseKnappX * 0.7), størrelse.størrelseKnappYInt);
            this.tekstN.Size = new System.Drawing.Size(Convert.ToInt32(størrelse.størrelseKnappX * 0.25), størrelse.størrelseKnappYInt);
            this.labelStørrelse.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY * 1.5));
            this.tekstStørrelse.Size = new System.Drawing.Size(Convert.ToInt32(størrelse.størrelseKnappX * 0.25), størrelse.størrelseKnappYInt);
            this.checkBoxLett.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.checkBoxVisKladd.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.checkBoxKladd.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonNy.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonLagSpill.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonLagre.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonÅpne.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonLås.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonSjekkFeil.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonSisteRette.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonHint.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonVisLøsning.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonAvslutt.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonFyllKladd.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.tekstFjernKladdVerdi.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonFjernKladdVerdi.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.buttonOptimaliserBrett.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, størrelse.størrelseKnappYInt);
            this.dg.Size = new System.Drawing.Size(Convert.ToInt32(størrelse.størrelseRute * n + nRot), Convert.ToInt32(størrelse.størrelseRute * n + nRot));
            this.labelTekst.Size = new System.Drawing.Size(størrelse.størrelseKnappXInt, Convert.ToInt32(størrelse.størrelseKnappY / 2));

            for (int i = 0; i < n; i++)
            {
                this.kolLi[i].Width = størrelse.størrelseRuteInt;
                this.radLi[i].Height = størrelse.størrelseRuteInt;

                int plassering = Convert.ToInt32(størrelse.størrelseRuteKladdKort * 1.5 + (i + 1) * størrelse.størrelseRute + Math.Floor(i / nRot * 1.0));
                this.tekstKladdelinjer[0, i].Location = new System.Drawing.Point(plassering, størrelse.plasseringLangVenstreOppInt);
                this.tekstKladdelinjer[1, i].Location = new System.Drawing.Point(plassering, størrelse.plasseringKortVenstreOppInt);
                this.tekstKladdelinjer[2, i].Location = new System.Drawing.Point(størrelse.plasseringLangVenstreOppInt, plassering);
                this.tekstKladdelinjer[3, i].Location = new System.Drawing.Point(størrelse.plasseringKortVenstreOppInt, plassering);
                this.tekstKladdelinjer[4, i].Location = new System.Drawing.Point(størrelse.plasseringKortHøyreNedInt, plassering);
                this.tekstKladdelinjer[5, i].Location = new System.Drawing.Point(størrelse.plasseringLangHøyreNedInt, plassering);
                this.tekstKladdelinjer[6, i].Location = new System.Drawing.Point(plassering, størrelse.plasseringKortHøyreNedInt);
                this.tekstKladdelinjer[7, i].Location = new System.Drawing.Point(plassering, størrelse.plasseringLangHøyreNedInt);

                this.tekstKladdelinjer[0, i].Size = new System.Drawing.Size(størrelse.størrelseRuteInt, størrelse.størrelseRuteKladdKortInt);
                this.tekstKladdelinjer[1, i].Size = new System.Drawing.Size(størrelse.størrelseRuteInt, størrelse.størrelseRuteKladdKortInt);
                this.tekstKladdelinjer[2, i].Size = new System.Drawing.Size(størrelse.størrelseRuteKladdKortInt, størrelse.størrelseRuteInt);
                this.tekstKladdelinjer[3, i].Size = new System.Drawing.Size(størrelse.størrelseRuteKladdKortInt, størrelse.størrelseRuteInt);
                this.tekstKladdelinjer[4, i].Size = new System.Drawing.Size(størrelse.størrelseRuteKladdKortInt, størrelse.størrelseRuteInt);
                this.tekstKladdelinjer[5, i].Size = new System.Drawing.Size(størrelse.størrelseRuteKladdKortInt, størrelse.størrelseRuteInt);
                this.tekstKladdelinjer[6, i].Size = new System.Drawing.Size(størrelse.størrelseRuteInt, størrelse.størrelseRuteKladdKortInt);
                this.tekstKladdelinjer[7, i].Size = new System.Drawing.Size(størrelse.størrelseRuteInt, størrelse.størrelseRuteKladdKortInt);
            }

            this.ClientSize = new System.Drawing.Size(Convert.ToInt32(størrelse.plasseringKnappX + størrelse.størrelseKnappX * 4.3 + 10 * størrelse.faktor), Convert.ToInt32(størrelse.plasseringKnappX + størrelse.størrelseKnappX + 10 * størrelse.faktor));

            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) this.dg[i, j].Style.Font = new System.Drawing.Font("", Convert.ToInt32(størrelse.størrelseRute / 2 * 1.1));
            this.labelStørrelse.Font = new System.Drawing.Font("", Convert.ToInt32(størrelse.størrelseRute / 8));
            this.tekstStørrelse.Font = new System.Drawing.Font("", Convert.ToInt32(størrelse.størrelseRute / 8));
        }

        private void CreateObjects()
        {
            this.tekstKladdelinjer = new TextBox[Kladd.antallLinjer, n];
            for (int l = 0; l < Kladd.antallLinjer; l++) for (int i = 0; i < n; i++) this.tekstKladdelinjer[l, i] = new System.Windows.Forms.TextBox();

            this.labelN = new System.Windows.Forms.Label();
            this.labelStørrelse = new System.Windows.Forms.Label();
            this.tekstN = new TextBox();
            this.tekstStørrelse = new TextBox();

            this.checkBoxLett = new System.Windows.Forms.CheckBox();
            this.checkBoxVisKladd = new System.Windows.Forms.CheckBox();
            this.checkBoxKladd = new System.Windows.Forms.CheckBox();
            this.buttonNy = new System.Windows.Forms.Button();
            this.buttonLagSpill = new System.Windows.Forms.Button();
            this.buttonLagre = new System.Windows.Forms.Button();
            this.buttonÅpne = new System.Windows.Forms.Button();
            this.buttonLås = new System.Windows.Forms.Button();
            this.buttonSjekkFeil = new System.Windows.Forms.Button();
            this.buttonSisteRette = new System.Windows.Forms.Button();
            this.buttonHint = new System.Windows.Forms.Button();
            this.buttonVisLøsning = new System.Windows.Forms.Button();
            this.buttonAvslutt = new System.Windows.Forms.Button();
            this.buttonFyllKladd = new System.Windows.Forms.Button();
            this.tekstFjernKladdVerdi = new System.Windows.Forms.TextBox();
            this.buttonFjernKladdVerdi = new System.Windows.Forms.Button();
            this.buttonOptimaliserBrett = new System.Windows.Forms.Button();
            this.dg = new System.Windows.Forms.DataGridView();

            this.radLi = new DataGridViewRow[n];
            this.kolLi = new DataGridViewTextBoxColumn[n];
            for (int i = 0; i < n; i++)
            {
                radLi[i] = new DataGridViewRow();
                kolLi[i] = new DataGridViewTextBoxColumn();
            }

            this.labelTekst = new System.Windows.Forms.Label();
        }

        private void Construct()
        {
            //
            //labelN
            //
            this.labelN.Name = "label2";
            this.labelN.TabIndex = 13;
            this.labelN.Text = "Antall ruter:";
            //
            //tekstN
            //
            this.tekstN.Text = nRot.ToString();
            this.tekstN.LostFocus += new EventHandler(TekstN_LostFocus);
            //
            //labelStørrelse
            //
            this.labelStørrelse.AutoSize = true;
            this.labelStørrelse.Name = "label2";
            this.labelStørrelse.TabIndex = 12;
            this.labelStørrelse.Text = "Størrelsefaktor for vindu:";
            //
            //tekstStørrelse
            //
            this.tekstStørrelse.Text = størrelse.faktor.ToString();
            this.tekstStørrelse.LostFocus += new EventHandler(TekstStørrelse_LostFocus);
            // 
            // checkBoxLett
            // 
            this.checkBoxLett.AutoSize = true;
            this.checkBoxLett.Name = "checkBoxLett";
            this.checkBoxLett.TabIndex = 0;
            this.checkBoxLett.Text = "Lett modus";
            this.checkBoxLett.UseVisualStyleBackColor = true;
            this.checkBoxLett.CheckedChanged += new System.EventHandler(this.CheckBoxLett_CheckedChanged);
            // 
            // checkBoxVisKladd
            // 
            this.checkBoxVisKladd.AutoSize = true;
            this.checkBoxVisKladd.Name = "checkBoxVisKladd";
            this.checkBoxVisKladd.TabIndex = 12;
            this.checkBoxVisKladd.Text = "Vis kladderuter";
            this.checkBoxVisKladd.UseVisualStyleBackColor = true;
            this.checkBoxVisKladd.CheckedChanged += new System.EventHandler(this.CheckBoxVisKladd_CheckedChanged);
            // 
            // checkBoxKladd
            // 
            this.checkBoxKladd.AutoSize = true;
            this.checkBoxKladd.Name = "checkBoxKladd";
            this.checkBoxKladd.TabIndex = 13;
            this.checkBoxKladd.Text = "Kladde modus";
            this.checkBoxKladd.UseVisualStyleBackColor = true;
            this.checkBoxKladd.Visible = false;
            this.checkBoxKladd.CheckedChanged += new System.EventHandler(this.CheckBoxKladd_CheckedChanged);
            // 
            // buttonNy
            // 
            this.buttonNy.Name = "buttonNy";
            this.buttonNy.TabIndex = 1;
            this.buttonNy.Text = "Nullstill brett";
            this.buttonNy.UseVisualStyleBackColor = true;
            this.buttonNy.Click += new System.EventHandler(this.ButtonNy_Click);
            // 
            // buttonLagSpill
            // 
            this.buttonLagSpill.Name = "buttonLagSpill";
            this.buttonLagSpill.TabIndex = 2;
            this.buttonLagSpill.Text = "Lag spill";
            this.buttonLagSpill.UseVisualStyleBackColor = true;
            this.buttonLagSpill.Click += new System.EventHandler(this.ButtonLagSpill_Click);
            // 
            // buttonLagre
            // 
            this.buttonLagre.Name = "buttonLagre";
            this.buttonLagre.TabIndex = 3;
            this.buttonLagre.Text = "Lagre";
            this.buttonLagre.UseVisualStyleBackColor = true;
            this.buttonLagre.Click += new System.EventHandler(this.ButtonLagre_Click);
            // 
            // buttonÅpne
            // 
            this.buttonÅpne.Name = "buttonÅpne";
            this.buttonÅpne.TabIndex = 4;
            this.buttonÅpne.Text = "Åpne";
            this.buttonÅpne.UseVisualStyleBackColor = true;
            this.buttonÅpne.Click += new System.EventHandler(this.ButtonÅpne_Click);
            // 
            // buttonLås
            // 
            this.buttonLås.Name = "buttonLås";
            this.buttonLås.TabIndex = 5;
            this.buttonLås.Text = "Lås brett";
            this.buttonLås.UseVisualStyleBackColor = true;
            this.buttonLås.Click += new System.EventHandler(this.ButtonLås_Click);
            // 
            // buttonSjekkFeil
            // 
            this.buttonSjekkFeil.Name = "buttonSjekkFeil";
            this.buttonSjekkFeil.TabIndex = 6;
            this.buttonSjekkFeil.Text = "Sjekk etter feil";
            this.buttonSjekkFeil.UseVisualStyleBackColor = true;
            this.buttonSjekkFeil.Click += new System.EventHandler(this.ButtonSjekkFeil_Click);
            // 
            // buttonSisteRette
            // 
            this.buttonSisteRette.Name = "buttonSisteRette";
            this.buttonSisteRette.TabIndex = 7;
            this.buttonSisteRette.Text = "Gå tilbake til før feil";
            this.buttonSisteRette.UseVisualStyleBackColor = true;
            this.buttonSisteRette.Click += new System.EventHandler(this.ButtonSisteRette_Click);
            //
            // buttonHint
            //
            this.buttonHint.Name = "buttonHint";
            this.buttonHint.TabIndex = 8;
            this.buttonHint.Text = "Løsningshint";
            this.buttonHint.UseVisualStyleBackColor = true;
            this.buttonHint.Click += new System.EventHandler(this.ButtonHint_Click);
            this.buttonHint.Leave += new System.EventHandler(ButtonHint_Leave);
            // 
            // buttonVisLøsning
            // 
            this.buttonVisLøsning.Name = "buttonVisLøsning";
            this.buttonVisLøsning.TabIndex = 9;
            this.buttonVisLøsning.Text = "Vis løsning";
            this.buttonVisLøsning.UseVisualStyleBackColor = true;
            this.buttonVisLøsning.Click += new System.EventHandler(this.ButtonVisLøsning_Click);
            // 
            // buttonAvslutt
            // 
            this.buttonAvslutt.Name = "buttonAvslutt";
            this.buttonAvslutt.TabIndex = 10;
            this.buttonAvslutt.Text = "Avslutt";
            this.buttonAvslutt.UseVisualStyleBackColor = true;
            this.buttonAvslutt.Click += new System.EventHandler(this.ButtonAvslutt_Click);
            // 
            // buttonFjernKladdVerdi
            // 
            this.buttonFyllKladd.Name = "buttonFyllKladd";
            this.buttonFyllKladd.TabIndex = 13;
            this.buttonFyllKladd.Text = "Fyll kladd";
            this.buttonFyllKladd.UseVisualStyleBackColor = true;
            this.buttonFyllKladd.Visible = false;
            this.buttonFyllKladd.Click += new System.EventHandler(this.ButtonFyllKladd_Click);
            // 
            // tekstFjernKladdVerdi
            // 
            this.tekstFjernKladdVerdi.TabIndex = 14;
            this.tekstFjernKladdVerdi.Visible = false;
            // 
            // buttonFyllKladd
            // 
            this.buttonFjernKladdVerdi.Name = "buttonFjernKladdVerdi";
            this.buttonFjernKladdVerdi.TabIndex = 15;
            this.buttonFjernKladdVerdi.Text = "Fjern verdi";
            this.buttonFjernKladdVerdi.UseVisualStyleBackColor = true;
            this.buttonFjernKladdVerdi.Visible = false;
            this.buttonFjernKladdVerdi.Click += new System.EventHandler(this.ButtonFjernKladdVerdi_Click);
            // 
            // buttonOptimaliserBrett
            // 
            this.buttonOptimaliserBrett.Name = "buttonOptimaliserBrett";
            this.buttonOptimaliserBrett.TabIndex = 1;
            this.buttonOptimaliserBrett.Text = "Optimaliser brett";
            this.buttonOptimaliserBrett.UseVisualStyleBackColor = true;
            this.buttonOptimaliserBrett.Click += new System.EventHandler(this.ButtonOptimaliserBrett_Click);
            // 
            // dg
            // 
            this.dg.AllowUserToAddRows = false;
            this.dg.AllowUserToDeleteRows = false;
            this.dg.ColumnHeadersHeightSizeMode =
            System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg.ColumnHeadersVisible = false;
            foreach (DataGridViewTextBoxColumn kol in this.kolLi) this.dg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { kol });
            foreach (DataGridViewRow rad in this.radLi) this.dg.Rows.AddRange(new System.Windows.Forms.DataGridViewRow[] { rad });
            this.dg.Name = "dg";
            this.dg.ReadOnly = true;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    this.dg[i, j].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.dg[i, j].Style.WrapMode = DataGridViewTriState.True;
                }
            }
            this.dg.RowHeadersVisible = false;
            this.dg.AllowUserToResizeColumns = false;
            this.dg.AllowUserToResizeRows = false;
            this.dg.TabIndex = 11;
            this.dg.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Dg_PreviewKeyDown);
            this.dg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TastTastet);
            this.dg.MouseClick += new MouseEventHandler(Dg_MouseClick);
            // 
            // Kolonner
            // 
            for (int i = 0; i < n; i++) SetKol(ref this.kolLi[i], i + 1);
            //
            //Rader
            //
            for (int i = 0; i < n; i++) SetRad(ref this.radLi[i], i + 1);
            // labelTekst
            // 
            this.labelTekst.AutoSize = true;
            this.labelTekst.Name = "labelTekst";
            this.labelTekst.TabIndex = 11;
            this.labelTekst.Text = "label1";
            //
            // tekstBoks
            //
            for (int i = 0; i < n; i++)
            {
                this.tekstKladdelinjer[2, i].Multiline = true;
                this.tekstKladdelinjer[3, i].Multiline = true;
                this.tekstKladdelinjer[4, i].Multiline = true;
                this.tekstKladdelinjer[5, i].Multiline = true;
            }
        }

        private void LayoutObjects()
        {
            Placement();
            this.Controls.Add(this.labelTekst);
            this.Controls.Add(this.dg);
            this.Controls.Add(this.buttonOptimaliserBrett);
            this.Controls.Add(this.buttonFjernKladdVerdi);
            this.Controls.Add(this.tekstFjernKladdVerdi);
            this.Controls.Add(this.buttonFyllKladd);
            this.Controls.Add(this.buttonAvslutt);
            this.Controls.Add(this.buttonVisLøsning);
            this.Controls.Add(this.buttonSisteRette);
            this.Controls.Add(this.buttonSjekkFeil);
            this.Controls.Add(this.buttonLås);
            this.Controls.Add(this.buttonÅpne);
            this.Controls.Add(this.buttonLagre);
            this.Controls.Add(this.buttonHint);
            this.Controls.Add(this.buttonLagSpill);
            this.Controls.Add(this.buttonNy);
            this.Controls.Add(this.checkBoxLett);
            this.Controls.Add(this.checkBoxVisKladd);
            this.Controls.Add(this.checkBoxKladd);
            this.Controls.Add(this.labelN);
            this.Controls.Add(this.tekstN);
            this.Controls.Add(this.labelStørrelse);
            this.Controls.Add(this.tekstStørrelse);
            for (int l = 0; l < Kladd.antallLinjer; l++) for (int i = 0; i < n; i++) this.Controls.Add(this.tekstKladdelinjer[l, i]);
        }

        private void RemoveObjects()
        {
            this.Controls.Remove(this.labelTekst);
            foreach (DataGridViewRow rad in this.radLi) this.dg.Rows.Remove(rad);
            foreach (DataGridViewTextBoxColumn kol in this.kolLi) this.dg.Columns.Remove(kol);
            this.Controls.Remove(this.dg);
            this.Controls.Remove(this.buttonOptimaliserBrett);
            this.Controls.Remove(this.buttonFjernKladdVerdi);
            this.Controls.Remove(this.tekstFjernKladdVerdi);
            this.Controls.Remove(this.buttonFyllKladd);
            this.Controls.Remove(this.buttonAvslutt);
            this.Controls.Remove(this.buttonVisLøsning);
            this.Controls.Remove(this.buttonSisteRette);
            this.Controls.Remove(this.buttonSjekkFeil);
            this.Controls.Remove(this.buttonLås);
            this.Controls.Remove(this.buttonÅpne);
            this.Controls.Remove(this.buttonLagre);
            this.Controls.Remove(this.buttonHint);
            this.Controls.Remove(this.buttonLagSpill);
            this.Controls.Remove(this.buttonNy);
            this.Controls.Remove(this.checkBoxLett);
            this.Controls.Remove(this.checkBoxVisKladd);
            this.Controls.Remove(this.checkBoxKladd);
            this.Controls.Remove(this.labelN);
            this.Controls.Remove(this.tekstN);
            this.Controls.Remove(this.labelStørrelse);
            this.Controls.Remove(this.tekstStørrelse);
            for (int l = 0; l < Kladd.antallLinjer; l++) for (int i = 0; i < n; i++) this.Controls.Remove(this.tekstKladdelinjer[l, i]);
        }

        /// <summary>
        /// Required method for Designer support -g do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {


            CreateObjects();
            BindKladd();

            ((System.ComponentModel.ISupportInitialize)(this.dg)).BeginInit();
            this.SuspendLayout();

            Construct();

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            LayoutObjects();
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxLett;
        private System.Windows.Forms.CheckBox checkBoxVisKladd;
        private System.Windows.Forms.CheckBox checkBoxKladd;
        private System.Windows.Forms.Button buttonNy;
        private System.Windows.Forms.Button buttonLagre;
        private System.Windows.Forms.Button buttonÅpne;
        private System.Windows.Forms.Button buttonLås;
        private System.Windows.Forms.Button buttonSjekkFeil;
        private System.Windows.Forms.Button buttonSisteRette;
        private System.Windows.Forms.Button buttonHint;
        private System.Windows.Forms.Button buttonVisLøsning;
        private System.Windows.Forms.Button buttonAvslutt;
        private System.Windows.Forms.Button buttonLagSpill;
        private System.Windows.Forms.Button buttonFyllKladd;
        private System.Windows.Forms.Button buttonFjernKladdVerdi;
        private System.Windows.Forms.Button buttonOptimaliserBrett;
        private System.Windows.Forms.DataGridView dg;
        private System.Windows.Forms.DataGridViewRow[] radLi;
        private System.Windows.Forms.DataGridViewTextBoxColumn[] kolLi;
        private Label labelTekst;
        private Label labelN;
        private Label labelStørrelse;
        private System.Windows.Forms.TextBox tekstN;
        private System.Windows.Forms.TextBox tekstStørrelse;
        private System.Windows.Forms.TextBox tekstFjernKladdVerdi;
        private System.Windows.Forms.TextBox[,] tekstKladdelinjer;
    }
}
