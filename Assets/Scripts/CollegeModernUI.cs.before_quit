using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

public class CollegeModernUI : MonoBehaviour
{
    public bool showStartScreen = true;

    TreasureGameManager game;
    FPSController fps;

    bool paused = false;
    bool started = false;

    float volume = 1f;
    float sensitivity = 2f;

    GUIStyle titleStyle;
    GUIStyle subtitleStyle;
    GUIStyle buttonStyle;
    GUIStyle textStyle;
    GUIStyle smallStyle;
    GUIStyle counterStyle;

    Texture2D white;

    void Start()
    {
        white = Texture2D.whiteTexture;

        game =
            FindFirstObjectByType<
                TreasureGameManager
            >();

        fps =
            FindFirstObjectByType<
                FPSController
            >();

        volume =
            AudioListener.volume;

        if (fps != null)
            sensitivity =
                fps.mouseSensitivity;

        if (showStartScreen)
        {
            started = false;
            paused = false;

            Time.timeScale = 0f;

            if (fps != null)
                fps.enabled = false;

            UnlockCursor();
        }
        else
        {
            StartGame();
        }
    }

    void Update()
    {
        // F10 permet de quitter immédiatement le jeu,
        // même depuis l'écran d'accueil ou le menu Pause.
        if (Input.GetKeyDown(KeyCode.F10))
        {
            QuitGame();
            return;
        }

        if (!started)
            return;

        if (
            game != null
            &&
            game.victory
        )
        {
            if (fps != null)
                fps.enabled = false;

            UnlockCursor();

            return;
        }

        if (
            Input.GetKeyDown(
                KeyCode.Escape
            )
        )
        {
            if (paused)
                Resume();
            else
                Pause();
        }
    }

    void StartGame()
    {
        started = true;
        paused = false;

        Time.timeScale = 1f;

        if (fps != null)
            fps.enabled = true;

        LockCursor();
    }

    void Pause()
    {
        paused = true;

        Time.timeScale = 0f;

        if (fps != null)
            fps.enabled = false;

        UnlockCursor();
    }

    void Resume()
    {
        paused = false;

        Time.timeScale = 1f;

        if (fps != null)
            fps.enabled = true;

        LockCursor();
    }

