using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Input;

public partial class InputHandler : CompositeComponent
{
    private readonly List<InputEvent> keyDownEvents = new();
    private readonly List<InputEvent> keyUpEvents = new();

    public void AddKeyDownEvent(InputEvent action) => keyDownEvents.Add(action);
    public InputEvent GetKeyDownEvent(int index) => keyDownEvents[index];
    public InputEvent GetKeyDownEvent(string name) => keyDownEvents.Find(input => input.Name == name);
    public void RemoveKeyDownEvent(InputEvent action) => keyDownEvents.Remove(action);

    public void AddKeyUpEvent(InputEvent action) => keyUpEvents.Add(action);
    public InputEvent GetKeyUpEvent(int index) => keyUpEvents[index];
    public InputEvent GetKeyUpEvent(string name) => keyUpEvents.Find(input => input.Name == name);
    public void RemoveKeyUpEvent(InputEvent action) => keyUpEvents.Remove(action);

    [BackgroundDependencyLoader]
    private void load()
    {
        Name = "Input Handler";
        AlwaysPresent = true;
        RelativeSizeAxes = Axes.Both;
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        Logger.Log("Input Handler Loaded");
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        foreach (var keyDownEvent in keyDownEvents.Where(keyDownEvent => e.Key == keyDownEvent.Key && keyDownEvent.Enabled).ToList())
        {
            Logger.Log($"[{keyDownEvent.Category}](down) {keyDownEvent.Name}: {keyDownEvent.Key}");
            keyDownEvent.Action?.Invoke();
        }

        return base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyUpEvent e)
    {
        foreach (var keyUpEvent in keyUpEvents.Where(keyUpEvent => keyUpEvent.Key == e.Key && keyUpEvent.Enabled).ToList())
        {
            Logger.Log($"[{keyUpEvent.Category}](up) {keyUpEvent.Name}: {keyUpEvent.Key}");
            keyUpEvent.Action?.Invoke();
        }

        base.OnKeyUp(e);
    }
}
