using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTypes : MonoBehaviour
{
    public static readonly Dictionary<int, string> TrashColor = new Dictionary<int, string>()
    {
        {0, "#FF0000"},
        {1, "#FFEE00"},
        {2, "#10FF00"},
        {3, "#00BBFF"},
        {4, "#FF0096"},
        {5, "#BC00FF"}
    };
}
