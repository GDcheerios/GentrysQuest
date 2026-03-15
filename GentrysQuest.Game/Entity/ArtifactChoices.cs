using System.Collections.Generic;
using System;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Entity
{
    public class ArtifactChoices
    {
        /// <summary>
        /// Can this drop multiple artifacts?
        /// </summary>
        public bool MultipleDrop = false;

        /// <summary>
        /// Is this a guaranteed drop?
        /// </summary>
        public bool GuaranteeDrop = false;

        private List<Artifact> artifacts = new();
        private List<int> chanceOfPicking = new();

        public void AddChoice(Artifact artifact, int chanceOfPicking)
        {
            artifacts.Add(artifact);
            this.chanceOfPicking.Add(chanceOfPicking);
        }

        public List<Artifact> GetChoice()
        {
            List<Artifact> result = [];

            while (true)
            {
                for (int i = 0; i < artifacts.Count; i++)
                {
                    if (MathBase.IsChanceSuccessful(chanceOfPicking[i], 100))
                    {
                        Artifact artifact = createArtifactInstance(artifacts[i]);
                        if (artifact == null) continue;
                        if (!MultipleDrop) return [artifact];

                        result.Add(artifact);
                    }
                }

                if (result.Count != 0 || !GuaranteeDrop) return result;
            }
        }

        private static Artifact createArtifactInstance(Artifact artifact)
        {
            if (artifact == null) return null;
            return Activator.CreateInstance(artifact.GetType()) as Artifact;
        }
    }
}
