using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts
{
    
    public class NpcRoadSlot : MonoBehaviour
    {
        public enum SlotStates
        {
            Empty,
            Full
        }

        public bool isCheckoutSlot;
        [FormerlySerializedAs("npcSlotState")] [SerializeField] private SlotStates slotState;

        public bool IsSlotAvailable()
        {
            return slotState is SlotStates.Empty;
        }

        public void FillSlot()
        {
            slotState = SlotStates.Full;
        }
        public void EmptySlot()
        {
            slotState = SlotStates.Empty;
        }
    }
}