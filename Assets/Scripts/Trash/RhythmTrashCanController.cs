using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmTrashCanController : MonoBehaviour
{
    InputManager inputManager;
    TrashTransparentLineController trashTransparentLineController;

    Renderer rend;
    [SerializeField] KeyCode assignedKey;
    [SerializeField] int trashType;
    [SerializeField] LayerMask trashLayer;
    [SerializeField] TrashCanController acrossCan;


    private void OnEnable()
    {
        if (inputManager == null)
        {
            inputManager = FindFirstObjectByType<InputManager>();
        }

        inputManager.KeyDownEvent += KeyDownEventReceiver;
        trashLayer = LayerMask.GetMask("Trashes");
        if (trashTransparentLineController == null) trashTransparentLineController = GetComponentInChildren<TrashTransparentLineController>(true);
        InitializeTrashCanType();
    }

    private void OnDisable()
    {
        inputManager.KeyDownEvent -= KeyDownEventReceiver;
    }



    private void KeyDownEventReceiver(object sender, InputManager.KeyDownEventArgs e)
    {
        if (e.pressedKey == assignedKey) { DetectTrash(); }
    }


    Coroutine CO_IsAttacking;
    public Coroutine GetIsAttackingCoroutine() => CO_IsAttacking;
    IEnumerator IsAttacking()
    {
        var flt_Count = 0f;
        var flt_MaxTime = 0.2f;

        while (flt_Count <= flt_MaxTime)
        {
            flt_Count += Time.deltaTime;
            yield return null;
        }

        CO_IsAttacking = null;
    }
    private void DetectTrash()
    {
        if (CO_IsAttacking != null) return;

        Vector3 halfExtents = transform.localScale / 2;
        Collider[] overlapTrash = Physics.OverlapBox(transform.position, halfExtents, transform.rotation, trashLayer);
        RhythmTrashController falseTrashFallback = null;

        foreach (Collider trash in overlapTrash)
        {
            if (trash == null) continue;
            RhythmTrashController rtc = trash.TryGetComponent(out RhythmTrashController r) ? r : null;
            if (rtc == null) continue;
            if (rtc.GetTrashType() == this.trashType) { GameManager.instance.OnSuccessSorting(1); rtc.SortTrash(true); return; }
            else { falseTrashFallback = rtc; }
        }

        if (falseTrashFallback != null)
        {
            GameManager.instance.ChangeWrongTrashValue(1);
            falseTrashFallback.SortTrash(false);
            falseTrashFallback = null;
        }
    }


    private void InitializeMaterial()
    {
        if (rend == null) rend = GetComponent<Renderer>();
        if (!rend.material.name.EndsWith("(Instance)"))
        {
            rend.material = new Material(rend.material);
        }
    }


    private void InitializeTrashCanType()
    {
        InitializeMaterial();

        Color color;
        if (!TrashTypes.TrashColor.TryGetValue(trashType, out string value)) return;
        if (ColorUtility.TryParseHtmlString(value, out color))
        {
            rend.material.color = color;
            //trashTransparentLineController.InitializeMaterial(value);
        }
    }
}
