using System;
using System.Collections.Generic;
using System.Text;
using FunkDiv = DiverseFunksjoner.Class1;

namespace Sudoku
{
    /// <summary>
    /// En todimensjonal array av Celler, heretter kalt brett. Beregnet for 9x9 sudoku. Merk at brettRad og brettKol peker på de samme Cellene.
    /// </summary>
    public class Brett : IEnumerable<Celle>
    {
        public readonly int nRot;
        public readonly int n;
        protected Celle[,] brettRad;
        protected Celle[,] brettKol, brettBlokk;
        private Celle[,] brettSisteRette;
        public bool feil, løst, låst;
        public int gjenværendeCeller;
        private BrettLøst brettLøst;
        public Kladd kladd, kladdSisteRette;

        public Brett(int nRot)
        {
            this.nRot = nRot;
            this.n = nRot * nRot;
            Initialiser();
        }

        /// <summary>
        /// Initialiserer default verdier, og rad, kolonne og blokk brett slik at cellene i kolonne og blokk peker på cellene i brettRad.
        /// </summary>
        protected void Initialiser()
        {
            feil = false;
            løst = false;
            låst = false;
            gjenværendeCeller = n * n;
            brettRad = new Celle[n, n];
            brettKol = new Celle[n, n];
            brettBlokk = new Celle[n, n];
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) brettRad[i, j] = new Celle(i, j, nRot);
            BindRadKolBlokk();
            kladd = new Kladd(n);
        }

