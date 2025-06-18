using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform trashPool;
    private void OnEnable()
    {
        if (CO_StartSpawningTrash != null) { StopCoroutine(CO_StartSpawningTrash); CO_StartSpawningTrash = null; }
        CO_StartSpawningTrash = StartCoroutine(StartSpawningTrash());
    }


    float currentTime;
    Coroutine CO_StartSpawningTrash;
    IEnumerator StartSpawningTrash()
    {
        if (trashPool == null) { CO_StartSpawningTrash = null; yield break; }
        var flt_MaxTime = 4f;
        currentTime = flt_MaxTime;

        for (int i = 0; i < trashPool.childCount; i++)
        {
            while(currentTime <= flt_MaxTime)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }

            trashPool.GetChild(i).gameObject.SetActive(true);
            currentTime = 0f;
        }

        CO_StartSpawningTrash = null;
    }
}
