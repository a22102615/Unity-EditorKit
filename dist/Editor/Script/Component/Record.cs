using System;

namespace Henry.EditorKit
{
    [Serializable]
    public class Record
    {
        public string compTypeFullName;
        public string compContent;

        public Record(string compTypeFullName)
        {
            this.compTypeFullName = compTypeFullName;
        }
    }
}