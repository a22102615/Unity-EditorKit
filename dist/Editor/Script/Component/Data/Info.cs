using System;

namespace Henry.EditorKit.Component
{
    [Serializable]
    public class Info
    {
        Config config;
        string typeFullName;
        Type componentType;

        public Config Config => config;

        public string TypeFullName => typeFullName;

        public Type ComponentType
        {
            get
            {
                if (componentType == null)
                {
                    componentType = Type.GetType(TypeFullName);
                }
                return componentType;
            }
        }

        public Info(Type type, Config config)
        {
            componentType = type;
            this.config = config;
            typeFullName = type.FullName;
        }
    }
}