using osu.Framework.Platform;

namespace GentrysQuest.Editor
{
    public partial class Editor : osu.Framework.Game
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();
            Add(new EditorScreen());
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            host.Window.Title = "Gentry's Quest Editor";
        }
    }
}
