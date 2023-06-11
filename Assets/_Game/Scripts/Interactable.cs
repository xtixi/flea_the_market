using HighlightPlus;

namespace _Game.Scripts
{
    public interface IInteractable
    {
        public void OnMouseDown();
        public void OnInteraction();
        public void UnInteraction();
    }
}