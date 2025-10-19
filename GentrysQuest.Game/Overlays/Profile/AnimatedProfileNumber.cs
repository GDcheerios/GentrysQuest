using GentrysQuest.Game.Graphics.UserInterface;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Overlays.Profile;

public partial class AnimatedProfileNumber : AnimatedNumberText
{
    [BackgroundDependencyLoader]
    private void load() => Font = FontUsage.Default.With(size: 32);
}
