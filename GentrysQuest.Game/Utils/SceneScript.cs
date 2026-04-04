using System.Collections.Generic;
using GentrysQuest.Game.Graphics.Dialogue;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osu.Framework.Threading;

namespace GentrysQuest.Game.Utils
{
    public class SceneScript
    {
        private List<SceneEvent> events = [];

        public SceneScript(List<SceneEvent> events) => this.events = events;
        public SceneScript() { }

        public void AddEvent(string name, SceneEvent sceneEvent)
        {
            Logger.Log($"Adding event {name} to scene script");
            events.Add(sceneEvent);
        }

        public List<SceneEvent> ReceieveEvents() => events;

        /// <summary>
        /// Start the script.
        /// </summary>
        /// <param name="container">to put elements inside</param>
        /// <param name="scheduler">to schedule events</param>
        public void Start(Container container, Scheduler scheduler)
        {
            double time = 0;

            foreach (SceneEvent sceneEvent in events)
            {
                time += sceneEvent.Delay;

                var time1 = time;
                scheduler.AddDelayed(() =>
                {
                    Logger.Log($"Event triggered at {time1}");

                    if (sceneEvent.DialogueEvent != null)
                    {
                        AutoDialogueBox dialogueBox = new AutoDialogueBox(
                            sceneEvent.DialogueEvent.Author,
                            sceneEvent.DialogueEvent.Text,
                            sceneEvent.DialogueEvent.Duration
                        );

                        container.Add(dialogueBox);
                        scheduler.AddDelayed(() =>
                        {
                            dialogueBox.FadeOut(150);
                        }, sceneEvent.Duration);
                    }

                    sceneEvent.Event?.DynamicInvoke();
                }, time);
                time += sceneEvent.Duration;
            }
        }
    }
}
