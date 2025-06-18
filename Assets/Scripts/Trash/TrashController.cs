using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int trashType;
    public int GetTrashType() => trashType;

    Renderer rend;
    TrashSpawnerGameObject spawnerGameObj;

    private void OnEnable()
    {
        rend = GetComponent<Renderer>();

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

        Vector3 spawnerPos = spawnerGameObj.GetTransform().position;
        Vector3 spawnerScale = spawnerGameObj.GetTransform().localScale;

        Vector3 spawnableAreaHalf = (spawnerScale - transform.localScale) / 2f;

        Vector3 randomOffset = new Vector3
            (
            0f,
            0f,
            Random.Range(-spawnableAreaHalf.z, spawnableAreaHalf.z)
            );

        var v3_RepositionPos = new Vector3(spawnerPos.x, transform.position.y, spawnerPos.z) + randomOffset;

        transform.position = v3_RepositionPos;
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


    public void SortTrash()
    {
        ChangePosition();
        ChangeTypes();
    }


    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        
        if (transform.position.x > 3.35f)
        {
            ChangePosition();
            ChangeTypes();
        }
    }
}
