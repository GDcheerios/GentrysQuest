using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Content.Artifacts;

public class EmptyGachaContainer : Artifact
{
    public EmptyGachaContainer()
    {
        Initialize(MathBase.RandomInt(5));
    }

    public EmptyGachaContainer(int starRating)
    {
        Initialize(starRating);
    }

    private void createMetaData()
    {
        Name = "Empty Gacha Container";
        Description = "An empty gacha container. It looks like there was something in here.";
    }
}
