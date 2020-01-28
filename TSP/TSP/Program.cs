using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP
{
    class Program
    {
        public static int lenght;
        public static int liczbaPopulacji = 40;
        public static int grupaTurniejowa = 3;
        public static float wspolczynnikMutacji = 0.0158f;
        public static int wspolczynnikKrzyzowania = 77;
        public static String droga = @"berlin52.txt";
        public static Random rnd = new Random();

        static void Main(string[] args)
        {
            int[,] odleglosci = Odczyt();
            int[] najlepszy = null;
            List<int[]> osobniki = Osobniki();
            List<int[]> mieszajOsobniki = Mieszaj(osobniki);
            List<int> ocena = Ocena(mieszajOsobniki, odleglosci);

            int best = ocena.Min();
            int i = 0;
            while (i < 1000000)
            {
                List<int[]> test = Turniej(osobniki, ocena);
                List<int[]> test2 = Krzyzowanie(test);
                List<int[]> test3 = Mutacja(test2);
                ocena = Ocena(test3, odleglosci);
                osobniki = test3;
                if (ocena.Min() < best)
                {
                    Console.WriteLine(ocena.Min());
                    best = ocena.Min();
                    najlepszy = osobniki[ocena.IndexOf(ocena.Min())];
                    if (i % 10 == 0)
                    {
                        Wyswietl(najlepszy);
                        Console.Write(best);
                        Console.WriteLine("");
                    }

                }
                i++;
            }
            Console.WriteLine("koniec");
            Wyswietl(najlepszy);
            Console.Write(best);
            using (System.IO.StreamWriter sr = new System.IO.StreamWriter("..\\..\\..\\wynik.txt"))
            {
                string wynik = "";
                for (int k = 0; k < najlepszy.Length; k++)
                {
                    if (k < najlepszy.Length - 1)
                    {
                        wynik += najlepszy[k] + "-";
                    }
                    else
                        wynik += najlepszy[k] + " ";
                }
                sr.WriteLine(wynik + best.ToString());
            }
            Console.ReadKey();
        }
        #region krzyżowanie
        public static List<int[]> Krzyzowanie(List<int[]> osobniki)
        {
            List<int[]> Newosobniki = new List<int[]>();

            for (int i = 0; i < liczbaPopulacji - 1; i += 2)
            {
                int czyKrzyzowac = rnd.Next(1, 101);
                int[] rodzic = osobniki[i];
                int[] rodzic2 = osobniki[i + 1];
                if (czyKrzyzowac <= wspolczynnikKrzyzowania)
                {
                    int[] potomek = new int[rodzic.Length];
                    int[] potomek2 = new int[rodzic2.Length];

                    Array.Copy(rodzic, potomek, potomek.Length);
                    Array.Copy(rodzic2, potomek2, potomek2.Length);

                    int p1 = rnd.Next(1, lenght - 3);
                    int p2 = rnd.Next(p1 + 1, lenght - 2);

                    potomek = Przepisz(potomek, osobniki, i + 1, p1, p2);
                    potomek2 = Przepisz(potomek2, osobniki, i, p1, p2);
                    Newosobniki.Add(potomek);
                    Newosobniki.Add(potomek2);
                }
                else
                {
                    Newosobniki.Add(rodzic);
                    Newosobniki.Add(rodzic2);
                }
            }
            return Newosobniki;
        }
        public static int[] Przepisz(int[] potomek, List<int[]> osobniki, int i, int p1, int p2)
        {
            for (int j = 0; j < potomek.Length; j++)
            {
                if (j < p1 || j > p2)
                {
                    int gen = osobniki[i][j];
                    int poz = -1;
                    while ((poz = Znajdz(potomek, gen, p1, p2)) > -1)
                    {
                        gen = osobniki[i][poz];
                    }
                    potomek[j] = gen;
                }
            }
            return potomek;
        }
        public static int Znajdz(int[] tab, int el, int start, int koniec)
        {
            for (int i = start; i <= koniec; i++)
            {
                if (tab[i] == el)
                {
                    return i;
                }
            }
            return -1;
        }
        public static void Wyswietl(int[] tab)
        {
            for (int i = 0; i < tab.Length; i++)
            {
                if (i < tab.Length - 1)
                {
                    Console.Write(tab[i] + "-");
                }
                else
                    Console.Write(tab[i] + " ");
            }
        }
        public static void Wyswietl(List<int[]> tab)
        {
            for (int i = 0; i < tab.Count; i++)
            {
                foreach (int j in tab[i])
                {
                    Console.Write(j + " ");
                }
                Console.WriteLine("");
            }
        }

        #endregion
        #region mutacja
        public static List<int[]> Mutacja(List<int[]> osobniki)
        {
            for (int i = 0; i < liczbaPopulacji; i++)
            {
                for (int j = 0; j < osobniki[i].Length; j++)
                {
                    if (rnd.NextDouble() < wspolczynnikMutacji)
                    {
                        int k = rnd.Next(0, lenght);
                        int tmp = osobniki[i][j];
                        osobniki[i][j] = osobniki[i][k];
                        osobniki[i][k] = tmp;
                    }
                }
            }
            return osobniki;
        }
        #endregion
        #region odczyt
        public static int[,] Odczyt()
        {
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(droga))
                {
                    lenght = int.Parse(sr.ReadLine());
                    string[] fileLines = new string[lenght];
                    int[,] arr = new int[fileLines.Length, fileLines.Length];
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        string[] tmp = sr.ReadLine().Split();
                        for (int j = 0; j <= i; j++)
                        {
                            arr[i, j] = arr[j, i] = int.Parse(tmp[j]);
                        }
                    }
                    return arr;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        #endregion
        #region osobniki
        public static List<int[]> Osobniki()
        {
            List<int[]> Osobniki = new List<int[]>();
            int[] tablica = new int[lenght];
            for (int i = 0; i < lenght; i++)
            {
                tablica[i] = i;
            }
            for (int i = 0; i < liczbaPopulacji; i++)
            {
                Osobniki.Add(tablica);
            }
            return Osobniki;
        }
        #endregion
        #region mieszaj
        public static List<int[]> Mieszaj(List<int[]> tab)
        {
            for (int i = 0; i < tab.Count; i++)
            {
                int[] mieszajTablice = tab[i].OrderBy(x => rnd.Next()).ToArray();
                tab[i] = mieszajTablice;
            }
            return tab;
        }
        #endregion
        #region ocena
        public static List<int> Ocena(List<int[]> mieszajOsobniki, int[,] odleglosci)
        {
            List<int> Oceny = new List<int>();
            int suma = 0;
            int m = 0;
            int n = 0;
            for (int i = 0; i < liczbaPopulacji; i++)
            {
                suma = 0;
                for (int j = 0; j < lenght; j++)
                {
                    m = mieszajOsobniki[i][j];
                    if (j + 1 > lenght - 1)
                    {
                        n = mieszajOsobniki[i][0];
                    }
                    else
                    {
                        n = mieszajOsobniki[i][j + 1];
                    }
                    suma += odleglosci[m, n];
                }
                Oceny.Add(suma);
            }

            return Oceny;
        }
        #endregion
        #region turniej
        public static List<int[]> Turniej(List<int[]> osobniki, List<int> ocena)
        {
            List<int[]> nowaPopulacja = new List<int[]>();

            int index = rnd.Next(0, liczbaPopulacji);
            int[] najlepszyOsobnik = osobniki[index];
            int ocenaNajlepszegoOsobnika = ocena[index];
            for (int i = 0; i < liczbaPopulacji; i++)
            {
                for (int k = 0; k < grupaTurniejowa; k++)
                {
                    index = rnd.Next(0, liczbaPopulacji);
                    if (ocena[index] < ocenaNajlepszegoOsobnika)
                    {
                        ocenaNajlepszegoOsobnika = ocena[index];
                        najlepszyOsobnik = osobniki[index];
                    }
                }
                nowaPopulacja.Add(najlepszyOsobnik);
                ocenaNajlepszegoOsobnika = int.MaxValue;
            }
            return nowaPopulacja;
        }
        #endregion
        /* #region ruletka
         public static List<int[]> OdwrocRuletka(List<int> ocena, List<int[]> osobniki)
         {
             int max = ocena.Max();
             List<int> odwroconaOcena = new List<int>();
             for (int i = 0; i < ocena.Count; i++)
             {
                 odwroconaOcena.Add((max - ocena[i] + 1));
             }
             for (int i = 0; i < liczbaPopulacji; i++)
             {
                 if (i > 0)
                 {
                     odwroconaOcena[i] = odwroconaOcena[i] + odwroconaOcena[i - 1];
                 }
             }
             int losuj = 0;
             int index = 0;
             List<int[]> NowaPopulacjaRuletka = osobniki;
             for (int i = 0; i < liczbaPopulacji; i++)
             {
                 losuj = rnd.Next(odwroconaOcena.Max());
                 for (int j = 0; j < odwroconaOcena.Count; j++)
                 {
                     if (losuj < odwroconaOcena[j])
                     {
                         index = j;
                     }
                 }
                 for (int k = 0; k < liczbaPopulacji; k++)
                 {
                     NowaPopulacjaRuletka[index][k] = osobniki[index][k];
                 }
             }
             return NowaPopulacjaRuletka;
         }
         #endregion*/
    }
}