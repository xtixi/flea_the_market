using UnityEngine;

namespace _Game.Scripts
{
    
    public class NpcRoadSlot : MonoBehaviour
    {
        private enum NpcSlotStates
        {
            Empty,
            Full
        }

        // public bool isCheckoutSlot;
        [SerializeField] private NpcSlotStates npcSlotState;

        public bool IsSlotAvailable()
        {
            return npcSlotState is NpcSlotStates.Empty;
        }

        public void FillSlot()
        {
            npcSlotState = NpcSlotStates.Full;
        }
        public void EmptySlot()
        {
            npcSlotState = NpcSlotStates.Empty;
        }
    }
}