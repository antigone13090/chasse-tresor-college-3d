using UnityEngine;
using System.Reflection;

public class CollegeTrailerController : MonoBehaviour
{
    class Shot
    {
        public Vector3 from;
        public Vector3 to;
        public Vector3 target;
        public string title;
        public string subtitle;

        public Shot(
            Vector3 from,
            Vector3 to,
            Vector3 target,
            string title,
            string subtitle)
        {
            this.from = from;
            this.to = to;
            this.target = target;
            this.title = title;
            this.subtitle = subtitle;
        }
    }

    const float SHOT_DURATION = 3.6f;

    CollegeModernUI modernUI;
    CollegeDemoController demoController;
    MiniMapHUD miniMap;
    FPSController fps;
    PlayerInteraction3D interaction;

    Camera playerCamera;
    Camera trailerCamera;

    AudioSource music;

    bool modernUIWasEnabled;
    bool demoWasEnabled;
    bool minimapWasEnabled;
    bool fpsWasEnabled;
    bool interactionWasEnabled;
    bool playerCameraWasEnabled;

    float oldMusicVolume;

    bool running = false;
    bool endScreen = false;

    float trailerTime = 0f;

    Shot[] shots;

    GameObject clue1;
    GameObject treasure;

    Texture2D white;

    GUIStyle hugeStyle;
    GUIStyle titleStyle;
    GUIStyle subtitleStyle;
    GUIStyle buttonStyle;
    GUIStyle smallStyle;

    static float X(float mapX)
    {
        return (mapX - 32f) * 2f;
    }

    static float Z(float mapY)
    {
        return (mapY - 22f) * 2f;
    }

    void Start()
    {
        white =
            Texture2D.whiteTexture;

        modernUI =
            FindFirstObjectByType<
                CollegeModernUI
            >();

        demoController =
            FindFirstObjectByType<
                CollegeDemoController
            >();

        miniMap =
            FindFirstObjectByType<
                MiniMapHUD
            >();

        fps =
            FindFirstObjectByType<
                FPSController
            >();

        interaction =
            FindFirstObjectByType<
                PlayerInteraction3D
            >();

        if (fps != null)
        {
            playerCamera =
                fps.GetComponentInChildren<
                    Camera
                >();
        }

        clue1 =
            GameObject.Find(
                "Indice_1_Vie_Scolaire"
            );

        treasure =
            GameObject.Find(
                "Tresor_Final"
            );

        GameObject musicObject =
            GameObject.Find(
                "BackgroundMusic"
            );

        if (musicObject != null)
        {
            music =
                musicObject.GetComponent<
                    AudioSource
                >();
        }

        CreateTrailerCamera();
        CreateShots();
    }

    void CreateTrailerCamera()
    {
        GameObject obj =
            new GameObject(
                "TrailerCamera"
            );

        obj.transform.SetParent(
            transform
        );

        trailerCamera =
            obj.AddComponent<Camera>();

        if (playerCamera != null)
        {
            trailerCamera.CopyFrom(
                playerCamera
            );
        }

        trailerCamera.fieldOfView =
            64f;

        trailerCamera.enabled =
            false;
    }

