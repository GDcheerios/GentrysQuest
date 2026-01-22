using System.Collections.Generic;

namespace GentrysQuest.Game.Graphics
{
    /// <summary>
    /// Easy mapping for textures.
    /// Makes setting and getting textures easy.
    /// </summary>
    public class TextureMapping
    {
        private List<string> textureKeys = new();
        private List<string> textureValues = new();

        /// <summary>
        /// Adds a key and value.
        /// </summary>
        /// <param name="key"> The texture key. </param>
        /// <param name="value"> The texture value. </param>
        public void Add(string key, string value)
        {
            textureKeys.Add(key);
            textureValues.Add(value);
        }

        /// <summary>
        /// Removes the key and the cooresponding value.
        /// </summary>
        /// <param name="key"> The key. </param>
        public void Remove(string key)
        {
            for (int i = 0; i < textureKeys.Count; i++)
            {
                if (textureKeys[i] == key)
                {
                    textureKeys.RemoveAt(i);
                    textureValues.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Gets the corresponding value from the key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns>The texture file name</returns>
        public string Get(string key)
        {
            for (int i = 0; i < textureKeys.Count; i++)
            {
                if (key == textureKeys[i]) return textureValues[i];
            }

            return "huh.png";
        }
    }
}
