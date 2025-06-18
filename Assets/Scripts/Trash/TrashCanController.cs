using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanController : MonoBehaviour
{
    InputManager inputManager;

    Renderer rend;
    [SerializeField] KeyCode assignedKey;
    [SerializeField] int trashType;
    [SerializeField] LayerMask trashLayer;


    private void OnEnable()
    {
        if (inputManager == null)
        {
            inputManager = FindFirstObjectByType<InputManager>();
        }

        inputManager.KeyDownEvent += KeyDownEventReceiver;
        trashLayer = LayerMask.GetMask("Trashes");
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



    private void DetectTrash()
    {
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


        if (boxCasts < 1) return;
        for (int i = 0; i < boxCasts; i++)
        {
            if (hits[i].collider == null) continue;
            Transform hitTransform = hits[i].transform;
            Debug.Log(i);
            TrashController trashController = hitTransform.TryGetComponent(out TrashController t) ? t : null;
            if (trashController == null) continue;
            if (trashController.GetTrashType() == this.trashType) { trashController.SortTrash(); return; }
        }

        Debug.Log("No trash detected!");
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
        }
    }


}
