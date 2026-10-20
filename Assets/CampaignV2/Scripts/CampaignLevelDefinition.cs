using UnityEngine;

namespace CollegeTreasure.Campaign
{
    public enum CampaignPuzzleKind
    {
        OriginalAdventure,
        HiddenCodes,
        LogicLab,
        SoundSequence,
        OrientationCourse,
        ArtPatterns,
        SecretCollege
    }

    [CreateAssetMenu(
        fileName = "CampaignLevel",
        menuName = "College Treasure/Campaign Level")]
    public sealed class CampaignLevelDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string levelId;
        [Min(1)] public int chapterNumber = 1;
        public string sceneName;
        public string title;
        [TextArea] public string description;
        public CampaignPuzzleKind puzzleKind;

        [Header("Progression")]
        [TextArea] public string initialObjective;
        [TextArea] public string completionMessage;
        [TextArea] public string[] hints;
        public int[] primarySequence;
        public bool[] switchSolution;
        public string[] labels;
        public bool optionalTimer;

        [Header("Presentation")]
        public Color primaryColor = new Color(0.15f, 0.45f, 0.75f);
        public Color accentColor = new Color(1f, 0.72f, 0.18f);
        public AudioClip ambience;
        public AudioClip interactionSound;
        public AudioClip successSound;

        public bool IsOriginalAdventure
        {
            get { return puzzleKind == CampaignPuzzleKind.OriginalAdventure; }
        }

        public bool IsValid(out string error)
        {
            if (string.IsNullOrWhiteSpace(levelId))
            {
                error = "Identifiant de niveau manquant.";
                return false;
            }

            if (chapterNumber < 1 || string.IsNullOrWhiteSpace(sceneName))
            {
                error = "Chapitre ou scene invalide pour " + levelId + ".";
                return false;
            }

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(initialObjective))
            {
                error = "Titre ou objectif manquant pour " + levelId + ".";
                return false;
            }

            if (!IsOriginalAdventure && (hints == null || hints.Length < 2))
            {
                error = "Au moins deux aides sont requises pour " + levelId + ".";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
