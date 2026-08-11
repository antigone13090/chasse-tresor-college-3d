using UnityEngine;
using System.Reflection;

public class CollegeDemoController : MonoBehaviour
{
    public AudioClip clueSound;

    CollegeModernUI modernUI;
    TreasureGameManager game;
    MiniMapHUD miniMap;

    FPSController fps;
    CharacterController characterController;

    Transform player;
    Camera playerCamera;

    bool demoRunning = false;
    bool clueSoundPlayed = false;
    bool finalScreen = false;

    bool gameWasEnabled;
    bool mapWasEnabled;
    bool characterWasEnabled;

    float demoTime = 0f;

    Vector3 originalPlayerPosition;
    Quaternion originalPlayerRotation;
    Quaternion originalCameraRotation;

    Vector3 approachStart;
    Vector3 approachEnd;

    GameObject clue1;

    AudioSource audioSource;

    GUIStyle demoTitle;
    GUIStyle demoSubtitle;
    GUIStyle demoText;
    GUIStyle demoButton;
    GUIStyle hugeStyle;
    GUIStyle clueStyle;

    Texture2D white;

    void Start()
    {
        white =
            Texture2D.whiteTexture;

        modernUI =
            FindFirstObjectByType<
                CollegeModernUI
            >();

        game =
            FindFirstObjectByType<
                TreasureGameManager
            >();

        miniMap =
            FindFirstObjectByType<
                MiniMapHUD
            >();

        fps =
            FindFirstObjectByType<
                FPSController
            >();

        if (fps != null)
        {
            player =
                fps.transform;

            characterController =
                fps.GetComponent<
                    CharacterController
                >();

            playerCamera =
                fps.GetComponentInChildren<
                    Camera
                >();
        }

        clue1 =
            GameObject.Find(
                "Indice_1_Vie_Scolaire"
            );

        audioSource =
            gameObject.AddComponent<
                AudioSource
            >();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = .85f;
    }

    void Update()
    {
        if (
            !demoRunning
            &&
            !finalScreen
            &&
            Input.GetKeyDown(KeyCode.F2)
            &&
            IsStartScreen()
        )
        {
            BeginDemo();
        }

        if (!demoRunning)
            return;

        demoTime +=
            Time.unscaledDeltaTime;

        UpdateCinematic();
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
            field.GetValue(modernUI);

        return
            value is bool
            &&
            !(bool)value;
    }

    void BeginDemo()
    {
        if (
            player == null
            ||
            playerCamera == null
            ||
            clue1 == null
        )
        {
            Debug.LogError(
                "DEMO : Player, caméra ou indice 1 introuvable."
            );

            return;
        }

        demoRunning = true;
        finalScreen = false;
        clueSoundPlayed = false;
        demoTime = 0f;

        originalPlayerPosition =
            player.position;

        originalPlayerRotation =
            player.rotation;

        originalCameraRotation =
            playerCamera.transform
                .localRotation;

        if (characterController != null)
        {
            characterWasEnabled =
                characterController.enabled;

            characterController.enabled =
                false;
        }

        if (game != null)
        {
            gameWasEnabled =
                game.enabled;

            game.enabled = false;
        }

        if (miniMap != null)
        {
            mapWasEnabled =
                miniMap.enabled;

            miniMap.enabled = false;
        }

        if (modernUI != null)
            modernUI.enabled = false;

        Time.timeScale = 0f;

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        float feetY =
            originalPlayerPosition.y;

        approachStart =
            new Vector3(
                clue1.transform.position.x + .9f,
                feetY,
                clue1.transform.position.z + 4.0f
            );

        approachEnd =
            new Vector3(
                clue1.transform.position.x + .25f,
                feetY,
                clue1.transform.position.z + 1.55f
            );

        Debug.Log(
            "DEMO CINEMATIQUE DEMARREE"
        );
    }

