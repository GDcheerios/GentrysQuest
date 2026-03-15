using System;
using osuTK.Input;

namespace GentrysQuest.Game.Input
{
    public class InputEvent
    {
        public string Name { get; set; }
        public Key Key { get; set; }
        public string Category { get; set; }
        public Action Action { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