    void Restart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager
                .GetActiveScene()
                .name
        );
    }

    void QuitGame()
    {
        Time.timeScale = 1f;

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        Debug.Log(
            "FERMETURE DU JEU"
        );

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void LockCursor()
    {
        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;
    }

    void PrepareStyles()
    {
        if (titleStyle != null)
            return;

        titleStyle =
            new GUIStyle(
                GUI.skin.label
            );

        titleStyle.fontSize = 34;
        titleStyle.fontStyle =
            FontStyle.Bold;

        titleStyle.alignment =
            TextAnchor.MiddleCenter;

        titleStyle.normal.textColor =
            Color.white;

        subtitleStyle =
            new GUIStyle(
                GUI.skin.label
            );

        subtitleStyle.fontSize = 18;
        subtitleStyle.alignment =
            TextAnchor.MiddleCenter;

        subtitleStyle.normal.textColor =
            new Color(
                .82f,
                .88f,
                .94f
            );

        buttonStyle =
            new GUIStyle(
                GUI.skin.button
            );

        buttonStyle.fontSize = 18;
        buttonStyle.fontStyle =
            FontStyle.Bold;

        textStyle =
            new GUIStyle(
                GUI.skin.label
            );

        textStyle.fontSize = 15;

        textStyle.normal.textColor =
            Color.white;

        textStyle.alignment =
            TextAnchor.MiddleLeft;

        smallStyle =
            new GUIStyle(
                GUI.skin.label
            );

        smallStyle.fontSize = 13;

        smallStyle.normal.textColor =
            new Color(
                .82f,
                .85f,
                .88f
            );

        counterStyle =
            new GUIStyle(
                GUI.skin.label
            );

        counterStyle.fontSize = 16;
        counterStyle.fontStyle =
            FontStyle.Bold;

        counterStyle.normal.textColor =
            Color.white;
    }

    void OnGUI()
    {
        PrepareStyles();

        if (!started)
        {
            DrawStartScreen();
            return;
        }

        DrawGameHUD();

        if (paused)
            DrawPauseMenu();

        if (
            game != null
            &&
            game.victory
        )
        {
            DrawVictoryOverlay();
        }
    }

    // =====================================================
    // ECRAN D'ACCUEIL
    // =====================================================

    void DrawStartScreen()
    {
        Fill(
            new Rect(
                0,
                0,
                Screen.width,
                Screen.height
            ),
            new Color(
                .025f,
                .045f,
                .065f,
                .94f
            )
        );

        float w =
            Mathf.Min(
                620f,
                Screen.width - 50f
            );

        float h = 500f;

        Rect panel =
            new Rect(
                (Screen.width-w)/2f,
                (Screen.height-h)/2f,
                w,
                h
            );

        Fill(
            panel,
            new Color(
                .055f,
                .075f,
                .095f,
                .98f
            )
        );

        Border(
            panel,
            new Color(
                .38f,
                .65f,
                .82f,
                .8f
            ),
            2f
        );

        GUI.Label(
            new Rect(
                panel.x + 20f,
                panel.y + 35f,
                panel.width - 40f,
                55f
            ),
            "CHASSE AU TRESOR",
            titleStyle
        );

        GUI.Label(
            new Rect(
                panel.x + 20f,
                panel.y + 90f,
                panel.width - 40f,
                35f
            ),
            "Explore le collège et retrouve les 6 indices.",
            subtitleStyle
        );

        Rect controls =
            new Rect(
                panel.x + 70f,
                panel.y + 155f,
                panel.width - 140f,
                175f
            );

        Fill(
            controls,
            new Color(
                .02f,
                .03f,
                .04f,
                .38f
            )
        );

        GUI.Label(
            new Rect(
                controls.x + 25f,
                controls.y + 14f,
                controls.width - 50f,
                30f
            ),
            "COMMANDES",
            counterStyle
        );

        GUI.Label(
            new Rect(
                controls.x + 25f,
                controls.y + 52f,
                controls.width - 50f,
                110f
            ),
            "Z / W       Avancer\n" +
            "S             Reculer\n" +
            "Q / A       Gauche\n" +
            "D             Droite\n" +
            "Souris       Regarder\n" +
            "E             Interagir / ouvrir\n" +
            "M             Mini-carte\n" +
            "Echap         Pause",
            smallStyle
        );

        if (
            GUI.Button(
                new Rect(
                    panel.x + 150f,
                    panel.y + 365f,
                    panel.width - 300f,
                    60f
                ),
                "COMMENCER",
                buttonStyle
            )
        )
        {
            StartGame();
        }

        GUI.Label(
            new Rect(
                panel.x + 20f,
                panel.y + 445f,
                panel.width - 40f,
                28f
            ),
            "Trouve les indices dans le bon ordre pour débloquer le trésor.   •   F10 : quitter",
            smallStyle
        );
    }

    // =====================================================
    // HUD EN JEU
    // =====================================================

    void DrawGameHUD()
    {
        if (game == null)
            return;

        int found =
            game.foundClues;

        int total =
            Mathf.Max(
                1,
                game.totalClues
            );

        float progress =
            Mathf.Clamp01(
                (float)found
                / total
            );

        Rect panel =
            new Rect(
                18f,
                18f,
                310f,
                88f
            );

        Fill(
            panel,
            new Color(
                .025f,
                .035f,
                .045f,
                .78f
            )
        );

        Border(
            panel,
            new Color(
                .34f,
                .61f,
                .78f,
                .70f
            ),
            1f
        );

        GUI.Label(
            new Rect(
                panel.x + 16f,
                panel.y + 10f,
                panel.width - 32f,
                28f
            ),
            "INDICES : "
            + found
            + " / "
            + total,
            counterStyle
        );

        Rect bg =
            new Rect(
                panel.x + 16f,
                panel.y + 48f,
                panel.width - 32f,
                16f
            );

        Fill(
            bg,
            new Color(
                .12f,
                .13f,
                .14f,
                .95f
            )
        );

        Rect progressRect =
            new Rect(
                bg.x,
                bg.y,
                bg.width
                    * progress,
                bg.height
            );

        Fill(
            progressRect,
            new Color(
                .20f,
                .72f,
                .34f,
                1f
            )
        );

        GUI.Label(
            new Rect(
                panel.x + 16f,
                panel.y + 66f,
                panel.width - 32f,
                18f
            ),
            GetObjective(),
            smallStyle
        );
    }

    // =====================================================
    // PAUSE
    // =====================================================

    void DrawPauseMenu()
    {
        Fill(
            new Rect(
                0,
                0,
                Screen.width,
                Screen.height
            ),
            new Color(
                0f,
                0f,
                0f,
                .68f
            )
        );

        float w = 470f;
        float h = 500f;

        Rect panel =
            new Rect(
                (Screen.width-w)/2f,
                (Screen.height-h)/2f,
                w,
                h
            );

        Fill(
            panel,
            new Color(
                .045f,
                .055f,
                .065f,
                .98f
            )
        );

        Border(
            panel,
            new Color(
                .35f,
                .62f,
                .82f,
                .85f
            ),
            2f
        );

        GUI.Label(
            new Rect(
                panel.x,
                panel.y + 25f,
                panel.width,
                50f
            ),
            "PAUSE",
            titleStyle
        );

        GUI.Label(
            new Rect(
                panel.x + 45f,
                panel.y + 100f,
                180f,
                25f
            ),
            "Volume général",
            textStyle
        );

        volume =
            GUI.HorizontalSlider(
                new Rect(
                    panel.x + 45f,
                    panel.y + 135f,
                    panel.width - 90f,
                    20f
                ),
                volume,
                0f,
                1f
            );

        AudioListener.volume =
            volume;

        GUI.Label(
            new Rect(
                panel.x + 45f,
                panel.y + 175f,
                230f,
                25f
            ),
            "Sensibilité souris : "
            + sensitivity
                .ToString("0.0"),
            textStyle
        );

        sensitivity =
            GUI.HorizontalSlider(
                new Rect(
                    panel.x + 45f,
                    panel.y + 210f,
                    panel.width - 90f,
                    20f
                ),
                sensitivity,
                .5f,
                5f
            );

        if (fps != null)
            fps.mouseSensitivity =
                sensitivity;

        if (
            GUI.Button(
                new Rect(
                    panel.x + 70f,
                    panel.y + 260f,
                    panel.width - 140f,
                    48f
                ),
                "REPRENDRE",
                buttonStyle
            )
        )
        {
            Resume();
        }

        if (
            GUI.Button(
                new Rect(
                    panel.x + 70f,
                    panel.y + 325f,
                    panel.width - 140f,
                    48f
                ),
                "RECOMMENCER",
                buttonStyle
            )
        )
        {
            Restart();
        }

        if (
            GUI.Button(
                new Rect(
                    panel.x + 70f,
                    panel.y + 390f,
                    panel.width - 140f,
                    48f
                ),
                "QUITTER LE JEU",
                buttonStyle
            )
        )
        {
            QuitGame();
        }

        GUI.Label(
            new Rect(
                panel.x,
                panel.y + 455f,
                panel.width,
                22f
            ),
            "Échap : reprendre   •   F10 : quitter",
            subtitleStyle
        );
    }

    // =====================================================
    // VICTOIRE
    // =====================================================

    void DrawVictoryOverlay()
    {
        Fill(
            new Rect(
                0,
                0,
                Screen.width,
                Screen.height
            ),
            new Color(
                .02f,
                .025f,
                .015f,
                .68f
            )
        );

        float w = 530f;
        float h = 350f;

        Rect panel =
            new Rect(
                (Screen.width-w)/2f,
                (Screen.height-h)/2f,
                w,
                h
            );

        Fill(
            panel,
            new Color(
                .10f,
                .075f,
                .02f,
                .97f
            )
        );

        Border(
            panel,
            new Color(
                1f,
                .70f,
                .12f,
                .95f
            ),
            3f
        );

        GUI.Label(
            new Rect(
                panel.x,
                panel.y + 35f,
                panel.width,
                55f
            ),
            "VICTOIRE !",
            titleStyle
        );

        GUI.Label(
            new Rect(
                panel.x + 30f,
                panel.y + 100f,
                panel.width - 60f,
                45f
            ),
            "Tu as trouvé le trésor du collège.",
            subtitleStyle
        );

        GUI.Label(
            new Rect(
                panel.x + 30f,
                panel.y + 145f,
                panel.width - 60f,
                32f
            ),
            "Les 6 indices ont été découverts.",
            subtitleStyle
        );

        if (
            GUI.Button(
                new Rect(
                    panel.x + 130f,
                    panel.y + 200f,
                    panel.width - 260f,
                    48f
                ),
                "REJOUER",
                buttonStyle
            )
        )
        {
            Restart();
        }

        if (
            GUI.Button(
                new Rect(
                    panel.x + 165f,
                    panel.y + 265f,
                    panel.width - 330f,
                    44f
                ),
                "QUITTER",
                buttonStyle
            )
        )
        {
            QuitGame();
        }
    }

    // =====================================================
    // OBJECTIF
    // utilise la réflexion pour rester compatible
    // avec différentes versions du GameManager
    // =====================================================

    string GetObjective()
    {
        if (game == null)
            return "";

        System.Type type =
            game.GetType();

        FieldInfo field =
            type.GetField(
                "objective",
                BindingFlags.Public
                |
                BindingFlags.NonPublic
                |
                BindingFlags.Instance
            );

        if (field != null)
        {
            object value =
                field.GetValue(game);

            if (value != null)
                return value.ToString();
        }

        PropertyInfo property =
            type.GetProperty(
                "objective",
                BindingFlags.Public
                |
                BindingFlags.NonPublic
                |
                BindingFlags.Instance
            );

        if (property != null)
        {
            object value =
                property.GetValue(
                    game,
                    null
                );

            if (value != null)
                return value.ToString();
        }

        return
            "Explore le collège et trouve le prochain indice.";
    }

    // =====================================================
    // DESSIN
    // =====================================================

    void Fill(
        Rect rect,
        Color color)
    {
        Color previous =
            GUI.color;

        GUI.color =
            color;

        GUI.DrawTexture(
            rect,
            white
        );

        GUI.color =
            previous;
    }

    void Border(
        Rect rect,
        Color color,
        float thickness)
    {
        Fill(
            new Rect(
                rect.x,
                rect.y,
                rect.width,
                thickness
            ),
            color
        );

        Fill(
            new Rect(
                rect.x,
                rect.yMax-thickness,
                rect.width,
                thickness
            ),
            color
        );

        Fill(
            new Rect(
                rect.x,
                rect.y,
                thickness,
                rect.height
            ),
            color
        );

        Fill(
            new Rect(
                rect.xMax-thickness,
                rect.y,
                thickness,
                rect.height
            ),
            color
        );
    }
}
