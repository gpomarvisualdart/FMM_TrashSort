using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class TrashCanController : MonoBehaviour
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
        if (CO_IsAttacking != null || acrossCan.GetIsAttackingCoroutine() != null) return;
        CO_IsAttacking = StartCoroutine(IsAttacking());
        RaycastHit[] hits = new RaycastHit[5];
        int boxCasts = RotaryHeart.Lib.PhysicsExtension.Physics.BoxCastNonAlloc
            (
                transform.position,
                transform.localScale / 2,
                transform.forward,
                hits,
                transform.rotation,
                Mathf.Infinity,
                trashLayer,
                RotaryHeart.Lib.PhysicsExtension.PreviewCondition.None
            );

        if(trashTransparentLineController != null) trashTransparentLineController.transform.gameObject.SetActive(true);

        if (boxCasts < 1) return;
        TrashController fallbackFalseTrash = null;
        for (int i = 0; i < boxCasts; i++)
        {
            if (hits[i].collider == null) continue;
            Transform hitTransform = hits[i].transform;
            Debug.Log($"{i} is {hitTransform.name}");
            TrashController trashController = hitTransform.TryGetComponent(out TrashController t) ? t : null;
            if (trashController == null) continue;
            if (trashController.GetTrashType() == this.trashType) { GameManager.instance.OnSuccessSorting(1); trashController.SortTrash(true); fallbackFalseTrash = null; return; }
            else { fallbackFalseTrash = trashController; }
        }

        if (fallbackFalseTrash != null)
        {
            GameManager.instance.ChangeWrongTrashValue(1);
            fallbackFalseTrash.SortTrash(false);
            fallbackFalseTrash = null;
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
            trashTransparentLineController.InitializeMaterial(value);
        }
    }


}