        /// <summary>
        /// Gjør at brettKol og brettBlokk peker på brettRad.
        /// </summary>
        protected void BindRadKolBlokk()
        {
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) brettKol[i, j] = brettRad[j, i];
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) brettBlokk[brettRad[i, j].blokk, brettRad[i, j].celleIBlokk] = brettRad[i, j]; //blokk peker på brettRad, og er ikke en kopi.
        }

        /// <summary>
        /// Kopierer brettGammel til brettNy.
        /// </summary>
        /// <param name="brettNy">Kopien.</param>
        /// <param name="brettGammel">Orginalen.</param>
        protected void Kopi(ref Celle[,] brettNy, Celle[,] brettGammel)
        {
            if (brettNy.GetLength(0) != brettGammel.GetLength(0)) throw new Exception("Feil i Brett.Kopi: n ikke lik.");

            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) brettNy[i, j].Kopi(brettGammel[i, j]);
        }

        /// <summary>
        /// Kopierer et brett.
        /// </summary>
        /// <param name="brett">Brettet som skal kopieres.</param>
        /// <returns>Kopi av brett.</returns>
        protected Celle[,] KopiNy(Celle[,] brett)
        {
            Celle[,] kopi = new Celle[n, n];
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) kopi[i, j] = new Celle(brett[i, j]);
            return kopi;
        }


        public Celle this[int indeks1, int indeks2] //Gjør det mulig å bruke hakeparenteser på en Celle, slik man har på array'er.
        {
            get { return new Celle(brettRad[indeks1, indeks2]); }
        }

        /// <summary>
        /// Returnerer en todimensjonal array over de tillatte verdiene. Rutene med verdi i er tomme.
        /// </summary>
        /// <returns>En todimensjonal array over de tillatte verdiene.</returns>
        public string[,] HentTillatteVerdier()
        {
            string[,] brettTillatt = new string[n, n];
            foreach (Celle c in brettRad) if (c.verdi == ' ') brettTillatt[c.rad, c.kol] = c.tillatteVerdier;
            return brettTillatt;
        }

        /// <summary>
        /// Fjerner verdi fra rute sin liste over tillatte verdier. Returnerer true hvis rute.tillatteVerdier har blitt endret. Sjekker også om det oppstår feil i ruten mhp. tillatte verdier.
        /// </summary>
        /// <param name="rute">Ruten i brettet som skal (muligens) endres.</param>
        /// <param name="verdi">Verdien som skal fjernes fra rutens tillatte verdier.</param>
        /// <returns>true hvis rute.tillatteVerdier har blitt endret.</returns>
        protected bool FjernTillattVerdi(ref Celle rute, char verdi)
        {
            if (verdi != ' ' && rute.verdi == ' ' && rute.tillatteVerdier.Contains(verdi))
            {
                rute.tillatteVerdier = rute.tillatteVerdier.Replace(verdi.ToString(), "");
                if (rute.tillatteVerdier == "")
                {
                    feil = true;
                }
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Oppdaterer tillatte verdier i brettet. Returnerer true hvis det har skjedd en endring i de tillatte verdiene. Bruk denne hvis brettet har fått en ny verdi i en celle uten en tidligere verdi. Hvis cellen hadde en annen verdi fra før av, bør en bruke BrettFornyTillatteVerdier.
        /// </summary>
        /// <param name="rute">Ruten som har blitt endret.</param>
        /// <returns>true hvis det har skjedd en endring i de tillatte verdiene.</returns>
        protected bool BrettOppdatering(Celle rute)
        {
            //if(feil) return false; //!Ingen vits i å oppdatere brette hvis det er feil.?
            bool endret = false;
            char verdi = rute.verdi;
            int rad = rute.rad;
            int kol = rute.kol;
            int blokk = rute.blokk;
            for (int i = 0; i < n; i++)
            {
                endret |= FjernTillattVerdi(ref brettRad[i, kol], verdi);
                endret |= FjernTillattVerdi(ref brettRad[rad, i], verdi);
                endret |= FjernTillattVerdi(ref brettBlokk[blokk, i], verdi);
            }
            return endret;
        }

        /// <summary>
        /// Sjekker om verdien i brett[i,jInit] er unik langs j-aksen. Returnerer true også hvis brett[i,jInit].verdi er null eller tom.
        /// </summary>
        /// <param name="brett">Vilkårlig brett</param>
        /// <param name="i">posisjonen langs x-aksen for verdien som skal sammenlignes.</param>
        /// <param name="j">posisjonen langs y-aksen for verdien som skal sammenlignes. Søket går langs denne aksen.</param>
        /// <param name="sjekkHeleJAksen">true hvis hele j-aksen skal sjekkes. True hvis bare j+1 til n skal sjekkes.</param>
        /// <returns>true hvis brett[i,jInit].verdi er unik.</returns>
        protected bool SjekkUnikVerdi(Celle[,] brett, int i, int j, bool sjekkHeleJAksen)
        {
            char verdi = brett[i, j].verdi;
            int jInit = j;
            if (sjekkHeleJAksen) j = 0;
            if (verdi == ' ') return true;
            for (j = jInit + 1; j < n; j++) if (brett[i, j].verdi == verdi) return false;
            return true;
        }

        /// <summary>
        /// Sjekker om verdien i brettRad[i, j] er unik. Returnerer tru hvis verdien er unik.
        /// </summary>
        /// <param name="i">rad</param>
        /// <param name="j">kolonne</param>
        /// <param name="sjekkHeleJAksen">true hvis hele j-aksen skal sjekkes. True hvis bare j+1 til n skal sjekkes.</param>
        /// <returns>true hvis brettRad[i, j].verdi er unik.</returns>
        protected bool SjekkUnikVerdiAlleBrett(int i, int j, bool sjekkHeleJAksen)
        {
            bool unik = true;
            if (!SjekkUnikVerdi(brettRad, i, j, sjekkHeleJAksen)) unik = false;
            else if (!SjekkUnikVerdi(brettKol, j, i, sjekkHeleJAksen)) unik = false;
            else if (!SjekkUnikVerdi(brettBlokk, brettRad[i, j].blokk, brettRad[i, j].celleIBlokk, sjekkHeleJAksen)) unik = false;
            return unik;
        }

        /// <summary>
        /// Sjekker om verdien i rute fører til feil. Returnerer true hvis det er feil.
        /// </summary>
        /// <param name="rute">Ruten som kan medføre feil.</param>
        /// <returns>true hvis verdien i rute fører til feil.</returns>
        protected bool SjekkFeil(Celle rute)
        {
            if (feil) return feil;
            if (låst && brettLøst != (Brett)null)
            {
                if (løst && brettLøst.enLøsning)
                {
                    if (rute.verdi != ' ') feil |= brettLøst[rute.rad, rute.kol].verdi != rute.verdi;
                }
                else
                {
                    if (rute.verdi == ' ')
                    {
                        if (brettLøst[rute.rad, rute.kol].verdi != ' ') feil |= !rute.tillatteVerdier.Contains(brettLøst[rute.rad, rute.kol].verdi);
                        else feil |= (rute.tillatteVerdier == "");
                    }
                    else if (brettLøst[rute.rad, rute.kol].verdi == ' ') feil |= !brettLøst[rute.rad, rute.kol].tillatteVerdier.Contains(rute.verdi);
                    else feil |= brettLøst[rute.rad, rute.kol].verdi != rute.verdi;
                }
            }
            return feil;
        }

        /// <summary>
        /// Sjekker om det er feil på brettet. Returnerer true hvis det er feil.
        /// </summary>
        /// <returns>true hvis det er feil på brettet.</returns>
        protected bool SjekkFeilGrundig()
        {
            feil = false;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (SjekkFeil(brettRad[i, j])) return feil;
                    if (!løst || !brettLøst.enLøsning) if (!SjekkUnikVerdiAlleBrett(i, j, false)) return (feil = true);
                }
            }
            return feil;
        }

        /// <summary>
        /// Fornyer de tillatte verdiene i alle rutene på brettet.
        /// </summary>
        protected void BrettFornyTillatteVerdier()
        {
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) brettRad[i, j].ResetTillatteVerdier();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int blokk = brettRad[i, j].blokk;
                    char verdi = brettRad[i, j].verdi;
                    if (verdi != ' ')
                    {
                        for (int l = 0; l < n; l++)
                        {
                            FjernTillattVerdi(ref brettRad[l, j], verdi);
                            FjernTillattVerdi(ref brettRad[i, l], verdi);
                            FjernTillattVerdi(ref brettBlokk[blokk, l], verdi);
                        }
                        brettRad[i, j].tillatteVerdier = "";
                    }
                }
            }
        }

        /// <summary>
        /// Setter brettRad[rad, kol].verdi til verdi, så sant brettRad[rad, kol].tillatteVerdier inneholder verdi. Returnerer true hvis brettRad[rad, kol] sin verdi har blitt endret. Sjekker også om brettet fortsatt stemmer.
        /// </summary>
        /// <param name="rad">Raden til ruten som skal (muligens) få oppdatert sin verdi.</param>
        /// <param name="kol">Kolonnen til ruten som skal (muligens) få oppdatert sin verdi.</param>
        /// <param name="verdi">Verdien som skal settes til rute.</param>
        /// <returns>true hvis brettRad[rad, kol] sin verdi har blitt endret.</returns>
        public bool SettVerdi(int rad, int kol, char verdi)
        {
            return SettVerdi(ref brettRad[rad, kol], verdi);
        }

        /// <summary>
        /// Setter rute.verdi til verdi, så sant rute.tillatteVerdier inneholder verdi. Returnerer true hvis rute sin verdi har blitt endret. Sjekker også om brettet fortsatt stemmer.
        /// </summary>
        /// <param name="rute">Ruten i brettet som skal (muligens) få oppdatert sin verdi.</param>
        /// <param name="verdi">Verdien som skal settes til rute.</param>
        /// <returns>true hvis rute sin verdi har blitt endret.</returns>
        protected bool SettVerdi(ref Celle rute, char verdi)
        {
            if (verdi == rute.verdi) return false;
            if (rute.låst) return false;
            bool feilFør = feil;

            if (verdi == ' ')
            {
                rute.verdi = ' ';
                gjenværendeCeller++;
                feil = false;
                BrettFornyTillatteVerdier();
                if (feilFør) SjekkFeilGrundig();
                return true;
            }

            bool verdiTidligere = (rute.verdi != ' ');
            rute.verdi = verdi;

            if (verdiTidligere)
            {
                feil = false;
                BrettFornyTillatteVerdier();
                if (feilFør) SjekkFeilGrundig();
                else SjekkFeil(rute);
            }
            else
            {
                if (!rute.tillatteVerdier.Contains(verdi)) feil = true;
                rute.tillatteVerdier = "";
                gjenværendeCeller--;
                BrettOppdatering(rute);
                SjekkFeil(rute);
            }

            if (!feilFør && feil)
            {
                SettVerdiSisteRette(ref brettRad[rute.rad, rute.kol], ' ');
                brettSisteRette = KopiNy(brettRad);
                SettVerdiSisteRette(ref brettRad[rute.rad, rute.kol], verdi);
                kladdSisteRette = new Kladd(kladd);
            }
            return true;
        }

        /// <summary>
        /// Resetter verdien i rute tilbake til verdi.
        /// </summary>
        /// <param name="rute">Ruten som skal få satt verdien sin til verdi.</param>
        /// <param name="verdi">Verdien som rute skal få.</param>
        protected void SettVerdiSisteRette(ref Celle rute, char verdi)
        {
            rute.verdi = verdi;
            rute.tillatteVerdier = "";
            BrettFornyTillatteVerdier();
        }

        /// <summary>
        /// Resetter brettet tilbake til siste rette brett.
        /// </summary>
        public void HentSisteRetteBrett()
        {
            if (!feil) return;
            Kopi(ref brettRad, brettSisteRette);
            låst = false;
            foreach (Celle c in brettRad) if (c.låst) låst = true;
            AntallLøsteCeller();
            feil = false;
            kladd.Kopi(kladdSisteRette);
        }

        /// <summary>
        /// Låser alle rutene med satt verdi i brettet (verdi kan ikke endres), samt prøver å løse brettet.
        /// </summary>
        /// <param name="tillatBruteForce">true hvis man skal tillatte brute force metode for å løse brettet.</param>
        private void LåsBrett(bool tillatBruteForce)
        {
            SjekkFeilGrundig();
            foreach (Celle c in brettRad) if (c.verdi != ' ') c.låst = true;
            låst = true;
            brettLøst = new BrettLøst(brettRad, tillatBruteForce);
            SjekkFeilGrundig();
            løst = brettLøst.løst;
        }

        /// <summary>
        /// Låser alle rutene med satt verdi i brettet (verdi kan ikke endres), samt prøver å løse brettet. (brute force tillatt.)
        /// </summary>
        public void LåsBrett()
        {
            LåsBrett(true);
        }

        /// <summary>
        /// Låser opp hele brettet.
        /// </summary>
        public void LåsOppBrett()
        {
            foreach (Celle c in brettRad) c.låst = false;
            låst = false;
            brettLøst = null;
            løst = false;
        }

        protected bool FilStrengTilBrettRad(System.IO.TextReader tr, out string beskjed)
        {
            string linje = tr.ReadLine();
            if (linje != n + "x" + n)
            {
                beskjed = "Feil brettstørrelse på lagret fil. Forventet " + n + "x" + n + ", men lest " + linje + ".";
                return false;
            }
            else beskjed = "";
            //if(linje != n + "x" + n) throw new FeilIÅpneFunksjonException("Feil brettstørrelse på lagret fil. Forventet " + n + "x" + n + ", men lest " + linje + "."); //Har problemer med denne.
            Initialiser();
            for (int i = 0; i < n; i++)
            {
                linje = tr.ReadLine();
                for (int j = 0; j < n; j++) SettVerdi(ref brettRad[i, j], linje[j]);
            }

            linje = tr.ReadLine();
            //string sisteLinje;
            while (linje != null)
            {
                if (linje == "Låste ruter")
                {
                    låst = true;
                    for (int i = 0; i < n; i++)
                    {
                        linje = tr.ReadLine();
                        for (int j = 0; j < n; j++) if (linje[j] == '1') brettRad[i, j].låst = true;
                    }
                    linje = tr.ReadLine();
                }
                else if (linje == "Siste rette brett")
                {
                    brettSisteRette = new Celle[n, n];
                    for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) brettSisteRette[i, j] = new Celle(i, j, nRot);
                    for (int i = 0; i < n; i++)
                    {
                        linje = tr.ReadLine();
                        for (int j = 0; j < n; j++) brettSisteRette[i, j].verdi = linje[j];
                    }
                    linje = tr.ReadLine();
                }
                else if (linje == "Kladdelinjer")
                {
                    //sisteLinje = "Kladdelinjer";
                    kladd.kladdelinjer[int.Parse(tr.ReadLine()), int.Parse(tr.ReadLine())].Verdi = tr.ReadLine().Replace("\\n", "\r\n");
                    linje = tr.ReadLine();
                }
                else if (linje == "Kladdebrett")
                {
                    //sisteLinje = "Kladdebrett";
                    kladd.brett[int.Parse(tr.ReadLine()), int.Parse(tr.ReadLine())] = tr.ReadLine().Replace("\\n", "\n");
                    linje = tr.ReadLine();
                }
            }
            return true;
        }

        protected string BrettTilFilStreng()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(n + "x" + n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) sb.Append(brettRad[i, j].verdi);
                sb.AppendLine();
            }
            if (låst)
            {
                sb.AppendLine("Låste ruter");
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (brettRad[i, j].låst) sb.Append("1");
                        else sb.Append(0);
                    }
                    sb.AppendLine();
                }
            }
            if (feil)
            {
                sb.AppendLine("Siste rette brett");
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++) sb.Append(brettSisteRette[i, j].verdi);
                    sb.AppendLine();
                }
            }
            for (int l = 0; l < Kladd.antallLinjer; l++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (!String.IsNullOrEmpty(kladd.kladdelinjer[l, i].Verdi))
                    {
                        sb.AppendLine("Kladdelinjer");
                        sb.AppendLine(l.ToString());
                        sb.AppendLine(i.ToString());
                        sb.AppendLine(FunkDiv.SkråstrekKorrigering(kladd.kladdelinjer[l, i].Verdi.Replace("\r", "")));
                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (!String.IsNullOrEmpty(kladd.brett[i, j]))
                    {
                        sb.AppendLine("Kladdebrett");
                        sb.AppendLine(i.ToString());
                        sb.AppendLine(j.ToString());
                        sb.AppendLine(FunkDiv.SkråstrekKorrigering(kladd.brett[i, j]));
                    }
                }
            }
            return sb.ToString();
        }

        public bool Åpne(string filnavn, out string beskjed)
        {
            //!Støtte for flere filer.
            //!Legg til versjonsnummer
            System.IO.TextReader tr = System.IO.File.OpenText(filnavn);
            if (!FilStrengTilBrettRad(tr, out beskjed)) return false;
            if (låst)
            {
                char[,] brettIkkeLåst = new char[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        brettIkkeLåst[i, j] = brettRad[i, j].verdi;
                        SettVerdi(ref brettRad[i, j], ' '); //Nullstiller ikkelåsteruter så løsningen ikke baserer seg på de.
                    }
                }
                brettLøst = new BrettLøst(brettRad, true);
                for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) SettVerdi(ref brettRad[i, j], brettIkkeLåst[i, j]);
                løst = brettLøst.løst;
            }
            else løst = false;
            tr.Close();
            return true;
        }

        public void Lagre(string filnavn)
        {
            //!Støtte for flere filer.
            //!Legg til versjonsnummer
            string mappe = FilFunksjoner.Class1.FjernFilFraMappeStreng(filnavn);
            if (!System.IO.Directory.Exists(mappe)) System.IO.Directory.CreateDirectory(mappe);
            string tilFil = BrettTilFilStreng();
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filnavn);
            sw.Write(tilFil);
            sw.Close();
        }

        public bool SjekkBrettLøst()
        {
            AntallLøsteCeller();
            if (gjenværendeCeller == 0)
            {
                if (SjekkFeilGrundig()) return false;
            }
            else return false;
            return true;
        }

        protected int AntallLøsteCeller()
        {
            int ant = 0;
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) if (brettRad[i, j].verdi != ' ') ant++;
            gjenværendeCeller = n * n - ant;
            return ant;
        }

        //2 funksjoner for å lage et tilfeldiggenerert brett.
        //protected void SettTilfeldigCelleVerdi(ref Random ran) {
        //    if(feil) throw new FeilIBrettException("Funksjonen SettTilfeldigCelleVerdi kalt mens det var en feil i brettet.");
        //    while(true) {
        //        int rad = ran.Next(n);
        //        int kol = ran.Next(n);
        //        char verdi = Celle.MuligeVerdier(n)[ran.Next(n)];
        //        if(brettRad[rad, kol].tillatteVerdier.Contains(verdi.ToString())) {
        //            SettVerdi(ref brettRad[rad, kol], verdi);
        //            if(feil) {
        //                feil = false;
        //                SettVerdi(ref brettRad[rad, kol], ' ');
        //            } else break;
        //        }
        //    }
        //}

        //protected void SettVerdiFraLøstBrett(ref Random ran, bool låse) {
        //    int i = ran.Next(brettLøst.metoderBrukt.Count); //!Fjern bruk av bruteforce ved løsing av lagd brett? Løse først uten, så med til midlertidig brett bare for å sjekke at brettet kan løses? Ha en dobbelarray med bool-verdi for om verdien er gjettet på?
        //    while(brettLøst.metoderBrukt[i] != "Gjetning") i = ran.Next(brettLøst.metoderBrukt.Count);
        //    SettVerdi(ref brettRad[brettLøst.ruterLøst[0][i], brettLøst.ruterLøst[1][i]], brettLøst.ruterLøstVerdi[i]);
        //    brettRad[brettLøst.ruterLøst[0][i], brettLøst.ruterLøst[1][i]].låst = låse;
        //}

        protected void NettsideTilBrett(System.IO.StreamReader sr)
        {
            Initialiser();
            string linje;
            while ((linje = sr.ReadLine()) != null)
            {
                if (linje.Contains("<div class=\"grid\"><table>"))
                {
                    for (int i = 0; i < n; i++)
                    {
                        sr.ReadLine();
                        for (int j = 0; j < n; j++)
                        {
                            linje = sr.ReadLine();
                            for (int k = 0; k < linje.Length; k++)
                            {
                                if (linje[k] == '>')
                                {
                                    string verdiStrTmp = "";
                                    for (++k; k < linje.Length; k++)
                                    {
                                        if (linje[k] == '<') break;
                                        else verdiStrTmp += linje[k];
                                    }
                                    if (int.TryParse(verdiStrTmp, out int verdiIntTmp))
                                    {
                                        SettVerdi(ref brettRad[i, j], Celle.MuligeVerdier(n)[verdiIntTmp - 1]);
                                    }
                                    break;
                                }
                            }
                        }
                        sr.ReadLine();
                    }
                    break;
                }
            }
            låst = true;
        }

        public bool LagBrettFraNett(string lenke, out string tekst)
        {
            try
            {
                System.Net.HttpWebRequest side = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(lenke);
                System.Net.HttpWebResponse respons = (System.Net.HttpWebResponse)side.GetResponse();
                if (System.Net.HttpStatusCode.OK == respons.StatusCode)
                {
                    System.IO.Stream dataStrøm = respons.GetResponseStream();
                    System.IO.StreamReader sr = new System.IO.StreamReader(dataStrøm);
                    NettsideTilBrett(sr);
                    sr.Close();
                    dataStrøm.Close();
                    respons.Close();
                    LåsBrett();
                    tekst = "";
                    if (brettLøst.løsning.Length > 0) tekst = brettLøst.løsning + '\n';
                    tekst += "Antall gitte celler er " + AntallLøsteCeller();
                    return true;
                }
                else
                {
                    respons.Close();
                    tekst = "Greide ikke å koble til nettsiden " + lenke + ". Sjekk om du har kontakt med internett";
                    return false;
                }
            }
            catch (Exception Ex)
            {
                tekst = "Det skjedde en feil.\n" + Ex.ToString();
                return false;
            }
        }

        //Funksjon for å lage et tilfeldiggenerert brett.
        //public bool LagBrettBruteForce(out string tekst) {
        //    Random ran = new Random();
        //    //bool bruktBruteForce;
        //    //bool flereLøsninger;
        //    while(true) {
        //        Initialiser();
        //        while(gjenværendeCeller > n * n - 17) SettTilfeldigCelleVerdi(ref ran); //9*9 brett trenger minimum 17 tall for å ha unik løsning.
        //        brettLøst = new BrettLøst(brettRad);
        //        if(!brettLøst.feil) break;
        //    }
        //    foreach(Celle c in brettRad) if(c.verdi != ' ') c.låst = true;
        //    while(!(brettLøst.løst && brettLøst.enLøsning)) {
        //        //bruktBruteForce = false;
        //        SettVerdiFraLøstBrett(ref ran, true);
        //        brettLøst = new BrettLøst(brettRad);
        //    }
        //    //Celle[,] brettLøstTmp = Kopi(brettLøst.brettRad);
        //    for(int i = 0; i < n;i++ ) {
        //        for(int j = 0; j < n; j++) {
        //            if(brettRad[i,j].verdi != ' ') {
        //                char verdiGammel = brettRad[i, j].verdi;
        //                brettRad[i, j].låst = false;
        //                SettVerdi(ref brettRad[i, j], ' ');
        //                brettRad[i, j].låst = true;
        //                brettLøst = new BrettLøst(brettRad);
        //                if(!brettLøst.løst || !brettLøst.enLøsning) {
        //                    brettRad[i, j].låst = false;
        //                    SettVerdi(ref brettRad[i, j], verdiGammel);
        //                    brettRad[i, j].låst = true;
        //                }
        //            }
        //        }
        //    }
        //    tekst = "";
        //    if(brettLøst.løsning.Length > 0) tekst = brettLøst.løsning + '\n';
        //    tekst += "Antall gitte celler er " + AntallLøsteCeller();
        //    return brettLøst.løst;
        //}

        public bool Hint(out string metoder, out char verdi, out int rad, out int kol)
        {
            verdi = ' ';
            rad = kol = 0;
            if (feil)
            {
                metoder = "Det er feil på brettet.";
                return false;
            }
            if (!låst)
            {
                metoder = "Brette må være låst før det kan gis noe hint.";
                return false;
            }
            bool fantVerdi = brettLøst.HintLøs1(brettRad, out metoder, out rad, out kol);
            verdi = brettLøst[rad, kol].verdi;
            return fantVerdi;
        }

        public string VisLøstBrett(out string løsning)
        {
            if (!låst)
            {
                løsning = "Brettet må være låst for å finne en løsning.";
                return "";
            }
            løsning = brettLøst.løsning;
            //if(!brettLøst.løst) return "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) sb.Append(brettLøst[i, j].verdi);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public string BrettTilStreng()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(n + "x" + n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) sb.Append(brettRad[i, j].verdi);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public string BrettTillatteVerdierTilStreng()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(n + "x" + n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) sb.Append(brettRad[i, j].tillatteVerdier + " ");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public int OptimaliserBrett()
        {
            if (låst) return 0;
            int verdierFjernet = 0;
            LåsBrett(false);
            bool brettLøst = løst;
            LåsOppBrett();
            if (!brettLøst) return 0;
            foreach (Celle c in brettRad)
            {
                if (c.verdi != ' ')
                {
                    char verdi = c.verdi;
                    SettVerdi(ref brettRad[c.rad, c.kol], ' ');
                    LåsBrett(false);
                    brettLøst = løst;
                    LåsOppBrett();
                    if (!brettLøst)
                    {
                        SettVerdi(ref brettRad[c.rad, c.kol], verdi);
                    }
                    else verdierFjernet++;
                }
            }
            return verdierFjernet;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<Celle> GetEnumerator()
        {
            foreach (Celle c in brettRad) yield return c;
        }
    }

    class BrettLøst : Brett
    {
        protected int radLøst;
        protected int kolLøst;
        protected StringBuilder metoderBrukt; //Løsningsmetodene som er brukt for nåværende verdi. Legges til metoderBrukt så fort en ny verdi blir funnet.
        protected string sisteMetodeBrukt; //Siste løsningsmetoden som er brukt. (Jeg vil ikke ha samme metode listet opp flere ganger rett etter hverandre.
        public string løsning;
        public bool enLøsning;
        protected bool finnEnVerdi;

        public BrettLøst(Celle[,] brett, bool tillatBruteForce)
            : base((int)Math.Sqrt(brett.GetLength(1)))
        {
            Initialiser(brett, tillatBruteForce);
        }

        private void Initialiser(Celle[,] brett, bool tillatBruteForce)
        {
            låst = true;
            Kopi(ref brettRad, brett);
            metoderBrukt = new StringBuilder();
            sisteMetodeBrukt = "";
            finnEnVerdi = false;
            LøsBrett(tillatBruteForce, false);
        }

        protected void LeggTilMetodeBrukt(string metode)
        {
            if (sisteMetodeBrukt != metode)
            {
                sisteMetodeBrukt = metode;
                metoderBrukt.AppendLine(sisteMetodeBrukt);
            }
        }

        new protected bool SjekkBrettLøst()
        {
            AntallLøsteCeller();
            if (gjenværendeCeller == 0)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n - 1; j++)
                    {
                        if (!SjekkUnikVerdi(brettRad, i, j, false)) return false;
                        if (!SjekkUnikVerdi(brettKol, i, j, false)) return false;
                        if (!SjekkUnikVerdi(brettBlokk, i, j, false)) return false;
                    }
                }
            }
            else return false;
            return true;
        }

        /// <summary>
        /// Setter rute.verdi til verdi, så sant rute.tillatteVerdier inneholder verdi. Returnerer true hvis rute sin verdi har blitt endret Sjekker også at brettet fortsatt stemmer.
        /// </summary>
        /// <param name="rute">Ruten i brettet som skal (muligens) få oppdatert sin verdi.</param>
        /// <param name="verdi">Verdien som skal settes til rute.</param>
        /// <returns>true hvis rute sin verdi har blitt endret.</returns>
        new protected bool SettVerdi(ref Celle rute, char verdi)
        {
            if (løst) return false;
            if (verdi == rute.verdi) return false;
            if (rute.låst) return false;
            if (verdi == ' ')
            {
                rute.verdi = ' ';
                gjenværendeCeller++;
                BrettFornyTillatteVerdier();
                return true;
            }
            if (rute.tillatteVerdier.Contains(verdi))
            {
                if (finnEnVerdi)
                {
                    radLøst = rute.rad;
                    kolLøst = rute.kol;
                    return true;
                }
                bool verdiTidligere = (rute.verdi != ' ');
                rute.verdi = verdi;
                if (verdiTidligere)
                {
                    BrettFornyTillatteVerdier();
                }
                else
                {
                    rute.tillatteVerdier = "";
                    gjenværendeCeller--;
                    BrettOppdatering(rute);
                }
                return true;
            }
            else return false;
        }

        protected bool LøsBruteForce_NesteKolNy(int rad, int kol, out int radNy, out int kolNy)
        {
            radNy = rad;
            kolNy = kol + 1;
            if (kolNy >= n)
            {
                kolNy = 0;
                radNy++;
                if (radNy >= n) return false;
            }
            return true;
        }

        protected void LøsBruteForce_Intern(ref Celle[,] brettLøst, int rad, int kol, ref int løsninger)
        {
            if (løsninger > 1) return;
            if (brettRad[rad, kol].verdi == ' ')
            {
                Celle[,] brettRadTmp = KopiNy(brettRad);
                foreach (char k in brettRadTmp[rad, kol].tillatteVerdier.Replace(" ", ""))
                {
                    //Denne metoden bruker mer tid (kopiering av brettRad 6 ganger istedenfor 3 ganger) men krever ikke omskriving av LøsBrett og dens funksjoner.
                    SettVerdi(ref brettRad[rad, kol], k);
                    if (!feil)
                    {
                        LøsBrett(false, true);
                        if (AntallLøsteCeller() == n * n)
                        {
                            bool like = true;
                            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) if (brettRad[i, j] != brettLøst[i, j]) like = false; //BruteForce metoden kan få samme brett flere ganger.
                            if (!like)
                            {
                                løsninger++;
                                if (løsninger == 1) Kopi(ref brettLøst, brettRad); //Løsningen for bare en løsning skal tas vare på.
                            }
                        }
                        else if (!feil)
                        { //Ikke vits i å gå videre med nye bruteforce kall hvis brettet er løst, ettersom vi da allerede har funnet den eneste verdien.
                            if (!LøsBruteForce_NesteKolNy(rad, kol, out int radNy, out int kolNy)) return;
                            LøsBruteForce_Intern(ref brettLøst, radNy, kolNy, ref løsninger);
                        }
                        if (løsninger > 1) return;
                    }
                    Kopi(ref brettRad, brettRadTmp);
                    feil = false;
                }
            }
            else
            { //Uten denne kommer en ikke forbi de ikke-tomme rutene.
                if (!LøsBruteForce_NesteKolNy(rad, kol, out int radNy, out int kolNy)) return;
                LøsBruteForce_Intern(ref brettLøst, radNy, kolNy, ref løsninger);
            }
        }

        protected int LøsBruteForce()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(n + "x" + n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) sb.Append(brettRad[i, j].verdi);
                sb.AppendLine();
            }
            string s = sb.ToString();

            if (feil) return 0;
            int løsninger = 0;
            sisteMetodeBrukt = "bruteforce"; //Jeg vil ikke ta vare på løsningene mens BruteForce metoden kjører (ettersom den hopper frem og tilbake inntil den finner en riktig løsning).
            //DateTime tid = DateTime.Now;
            Celle[,] brettOrig = KopiNy(brettRad);
            Celle[,] brettLøst = new Celle[n, n];
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) brettLøst[i, j] = new Celle(i, j, nRot);
            LøsBruteForce_Intern(ref brettLøst, 0, 0, ref løsninger);
            //double tidDifMin = DateTime.Now.Subtract(tid).TotalMinutes;
            //MessageBox.Show("Kjøretid er " + Math.Round(tidDifMin, 2) + " minutter.");
            if (løsninger == 0) Kopi(ref brettLøst, brettRad);
            else if (løsninger > 1)
            {
                Kopi(ref brettRad, brettOrig);
                Kopi(ref brettLøst, brettOrig);
            }
            else Kopi(ref brettRad, brettLøst);
            return løsninger;
        }

        protected bool LøsBrett(bool tillatBruteForce, bool bruteForceKjører)
        {
            if (!bruteForceKjører && !finnEnVerdi) SjekkFeilGrundig();
            if (feil)
            {
                løsning = "Feil funnet i brettet. Greier derfor ikke å løse det.";
                enLøsning = false;
                return false;
            }
            løst = SjekkBrettLøst();
            while (!løst)
            {
                bool endret = true;
                while (endret)
                {
                    endret = Løs1TillattVerdi();
                    if (endret && finnEnVerdi)
                    {
                        LeggTilMetodeBrukt("Løs1TillattVerdi");
                        return true;
                    }
                }
                endret |= Løs1Verdi(ref brettRad);
                if (endret && finnEnVerdi)
                {
                    LeggTilMetodeBrukt("Løs1VerdiRad");
                    return true;
                }
                endret |= Løs1Verdi(ref brettKol);
                if (endret && finnEnVerdi)
                {
                    LeggTilMetodeBrukt("Løs1VerdiKol");
                    return true;
                }
                endret |= Løs1Verdi(ref brettBlokk);
                if (endret && finnEnVerdi)
                {
                    LeggTilMetodeBrukt("Løs1VerdiBlokk");
                    return true;
                }
                if (!endret)
                {
                    endret |= Løs2x2RadKolBlokk(); //Avanserte løsningsmetoder
                    if (endret && finnEnVerdi) LeggTilMetodeBrukt("Løs2x2RadKolBlokk");
                }
                if (!endret)
                {
                    endret |= LøsVerdiUnikPåRadKolIBlokk(); //Avanserte løsningsmetoder
                    if (endret && finnEnVerdi) LeggTilMetodeBrukt("LøsVerdiUnikPåRadKolIBlokk");
                }
                løst = SjekkBrettLøst();
                if (feil)
                {
                    løsning = "Dette brettet har ingen løsninger";
                    enLøsning = false;
                    return false;
                }
                if (!endret && !løst)
                {
                    if (finnEnVerdi) return false;
                    else if (tillatBruteForce)
                    {
                        int løsninger = LøsBruteForce();
                        if (løsninger == 1)
                        {
                            løsning = "Måtte gjette frem til en løsning.";
                            enLøsning = true;
                            return true;
                        }
                        else if (løsninger < 1)
                        {
                            feil = true;
                            løsning = "Dette brettet har ingen løsninger";
                            enLøsning = false;
                            return false;
                        }
                        else if (løsninger > 1)
                        {
                            løsning = "Dettet brettet har flere løsninger. Det er derfor ikke sikkert at kommandoene \"Gå til siste rette\", \"Vis løsning\" og lignende vil virke.";
                            enLøsning = false;
                            return false;
                        }
                    }
                    else
                    {
                        if (!bruteForceKjører) løsning = "Greide ikke å finne noen løsning.";
                        else løsning = "";
                        enLøsning = false;
                        return false;
                    }
                }
            }
            if (finnEnVerdi) return true;
            //løst = true;
            løsning = ""; //tekst = "Løsning funnet.";
            enLøsning = true;
            return true;
        }

        /// <summary>
        /// Løser løsningsbrettet ved å finne alle celler som bare har en tillatt verdi. Returnerer true hvis det har skjedd en endring i brettet.
        /// </summary>
        /// <returns>true hvis det har skjedd en endring i brettet.</returns>
        protected bool Løs1TillattVerdi()
        {
            bool endret = false; ;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (brettRad[i, j].tillatteVerdier.Length == 1)
                    {
                        if (SettVerdi(ref brettRad[i, j], brettRad[i, j].tillatteVerdier[0]))
                        {
                            endret = true;
                            if (finnEnVerdi) return endret;
                        }
                    }
                }
            }
            return endret;
        }

        /// <summary>
        /// Løser brettet ved å finne en verdi som er unik i en rad, kolonne eller blokk. Returnerer true hvis det har skjedd en endring i brettet.
        /// </summary>
        /// <param name="brett">Vilkårlig brett</param>
        /// <returns>true hvis det har skjedd en endring i brettet.</returns>
        protected bool Løs1Verdi(ref Celle[,] brett)
        {
            bool endret = false;
            for (int i = 0; i < n; i++)
            {
                Dictionary<char, int> verdi_indeks_Unik = new Dictionary<char, int>();
                int[] verdiTeller = new int[n]; //Vil inneholde n+1 eller kolonnenummeret j.
                for (int k = 0; k < n; k++) verdiTeller[k] = n + 1;

                for (int j = 0; j < n; j++)
                {  //finner alle (unike) verdier.
                    if (brett[i, j].verdi == ' ')
                    {
                        foreach (char c in brett[i, j].tillatteVerdier)
                        {
                            int k = Celle.IndeksIMuligeVerdierForVerdi(c, n);
                            if (!verdi_indeks_Unik.ContainsKey(c))
                            {
                                verdi_indeks_Unik.Add(c, k);
                                verdiTeller[k] = j;
                            }
                            else verdiTeller[k] = n + 1;
                        }
                    }
                }

                foreach (KeyValuePair<char, int> verdi_indeks in verdi_indeks_Unik)
                { //Løper gjennom alle de mulig unike verdiene.
                    int k = verdi_indeks.Value;
                    if (verdiTeller[k] != n + 1)
                    { //Finner ut om verdiene er unike.
                        if (SettVerdi(ref brett[i, verdiTeller[k]], verdi_indeks.Key))
                        {
                            endret = true;
                            if (finnEnVerdi) return endret;
                        }
                    }
                }
            }
            return endret;
        }

        /// <summary>
        /// Fjerner verdi fra brettRadKol's tillatteVerdier langs "raden" i, unntatt i området innenfor brettets blokk (som bestemmes av i og j. Returnerer true hvis det har skjedd en endring i brettet.
        /// </summary>
        /// <param name="brettRadKol">brettRad eller brettKol.</param>
        /// <param name="verdi">Verdien som skal fjernes fra tillatteVerdier.</param>
        /// <param name="i">Linjen som skal endres.</param>
        /// <param name="j">Posisjonen langs j-aksen for blokken.</param>
        /// <returns>true hvis det har skjedd en endring i brettet.</returns>
        protected bool LøsVerdiUnikPåRadKolIBlokk_FjernTillatteVerdier(ref Celle[,] brettRadKol, char verdi, int i, int j)
        {
            bool endret = false;
            int jMidt = (j / nRot) * nRot;
            for (j = 0; j < jMidt; j++) if (FjernTillattVerdi(ref brettRadKol[i, j], verdi)) endret = true;
            if (feil) return endret;
            for (j = jMidt + nRot; j < n; j++) if (FjernTillattVerdi(ref brettRadKol[i, j], verdi)) endret = true;
            return endret;
        }

        /// <summary>
        /// Sjekker om en blokk har tillatte verdier som bare forekommer på en av sine rader eller kolonner. Fjerner da disse verdiene fra andre blokker langs samme rad/kolonne. Returnerer true hvis det har skjedd en endring i brettet.
        /// </summary>
        /// <returns>true hvis det har skjedd en endring i brettet.</returns>
        protected bool LøsVerdiUnikPåRadKolIBlokk()
        {
            bool endret = false;
            for (int b = 0; b < n; b++)
            {
                int[] verdiLiRad = new int[n]; //Antall rader hver verdi forekommer i.
                int[] verdiLiKol = new int[n];//Antall kolonner hver verdi forekommer i.
                bool[,] verdiLiRadTmp = new bool[nRot, n]; //Radnr, verdi
                bool[,] verdiLiKolTmp = new bool[nRot, n]; //Kolnr, verdi
                int[] verdiLiCelleNr = new int[n]; //celleIBlokk nr.
                for (int l = 0; l < n; l++)
                {
                    if (brettBlokk[b, l].verdi == ' ')
                    {
                        foreach (char c in brettBlokk[b, l].tillatteVerdier)
                        {
                            int k = Celle.IndeksIMuligeVerdierForVerdi(c, n);
                            verdiLiRadTmp[brettBlokk[b, l].rad % nRot, k] = true;
                            verdiLiKolTmp[brettBlokk[b, l].kol % nRot, k] = true;
                            verdiLiCelleNr[k] = l;
                        }
                    }
                }
                for (int i = 0; i < nRot; i++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        if (verdiLiRadTmp[i, k]) verdiLiRad[k]++;
                        if (verdiLiKolTmp[i, k]) verdiLiKol[k]++;
                    }
                }

                //Har nå funnet hvor mange rader og kolonner hver verdi forekommer i. Skal nå gå videre med de verdiene som forekommer 1 gang, og fjerne dem fra de andre blokkene langs raden/kolonnen.
                for (int k = 0; k < n; k++)
                {
                    if (verdiLiRad[k] == 1)
                    {
                        int rad = brettBlokk[b, verdiLiCelleNr[k]].rad;
                        int kol = brettBlokk[b, verdiLiCelleNr[k]].kol;
                        if (LøsVerdiUnikPåRadKolIBlokk_FjernTillatteVerdier(ref brettRad, Celle.MuligeVerdier(n)[k], rad, kol)) endret = true;
                    }
                    else if (verdiLiKol[k] == 1)
                    {
                        int kol = brettBlokk[b, verdiLiCelleNr[k]].kol;
                        int rad = brettBlokk[b, verdiLiCelleNr[k]].rad;
                        if (LøsVerdiUnikPåRadKolIBlokk_FjernTillatteVerdier(ref brettKol, Celle.MuligeVerdier(n)[k], kol, rad)) endret = true;
                    }
                    if (feil) return endret;
                }
            }
            return endret;
        }

        protected bool ListLike(List<int> vs, List<int> hs)
        {
            if (vs.Count != hs.Count) return false;
            for (int i = 0; i < vs.Count; i++) if (vs[i] != hs[i]) return false;
            return true;
        }

        /// <summary>
        /// Sjekker om verdiene i potensiellVerdiLi oppfyller kravene til Løs2x2RadKolBlokk.
        /// </summary>
        /// <param name="x">Antall verdier som skal stemme.</param>
        /// <param name="verdiLi">Liste over de endelige verdiene for hvert par.</param>
        /// <param name="adresseLi">Liste over adressen til de endelige verdiene for hvert par.</param>
        /// <param name="potensiellVerdiLi">Liste med verdiene som skal sjekkes.</param>
        /// <param name="verdiAdresse">Adressliste for de potensielle verdiene. Denne sjekkes sammen med potensiellVerdiLi.</param>
        protected void Løs2x2RadKolBlokk_PreMetode_SjekkVerdi(int x, out List<string> verdiLi, out List<int[]> adresseLi, List<string> potensiellVerdiLi, List<int>[] verdiAdresse)
        {
            verdiLi = new List<string>();
            adresseLi = new List<int[]>();

            for (int par = 0; par < Math.Floor(1.0 * n / x); par++)
            {
                bool parFunnet = false;
                //k er indeks over verdiene i potensiellVerdiLi.
                for (int k1 = 0; k1 < potensiellVerdiLi[potensiellVerdiLi.Count - 1].Length; k1++)
                {
                    int adresseIndeks1 = Celle.IndeksIMuligeVerdierForVerdi(potensiellVerdiLi[potensiellVerdiLi.Count - 1][k1], n);
                    string verdiLiTmp = potensiellVerdiLi[potensiellVerdiLi.Count - 1][k1].ToString(); //Tar vare på verdiene som kan være aktuelle.
                    for (int k2 = k1 + 1; k2 < potensiellVerdiLi[potensiellVerdiLi.Count - 1].Length; k2++)
                    {
                        int adresseIndeks2 = Celle.IndeksIMuligeVerdierForVerdi(potensiellVerdiLi[potensiellVerdiLi.Count - 1][k2], n);
                        if (ListLike(verdiAdresse[adresseIndeks1], verdiAdresse[adresseIndeks2])) verdiLiTmp += potensiellVerdiLi[potensiellVerdiLi.Count - 1][k2];
                    }
                    if (x > 2)
                    {
                        for (int xIndeks = potensiellVerdiLi.Count - 2; xIndeks >= 0; xIndeks--)
                        {
                            for (int k2 = 0; k2 < potensiellVerdiLi[xIndeks].Length; k2++)
                            {
                                int adresseIndeks2 = Celle.IndeksIMuligeVerdierForVerdi(potensiellVerdiLi[xIndeks][k2], n);
                                bool like = true;
                                foreach (int j in verdiAdresse[adresseIndeks2]) if (!verdiAdresse[adresseIndeks1].Contains(j)) like = false;
                                if (like) verdiLiTmp += potensiellVerdiLi[xIndeks][k2];
                            }
                        }
                    }
                    if (verdiLiTmp.Length == x)
                    {
                        verdiLi.Add(verdiLiTmp);
                        adresseLi.Add(verdiAdresse[adresseIndeks1].ToArray());
                        parFunnet = true;
                        break;
                    }
                }
                if (parFunnet) foreach (char c in verdiLi[verdiLi.Count - 1]) for (int xIndex = 0; xIndex < potensiellVerdiLi.Count; xIndex++) potensiellVerdiLi[xIndex] = potensiellVerdiLi[xIndex].Replace(c.ToString(), ""); //Fjerner verdiene jeg fant, slik at jeg ikke finner dem på nytt.
            }
        }

        /// <summary>
        /// Finner opptil n verdier som bare forekommer n ganger i de samme n cellene i samme rad, kolonne eller blokk. Kan da fjerne andre tillatte verdier fra de n cellene. Returnerer true hvis det har skjedd en endring i brettet.
        /// </summary>
        /// <param name="brett">Vilkårlig brett</param>
        /// <returns>true hvis det har skjedd en endring i brettet.</returns>
        protected bool Løs2x2RadKolBlokk_PreMetode(ref Celle[,] brett)
        {
            bool endret = false;
            for (int i = 0; i < n; i++)
            {
                string verdier = "";
                int[] verdiTeller = new int[n]; //Inneholder antall ganger hver verdi forekommer i en 'rad'.
                List<int>[] verdiAdresse = new List<int>[n]; //Inneholder 'kolonne' adressene til hver verdi.
                for (int j = 0; j < verdiAdresse.Length; j++) verdiAdresse[j] = new List<int>();
                int xMax = 0;

                for (int j = 0; j < n; j++)
                { //Lister opp hvor mange ganger hver verdi forekommer i denne 'raden'.
                    if (brett[i, j].verdi == ' ')
                    {
                        foreach (char c in brett[i, j].tillatteVerdier)
                        {
                            int k = Celle.IndeksIMuligeVerdierForVerdi(c, n);
                            if (!verdier.Contains(c)) verdier += c;
                            verdiAdresse[k].Add(j);
                            verdiTeller[k]++;
                        }
                        xMax++;
                    }
                }

                //Leter (x er antall verdier den skal finne).
                List<string> potensiellVerdiLi = new List<string>(); ; //Inneholder de verdiene som forekommer x ganger.
                for (int x = 2; x < xMax - 2; x++)
                { //Antall ganger verdien k skal forekomme i 'raden'. (x=9 er poengløst, og x=8 vil slå ut i Løs1TillattVerdi.)
                    string potensielleVerdier = "";
                    foreach (char c in verdier)
                    { //Sjekker om verdien k forekommer x ganger.
                        int k = Celle.IndeksIMuligeVerdierForVerdi(c, n);
                        if (verdiTeller[k] == x) potensielleVerdier += c;
                    }
                    potensiellVerdiLi.Add(potensielleVerdier);
                    Løs2x2RadKolBlokk_PreMetode_SjekkVerdi(x, out List<string> verdiLi, out List<int[]> adresseLi, potensiellVerdiLi, verdiAdresse);
                    //Har nå funnet de x verdiene (og adressene deres) som opptrer bare x ganger i x ruter. Nå skal jeg fjerne de andre tillatte verdiene fra disse rutene.
                    for (int t = 0; t < verdiLi.Count; t++) foreach (int j in adresseLi[t]) foreach (char c in brett[i, j].tillatteVerdier) if (!verdiLi[t].Contains(c)) endret = FjernTillattVerdi(ref brett[i, j], c);
                }
            }
            return endret;
        }

        /// <summary>
        /// Finner x ruter som har nøyaktig x like tillatte verdier. Returner en strengarray over 'kolonne'adressene (med et element for hvert mulig 'par') for rutene som oppfyller dette kravet.
        /// </summary>
        /// <param name="brett">Vilkårlig brett.</param>
        /// <param name="i">Den nåværende 'raden'.</param>
        /// <param name="x">Antall verdier og ruter som skal være like.</param>
        /// <returns>Strengarray over 'kolonne'adressene (med et element for hvert mulig 'par') for rutene som oppfyller funksjonens krav.</returns>
        protected List<int>[] Løs2x2RadKolBlokk_FinnRuter(Celle[,] brett, int i, int x)
        {
            List<int>[] jLi = new List<int>[(int)Math.Floor(1.0 * n / x)];
            for (int j = 0; j < jLi.Length; j++) jLi[j] = new List<int>();
            int teller = 0; //teller antall 'par'.

            for (int j = 0; j < n - (x - 1); j++)
            {
                if (brett[i, j].tillatteVerdier.Replace(" ", "").Length == x)
                {
                    string tillatteVerdier = brett[i, j].tillatteVerdier;
                    jLi[teller].Add(j);
                    for (int l = j + 1; l < n; l++) if (brett[i, l].tillatteVerdier == tillatteVerdier) jLi[teller].Add(l);
                    if (jLi[teller].Count == x)
                    {
                        teller++;
                        if (teller >= jLi.Length) return jLi;
                    }
                    else jLi[teller].Clear();
                }
            }
            return jLi;
        }

        /// <summary>
        /// Finner x ruter som bare har de samme x verdiene i tillatteVerdier. Fjerner deretter disse verdiene fra de andre rutenes tillatteVerdier i samme 'rad'. Returnerer true hvis det har skjedd en endring i brettet.
        /// </summary>
        /// <param name="brett">Vilkårlig brett.</param>
        /// <returns>true hvis det har skjedd en endring i brettet.</returns>
        protected bool Løs2x2RadKolBlokk_HovedMetode(ref Celle[,] brett)
        {
            bool endret = false;
            for (int x = 2; x < n - 2; x++)
            {
                for (int i = 0; i < n; i++)
                {
                    List<int>[] jLi = Løs2x2RadKolBlokk_FinnRuter(brett, i, x);
                    foreach (List<int> jTemp in jLi)
                    {
                        if (jTemp.Count == 0) break;
                        for (int j = 0; j < n; j++) if (!jTemp.Contains(j) && brett[i, j].verdi == ' ') foreach (char c in brett[i, jTemp[0]].tillatteVerdier) endret |= FjernTillattVerdi(ref brett[i, j], c);
                    }
                }
            }
            return endret;
        }

        /// <summary>
        /// Finner x ruter med de samme y verdiene (hvor y er mindre eller lik x) og oppdaterer tillatteVerdier. Returnerer true hvis det har skjedd en endring i brettet.
        /// </summary>
        /// <returns>true hvis det har skjedd en endring i brettet.</returns>
        protected bool Løs2x2RadKolBlokk()
        {
            bool endret = false;
            endret |= Løs2x2RadKolBlokk_PreMetode(ref brettRad);
            if (feil) return endret;
            endret |= Løs2x2RadKolBlokk_HovedMetode(ref brettRad);
            if (feil) return endret;

            endret |= Løs2x2RadKolBlokk_PreMetode(ref brettKol);
            if (feil) return endret;
            endret |= Løs2x2RadKolBlokk_HovedMetode(ref brettKol);
            if (feil) return endret;

            endret |= Løs2x2RadKolBlokk_PreMetode(ref brettBlokk);
            if (feil) return endret;
            endret |= Løs2x2RadKolBlokk_HovedMetode(ref brettBlokk);

            return endret;
        }

        public bool HintLøs1(Celle[,] brett, out string metoder, out int rad, out int kol)
        {
            rad = kol = 0;
            if (feil)
            {
                metoder = "Feil på brettet.";
                return false;
            }
            //else if(!løst) {
            //    metoder = "Brettet har ikke blitt løst.";
            //} else if(!enLøsning) {
            //    metoder = "Brettet har flere løsninger.";
            //}

            Celle[,] brettLøstOpprinnelig = KopiNy(brettRad);
            bool løstOpprinnelig = løst;
            string løsningOpprinnelig = løsning;
            Kopi(ref brettRad, brett);
            int gjenværendeCellerNå = n * n - AntallLøsteCeller();

            finnEnVerdi = true;
            bool fantVerdi = LøsBrett(false, false);
            if (fantVerdi) metoder = metoderBrukt.ToString();
            else metoder = løsning;
            rad = radLøst;
            kol = kolLøst;
            finnEnVerdi = false;
            sisteMetodeBrukt = "";
            metoderBrukt = new StringBuilder();

            Kopi(ref brettRad, brettLøstOpprinnelig);
            løst = løstOpprinnelig;
            løsning = løsningOpprinnelig;

            return fantVerdi;
        }
    }
}