    void CreateShots()
    {
        Vector3 clueTarget =
            clue1 != null
            ? clue1.transform.position
                + Vector3.up * .45f
            : new Vector3(
                X(24f),
                1f,
                Z(28.5f)
            );

        Vector3 treasureTarget =
            treasure != null
            ? treasure.transform.position
                + Vector3.up * .75f
            : new Vector3(
                X(43.5f),
                .8f,
                Z(27.5f)
            );

        shots =
            new Shot[]
            {
                // -------------------------------------------------
                // 1 - EXTÉRIEUR
                // -------------------------------------------------

                new Shot(
                    new Vector3(
                        X(14f),
                        10f,
                        Z(35f)
                    ),
                    new Vector3(
                        X(20f),
                        7f,
                        Z(31f)
                    ),
                    new Vector3(
                        X(27f),
                        1.4f,
                        Z(26f)
                    ),
                    "UN COLLÈGE À EXPLORER",
                    "Mais quelque chose y a été caché..."
                ),

                // -------------------------------------------------
                // 2 - COUR
                // -------------------------------------------------

                new Shot(
                    new Vector3(
                        X(23f),
                        4.5f,
                        Z(18f)
                    ),
                    new Vector3(
                        X(31f),
                        3.4f,
                        Z(20.5f)
                    ),
                    new Vector3(
                        X(33f),
                        1.2f,
                        Z(23f)
                    ),
                    "EXPLORE LA COUR",
                    "Observe les lieux. Cherche les passages."
                ),

                // -------------------------------------------------
                // 3 - PREMIER INDICE
                // -------------------------------------------------

                new Shot(
                    new Vector3(
                        clueTarget.x - 3.5f,
                        1.8f,
                        clueTarget.z + 4f
                    ),
                    new Vector3(
                        clueTarget.x - .4f,
                        1.55f,
                        clueTarget.z + 2.1f
                    ),
                    clueTarget,
                    "UN PREMIER MYSTÈRE...",
                    "Un indice vient d'être découvert."
                ),

                // -------------------------------------------------
                // 4 - ESCALIER / CDI
                // -------------------------------------------------

                new Shot(
                    new Vector3(
                        X(22f),
                        2.2f,
                        Z(27f)
                    ),
                    new Vector3(
                        X(25f),
                        4.8f,
                        Z(24f)
                    ),
                    new Vector3(
                        X(31f),
                        4.0f,
                        Z(22.5f)
                    ),
                    "MONTE. CHERCHE. OBSERVE.",
                    "Certains indices ne sont pas au rez-de-chaussée."
                ),

                // -------------------------------------------------
                // 5 - GYMNASE
                // -------------------------------------------------

                new Shot(
                    new Vector3(
                        X(18.5f),
                        3.4f,
                        Z(36f)
                    ),
                    new Vector3(
                        X(16.5f),
                        2.4f,
                        Z(37.5f)
                    ),
                    new Vector3(
                        X(12f),
                        1.15f,
                        Z(37.5f)
                    ),
                    "DU GYMNASE...",
                    "Chaque lieu peut dissimuler une nouvelle piste."
                ),

                // -------------------------------------------------
                // 6 - AUDITORIUM
                // -------------------------------------------------

                new Shot(
                    new Vector3(
                        X(45.3f),
                        2.4f,
                        Z(22f)
                    ),
                    new Vector3(
                        X(48f),
                        1.9f,
                        Z(22f)
                    ),
                    new Vector3(
                        X(50.5f),
                        1.3f,
                        Z(22f)
                    ),
                    "À L'AUDITORIUM...",
                    "La chasse t'emmène dans tout le collège."
                ),

                // -------------------------------------------------
                // 7 - SELF
                // -------------------------------------------------

                new Shot(
                    new Vector3(
                        X(49f),
                        2.7f,
                        Z(18f)
                    ),
                    new Vector3(
                        X(51.5f),
                        1.9f,
                        Z(17.1f)
                    ),
                    new Vector3(
                        X(56f),
                        1.1f,
                        Z(15.5f)
                    ),
                    "JUSQU'AU SELF...",
                    "Six indices doivent être retrouvés dans le bon ordre."
                ),

                // -------------------------------------------------
                // 8 - TRÉSOR
                // -------------------------------------------------

                new Shot(
                    new Vector3(
                        treasureTarget.x - 4.2f,
                        2.7f,
                        treasureTarget.z + 3.5f
                    ),
                    new Vector3(
                        treasureTarget.x - 1.7f,
                        1.7f,
                        treasureTarget.z + 2f
                    ),
                    treasureTarget,
                    "LE TRÉSOR EST PROCHE...",
                    "Mais il restera verrouillé tant que l'énigme n'est pas résolue."
                )
            };
    }

    void Update()
    {
        if (
            !running
            &&
            !endScreen
            &&
            Input.GetKeyDown(KeyCode.F3)
            &&
            IsStartScreen()
        )
        {
            BeginTrailer();
        }

        if (!running)
            return;

        if (
            Input.GetKeyDown(
                KeyCode.Escape
            )
        )
        {
            ReturnToMenu();
            return;
        }

        trailerTime +=
            Time.unscaledDeltaTime;

        float shotsDuration =
            shots.Length
            * SHOT_DURATION;

        if (
            trailerTime
            < shotsDuration
        )
        {
            UpdateShot();
        }
        else
        {
            UpdateOutro(
                trailerTime
                - shotsDuration
            );
        }
    }

