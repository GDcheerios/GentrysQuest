using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Artifacts;

public class EmptyGachaContainer : Artifact
{
    public EmptyGachaContainer()
    {
        Name = "Empty Gacha Container";
        Description = "An empty gacha container. It looks like there was something in here.";
        ValidStarRatings = [1];
    }
}
