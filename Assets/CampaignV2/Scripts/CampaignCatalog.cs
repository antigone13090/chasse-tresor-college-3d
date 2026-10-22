using System;
using UnityEngine;

namespace CollegeTreasure.Campaign
{
    public static class CampaignCatalog
    {
        static CampaignLevelDefinition[] cached;

        public static CampaignLevelDefinition[] All
        {
            get
            {
                if (cached == null)
                {
                    cached = Resources.LoadAll<CampaignLevelDefinition>("CampaignLevels");
                    Array.Sort(cached, (left, right) =>
                        left.chapterNumber.CompareTo(right.chapterNumber));
                }

                return cached;
            }
        }

        public static CampaignLevelDefinition FindById(string levelId)
        {
            foreach (CampaignLevelDefinition definition in All)
            {
                if (definition.levelId == levelId)
                    return definition;
            }

            return null;
        }

        public static CampaignLevelDefinition FindByChapter(int chapterNumber)
        {
            foreach (CampaignLevelDefinition definition in All)
            {
                if (definition.chapterNumber == chapterNumber)
                    return definition;
            }

            return null;
        }

        public static CampaignLevelDefinition NextAfter(CampaignLevelDefinition current)
        {
            return current == null ? null : FindByChapter(current.chapterNumber + 1);
        }

#if UNITY_EDITOR
        public static void ClearCacheForEditor()
        {
            cached = null;
        }
#endif
    }
}
