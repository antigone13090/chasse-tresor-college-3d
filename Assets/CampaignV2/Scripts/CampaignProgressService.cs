using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CollegeTreasure.Campaign
{
    [Serializable]
    public sealed class CampaignCheckpoint
    {
        public string levelId;
        public int step;
        public string state;
    }

    [Serializable]
    public sealed class CampaignSaveData
    {
        public int schemaVersion = 1;
        public int unlockedThroughChapter = 2;
        public string lastPlayedLevelId = "cdi-codes";
        public List<string> completedLevelIds = new List<string>();
        public List<CampaignCheckpoint> checkpoints = new List<CampaignCheckpoint>();
    }

    public static class CampaignProgressService
    {
        const int CurrentSchemaVersion = 1;
        const string SaveFileName = "campaign-v2.json";

        static CampaignSaveData cached;
        static bool suppressDiskWrites;

        public static CampaignSaveData Data
        {
            get
            {
                if (cached == null)
                    cached = LoadFromDisk();

                return cached;
            }
        }

        public static string SavePath
        {
            get { return Path.Combine(Application.persistentDataPath, SaveFileName); }
        }

        public static bool IsUnlocked(int chapterNumber)
        {
            return chapterNumber >= 1 && chapterNumber <= 7 && chapterNumber <= Data.unlockedThroughChapter;
        }

        public static bool IsCompleted(string levelId)
        {
            return Data.completedLevelIds.Contains(levelId);
        }

        public static void MarkLevelStarted(string levelId)
        {
            Data.lastPlayedLevelId = levelId;
            SaveToDisk();
        }

        public static void MarkLevelCompleted(string levelId, int chapterNumber)
        {
            if (!Data.completedLevelIds.Contains(levelId))
                Data.completedLevelIds.Add(levelId);

            Data.unlockedThroughChapter = Mathf.Max(
                Data.unlockedThroughChapter,
                Mathf.Clamp(chapterNumber + 1, 2, 7));

            RemoveCheckpoint(levelId);
            SaveToDisk();
        }

        public static CampaignCheckpoint GetCheckpoint(string levelId)
        {
            for (int i = 0; i < Data.checkpoints.Count; i++)
            {
                if (Data.checkpoints[i].levelId == levelId)
                    return Data.checkpoints[i];
            }

            return null;
        }

        public static void SaveCheckpoint(string levelId, int step, string state)
        {
            CampaignCheckpoint checkpoint = GetCheckpoint(levelId);

            if (checkpoint == null)
            {
                checkpoint = new CampaignCheckpoint { levelId = levelId };
                Data.checkpoints.Add(checkpoint);
            }

            checkpoint.step = Mathf.Max(0, step);
            checkpoint.state = state ?? string.Empty;
            Data.lastPlayedLevelId = levelId;
            SaveToDisk();
        }

        public static void ResetLevel(string levelId)
        {
            RemoveCheckpoint(levelId);
            SaveToDisk();
        }

        public static void ResetCampaign()
        {
            cached = NewSave();
            SaveToDisk();
        }

#if UNITY_EDITOR
        public static void BeginEditorTestSession()
        {
            suppressDiskWrites = true;
            cached = NewSave();
            cached.unlockedThroughChapter = 7;
        }

        public static void EndEditorTestSession()
        {
            cached = null;
            suppressDiskWrites = false;
        }
#endif

        public static string SerializeForTests(CampaignSaveData data)
        {
            return JsonUtility.ToJson(data, true);
        }

        public static CampaignSaveData DeserializeForTests(string json)
        {
            return Normalize(JsonUtility.FromJson<CampaignSaveData>(json));
        }

        static CampaignSaveData LoadFromDisk()
        {
            return LoadFromPaths(SavePath, SavePath + ".bak");
        }

        static CampaignSaveData LoadFromPaths(string path, string backup)
        {

            CampaignSaveData loaded = TryRead(path);
            if (IsSupportedSave(loaded))
                return Normalize(loaded);

            loaded = TryRead(backup);
            if (IsSupportedSave(loaded))
                return Normalize(loaded);

            return NewSave();
        }

        static CampaignSaveData TryRead(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return null;

                return JsonUtility.FromJson<CampaignSaveData>(File.ReadAllText(path));
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Sauvegarde V2 illisible: " + exception.Message);
                return null;
            }
        }

#if UNITY_EDITOR
        public static CampaignSaveData LoadFromPathsForEditorTest(string primary, string backup)
        {
            return LoadFromPaths(primary, backup);
        }
#endif

        static bool IsSupportedSave(CampaignSaveData data)
        {
            return data != null && data.schemaVersion >= 1 &&
                data.schemaVersion <= CurrentSchemaVersion;
        }

        static CampaignSaveData Normalize(CampaignSaveData data)
        {
            if (data == null)
                return NewSave();

            if (data.schemaVersion > CurrentSchemaVersion || data.schemaVersion < 1)
                return NewSave();

            data.schemaVersion = CurrentSchemaVersion;
            data.unlockedThroughChapter = Mathf.Clamp(data.unlockedThroughChapter, 2, 7);
            data.completedLevelIds = data.completedLevelIds ?? new List<string>();
            data.checkpoints = data.checkpoints ?? new List<CampaignCheckpoint>();
            data.completedLevelIds.RemoveAll(id => string.IsNullOrWhiteSpace(id));
            data.completedLevelIds = new List<string>(new HashSet<string>(data.completedLevelIds));
            data.checkpoints.RemoveAll(item => item == null || string.IsNullOrWhiteSpace(item.levelId) || item.step < 0);

            if (string.IsNullOrWhiteSpace(data.lastPlayedLevelId))
                data.lastPlayedLevelId = "cdi-codes";

            return data;
        }

        static CampaignSaveData NewSave()
        {
            return new CampaignSaveData();
        }

        static void RemoveCheckpoint(string levelId)
        {
            Data.checkpoints.RemoveAll(item => item.levelId == levelId);
        }

        static void SaveToDisk()
        {
            if (suppressDiskWrites)
                return;

            try
            {
                string path = SavePath;
                string temp = path + ".tmp";
                string backup = path + ".bak";
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(temp, JsonUtility.ToJson(Data, true));

                if (File.Exists(path))
                {
                    // Preserve a known-good backup before swapping the current save.
                    // Never overwrite the backup with a corrupt or unsupported primary.
                    if (IsSupportedSave(TryRead(path)))
                        File.Copy(path, backup, true);
                    File.Replace(temp, path, null);
                }
                else
                {
                    File.Move(temp, path);
                }

            }
            catch (Exception exception)
            {
                Debug.LogWarning("Sauvegarde V2 impossible: " + exception.Message);
            }
        }
    }
}
