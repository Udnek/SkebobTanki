namespace UI.Slots
{
    public class AbilitySlot : AbstractSlot
    {
        public override Type type => Type.ABILITY;
        public override bool isEmpty => true;

        public override void UpdateIcon()
        {
        }
    }
}