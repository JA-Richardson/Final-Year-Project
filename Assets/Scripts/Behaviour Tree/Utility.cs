using System.Collections.Generic;

public static class Utility
{
    public static System.Random r = new();
    // Method to shuffle a list
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = r.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
