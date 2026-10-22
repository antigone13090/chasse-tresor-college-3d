using UnityEngine;
using UnityEngine.SceneManagement;

namespace CollegeTreasure.Campaign
{
    public sealed class CampaignLevelController : MonoBehaviour
    {
        CampaignLevelDefinition definition;
        CampaignPuzzleBase puzzle;
        CampaignPlayerInteractor interactor;
        CampaignFirstPersonController playerController;
        AudioSource audioSource;

        string objective;
        string progressText;
        string message;
        float messageUntil;
        int hintIndex;
        bool started;
        bool paused;
        bool completed;

        GUIStyle titleStyle;
        GUIStyle bodyStyle;
        GUIStyle buttonStyle;
        GUIStyle smallStyle;
        Texture2D white;

        public CampaignLevelDefinition Definition { get { return definition; } }
        public bool IsCompleted { get { return completed; } }

        public void Initialize(CampaignLevelDefinition levelDefinition)
        {
            definition = levelDefinition;
            objective = definition.initialObjective;
            progressText = "Progression : 0 %";
            white = Texture2D.whiteTexture;

            RenderSettings.ambientLight = Color.Lerp(Color.gray, definition.primaryColor, 0.25f);
            RenderSettings.fog = true;
            RenderSettings.fogColor = definition.primaryColor * 0.16f;
            RenderSettings.fogDensity = 0.007f;

            GameObject environment = new GameObject("Environnement_" + definition.levelId);
            CampaignWorldBuilder world = new CampaignWorldBuilder(environment.transform, definition);
            GameObject player = CampaignWorldBuilder.CreatePlayer(new Vector3(0f, 0.1f, -8f));
            interactor = player.GetComponent<CampaignPlayerInteractor>();
            playerController = player.GetComponent<CampaignFirstPersonController>();

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 0f;
            audioSource.volume = 0.45f;

            if (definition.ambience != null)
            {
                AudioSource ambience = gameObject.AddComponent<AudioSource>();
                ambience.clip = definition.ambience;
                ambience.loop = true;
                ambience.volume = 0.16f;
                ambience.Play();
            }

            puzzle = CreatePuzzle(definition.puzzleKind);
            CampaignCheckpoint checkpoint = CampaignProgressService.GetCheckpoint(definition.levelId);
            puzzle.Initialize(this, definition, world, checkpoint);

            started = false;
            paused = false;
            Time.timeScale = 0f;
            SetPlayerEnabled(false);
            UnlockCursor();

            if (checkpoint != null && checkpoint.step > 0)
                ShowMessage("Point de reprise charge automatiquement.", 5f);
        }

        void Update()
        {
            if (!started || paused || completed)
                return;

            puzzle.Tick(Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.F1))
                ShowNextHint();

            if (Input.GetKeyDown(KeyCode.Escape))
                Pause();
        }

        void OnApplicationPause(bool applicationPaused)
        {
            if (applicationPaused && puzzle != null && !completed)
                puzzle.SaveCheckpoint();
        }

        CampaignPuzzleBase CreatePuzzle(CampaignPuzzleKind kind)
        {
            switch (kind)
            {
                case CampaignPuzzleKind.HiddenCodes: return new HiddenCodesPuzzle();
                case CampaignPuzzleKind.LogicLab: return new LogicLabPuzzle();
                case CampaignPuzzleKind.SoundSequence: return new SoundSequencePuzzle();
                case CampaignPuzzleKind.OrientationCourse: return new OrientationCoursePuzzle();
                case CampaignPuzzleKind.ArtPatterns: return new ArtPatternsPuzzle();
                case CampaignPuzzleKind.SecretCollege: return new SecretCollegePuzzle();
                default: throw new System.InvalidOperationException("Type d'enigme non jouable: " + kind);
            }
        }

        public void SetObjective(string value)
        {
            objective = value;
        }

        public void SetProgress(string value)
        {
            progressText = value;
        }

        public void ShowMessage(string value, float duration = 4f)
        {
            message = value;
            messageUntil = Time.unscaledTime + duration;
        }

