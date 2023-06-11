using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts;
using UnityEngine;

public class StorageSlots : MonoBehaviour
{
    [SerializeField] private List<StorageSlot> slots;

    public bool IsRoadFull()
    {
        return false;
        return !slots.First().IsSlotAvailable();
    }

    public StorageSlot GetAvailableSlot()
    {
        // return slots.FirstOrDefault(x => x.IsSlotAvailable());
        var  asd = slots.FirstOrDefault(x => x.IsSlotAvailable());
        if (asd == null)
        {
            asd = slots[Random.Range(0, slots.Count)];
        }

        return asd;
    }
}