    void UpdateCinematic()
    {
        // -------------------------------------------------
        // 0 à 2 s : plan d'introduction
        // -------------------------------------------------

        if (demoTime < 2.0f)
        {
            player.position =
                originalPlayerPosition;

            Vector3 look =
                clue1.transform.position
                + Vector3.up * .35f;

            LookAtSmooth(
                look,
                1.5f
            );

            return;
        }

        // -------------------------------------------------
        // 2 à 2.8 s : écran noir / changement de plan
        // -------------------------------------------------

        if (demoTime < 2.8f)
        {
            if (demoTime > 2.4f)
            {
                player.position =
                    approachStart;

                LookAtImmediate(
                    clue1.transform.position
                );
            }

            return;
        }

        // -------------------------------------------------
        // 2.8 à 6.2 s : approche lente de l'indice
        // -------------------------------------------------

        if (demoTime < 6.2f)
        {
            float t =
                Mathf.InverseLerp(
                    2.8f,
                    6.2f,
                    demoTime
                );

            t =
                Mathf.SmoothStep(
                    0f,
                    1f,
                    t
                );

            player.position =
                Vector3.Lerp(
                    approachStart,
                    approachEnd,
                    t
                );

            LookAtImmediate(
                clue1.transform.position
                + Vector3.up * .15f
            );

            return;
        }

        // -------------------------------------------------
        // 6.2 s : découverte
        // -------------------------------------------------

        player.position =
            approachEnd;

        LookAtImmediate(
            clue1.transform.position
            + Vector3.up * .15f
        );

        if (!clueSoundPlayed)
        {
            clueSoundPlayed = true;

            if (clueSound != null)
            {
                audioSource.PlayOneShot(
                    clueSound
                );
            }
        }

        // -------------------------------------------------
        // Fin de la démo
        // -------------------------------------------------

        if (demoTime >= 12.5f)
        {
            demoRunning = false;
            finalScreen = true;

            Cursor.lockState =
                CursorLockMode.None;

            Cursor.visible = true;
        }
    }

    void LookAtImmediate(
        Vector3 target)
    {
        Vector3 direction =
            target
            - playerCamera.transform.position;

        if (
            direction.sqrMagnitude
            < .001f
        )
            return;

        Quaternion look =
            Quaternion.LookRotation(
                direction.normalized,
                Vector3.up
            );

        Vector3 angles =
            look.eulerAngles;

        float pitch =
            angles.x;

        if (pitch > 180f)
            pitch -= 360f;

        player.rotation =
            Quaternion.Euler(
                0f,
                angles.y,
                0f
            );

        playerCamera.transform
            .localRotation =
            Quaternion.Euler(
                pitch,
                0f,
                0f
            );
    }

    void LookAtSmooth(
        Vector3 target,
        float speed)
    {
        Vector3 direction =
            target
            - playerCamera.transform.position;

        direction.y = 0f;

        if (
            direction.sqrMagnitude
            < .001f
        )
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(
                direction.normalized,
                Vector3.up
            );

        player.rotation =
            Quaternion.Slerp(
                player.rotation,
                targetRotation,
                Time.unscaledDeltaTime
                * speed
            );
    }

    void RestoreSceneState()
    {
        if (player != null)
        {
            player.position =
                originalPlayerPosition;

            player.rotation =
                originalPlayerRotation;
        }

        if (playerCamera != null)
        {
            playerCamera.transform
                .localRotation =
                originalCameraRotation;
        }

        if (characterController != null)
        {
            characterController.enabled =
                characterWasEnabled;
        }

        if (game != null)
            game.enabled =
                gameWasEnabled;

        if (miniMap != null)
            miniMap.enabled =
                mapWasEnabled;

        if (modernUI != null)
            modernUI.enabled = true;
    }

