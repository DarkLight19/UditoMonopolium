using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private static List<Node> nodeAtvaltas(List<Node> graf, int melyikudito, int index)
        {
            List<Node> grafCopy = new List<Node>();
            foreach (var item in graf)
                grafCopy.Add(new Node(item.melyiketSzereti, new List<int>(item.connectedTo)));//muszáj így, hogy ne memóriacímet kapjon

            if (graf[index].melyiketSzereti == melyikudito) //eddig is azt szerette
                return grafCopy;

            List<Node> list = new List<Node>();
            list.Add(grafCopy[index]); //mivel "komplex" változó csak memóriacímet kap

            while(list.Count != 0)
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
                if(item.melyiketSzereti != melyikItal)
                {
                    mindegyikUgyanaz = false;
                    break;
                }
            return mindegyikUgyanaz;
        }
        static void Main(string[] args)
        {
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
                    s = Console.ReadLine().Split(' ').Select(x => int.Parse(x.ToString())-1).ToArray();
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
    }
}
