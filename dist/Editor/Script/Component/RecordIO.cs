using System.IO;

namespace Henry.EditorKit
{
    internal class RecordIO
    {
        const string dirPath = "UserSettings\\EditorKit\\";
        const string recordAssetName = "ComponentRecord.asset";

        private RecordIO() { }

        ///<summary>將紀錄寫回本地</summary>
        public static void Save(UnityEngine.Object record)
        {
            if (CheckIsRecordDirExist() is false)
            {
                CreateDir();
            }

            var obj = new[] { record };
            
            UnityEditorInternal.InternalEditorUtility.SaveToSerializedFileAndForget(obj, dirPath + recordAssetName, true);
        }

        ///<summary>載入本地紀錄</summary>
        public static UnityEngine.Object Load()
        {
            if (CheckIsRecordDirExist() is false)
            {
                CreateDir();
            }

            var recordSource = UnityEditorInternal.InternalEditorUtility.LoadSerializedFileAndForget(dirPath + recordAssetName);

            if (recordSource != null && recordSource.Length > 0)
            {
                return recordSource[0];
            }
            else return null;
        }

        static bool CheckIsRecordDirExist()
        {
            return Directory.Exists(dirPath);
        }

        static void CreateDir()
        {
            Directory.CreateDirectory(dirPath);
        }
    }
}