        public void PlayInteractionSound(float pitch = 1f)
        {
            if (definition.interactionSound == null)
                return;

            audioSource.pitch = pitch;
            audioSource.PlayOneShot(definition.interactionSound);
        }

        public void SaveCheckpoint(int step, string state)
        {
            if (!completed)
                CampaignProgressService.SaveCheckpoint(definition.levelId, step, state);
        }

        public void CompleteLevel()
        {
            if (completed)
                return;

            completed = true;
            CampaignProgressService.MarkLevelCompleted(definition.levelId, definition.chapterNumber);
            objective = "Niveau termine";
            progressText = "Progression : 100 %";
            message = definition.completionMessage;
            messageUntil = float.PositiveInfinity;
            SetPlayerEnabled(false);
            UnlockCursor();

            if (definition.successSound != null)
                audioSource.PlayOneShot(definition.successSound);
        }

        void ShowNextHint()
        {
            if (definition.hints == null || definition.hints.Length == 0)
                return;

            ShowMessage("Aide " + (hintIndex + 1) + " : " + definition.hints[hintIndex], 8f);
            hintIndex = Mathf.Min(hintIndex + 1, definition.hints.Length - 1);
        }

        void StartLevel()
        {
            started = true;
            paused = false;
            Time.timeScale = 1f;
            SetPlayerEnabled(true);
            LockCursor();
        }

        void Pause()
        {
            paused = true;
            Time.timeScale = 0f;
            puzzle.SaveCheckpoint();
            SetPlayerEnabled(false);
            UnlockCursor();
        }

        void Resume()
        {
            paused = false;
            Time.timeScale = 1f;
            SetPlayerEnabled(true);
            LockCursor();
        }

