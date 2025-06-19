using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Transform trashPool;

    [Header("PlayerStats")]
    [SerializeField] int maxWrongTrash;
    public int currentWrongTrash;
    [SerializeField] int currentCoins;


    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }


    private void OnEnable()
    {
        currentWrongTrash = maxWrongTrash;
        currentCoins = 0;
        UIManager.instance.ChangeCoins(currentCoins);
        ChangeWrongTrashUIValue(currentWrongTrash);
        if (CO_StartSpawningTrash != null) { StopCoroutine(CO_StartSpawningTrash); CO_StartSpawningTrash = null; }
        CO_StartSpawningTrash = StartCoroutine(StartSpawningTrash());
    }


    public void RestartScene()
    {
        SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
        PauseGame(false);
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


    public void OnSuccessSorting(int val)
    {
        currentCoins += val;
        UIManager.instance.ChangeCoins(currentCoins);
    }


    public void ChangeWrongTrashValue(int value)
    {
        currentWrongTrash -= value;
        ChangeWrongTrashUIValue(currentWrongTrash);
        if (currentWrongTrash < 1) { UIManager.instance.ActivateGameOverScreen(true); PauseGame(true); }
    }


    private void ChangeWrongTrashUIValue(int value)
    {
        UIManager.instance.ChangeMissedTrashValue(value);
    }


    bool isPaused = false;
    public void PauseGame(bool isActive)
    {
        if (isActive)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
           Time.timeScale = 1f;
           isPaused = false;
        }
    }

    private void Update()
    {
        //Debug.Log(Time.time);
    }
}
