using System;

namespace Henry.EditorKit.Component
{
    [Serializable]
    public class Config
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string ReadmePath { get; set; } = string.Empty;

        public Config(string name)
        {
            Name = name;
        }
    }
}