using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace _1213Graf
{
    class Program
    {
        /*
         input:
2
4 3
0 1 0 0
1 2
2 3
2 4
5 4
0 1 1 0 1
1 2
2 3
3 4
4 5

1
4 3
0 1 0 0
1 2
2 3
2 4
         */
        class Node
        {
            public int melyiketSzereti;
            public List<int> connectedTo = new List<int>();
            public Node(int a, List<int> b)
            {
                melyiketSzereti = a;
                connectedTo = b;
            }
        }

        public static Stopwatch timer = new Stopwatch();

        #region take1
        private static List<Node> nodeAtvaltas(List<Node> graf, int melyikudito, int index)
        {
            List<Node> grafCopy = new List<Node>();
            foreach (var item in graf)
                grafCopy.Add(new Node(item.melyiketSzereti, new List<int>(item.connectedTo)));//muszáj így, hogy ne memóriacímet kapjon

            if (graf[index].melyiketSzereti == melyikudito) //eddig is azt szerette
                return grafCopy;

            List<Node> list = new List<Node>();
            list.Add(grafCopy[index]); //mivel "komplex" változó csak memóriacímet kap

            while (list.Count != 0)
            {
                List<Node> hozaadjuk = new List<Node>();
                foreach (var item in list)
                    foreach (var i in item.connectedTo)
                        if (grafCopy[i].melyiketSzereti != melyikudito)
                            hozaadjuk.Add(grafCopy[i]);

                foreach (var item in hozaadjuk)
                    list.Add(item);

                list[0].melyiketSzereti = melyikudito; //Emiatt itt is konkrétan a 'grafCopy' elemét változtatjuk
                list.RemoveAt(0);
            }

            return grafCopy;
        }

        private static int rekurzivVerzioIndito(List<Node> graf)
        {
            int solution = rekurzivMegoldas(graf, 0, 0);
            int solutionTwo = rekurzivMegoldas(graf, 1, 0);

            return solution < solutionTwo ? solution : solutionTwo;
        }

        public static int aktMin = int.MaxValue;
        private static int rekurzivMegoldas(List<Node> graf, int milyencsomag, int hanyadikkor)
        {
            if (rekurzivMegoldasCheck(graf))
            {
                aktMin = hanyadikkor;
                return hanyadikkor;
            }
            else
            {
                if (aktMin > hanyadikkor)
                {
                    List<int> megoldasok = new List<int>();
                    for (int i = 0; i < graf.Count; i++) //mindegyik Node ra elküldjük ugyanazt a csomagot, mindegyik egyszer vissza fog adni egy megoldást
                        megoldasok.Add(rekurzivMegoldas(nodeAtvaltas(graf, milyencsomag, i), milyencsomag == 1 ? 0 : 1, hanyadikkor + 1));

                    megoldasok.Sort();
                    return megoldasok[0];
                }
                else
                    return int.MaxValue;
            }
        }

        private static bool rekurzivMegoldasCheck(List<Node> graf)
        {
            bool mindegyikUgyanaz = true;
            int melyikItal = graf[0].melyiketSzereti;
            foreach (var item in graf)
                if (item.melyiketSzereti != melyikItal)
                {
                    mindegyikUgyanaz = false;
                    break;
                }
            return mindegyikUgyanaz;
        }

        #endregion

        #region take2
        private static List<Node> takeTwoNodeAtvaltas(List<Node> graf, int melyikudito, int index)
        {
            List<Node> grafCopy = new List<Node>();
            foreach (var item in graf)
                grafCopy.Add(new Node(item.melyiketSzereti, new List<int>(item.connectedTo)));//muszáj így, hogy ne memóriacímet kapjon

            if (graf[index].melyiketSzereti == melyikudito) //eddig is azt szerette
                return grafCopy;

            List<Node> list = new List<Node>();
            list.Add(grafCopy[index]); //mivel "komplex" változó csak memóriacímet kap

            List<int> nodesThatAreTheSameColor = new List<int>();

            while (list.Count != 0)
            {
                List<Node> hozaadjuk = new List<Node>();
                foreach (var item in list)
                    foreach (var i in item.connectedTo)
                        if (grafCopy[i].melyiketSzereti != melyikudito)
                            if (!nodesThatAreTheSameColor.Contains(i))
                            {
                                hozaadjuk.Add(grafCopy[i]);
                                nodesThatAreTheSameColor.Add(i);
                            }

                foreach (var item in hozaadjuk)
                    list.Add(item);

                list[0].melyiketSzereti = melyikudito; //Emiatt itt is konkrétan a 'grafCopy' elemét változtatjuk
                list.RemoveAt(0);
            }

            #region kiiras
            Console.WriteLine("-!-!-!-!-!-!-!-!-!-");
            Console.WriteLine("ugyanolyan színű talált elemek listája:");
            foreach (var item in nodesThatAreTheSameColor)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("-!-!-!-!-!-!-!-!-!-");
            nodesThatAreTheSameColor.Remove(index);
            takeTwoOsszevonas(nodesThatAreTheSameColor, grafCopy, index);

            Console.WriteLine("---------------------");
            Console.WriteLine("Gráf kiírva:");
            for (int i = 0; i < grafCopy.Count; i++)
            {
                Console.Write(i + " ");
                foreach (var item in grafCopy[i].connectedTo)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("---------------------");
            #endregion
            return grafCopy;
        }

        private static void takeTwoOsszevonas(List<int> sameColor, List<Node> graf, int index)//megint memóriacímes
        {
            List<int> csatlakozasok = new List<int>();
            foreach (var item in sameColor)
                for (int i = 0; i < graf[item].connectedTo.Count; i++)
                    if (!sameColor.Contains(graf[item].connectedTo[i]) && graf[item].connectedTo[i] != index && !graf[index].connectedTo.Contains(graf[item].connectedTo[i]))
                        foreach (var temp in graf[item].connectedTo)
                        {
                            if (!csatlakozasok.Contains(temp))
                                csatlakozasok.Add(temp);
                        }

            foreach (var item in sameColor)
                graf[index].connectedTo.Remove(item);

            foreach (var item in sameColor)
                graf[item].connectedTo = new List<int>();

            foreach (var item in csatlakozasok)
                graf[index].connectedTo.Add(item);
        }

        private static int takeTwoRekurzivVerzioIndito(List<Node> graf)
        {
            int solution = takeTwoRekurzivMegoldas(graf, 0, 0);
            int solutionTwo = takeTwoRekurzivMegoldas(graf, 1, 0);

            return solution < solutionTwo ? solution : solutionTwo;
        }

        public static int takeTwoAktMin = int.MaxValue;
        private static int takeTwoRekurzivMegoldas(List<Node> graf, int milyencsomag, int hanyadikkor)
        {
            if (takeTwoRekurzivMegoldasCheck(graf))
            {
                takeTwoAktMin = hanyadikkor;
                return hanyadikkor;
            }
            else
            {
                if (takeTwoAktMin > hanyadikkor)
                {
                    List<int> megoldasok = new List<int>();
                    for (int i = 0; i < graf.Count; i++) //mindegyik Node ra elküldjük ugyanazt a csomagot, mindegyik egyszer vissza fog adni egy megoldást
                        if (graf[i].connectedTo.Count != 0) //ha már össze lett vonva egy másikkal akkor arra ne küldjünk
                            megoldasok.Add(takeTwoRekurzivMegoldas(takeTwoNodeAtvaltas(graf, milyencsomag, i), milyencsomag == 1 ? 0 : 1, hanyadikkor + 1));

                    megoldasok.Sort();
                    Console.WriteLine("OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
                    Console.WriteLine(megoldasok[0]);
                    Console.WriteLine("OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
                    return megoldasok[0];//min
                }
                else
                    return int.MaxValue;
            }
        }

        private static bool takeTwoRekurzivMegoldasCheck(List<Node> graf)
        {
            bool mindegyikUgyanaz = true;
            int melyikItal = 0;
            foreach (var item in graf)
                if (item.connectedTo.Count != 0)
                {
                    melyikItal = item.melyiketSzereti;
                    break;
                }

            foreach (var item in graf)
                if (item.melyiketSzereti != melyikItal && item.connectedTo.Count != 0)
                {
                    mindegyikUgyanaz = false;
                    break;
                }

            return mindegyikUgyanaz;
        }

        #endregion

        #region take3

        List<Node> savedStates = new List<Node>();
        private static List<Node> takeThreeNodeAtvaltas(List<Node> graf, int melyikudito, int index)
        {
            List<Node> grafCopy = new List<Node>();
            foreach (var item in graf)
                grafCopy.Add(new Node(item.melyiketSzereti, new List<int>(item.connectedTo)));//muszáj így, hogy ne memóriacímet kapjon

            if (graf[index].melyiketSzereti == melyikudito) //eddig is azt szerette
                return grafCopy;

            List<Node> list = new List<Node>();
            list.Add(grafCopy[index]); //mivel "komplex" változó csak memóriacímet kap

            List<int> nodesThatAreTheSameColor = new List<int>();

            while (list.Count != 0)
            {
                List<Node> hozaadjuk = new List<Node>();
                foreach (var item in list)
                    foreach (var i in item.connectedTo)
                        if (grafCopy[i].melyiketSzereti != melyikudito)
                            if (!nodesThatAreTheSameColor.Contains(i))
                            {
                                hozaadjuk.Add(grafCopy[i]);
                                nodesThatAreTheSameColor.Add(i);
                            }

                foreach (var item in hozaadjuk)
                    list.Add(item);

                list[0].melyiketSzereti = melyikudito; //Emiatt itt is konkrétan a 'grafCopy' elemét változtatjuk
                list[0].connectedTo = new List<int>(); //összeolvasztva
                list.RemoveAt(0);
            }

            List<int> toBeDeleted = new List<int>();
            foreach (var item in grafCopy[index].connectedTo)
                if (nodesThatAreTheSameColor.Contains(item))
                    toBeDeleted.Add(item);

            toBeDeleted.Sort();
            toBeDeleted.Reverse();
            foreach (var item in toBeDeleted)
                grafCopy[index].connectedTo.RemoveAt(item);

            foreach (var item in graf)
                for (int i = 0; i < item.connectedTo.Count; i++)
                {
                    if (nodesThatAreTheSameColor.Contains(item.connectedTo[i]))//aki eddig egy összeolvasztott node-ra mutatott az az eredetire fog
                    {
                        item.connectedTo.RemoveAt(i);
                        if (!item.connectedTo.Contains(index))
                            item.connectedTo.Add(index);
                        --i;//nehogy kihagyjunk egyet
                    }
                }

            return grafCopy;
        }

        private static int takeThreeRekurzivVerzioIndito(List<Node> graf)
        {
            int solution = rekurzivMegoldas(graf, 0, 0);
            int solutionTwo = rekurzivMegoldas(graf, 1, 0);

            return solution < solutionTwo ? solution : solutionTwo;
        }

        public static int takeThreeAktMin = int.MaxValue;
        private static int takeThreeRekurzivMegoldas(List<Node> graf, int milyencsomag, int hanyadikkor)
        {
            if (takeThreeRekurzivMegoldasCheck(graf))
            {
                takeThreeAktMin = hanyadikkor;
                return hanyadikkor;
            }
            else
            {
                if (takeThreeAktMin > hanyadikkor)
                {
                    List<int> megoldasok = new List<int>();
                    for (int i = 0; i < graf.Count; i++) //mindegyik Node ra elküldjük ugyanazt a csomagot, mindegyik egyszer vissza fog adni egy megoldást
                        if (graf[i].connectedTo.Count != 0) //ha már össze lett vonva egy másikkal akkor arra ne küldjünk
                            megoldasok.Add(takeThreeRekurzivMegoldas(takeThreeNodeAtvaltas(graf, milyencsomag, i), milyencsomag == 1 ? 0 : 1, hanyadikkor + 1));

                    megoldasok.Sort();
                    return megoldasok[0];//min
                }
                else
                    return int.MaxValue;
            }
        }

        private static bool takeThreeRekurzivMegoldasCheck(List<Node> graf)
        {
            bool mindegyikUgyanaz = true;
            int melyikItal = graf[0].melyiketSzereti;
            foreach (var item in graf)
                if (item.melyiketSzereti != melyikItal)
                {
                    mindegyikUgyanaz = false;
                    break;
                }
            return mindegyikUgyanaz;
        }

        #endregion

        static void Main(string[] args)
        {
            string melyiketFuttassuk = "take2";

            //kissebbekre jobb az 1. megoldás
            /*
            if (N < 100)
                melyiketFuttassuk = "take1";
            else
                melyiketFuttassuk = "take2";*/


            if (melyiketFuttassuk == "take1")
            {
                timer.Start();//időzítő elindítása
                int T = int.Parse(Console.ReadLine());

                for (int i = 0; i < T; i++)
                {
                    int[] s = Console.ReadLine().Split(' ').Select(x => int.Parse(x.ToString())).ToArray();
                    int N = s[0];
                    int M = s[1];

                    List<Node> graf = new List<Node>();
                    s = Console.ReadLine().Split(' ').Select(x => int.Parse(x.ToString())).ToArray();
                    for (int j = 0; j < N; j++)
                        graf.Add(new Node(s[j], new List<int>()));
                    for (int j = 0; j < M; j++)
                    {
                        s = Console.ReadLine().Split(' ').Select(x => int.Parse(x.ToString()) - 1).ToArray();
                        if (!graf[s[0]].connectedTo.Contains(s[1]))
                            graf[s[0]].connectedTo.Add(s[1]);
                        if (!graf[s[1]].connectedTo.Contains(s[0]))
                            graf[s[1]].connectedTo.Add(s[0]);
                    }

                    aktMin = int.MaxValue;
                    Console.WriteLine();
                    Console.WriteLine(i + 1 + ". feladat megoldása: ");
                    Console.WriteLine(rekurzivVerzioIndito(graf));
                    Console.WriteLine();
                }
            }

            else if (melyiketFuttassuk == "take2")
            {
                timer.Start();//időzítő elindítása
                int T = int.Parse(Console.ReadLine());

                for (int i = 0; i < T; i++)
                {
                    int[] s = Console.ReadLine().Split(' ').Select(x => int.Parse(x.ToString())).ToArray();
                    int N = s[0];
                    int M = s[1];

                    List<Node> graf = new List<Node>();
                    s = Console.ReadLine().Split(' ').Select(x => int.Parse(x.ToString())).ToArray();
                    for (int j = 0; j < N; j++)
                        graf.Add(new Node(s[j], new List<int>()));
                    for (int j = 0; j < M; j++)
                    {
                        s = Console.ReadLine().Split(' ').Select(x => int.Parse(x.ToString()) - 1).ToArray();
                        if (!graf[s[0]].connectedTo.Contains(s[1]))
                            graf[s[0]].connectedTo.Add(s[1]);
                        if (!graf[s[1]].connectedTo.Contains(s[0]))
                            graf[s[1]].connectedTo.Add(s[0]);
                    }

                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    Console.WriteLine("Gráf kiírva:");
                    for (int j = 0; j < graf.Count; j++)
                    {
                        Console.Write(j + " ");
                        foreach (var item in graf[j].connectedTo)
                        {
                            Console.Write(item + " ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                    aktMin = int.MaxValue;
                    Console.WriteLine();
                    Console.WriteLine(i + 1 + ". feladat megoldása: ");
                    Console.WriteLine(takeTwoRekurzivVerzioIndito(graf));
                    Console.WriteLine();
                }
            }

            Console.WriteLine("A program : " + timer.Elapsed + " másodperc alatt futott le.");//időzítő kiirása
        }
    }
}

