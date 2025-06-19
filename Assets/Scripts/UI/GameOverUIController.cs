using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] GameObject overallUI;


    private void OnEnable()
    {
        UIManager.instance.ActivateGameOverEvent += ActivateGameOverEventReceiver;
    }
    private void OnDisable()
    {
        UIManager.instance.ActivateGameOverEvent -= ActivateGameOverEventReceiver;
    }


    private void ActivateGameOverEventReceiver(bool obj)
    {
        overallUI.SetActive(obj);
    }
}
