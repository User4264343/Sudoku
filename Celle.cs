using System;

namespace Sudoku
{
    /// <summary>
    /// En celle i sudoku brettet.
    /// </summary>
    public class Celle
    {
        public readonly int n;
        /// <summary>
        /// Raden (0-basis)
        /// </summary>
        readonly public int rad;
        /// <summary>
        /// Kolonnen (0-basis)
        /// </summary>
        readonly public int kol;
        /// <summary>
        /// true hvis cellen ikke skal kunne endres. Default er false.
        /// </summary>
        public bool låst = false;
        /// <summary>
        /// Verdien til cellen (1-9 for 3x3 brett). Skal være et tegn. Default er mellomrom (' ').
        /// </summary>
        public char verdi = ' ';
        /// <summary>
        /// Blokknr til cellen (0-basis)
        /// </summary>
        readonly public int blokk;
        /// <summary>
        /// Hvilket cellenr cellen er i blokken sin (0-basis)
        /// </summary>
        readonly public int celleIBlokk;
        /// <summary>
        /// Antall mulige verdier for cellen.
        /// </summary>
        public readonly string muligeVerdier;
        /// <summary>
        /// Hvilke verdier som 'verdi' kan ta. Default er "123456789 for 3x3 brett".
        /// </summary>
        public string tillatteVerdier;

        /// <summary>
        /// true hvis rad, kol og verdi er like, eller hvis begge cellene er null.
        /// </summary>
        /// <param name="vs">Vilkårlig Celle.</param>
        /// <param name="hs">Vilkårlig Celle.</param>
        /// <returns>true hvis rad, kol og verdi er like, eller hvis begge cellene er null.</returns>
        public static bool operator ==(Celle vs, Celle hs)
        {
            if (vs is null && hs is null) return true;
            if (vs is null || hs is null) return false;
            if (vs.n != hs.n || vs.rad != hs.rad || vs.kol != hs.kol || vs.verdi != hs.verdi) return false;
            return true;
        }
        /// <summary>
        /// true hvis rad, kol og verdi er like, eller hvis begge cellene er null.
        /// </summary>
        /// <param name="vs">Vilkårlig Celle.</param>
        /// <param name="hs">Vilkårlig Celle.</param>
        /// <returns>true hvis rad, kol og verdi er like, eller hvis begge cellene er null.</returns>
        public static bool operator !=(Celle vs, Celle hs)
        {
            return !(vs == hs);
        }
        /// <summary>
        /// true hvis rad, kol og verdi er like, eller hvis begge cellene er null.
        /// </summary>
        /// <param name="obj">variabel som blir cast'et til Celle.</param>
        /// <returns>true hvis rad, kol og verdi er like, eller hvis begge cellene er null.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Celle celle) return this.Equals(celle);
            return this.Equals(null);
        }
        /// <summary>
        /// true hvis rad, kol og verdi er like, eller hvis begge cellene er null.
        /// </summary>
        /// <param name="celle">Vilkårlig Celle.</param>
        /// <returns>true hvis rad, kol og verdi er like, eller hvis begge cellene er null.</returns>
        public bool Equals(Celle celle)
        {
            if (this is null && celle is null) return true;
            if (this is null || celle is null) return false;
            if (this.n != celle.n || this.rad != celle.rad || this.kol != celle.kol || this.verdi != celle.verdi) return false;
            return true;
        }
        //public override int GetHashCode() {
        //    return GetHashCodeOverload.Hashkode.KombinerHashKoder(this.rad, this.kol, this.verdi);
        //}

        /// <summary>
        /// Initialiserer cellen med verdier for rad, kolonne, blokk og celleIBlokk.
        /// </summary>
        /// <param name="rad">Raden (0-basert)</param>
        /// <param name="kol">Kolonnen (0-basert)</param>
        /// <param name="nRot">Størrelsen på brettet (3 for et brett med 3x3 blokker med 3x3 ruter i hver blokk).</param>
        public Celle(int rad, int kol, int nRot)
        {
            this.rad = rad;
            this.kol = kol;
            this.n = nRot * nRot;
            this.muligeVerdier = MuligeVerdier(n);
            this.tillatteVerdier = this.muligeVerdier;
            this.blokk = (rad / nRot) * nRot + kol / nRot;
            this.celleIBlokk = kol % nRot + (rad % nRot) * nRot;
        }

        /// <summary>
        /// Kopierer celle til cellen.
        /// </summary>
        /// <param name="celle">Cellen som skal kopieres.</param>
        public Celle(Celle celle)
        {
            this.n = celle.n;
            this.muligeVerdier = celle.muligeVerdier;
            this.verdi = celle.verdi;
            this.tillatteVerdier = celle.tillatteVerdier;
            this.rad = celle.rad;
            this.kol = celle.kol;
            this.blokk = celle.blokk;
            this.celleIBlokk = celle.celleIBlokk;
            this.låst = celle.låst;
        }

        /// <summary>
        /// Lager en kopi fra celle, av verdi, tillatteVerdier og låst. Andre elementer, som rad og kol, blir ikke kopiert.
        /// </summary>
        /// <param name="celle">Cellen som skal kopieres.</param>
        public void Kopi(Celle celle)
        {
            if (this.n != celle.n) throw new Exception("Feil i Celle.Kopi: n ikke lik.");
            else if (this.rad != celle.rad) throw new Exception("Feil i Celle.Kopi: rad ikke lik.");
            else if (this.kol != celle.kol) throw new Exception("Feil i Celle.Kopi: kol ikke lik.");
            else if (this.blokk != celle.blokk) throw new Exception("Feil i Celle.Kopi: blokk ikke lik.");

            this.verdi = celle.verdi;
            this.tillatteVerdier = celle.tillatteVerdier;
            this.låst = celle.låst;
        }

        /// <summary>
        /// Gir indeksen i muligeVerdier hvor en finner verdien verdi.
        /// </summary>
        /// <param name="verdi">Verdien som en vil ha indeksnr. for.</param>
        /// <param name="n">Antall verdier en Celle kan ha.</param>
        /// <returns>Indeksen i muligeVerdier hvor en finner verdien verdi.</returns>
        public static int IndeksIMuligeVerdierForVerdi(char verdi, int n)
        {
            string verdiLi = MuligeVerdier(n);
            for (int i = 0; i < verdiLi.Length; i++) if (verdiLi[i] == verdi) return i;
            return -1;
        }

        /// <summary>
        /// Gir en streng med alle mulige verdier for en Celle med n mulige verdier.
        /// </summary>
        /// <param name="n">Antall verdier en Celle kan ha.</param>
        /// <returns>Streng med alle mulige verdier for en Celle med n mulige verdier.</returns>
        public static string MuligeVerdier(int n)
        {
            string muligeVerdierTemp = "";
            string tallLi = "1234567890";
            string bokstavLi = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (n > tallLi.Length + bokstavLi.Length) throw new Exception("For stor størrelse på brettet.");
            for (int i = 0; i < (tallLi.Length < n ? tallLi.Length : n); i++) muligeVerdierTemp += tallLi[i];
            for (int i = 0; i < (bokstavLi.Length < (n - tallLi.Length) ? bokstavLi.Length : (n - tallLi.Length)); i++) muligeVerdierTemp += bokstavLi[i];
            return muligeVerdierTemp;
        }

        /// <summary>
        /// Nullstiller tillatteVerdier.
        /// </summary>
        public void ResetTillatteVerdier()
        {
            tillatteVerdier = muligeVerdier;
        }
    }
}

