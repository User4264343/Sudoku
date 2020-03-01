namespace Sudoku
{
    public class Kladd
    {
        public const int antallLinjer = 8;
        public KladdIntern[,] kladdelinjer;
        public string[,] brett;

        public Kladd(int n)
        {
            kladdelinjer = new KladdIntern[antallLinjer, n];
            for (int l = 0; l < antallLinjer; l++) for (int i = 0; i < n; i++) kladdelinjer[l, i] = new KladdIntern();
            brett = new string[n, n];
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) brett[i, j] = "";
        }

        public Kladd(Kladd kladd)
        {
            kladdelinjer = new KladdIntern[kladd.kladdelinjer.GetLength(0), kladd.kladdelinjer.GetLength(1)];
            for (int l = 0; l < kladdelinjer.GetLength(0); l++) for (int i = 0; i < kladdelinjer.GetLength(1); i++) kladdelinjer[l, i] = new KladdIntern(kladd.kladdelinjer[l, i]);
            brett = (string[,])kladd.brett.Clone();
        }

        public void Kopi(Kladd kladd)
        {
            //kladdLi = new KladdIntern[kladd.kladdLi.Length];
            for (int l = 0; l < kladdelinjer.GetLength(0); l++) for (int i = 0; i < kladdelinjer.GetLength(1); i++) kladdelinjer[l, i].Kopi(kladd.kladdelinjer[l, i]);
            brett = (string[,])kladd.brett.Clone();
        }
    }

    public class KladdIntern
    {
        private string verdi = "";

        public string Verdi
        {
            get { return verdi; }
            set { verdi = value; }
        }

        public KladdIntern() { }

        public KladdIntern(KladdIntern kladd)
        {
            verdi = kladd.verdi;

        }

        public void Kopi(KladdIntern kladd)
        {
            verdi = kladd.verdi;
        }
    }
}






//    public class Kladd {
//        public KladdIntern[] kladdLi;
//        public string[,] brett;

//        public Kladd(int n) {
//            kladdLi = new KladdIntern[n];
//            for(int i = 0; i < n; i++) {
//                kladdLi[i] = new KladdIntern();
//            }
//            brett = new string[n, n];
//            for(int i = 0; i < n; i++) for(int j = 0; j < n; j++) brett[i, j] = "";
//        }

//        public Kladd(Kladd kladd) {
//            kladdLi = new KladdIntern[kladd.kladdLi.Length];
//            for(int i = 0; i < kladdLi.Length; i++) {
//                kladdLi[i] = new KladdIntern(kladd.kladdLi[i]);
//            }
//            brett = (string[,])kladd.brett.Clone();
//        }

//        public void Kopi(Kladd kladd) {
//            //kladdLi = new KladdIntern[kladd.kladdLi.Length];
//            for(int i = 0; i < kladdLi.Length; i++) {
//                kladdLi[i].Kopi(kladd.kladdLi[i]);
//            }
//            brett = (string[,])kladd.brett.Clone();
//        }
//    }

//    public class KladdIntern {
//        private string oppLi;
//        private string oppMidtLi;
//        private string venstreLi;
//        private string venstreMidtLi;
//        private string høyreLi;
//        private string høyreMidtLi;
//        private string nedLi;
//        private string nedMidtLi;

//        public string OppLi {
//            get { return oppLi; }
//            set { oppLi = value; }
//        }
//        public string OppMidtLi {
//            get { return oppMidtLi; }
//            set { oppMidtLi = value; }
//        }
//        public string VenstreLi {
//            get { return venstreLi; }
//            set { venstreLi = value; }
//        }
//        public string VenstreMidtLi {
//            get { return venstreMidtLi; }
//            set { venstreMidtLi = value; }
//        }
//        public string HøyreLi {
//            get { return høyreLi; }
//            set { høyreLi = value; }
//        }
//        public string HøyreMidtLi {
//            get { return høyreMidtLi; }
//            set { høyreMidtLi = value; }
//        }
//        public string NedLi {
//            get { return nedLi; }
//            set { nedLi = value; }
//        }
//        public string NedMidtLi {
//            get { return nedMidtLi; }
//            set { nedMidtLi = value; }
//        }

//        public KladdIntern() { }

//        public KladdIntern(KladdIntern kladd) {
//            oppLi = kladd.oppLi;
//            oppMidtLi = kladd.oppMidtLi;
//            venstreLi = kladd.venstreLi;
//            venstreMidtLi = kladd.venstreMidtLi;
//            høyreLi = kladd.høyreLi;
//            høyreMidtLi = kladd.høyreMidtLi;
//            nedLi = kladd.nedLi;
//            nedMidtLi = kladd.nedMidtLi;

//        }

//        public void Kopi(KladdIntern kladd) {
//            oppLi = kladd.oppLi;
//            oppMidtLi = kladd.oppMidtLi;
//            venstreLi = kladd.venstreLi;
//            venstreMidtLi = kladd.venstreMidtLi;
//            høyreLi = kladd.høyreLi;
//            høyreMidtLi = kladd.høyreMidtLi;
//            nedLi = kladd.nedLi;
//            nedMidtLi = kladd.nedMidtLi;

//        }
//    }
//}


//public class Kladd {
//    public string[] oppLi;
//    public string[] oppMidtLi;
//    public string[] venstreLi;
//    public string[] venstreMidtLi;
//    public string[] høyreLi;
//    public string[] høyreMidtLi;
//    public string[] nedLi;
//    public string[] nedMidtLi;
//    public string[,] brett;

//    public Kladd(int n) {
//        oppLi = new string[n];
//        oppMidtLi = new string[n];
//        venstreLi = new string[n];
//        venstreMidtLi = new string[n];
//        høyreLi = new string[n];
//        høyreMidtLi = new string[n];
//        nedLi = new string[n];
//        nedMidtLi = new string[n];
//        brett = new string[n, n];
//    }

//    public Kladd(Kladd kladd) {
//        oppLi = (string[])kladd.oppLi.Clone();
//        oppMidtLi = (string[])kladd.oppMidtLi.Clone();
//        venstreLi = (string[])kladd.venstreLi.Clone();
//        venstreMidtLi = (string[])kladd.venstreMidtLi.Clone();
//        høyreLi = (string[])kladd.høyreLi.Clone();
//        høyreMidtLi = (string[])kladd.høyreMidtLi.Clone();
//        nedLi = (string[])kladd.nedLi.Clone();
//        nedMidtLi = (string[])kladd.nedMidtLi.Clone();
//        brett = (string[,])kladd.brett.Clone();
//    }

//    public void Kopi(Kladd kladd) {
//        oppLi = (string[])kladd.oppLi.Clone();
//        oppMidtLi = (string[])kladd.oppMidtLi.Clone();
//        venstreLi = (string[])kladd.venstreLi.Clone();
//        venstreMidtLi = (string[])kladd.venstreMidtLi.Clone();
//        høyreLi = (string[])kladd.høyreLi.Clone();
//        høyreMidtLi = (string[])kladd.høyreMidtLi.Clone();
//        nedLi = (string[])kladd.nedLi.Clone();
//        nedMidtLi = (string[])kladd.nedMidtLi.Clone();
//        brett = (string[,])kladd.brett.Clone();
//    }
//}
