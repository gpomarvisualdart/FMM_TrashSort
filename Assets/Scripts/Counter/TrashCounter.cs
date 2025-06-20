using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : MonoBehaviour
{
    [SerializeField] int totalTrash = 0;
    public int GetTotalTrashCount() => totalTrash;
    [SerializeField] int currentTrashRow = 0;
    public int GetCurrentTrashRow() => currentTrashRow;
    [SerializeField] int currentComboMilestone = 0;
    public float GetCurrentComboMilestone() => currentComboMilestone;
    [SerializeField] int trashType;
    public int GetTrashType() => trashType;
    int GetRequiredNextMilestone() => 3 * (1 << currentComboMilestone);


    public void ChangeTrashCount(int i)
    {
        totalTrash += i;
        currentTrashRow += i;

        if (currentTrashRow >= GetRequiredNextMilestone())
        {
            currentComboMilestone++;
            currentTrashRow = 0;
        }
    }


    public void ComboBreak()
    {
        currentComboMilestone = 0;
        currentTrashRow = 0;
    }
}
