using System;
using System.Collections.Generic;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Entity
{
    public class Family
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        protected List<Type> Artifacts = new();
        public TwoSetBuff TwoSetBuff { get; protected set; } = new TwoSetBuff(new Buff(20, StatType.Health, true));
        public FourSetBuff FourSetBuff { get; protected set; }

        public Artifact GetArtifact()
        {
            var artifact = Artifacts[MathBase.RandomChoice(Artifacts.Count)];
            return (Artifact)Activator.CreateInstance(artifact);
        }

        // TODO: Improve family workings => (Brayden Messerschmidt)
        public Artifact GetArtifact(string name)
        {
            List<Artifact> artifacts = new();
            foreach (Type artifact in Artifacts) artifacts.Add((Artifact)Activator.CreateInstance(artifact));
            return artifacts.Find(artifact => artifact.Name == name);
        }

        public List<Type> GetArtifacts() => Artifacts;
    }
}
