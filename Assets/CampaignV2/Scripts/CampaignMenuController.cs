using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CollegeTreasure.Campaign
{
    public sealed class CampaignMenuController : MonoBehaviour
    {
        CampaignLevelDefinition[] levels;
        Vector2 scroll;
        bool confirmReset;
        GUIStyle titleStyle;
        GUIStyle chapterStyle;
        GUIStyle bodyStyle;
        GUIStyle buttonStyle;
        Texture2D white;

        void Start()
        {
            levels = CampaignCatalog.All;
            white = Texture2D.whiteTexture;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Array.IndexOf(Environment.GetCommandLineArgs(), "-campaignSmokeTest") >= 0)
            {
                bool valid = levels.Length == 7 && SceneManager.sceneCountInBuildSettings == 8;
                Debug.Log(valid
                    ? "CAMPAIGN_V2_PLAYER_SMOKE_OK"
                    : "CAMPAIGN_V2_PLAYER_SMOKE_FAILED");
                Application.Quit(valid ? 0 : 2);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        void PrepareStyles()
        {
            if (titleStyle != null)
                return;

            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 32,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            titleStyle.normal.textColor = Color.white;

            chapterStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 19,
                fontStyle = FontStyle.Bold
            };
            chapterStyle.normal.textColor = Color.white;

            bodyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                wordWrap = true
            };
            bodyStyle.normal.textColor = new Color(0.84f, 0.89f, 0.94f);
            buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 16, fontStyle = FontStyle.Bold };
        }

        void OnGUI()
        {
            PrepareStyles();
            Color previous = GUI.color;
            GUI.color = new Color(0.018f, 0.032f, 0.052f, 1f);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), white);
            GUI.color = previous;

            GUI.Label(new Rect(20f, 16f, Screen.width - 40f, 50f),
                "CHASSE AU TRESOR DU COLLEGE — CAMPAGNE V2", titleStyle);
            GUI.Label(new Rect(20f, 62f, Screen.width - 40f, 28f),
                "L'aventure originale est conservee. Les chapitres 2 a 7 sont six niveaux autonomes.", bodyStyle);

            Rect view = new Rect(35f, 104f, Screen.width - 70f, Screen.height - 175f);
            Rect content = new Rect(0f, 0f, view.width - 20f, Mathf.Max(view.height, levels.Length * 112f));
            scroll = GUI.BeginScrollView(view, scroll, content);

            for (int i = 0; i < levels.Length; i++)
                DrawLevel(levels[i], new Rect(5f, i * 112f, content.width - 10f, 100f));

            GUI.EndScrollView();

            if (!confirmReset)
            {
                if (GUI.Button(new Rect(35f, Screen.height - 56f, 230f, 38f), "EFFACER LA PROGRESSION", buttonStyle))
                    confirmReset = true;
            }
            else
            {
                GUI.Label(new Rect(35f, Screen.height - 60f, 260f, 40f), "Confirmer la remise a zero ?", bodyStyle);
                if (GUI.Button(new Rect(290f, Screen.height - 58f, 90f, 38f), "OUI", buttonStyle))
                {
                    CampaignProgressService.ResetCampaign();
                    confirmReset = false;
                }
                if (GUI.Button(new Rect(390f, Screen.height - 58f, 90f, 38f), "NON", buttonStyle))
                    confirmReset = false;
            }

            GUI.Label(new Rect(Screen.width - 310f, Screen.height - 53f, 275f, 32f),
                "F10 : quitter • sauvegarde automatique", bodyStyle);
        }

        void DrawLevel(CampaignLevelDefinition level, Rect rect)
        {
            bool unlocked = CampaignProgressService.IsUnlocked(level.chapterNumber);
            bool completed = CampaignProgressService.IsCompleted(level.levelId);
            Color previous = GUI.color;
            GUI.color = unlocked
                ? new Color(level.primaryColor.r * 0.34f, level.primaryColor.g * 0.34f, level.primaryColor.b * 0.34f, 0.96f)
                : new Color(0.08f, 0.08f, 0.09f, 0.92f);
            GUI.DrawTexture(rect, white);
            GUI.color = previous;
            GUI.Box(rect, GUIContent.none);

            string status = level.IsOriginalAdventure
                ? "AVENTURE ORIGINALE"
                : completed ? "TERMINE" : unlocked ? "DISPONIBLE" : "VERROUILLE";

            GUI.Label(new Rect(rect.x + 18f, rect.y + 10f, rect.width - 215f, 28f),
                "Chapitre " + level.chapterNumber + " — " + level.title + "  [" + status + "]", chapterStyle);
            GUI.Label(new Rect(rect.x + 18f, rect.y + 42f, rect.width - 215f, 48f), level.description, bodyStyle);

            GUI.enabled = unlocked;
            if (GUI.Button(new Rect(rect.xMax - 180f, rect.y + 25f, 155f, 50f),
                completed ? "REJOUER" : "JOUER", buttonStyle))
            {
                CampaignFlow.LoadLevel(level);
            }
            GUI.enabled = true;
        }
    }
}
