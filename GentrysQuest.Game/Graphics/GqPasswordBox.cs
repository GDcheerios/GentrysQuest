using osu.Framework.Allocation;
using osu.Framework.Input;

namespace GentrysQuest.Game.Graphics
{
    public partial class GqPasswordBox : GqTextBox
    {
        protected override float LeftRightPadding => 10;
        protected override float CaretWidth => 5;

        public GqPasswordBox()
        {
            InputProperties = new TextInputProperties
            {
                Type = TextInputType.Password
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            PlaceholderText = "password";
        }
    }
}
