using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts;
using UnityEngine;

public class NpcRoad : MonoBehaviour
{
    [SerializeField] private List<NpcRoadSlot> slots;

    public bool IsRoadFull()
    {
        return !slots.First().IsSlotAvailable();
    }

    public NpcRoadSlot GetAvailableSlot()
    {
        return slots.Last(x => x.IsSlotAvailable());
        // for (int i = slots.Count - 1; i  >= 0; i--)
        // {
        //     if (slots[i].IsSlotAvailable())
        //     {
        //         return slots[i];
        //     }
        // }
        //
        // return null;
    }
}