    void PlayGameNow()
    {
        RestoreSceneState();

        finalScreen = false;

        if (modernUI != null)
        {
            MethodInfo method =
                typeof(CollegeModernUI)
                .GetMethod(
                    "StartGame",
                    BindingFlags.Instance
                    |
                    BindingFlags.NonPublic
                );

            if (method != null)
            {
                method.Invoke(
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

    void ReturnToMenu()
    {
        RestoreSceneState();

        finalScreen = false;
        demoRunning = false;

        Time.timeScale = 0f;

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;
    }

    void PrepareStyles()
    {
        if (demoTitle != null)
            return;

        demoTitle =
            new GUIStyle(
                GUI.skin.label
            );

        demoTitle.fontSize = 38;
        demoTitle.fontStyle =
            FontStyle.Bold;

        demoTitle.alignment =
            TextAnchor.MiddleCenter;

        demoTitle.normal.textColor =
            Color.white;

        demoSubtitle =
            new GUIStyle(
                GUI.skin.label
            );

        demoSubtitle.fontSize = 20;
        demoSubtitle.alignment =
            TextAnchor.MiddleCenter;

        demoSubtitle.normal.textColor =
            new Color(
                .86f,.90f,.94f
            );

        demoText =
            new GUIStyle(
                GUI.skin.label
            );

        demoText.fontSize = 17;
        demoText.alignment =
            TextAnchor.MiddleCenter;

        demoText.wordWrap = true;

        demoText.normal.textColor =
            Color.white;

        clueStyle =
            new GUIStyle(
                GUI.skin.label
            );

        clueStyle.fontSize = 24;
        clueStyle.fontStyle =
            FontStyle.Bold;

        clueStyle.alignment =
            TextAnchor.MiddleCenter;

        clueStyle.wordWrap = true;

        clueStyle.normal.textColor =
            new Color(
                .46f,
                1f,
                .54f
            );

        hugeStyle =
            new GUIStyle(
                GUI.skin.label
            );

        hugeStyle.fontSize = 44;
        hugeStyle.fontStyle =
            FontStyle.Bold;

        hugeStyle.alignment =
            TextAnchor.MiddleCenter;

        hugeStyle.normal.textColor =
            new Color(
                1f,
                .73f,
                .18f
            );

        demoButton =
            new GUIStyle(
                GUI.skin.button
            );

        demoButton.fontSize = 18;
        demoButton.fontStyle =
            FontStyle.Bold;
    }

    void OnGUI()
    {
        PrepareStyles();

        if (
            !demoRunning
            &&
            !finalScreen
        )
        {
            DrawDemoMenuButton();
            return;
        }

        if (demoRunning)
            DrawDemoOverlay();

        if (finalScreen)
            DrawFinalScreen();
    }

    // =====================================================
    // BOUTON SUR L'ECRAN D'ACCUEIL
    // =====================================================

    void DrawDemoMenuButton()
    {
        if (!IsStartScreen())
            return;

        float panelWidth =
            Mathf.Min(
                620f,
                Screen.width - 50f
            );

        float panelX =
            (Screen.width - panelWidth)
            / 2f;

        float panelY =
            (Screen.height - 500f)
            / 2f;

        if (
            GUI.Button(
                new Rect(
                    panelX + 25f,
                    panelY + 365f,
                    110f,
                    60f
                ),
                "DEMO",
                demoButton
            )
        )
        {
            BeginDemo();
        }

        GUI.Label(
            new Rect(
                panelX + 20f,
                panelY + 425f,
                120f,
                25f
            ),
            "ou F2",
            demoSubtitle
        );
    }

    // =====================================================
    // CINEMATIQUE
    // =====================================================

    void DrawDemoOverlay()
    {
        DrawLetterbox();

        // INTRO
        if (demoTime < 2f)
        {
            GUI.Label(
                new Rect(
                    0,
                    Screen.height * .16f,
                    Screen.width,
                    65f
                ),
                "CHASSE AU TRESOR",
                demoTitle
            );

            GUI.Label(
                new Rect(
                    0,
                    Screen.height * .16f + 58f,
                    Screen.width,
                    35f
                ),
                "Un collège. Six indices. Un trésor.",
                demoSubtitle
            );
        }

        // FONDU
        if (
            demoTime >= 1.8f
            &&
            demoTime <= 2.8f
        )
        {
            float alpha;

            if (demoTime < 2.3f)
                alpha =
                    Mathf.InverseLerp(
                        1.8f,
                        2.3f,
                        demoTime
                    );
            else
                alpha =
                    1f
                    -
                    Mathf.InverseLerp(
                        2.3f,
                        2.8f,
                        demoTime
                    );

            Fill(
                new Rect(
                    0,0,
                    Screen.width,
                    Screen.height
                ),
                new Color(
                    0f,0f,0f,
                    alpha
                )
            );
        }

        // APPROCHE
        if (
            demoTime >= 3.0f
            &&
            demoTime < 5.6f
        )
        {
            DrawBottomMessage(
                "Explore le collège. Observe ton environnement."
            );
        }

        if (
            demoTime >= 5.3f
            &&
            demoTime < 6.2f
        )
        {
            DrawBottomMessage(
                "Quelque chose semble t'attendre..."
            );
        }

        // DECOUVERTE
        if (
            demoTime >= 6.2f
            &&
            demoTime < 9.0f
        )
        {
            Rect box =
                new Rect(
                    Screen.width/2f - 300f,
                    Screen.height * .16f,
                    600f,
                    150f
                );

            Fill(
                box,
                new Color(
                    .02f,.05f,.03f,.88f
                )
            );

            Border(
                box,
                new Color(
                    .25f,1f,.38f,.9f
                ),
                2f
            );

            GUI.Label(
                new Rect(
                    box.x + 20f,
                    box.y + 12f,
                    box.width - 40f,
                    40f
                ),
                "INDICE DECOUVERT !",
                clueStyle
            );

            GUI.Label(
                new Rect(
                    box.x + 25f,
                    box.y + 58f,
                    box.width - 50f,
                    75f
                ),
                "Premier indice : là où les absences sont notées.",
                demoText
            );
        }

        // OBJECTIF
        if (
            demoTime >= 9.0f
            &&
            demoTime < 12.5f
        )
        {
            Rect box =
                new Rect(
                    Screen.width/2f - 330f,
                    Screen.height * .70f,
                    660f,
                    115f
                );

            Fill(
                box,
                new Color(
                    .025f,.035f,.045f,.90f
                )
            );

            Border(
                box,
                new Color(
                    .25f,.65f,1f,.85f
                ),
                2f
            );

            GUI.Label(
                new Rect(
                    box.x + 20f,
                    box.y + 12f,
                    box.width - 40f,
                    35f
                ),
                "1 / 6 INDICES",
                clueStyle
            );

            GUI.Label(
                new Rect(
                    box.x + 20f,
                    box.y + 52f,
                    box.width - 40f,
                    45f
                ),
                "Prochaine mission : retrouve le CDI...",
                demoText
            );
        }
    }

    // =====================================================
    // ECRAN FINAL
    // =====================================================

    void DrawFinalScreen()
    {
        Fill(
            new Rect(
                0,
                0,
                Screen.width,
                Screen.height
            ),
            new Color(
                .01f,.018f,.025f,.92f
            )
        );

        GUI.Label(
            new Rect(
                0,
                Screen.height * .23f,
                Screen.width,
                70f
            ),
            "A TOI DE JOUER",
            hugeStyle
        );

        GUI.Label(
            new Rect(
                Screen.width/2f - 360f,
                Screen.height * .23f + 75f,
                720f,
                80f
            ),
            "Cinq autres indices t'attendent quelque part dans le collège.\nLe trésor ne s'ouvrira que si tu les retrouves tous.",
            demoText
        );

        if (
            GUI.Button(
                new Rect(
                    Screen.width/2f - 170f,
                    Screen.height * .55f,
                    340f,
                    62f
                ),
                "COMMENCER LA CHASSE",
                demoButton
            )
        )
        {
            PlayGameNow();
        }

        if (
            GUI.Button(
                new Rect(
                    Screen.width/2f - 120f,
                    Screen.height * .55f + 82f,
                    240f,
                    44f
                ),
                "RETOUR",
                demoButton
            )
        )
        {
            ReturnToMenu();
        }
    }

    void DrawBottomMessage(
        string message)
    {
        Rect r =
            new Rect(
                Screen.width/2f - 350f,
                Screen.height - 150f,
                700f,
                70f
            );

        Fill(
            r,
            new Color(
                0f,0f,0f,.66f
            )
        );

        GUI.Label(
            r,
            message,
            demoSubtitle
        );
    }

    void DrawLetterbox()
    {
        float bar =
            Screen.height * .075f;

        Fill(
            new Rect(
                0,
                0,
                Screen.width,
                bar
            ),
            Color.black
        );

        Fill(
            new Rect(
                0,
                Screen.height-bar,
                Screen.width,
                bar
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

        GUI.color =
            color;

        GUI.DrawTexture(
            rect,
            white
        );

        GUI.color =
            old;
    }

    void Border(
        Rect rect,
        Color color,
        float size)
    {
        Fill(
            new Rect(
                rect.x,
                rect.y,
                rect.width,
                size
            ),
            color
        );

        Fill(
            new Rect(
                rect.x,
                rect.yMax-size,
                rect.width,
                size
            ),
            color
        );

        Fill(
            new Rect(
                rect.x,
                rect.y,
                size,
                rect.height
            ),
            color
        );

        Fill(
            new Rect(
                rect.xMax-size,
                rect.y,
                size,
                rect.height
            ),
            color
        );
    }
}
