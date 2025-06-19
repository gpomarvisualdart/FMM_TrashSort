using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WrongTrashUIController : MonoBehaviour
{
    [SerializeField] GameObject overallUI;
    [SerializeField] TextMeshProUGUI txt;

    private void OnEnable()
    {
        UIManager.instance.ChangeMissedTrashValueEvent += ChangeMissedTrashValueEventReceiver;
    }


    private void ChangeMissedTrashValueEventReceiver(int obj)
    {
        txt.text= obj.ToString();
    }
}
