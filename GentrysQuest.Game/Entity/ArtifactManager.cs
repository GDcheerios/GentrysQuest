using System;
using System.Linq;

namespace GentrysQuest.Game.Entity
{
    public class ArtifactManager(Character parent)
    {
        private Artifact[] artifacts = new Artifact[5];
        public Action OnChangeArtifact;

        public int GetArtifactCount() => artifacts.Count(artifact => artifact != null);

        public Artifact Get(int index) => artifacts[index];

        public Artifact[] Get() => artifacts;

        public void Equip(Artifact artifact, int index)
        {
            artifacts[index] = artifact;
            artifact.Holder = parent;
            OnChangeArtifact?.Invoke();
        }

        public void Clear() => artifacts = new Artifact[5];

        public Artifact Remove(int index)
        {
            Artifact artifact = artifacts[index];
            artifacts[index].Holder = null;
            artifacts[index] = null;
            OnChangeArtifact?.Invoke();
            return artifact;
        }
    }
}
