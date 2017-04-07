using System;
using System.Collections;
using System.Collections.Generic;

namespace DevilDesireDevLib.Implementation.Extensions
{
    public static class RandomExtensions
    {
        public static double NextGausian(this Random random, double mu = 0, double sigma = 1)
        {
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double randNormal = mu + sigma * randStdNormal;

            return randNormal;
        }

        public static double NextTriangular(this Random r, double a, double b, double c)
        {
            double u = r.NextDouble();

            return u < (c - a) / (b - a)
                ? a + Math.Sqrt(u * (b - a) * (c - a))
                : b - Math.Sqrt((1 - u) * (b - a) * (b - c));
        }

        public static void Shuffle(this Random r, IList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = r.Next(0, i + 1);

                object temp = list[j];
                list[j] = list[i];
                list[i] = temp;
            }
        }

        public static int[] Permutation(this Random rand, int n, int k)
        {
            List<int> result = new List<int>();
            SortedSet<int> sorted = new SortedSet<int>();

            for (int i = 0; i < k; i++)
            {
                int r = rand.Next(1, n + 1 - i);

                foreach (int q in sorted)
                    if (r >= q) r++;

                result.Add(r);
                sorted.Add(r);
            }

            return result.ToArray();
        }
    }
}