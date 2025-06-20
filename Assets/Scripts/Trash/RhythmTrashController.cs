using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmTrashController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxSpd;
    [SerializeField] int trashType;
    public int GetTrashType() => trashType;
    public LayerMask trashesLayer;

    Renderer rend;
    TrashSpawnerGameObject spawnerGameObj;

    private void OnEnable()
    {
        rend = GetComponent<Renderer>();
        trashesLayer = LayerMask.GetMask("Trashes");
        if (!rend.material.name.EndsWith("(Instance)"))
        {
            rend.material = new Material(rend.material);
        }

        trashType = GetRandomTrashType(TrashTypes.TrashColor);
        if (spawnerGameObj != null) return;
        spawnerGameObj = FindObjectOfType<TrashSpawnerGameObject>();
        ChangeTypes();
        ChangePosition();
    }



    private int GetRandomTrashType(Dictionary<int, string> trashType)
    {
        if (trashType.Count < 1) return -1;
        else
        {
            int targetIndex = Random.Range(0, trashType.Count);
            while (targetIndex == this.trashType)
            {
                targetIndex = Random.Range(0, trashType.Count);
            }


            for (int i = 0; i < trashType.Count; i++)
            {
                if (i == targetIndex) return i;
            }
            return -1;
        }
    }



    private void ChangePosition()
    {
        if (spawnerGameObj == null) return;
        if (trashType < 0 || trashType > spawnerGameObj.GetTransform().childCount - 1) return;
        Vector3 newPosition = spawnerGameObj.GetTransform().GetChild(trashType).position;
        Vector3 halfExtents = transform.localScale / 2;

        float spacingBuffer = 0.5f;
        int maxTry = 5;

        for (int i = 0; i < maxTry; i++)
        {
            Collider[] overlaps = Physics.OverlapBox(newPosition, halfExtents, transform.rotation, trashesLayer);
            if (overlaps.Length < 1) { transform.position = newPosition; return; }
            
            float maxOverlapSize = 0f;
            foreach (Collider col in overlaps)
            {
                if (col == null) continue;
                Vector3 overlapScale = col.transform.localScale;
                maxOverlapSize = Mathf.Max(maxOverlapSize, Mathf.Max(overlapScale.x, overlapScale.z));
            }

            // Geser posisi
            float stepDistance = (Mathf.Max(transform.localScale.x, transform.localScale.z) + maxOverlapSize) / 2f + spacingBuffer;
            newPosition += -Vector3.right * stepDistance;
        }

        transform.position = newPosition;
    }



    private void ChangeTypes()
    {
        Color color;

        trashType = GetRandomTrashType(TrashTypes.TrashColor);
        if (!TrashTypes.TrashColor.TryGetValue(trashType, out string value)) return;
        if (ColorUtility.TryParseHtmlString(value, out color))
        {
            rend.material.color = color;
        }
    }


    public void SortTrash(bool isCorrect)
    {
        ChangeTypes();
        ChangePosition();
    }


    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x > 3.35f)
        {
            if (trashType != 6) { GameManager.instance.ChangeWrongTrashValue(1); }

            ChangeTypes();
            ChangePosition();
        }
        if (GameManager.instance.GetStartSpawningTrashCO() == null) 
        { 
            if (speed < maxSpd) speed += Time.deltaTime * 0.07f;
        }
        else 
        { 
            speed = 2; 
        }
    }
}
