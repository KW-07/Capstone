using UnityEngine;

public static class UtilScripts
{

    public static int[] RandomArray(int minCount, int maxCount, int n)
    {
        int[] defaults = new int[maxCount];
        int[] results = new int[n];

        for (int i = 0; i < maxCount; i++)
        {
            defaults[i] = i;
        }

        for(int i = 0; i< n;i++)
        {
            int index = Random.Range(minCount, maxCount);

            results[i] = defaults[index];
            defaults[index] = defaults[maxCount - 1];

            maxCount--;
        }

        return results;
    }

}