    bool IsStartScreen()
    {
        if (modernUI == null)
            return false;

        FieldInfo field =
            typeof(CollegeModernUI)
            .GetField(
                "started",
                BindingFlags.Instance
                |
                BindingFlags.NonPublic
            );

        if (field == null)
            return Time.timeScale == 0f;

        object value =
            field.GetValue(
                modernUI
            );

        return
            value is bool
            &&
            !(bool)value;
    }

    void BeginTrailer()
    {
        if (
            trailerCamera == null
            ||
            shots == null
            ||
            shots.Length == 0
        )
            return;

        SaveStates();

        running = true;
        endScreen = false;

        trailerTime = 0f;

        if (modernUI != null)
            modernUI.enabled = false;

        if (demoController != null)
            demoController.enabled = false;

        if (miniMap != null)
            miniMap.enabled = false;

        if (fps != null)
            fps.enabled = false;

        if (interaction != null)
            interaction.enabled = false;

        if (playerCamera != null)
            playerCamera.enabled = false;

        trailerCamera.enabled =
            true;

        if (music != null)
            music.volume = .31f;

        Time.timeScale = 1f;

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        Debug.Log(
            "BANDE-ANNONCE DEMARREE"
        );
    }

    void SaveStates()
    {
        if (modernUI != null)
            modernUIWasEnabled =
                modernUI.enabled;

        if (demoController != null)
            demoWasEnabled =
                demoController.enabled;

        if (miniMap != null)
            minimapWasEnabled =
                miniMap.enabled;

        if (fps != null)
            fpsWasEnabled =
                fps.enabled;

        if (interaction != null)
            interactionWasEnabled =
                interaction.enabled;

        if (playerCamera != null)
            playerCameraWasEnabled =
                playerCamera.enabled;

        if (music != null)
            oldMusicVolume =
                music.volume;
    }

    void UpdateShot()
    {
        int index =
            Mathf.Clamp(
                Mathf.FloorToInt(
                    trailerTime
                    / SHOT_DURATION
                ),
                0,
                shots.Length - 1
            );

        float local =
            trailerTime
            - index * SHOT_DURATION;

        float t =
            Mathf.Clamp01(
                local
                / SHOT_DURATION
            );

        t =
            Mathf.SmoothStep(
                0f,
                1f,
                t
            );

        Shot shot =
            shots[index];

        trailerCamera.transform.position =
            Vector3.Lerp(
                shot.from,
                shot.to,
                t
            );

        LookAt(
            shot.target
        );

        // léger changement de focale
        trailerCamera.fieldOfView =
            Mathf.Lerp(
                66f,
                58f,
                t
            );
    }

    void LookAt(
        Vector3 target)
    {
        Vector3 direction =
            target
            - trailerCamera
                .transform.position;

        if (
            direction.sqrMagnitude
            < .001f
        )
            return;

        Quaternion wanted =
            Quaternion.LookRotation(
                direction.normalized,
                Vector3.up
            );

        trailerCamera.transform.rotation =
            Quaternion.Slerp(
                trailerCamera
                    .transform.rotation,
                wanted,
                Time.unscaledDeltaTime
                * 7f
            );
    }

    void UpdateOutro(
        float outroTime)
    {
        if (outroTime >= 6f)
        {
            running = false;
            endScreen = true;

            Cursor.lockState =
                CursorLockMode.None;

            Cursor.visible = true;
        }
    }

    void RestoreStates()
    {
        Time.timeScale = 0f;

        if (trailerCamera != null)
            trailerCamera.enabled = false;

        if (playerCamera != null)
            playerCamera.enabled =
                playerCameraWasEnabled;

        if (modernUI != null)
            modernUI.enabled =
                modernUIWasEnabled;

        if (demoController != null)
            demoController.enabled =
                demoWasEnabled;

        if (miniMap != null)
            miniMap.enabled =
                minimapWasEnabled;

        if (fps != null)
            fps.enabled =
                fpsWasEnabled;

        if (interaction != null)
            interaction.enabled =
                interactionWasEnabled;

        if (music != null)
            music.volume =
                oldMusicVolume;

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;
    }

