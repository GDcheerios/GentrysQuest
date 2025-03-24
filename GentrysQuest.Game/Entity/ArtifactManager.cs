using System;
using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Content;

namespace GentrysQuest.Game.Entity
{
    public class ArtifactManager(Character parent)
    {
        private Artifact[] artifacts = new Artifact[5];
        private float averageRating;
        public Action OnChangeArtifact;

        private void findAverageRating()
        {
            int count = artifacts.Where(artifact => artifact != null).Sum(artifact => artifact.StarRating.Value);
            averageRating = count / GetArtifactCount();
        }

        public int GetArtifactCount() => artifacts.Count(artifact => artifact != null);

        public Artifact Get(int index) => artifacts[index];

        public Artifact[] Get() => artifacts;

        public void Equip(Artifact artifact, int index)
        {
            artifacts[index] = artifact;
            artifact.Holder = parent;
            findAverageRating();
            OnChangeArtifact?.Invoke();
            UpdateStats();
        }

        public void UpdateStats()
        {
            Dictionary<string, int> familyCount = new Dictionary<string, int>();

            foreach (Artifact artifact in artifacts)
            {
                if (artifact == null) continue;

                if (!familyCount.TryAdd(artifact.family.Name, 1)) familyCount[artifact.family.Name]++;
            }

            foreach (string name in familyCount.Keys)
            {
                ContentManager.GetFamily(name).FourSetBuff?.RemoveFromCharacter(parent);
                if (familyCount[name] >= 2) ContentManager.GetFamily(name).TwoSetBuff?.ApplyToCharacter(parent);
                if (familyCount[name] >= 4) ContentManager.GetFamily(name).FourSetBuff?.ApplyToCharacter(parent);
                if (familyCount[name] == 5) parent.Stats.Boost((int)averageRating);
            }
        }

        public void Clear() => artifacts = new Artifact[5];

        public Artifact Remove(int index)
        {
            Artifact artifact = artifacts[index];
            artifacts[index].Holder = null;
            artifacts[index] = null;
            OnChangeArtifact?.Invoke();
            UpdateStats();
            return artifact;
        }
    }
}