        void RestartLevel()
        {
            CampaignProgressService.ResetLevel(definition.levelId);
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void SetPlayerEnabled(bool value)
        {
            if (playerController != null) playerController.enabled = value;
            if (interactor != null) interactor.enabled = value;
        }

        static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        static void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void PrepareStyles()
        {
            if (titleStyle != null)
                return;

            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 28,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            titleStyle.normal.textColor = Color.white;

            bodyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 17,
                wordWrap = true,
                alignment = TextAnchor.MiddleLeft
            };
            bodyStyle.normal.textColor = Color.white;

            smallStyle = new GUIStyle(bodyStyle) { fontSize = 14 };
            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 17,
                fontStyle = FontStyle.Bold
            };
        }

        void OnGUI()
        {
            PrepareStyles();

            if (!started)
            {
                DrawIntro();
                return;
            }

            DrawHud();

            if (paused)
                DrawPause();

            if (completed)
                DrawCompletion();
        }

        void DrawIntro()
        {
            DrawOverlay(new Color(0.015f, 0.025f, 0.04f, 0.96f));
            Rect panel = CenteredRect(700f, 440f);
            DrawPanel(panel);
            GUI.Label(new Rect(panel.x + 25f, panel.y + 28f, panel.width - 50f, 55f),
                "CHAPITRE " + definition.chapterNumber + " — " + definition.title.ToUpperInvariant(), titleStyle);
            GUI.Label(new Rect(panel.x + 55f, panel.y + 100f, panel.width - 110f, 115f),
                definition.description, bodyStyle);
            GUI.Label(new Rect(panel.x + 55f, panel.y + 220f, panel.width - 110f, 70f),
                "Objectif : " + definition.initialObjective, bodyStyle);
            GUI.Label(new Rect(panel.x + 55f, panel.y + 292f, panel.width - 110f, 45f),
                "ZQSD/WASD : bouger • Souris/fleches : regarder • E/Entree : agir • Tab : cibler • H/F1 : aide", smallStyle);
            if (GUI.Button(new Rect(panel.x + 225f, panel.y + 355f, 250f, 54f), "COMMENCER", buttonStyle))
                StartLevel();
        }

        void DrawHud()
        {
            Rect panel = new Rect(18f, 18f, Mathf.Min(610f, Screen.width - 36f), 122f);
            DrawPanel(panel);
            GUI.Label(new Rect(panel.x + 16f, panel.y + 8f, panel.width - 32f, 30f),
                "Chapitre " + definition.chapterNumber + " — " + definition.title, bodyStyle);
            GUI.Label(new Rect(panel.x + 16f, panel.y + 40f, panel.width - 32f, 44f),
                "Objectif : " + objective, smallStyle);
            GUI.Label(new Rect(panel.x + 16f, panel.y + 88f, panel.width - 32f, 22f),
                progressText + "     H/F1 : aide     Echap : pause", smallStyle);

            string prompt = interactor == null ? string.Empty : interactor.CurrentPrompt;
            if (!string.IsNullOrEmpty(prompt) && !paused && !completed)
                GUI.Box(new Rect(Screen.width * 0.5f - 190f, Screen.height - 92f, 380f, 44f), prompt);

            if (!string.IsNullOrEmpty(message) && Time.unscaledTime < messageUntil)
                GUI.Box(new Rect(Screen.width * 0.5f - 320f, Screen.height - 165f, 640f, 62f), message);

            if (!paused && !completed)
                GUI.Label(new Rect(Screen.width * 0.5f - 10f, Screen.height * 0.5f - 15f, 20f, 30f), "+", bodyStyle);
        }

        void DrawPause()
        {
            DrawOverlay(new Color(0f, 0f, 0f, 0.72f));
            Rect panel = CenteredRect(460f, 390f);
            DrawPanel(panel);
            GUI.Label(new Rect(panel.x, panel.y + 25f, panel.width, 50f), "PAUSE", titleStyle);
            if (GUI.Button(new Rect(panel.x + 95f, panel.y + 95f, 270f, 48f), "REPRENDRE", buttonStyle)) Resume();
            if (GUI.Button(new Rect(panel.x + 95f, panel.y + 158f, 270f, 48f), "AFFICHER UNE AIDE", buttonStyle)) ShowNextHint();
            if (GUI.Button(new Rect(panel.x + 95f, panel.y + 221f, 270f, 48f), "REINITIALISER LE NIVEAU", buttonStyle)) RestartLevel();
            if (GUI.Button(new Rect(panel.x + 95f, panel.y + 284f, 270f, 48f), "MENU DES CHAPITRES", buttonStyle)) CampaignFlow.LoadMenu();
        }

        void DrawCompletion()
        {
            DrawOverlay(new Color(0.02f, 0.025f, 0.015f, 0.80f));
            Rect panel = CenteredRect(620f, 380f);
            DrawPanel(panel);
            GUI.Label(new Rect(panel.x, panel.y + 28f, panel.width, 52f), "NIVEAU REUSSI !", titleStyle);
            GUI.Label(new Rect(panel.x + 45f, panel.y + 95f, panel.width - 90f, 100f),
                definition.completionMessage, bodyStyle);
            CampaignLevelDefinition next = CampaignCatalog.NextAfter(definition);
            string nextLabel = next == null ? "RETOUR AU MENU" : "NIVEAU SUIVANT";
            if (GUI.Button(new Rect(panel.x + 175f, panel.y + 215f, 270f, 50f), nextLabel, buttonStyle))
                CampaignFlow.LoadNext(definition);
            if (GUI.Button(new Rect(panel.x + 175f, panel.y + 280f, 270f, 46f), "REJOUER", buttonStyle))
                RestartLevel();
            if (GUI.Button(new Rect(panel.x + 175f, panel.y + 336f, 270f, 34f), "MENU DES CHAPITRES", buttonStyle))
                CampaignFlow.LoadMenu();
        }

        void DrawOverlay(Color color)
        {
            Color previous = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), white);
            GUI.color = previous;
        }

        void DrawPanel(Rect rect)
        {
            Color previous = GUI.color;
            GUI.color = new Color(0.035f, 0.055f, 0.075f, 0.96f);
            GUI.DrawTexture(rect, white);
            GUI.color = previous;
            GUI.Box(rect, GUIContent.none);
        }

        static Rect CenteredRect(float width, float height)
        {
            width = Mathf.Min(width, Screen.width - 30f);
            height = Mathf.Min(height, Screen.height - 30f);
            return new Rect((Screen.width - width) * 0.5f, (Screen.height - height) * 0.5f, width, height);
        }
    }
}
