namespace UI.Slots
{
    public interface SlotListener
    {
        public void OnSlotClicked(AbstractSlot abstractSlot);
        public void OnSlotHovered(AbstractSlot abstractSlot);
        public void OnSlotUnhovered(AbstractSlot abstractSlot);
    }
}