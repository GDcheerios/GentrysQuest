// * Name              : GentrysQuest.Desktop
//  * Author           : Brayden J Messerschmidt
//  * Created          : 05/08/2024
//  * Course           : CIS 169 C#
//  * Version          : 1.0
//  * OS               : Windows 11 22H2
//  * IDE              : Jet Brains Rider 2023
//  * Copyright        : This is my work.
//  * Description      : desc.
//  * Academic Honesty : I attest that this is my original work.
//  * I have not used unauthorized source code, either modified or
//  * unmodified. I have not given other fellow student(s) access
//  * to my program.

using System.Reflection;
using GentrysQuest.Game;
using GentrysQuest.Game.Updating;
using osu.Framework.Platform;

namespace GentrysQuest.Desktop;

public partial class GentrysQuestDesktop : GentrysQuestGame
{
    protected override IGameUpdater CreateGameUpdater() => new DesktopGameUpdater();

    public override void SetHost(GameHost host)
    {
        base.SetHost(host);

        var iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetType(), "game.ico");
        if (iconStream != null)
            host.Window.SetIconFromStream(iconStream);

        host.Window.CursorState |= CursorState.Hidden;
        host.Window.Title = "Gentry's Quest";
    }
}