    void ReturnToMenu()
    {
        running = false;
        endScreen = false;

        RestoreStates();
    }

    void Replay()
    {
        running = true;
        endScreen = false;

        trailerTime = 0f;

        trailerCamera.enabled =
            true;

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    void PlayGame()
    {
        if (trailerCamera != null)
            trailerCamera.enabled = false;

        if (playerCamera != null)
            playerCamera.enabled = true;

        if (demoController != null)
            demoController.enabled =
                demoWasEnabled;

        if (miniMap != null)
            miniMap.enabled = true;

        if (interaction != null)
            interaction.enabled = true;

        if (music != null)
            music.volume =
                oldMusicVolume;

        endScreen = false;
        running = false;

        if (modernUI != null)
        {
            modernUI.enabled = true;

            MethodInfo start =
                typeof(CollegeModernUI)
                .GetMethod(
                    "StartGame",
                    BindingFlags.Instance
                    |
                    BindingFlags.NonPublic
                );

            if (start != null)
            {
                start.Invoke(
                    modernUI,
                    null
                );

                return;
            }
        }

        Time.timeScale = 1f;

        if (fps != null)
            fps.enabled = true;

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    // =====================================================
    // INTERFACE
    // =====================================================

    void PrepareStyles()
    {
        if (titleStyle != null)
            return;

        hugeStyle =
            new GUIStyle(
                GUI.skin.label
            );

        hugeStyle.fontSize = 48;
        hugeStyle.fontStyle =
            FontStyle.Bold;

        hugeStyle.alignment =
            TextAnchor.MiddleCenter;

        hugeStyle.normal.textColor =
            new Color(
                1f,
                .72f,
                .16f
            );

        titleStyle =
            new GUIStyle(
                GUI.skin.label
            );

        titleStyle.fontSize = 30;
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

        subtitleStyle.wordWrap =
            true;

        subtitleStyle.normal.textColor =
            new Color(
                .88f,
                .91f,
                .94f
            );

        smallStyle =
            new GUIStyle(
                GUI.skin.label
            );

        smallStyle.fontSize = 13;
        smallStyle.alignment =
            TextAnchor.MiddleCenter;

        smallStyle.normal.textColor =
            new Color(
                .80f,
                .84f,
                .88f
            );

        buttonStyle =
            new GUIStyle(
                GUI.skin.button
            );

        buttonStyle.fontSize = 17;
        buttonStyle.fontStyle =
            FontStyle.Bold;
    }

    void OnGUI()
    {
        PrepareStyles();

        if (
            !running
            &&
            !endScreen
        )
        {
            DrawMenuButton();
            return;
        }

        if (running)
            DrawTrailerOverlay();

        if (endScreen)
            DrawEndScreen();
    }

    void DrawMenuButton()
    {
        if (!IsStartScreen())
            return;

        float panelWidth =
            Mathf.Min(
                620f,
                Screen.width - 50f
            );

        float panelX =
            (Screen.width
            - panelWidth)
            / 2f;

        float panelY =
            (Screen.height
            - 500f)
            / 2f;

        if (
            GUI.Button(
                new Rect(
                    panelX
                    + panelWidth
                    - 145f,
                    panelY + 365f,
                    120f,
                    60f
                ),
                "BANDE-\nANNONCE",
                buttonStyle
            )
        )
        {
            BeginTrailer();
        }

        GUI.Label(
            new Rect(
                panelX
                + panelWidth
                - 145f,
                panelY + 425f,
                120f,
                25f
            ),
            "ou F3",
            smallStyle
        );
    }

    void DrawTrailerOverlay()
    {
        DrawLetterbox();

        float shotsDuration =
            shots.Length
            * SHOT_DURATION;

        if (
            trailerTime
            < shotsDuration
        )
        {
            int index =
                Mathf.Clamp(
                    Mathf.FloorToInt(
                        trailerTime
                        / SHOT_DURATION
                    ),
                    0,
                    shots.Length - 1
                );

            float local =
                trailerTime
                - index * SHOT_DURATION;

            Shot shot =
                shots[index];

            // titre du plan
            if (local < 2.4f)
            {
                float alpha = 1f;

                if (local > 1.7f)
                {
                    alpha =
                        1f
                        -
                        Mathf.InverseLerp(
                            1.7f,
                            2.4f,
                            local
                        );
                }

                Color old =
                    GUI.color;

                GUI.color =
                    new Color(
                        1f,1f,1f,
                        alpha
                    );

                GUI.Label(
                    new Rect(
                        0,
                        Screen.height
                            - 160f,
                        Screen.width,
                        40f
                    ),
                    shot.title,
                    titleStyle
                );

                GUI.Label(
                    new Rect(
                        Screen.width/2f
                            - 400f,
                        Screen.height
                            - 120f,
                        800f,
                        45f
                    ),
                    shot.subtitle,
                    subtitleStyle
                );

                GUI.color = old;
            }

            // flash spécial indice 1
            if (
                index == 2
                &&
                local > 2.25f
            )
            {
                GUI.Label(
                    new Rect(
                        0,
                        Screen.height
                            * .17f,
                        Screen.width,
                        55f
                    ),
                    "INDICE DÉCOUVERT",
                    hugeStyle
                );
            }

            return;
        }

        float outro =
            trailerTime
            - shotsDuration;

        float blackAlpha =
            Mathf.Clamp01(
                outro / 1.2f
            );

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
                blackAlpha
            )
        );

        if (outro > 1.1f)
        {
            GUI.Label(
                new Rect(
                    0,
                    Screen.height
                        * .25f,
                    Screen.width,
                    75f
                ),
                "CHASSE AU TRÉSOR",
                hugeStyle
            );
        }

        if (outro > 2.0f)
        {
            GUI.Label(
                new Rect(
                    0,
                    Screen.height
                        * .25f
                        + 82f,
                    Screen.width,
                    40f
                ),
                "6 INDICES  •  1 TRÉSOR",
                titleStyle
            );
        }

        if (outro > 3.1f)
        {
            GUI.Label(
                new Rect(
                    Screen.width/2f
                        - 420f,
                    Screen.height
                        * .25f
                        + 140f,
                    840f,
                    75f
                ),
                "Sauras-tu explorer le collège et résoudre la chasse avant de trouver le coffre ?",
                subtitleStyle
            );
        }
    }

