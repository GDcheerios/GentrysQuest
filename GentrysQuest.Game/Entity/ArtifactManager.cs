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

        /// <summary>
        /// Get the amount of artifacts of a certain name
        /// </summary>
        public int GetArtifactCountByName(string artifactName) => artifacts.Count(artifact => artifact != null && artifact.Name == artifactName);

        public void Equip(Artifact artifact, int index)
        {
            artifact.Holder = parent;
            if (GetArtifactCountByName(artifact.Name) == 0) artifact.OnEquip(parent);
            artifacts[index] = artifact;
            OnChangeArtifact?.Invoke();
        }

        public void Clear() => artifacts = new Artifact[5];

        public Artifact Remove(int index)
        {
            Artifact artifact = artifacts[index];
            artifacts[index].Holder = null;
            artifacts[index] = null;
            if (GetArtifactCountByName(artifact.Name) == 0) artifact.OnUnequip(parent);
            OnChangeArtifact?.Invoke();
            return artifact;
        }
    }
}
