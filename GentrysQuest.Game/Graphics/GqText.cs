using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Graphics;

public partial class GqText : SpriteText
{
    public GqText(string text)
    {
        Text = text;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
    }
}
