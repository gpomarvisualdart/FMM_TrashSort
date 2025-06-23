using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int trashType;
    public int GetTrashType() => trashType;
    [SerializeField] List<GameObject> trashTypePool = new List<GameObject>();

    Renderer rend;
    TrashSpawnerGameObject spawnerGameObj;
    LayerMask trashLayer;
    Collider myCollider;

    private void OnEnable()
    {
        rend = GetComponent<Renderer>();
        myCollider = GetComponent<Collider>();
        trashLayer = LayerMask.GetMask("Trashes");

        if (!rend.material.name.EndsWith("(Instance)"))
        {
            rend.material = new Material(rend.material);
        }

        trashType = GetRandomTrashType(TrashTypes.TrashColor);
        if (spawnerGameObj != null) return;
        spawnerGameObj = FindObjectOfType<TrashSpawnerGameObject>();
        ChangePosition();
        ChangeTypes();
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

        Vector3 newPos = spawnerGameObj.GetTransform().GetChild(Random.Range(0, spawnerGameObj.GetTransform().childCount)).position;
        Vector3 colliderHalfExtents = myCollider.bounds.size / 2;
        int maxTries = 5;
        float spaceBuffer = 1.2f;

        for (int i = 0; i < maxTries; i++)
        {
            Collider[] overlapColliders = new Collider[7];
            int hits = RotaryHeart.Lib.PhysicsExtension.Physics.OverlapBoxNonAlloc(newPos, colliderHalfExtents, overlapColliders, transform.rotation, trashLayer);

            if (hits < 1) { transform.position = newPos; return; }

            float maxOverlapSizes = 0f;
            foreach (Collider col in overlapColliders)
            {
                if (col == null) continue;
                Vector3 collidedSize = col.bounds.size;
                maxOverlapSizes = Mathf.Max(maxOverlapSizes, Mathf.Max(collidedSize.x, collidedSize.z));
            }
            float stepDistance = (Mathf.Max(myCollider.bounds.size.x, myCollider.bounds.size.z)) + maxOverlapSizes / 2 + spaceBuffer;
            newPos += Vector3.right * stepDistance;
        }
        transform.position = newPos;
    }



    private void ChangeTypes()
    {
        /*Color color;

        trashType = GetRandomTrashType(TrashTypes.TrashColor);
        if (!TrashTypes.TrashColor.TryGetValue(trashType, out string value)) return;
        if (ColorUtility.TryParseHtmlString(value, out color))
        {
            rend.material.color = color;
        }*/
        if (trashTypePool.Count < 1) return;
        trashTypePool[trashType].SetActive(false);
        trashType = GetRandomTrashType(TrashTypes.TrashColor);
        while (trashType > trashTypePool.Count)
        {
            trashType = GetRandomTrashType(TrashTypes.TrashColor);
        }
         trashTypePool[trashType].SetActive(true);
    }


    public void SortTrash(bool isCorrect)
    {
        ChangePosition();
        ChangeTypes();
    }


    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        
        if (transform.position.x > 3.35f)
        {
            if(trashType != 6) { GameManager.instance.ChangeWrongTrashValue(1); }

            ChangePosition();
            ChangeTypes();

        }
    }
}
