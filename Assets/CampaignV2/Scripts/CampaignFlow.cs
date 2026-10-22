using UnityEngine;
using UnityEngine.SceneManagement;

namespace CollegeTreasure.Campaign
{
    public static class CampaignFlow
    {
        public const string MenuSceneName = "CampaignMenu";

        public static void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(MenuSceneName);
        }

        public static void LoadLevel(CampaignLevelDefinition definition)
        {
            if (definition == null || !CampaignProgressService.IsUnlocked(definition.chapterNumber))
                return;

            if (!definition.IsOriginalAdventure)
                CampaignProgressService.MarkLevelStarted(definition.levelId);

            Time.timeScale = 1f;
            SceneManager.LoadScene(definition.sceneName);
        }

        public static void LoadNext(CampaignLevelDefinition current)
        {
            CampaignLevelDefinition next = CampaignCatalog.NextAfter(current);
            if (next == null)
                LoadMenu();
            else
                LoadLevel(next);
        }
    }
}
