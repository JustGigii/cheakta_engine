using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuePasser : MonoBehaviour
{
    public static bool loadModel;
    public static bool isTest;
    public static string plane;
    public static char Separator = ((char)007);
    public static bool IsMove;

    static public void Shuffle<T>(List<T> list)
    {

        for (int i = list.Count - 1; i > 0; i--)
        {
            // Generate a random index between 0 and i (inclusive)
            int j = Random.Range(0,i + 1);
            // Swap elements at indices i and j
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
