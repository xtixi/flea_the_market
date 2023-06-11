using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using Unity.Collections;
using UnityEngine;

public class StorageSlot : MonoBehaviour
{
    [SerializeField,ReadOnly] private NpcRoadSlot.SlotStates slotState;
    public bool IsSlotAvailable()
    {
        return slotState is NpcRoadSlot.SlotStates.Empty;
    }

    public void FillSlot()
    {
        slotState = NpcRoadSlot.SlotStates.Full;
    }
    public void EmptySlot()
    {
        slotState = NpcRoadSlot.SlotStates.Empty;
    }
}
