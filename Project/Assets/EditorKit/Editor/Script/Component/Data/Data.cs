using System;
using UnityEngine;

namespace Henry.EditorKit.Component
{
    [Serializable]
    public class Data
    {
        [SerializeField] ScriptableObject target;
        [SerializeField] Info info;
        [SerializeField] Record record;
        [SerializeField] string guid;

        IComponent component;

        public IComponent Component
        {
            get
            {
                if (component == null)
                {
                    component = target as IComponent;
                }
                return component;
            }
        }

        public ScriptableObject TargetSO => target;

        public Info Info => info;

        public Record Record => record;

        public string ID => guid;

        public Data(ScriptableObject target, Info info, Record record)
        {
            this.target = target;
            this.info = info;
            this.record = record;
            guid = Guid.NewGuid().ToString();
        }

        public void SetInfo(Info info)
        {
            this.info = info;
        }
    }
}