    void DrawEndScreen()
    {
        Fill(
            new Rect(
                0,
                0,
                Screen.width,
                Screen.height
            ),
            new Color(
                .008f,
                .012f,
                .018f,
                .98f
            )
        );

        GUI.Label(
            new Rect(
                0,
                Screen.height
                    * .20f,
                Screen.width,
                80f
            ),
            "CHASSE AU TRÉSOR",
            hugeStyle
        );

        GUI.Label(
            new Rect(
                0,
                Screen.height
                    * .20f
                    + 82f,
                Screen.width,
                40f
            ),
            "L'aventure commence maintenant.",
            titleStyle
        );

        if (
            GUI.Button(
                new Rect(
                    Screen.width/2f
                        - 180f,
                    Screen.height
                        * .50f,
                    360f,
                    62f
                ),
                "COMMENCER LA CHASSE",
                buttonStyle
            )
        )
        {
            PlayGame();
        }

        if (
            GUI.Button(
                new Rect(
                    Screen.width/2f
                        - 140f,
                    Screen.height
                        * .50f
                        + 82f,
                    280f,
                    48f
                ),
                "REVOIR LA BANDE-ANNONCE",
                buttonStyle
            )
        )
        {
            Replay();
        }

        if (
            GUI.Button(
                new Rect(
                    Screen.width/2f
                        - 100f,
                    Screen.height
                        * .50f
                        + 148f,
                    200f,
                    44f
                ),
                "RETOUR",
                buttonStyle
            )
        )
        {
            ReturnToMenu();
        }
    }

    void DrawLetterbox()
    {
        float h =
            Screen.height
            * .075f;

        Fill(
            new Rect(
                0,
                0,
                Screen.width,
                h
            ),
            Color.black
        );

        Fill(
            new Rect(
                0,
                Screen.height-h,
                Screen.width,
                h
            ),
            Color.black
        );
    }

    void Fill(
        Rect rect,
        Color color)
    {
        Color old =
            GUI.color;

        GUI.color = color;

        GUI.DrawTexture(
            rect,
            white
        );

        GUI.color = old;
    }
}
