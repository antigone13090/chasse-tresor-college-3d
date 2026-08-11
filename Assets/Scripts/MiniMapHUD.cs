using UnityEngine;

public class MiniMapHUD : MonoBehaviour
{
    public bool visible = true;

    private Transform player;

    const float MAP_WIDTH = 64f;
    const float MAP_HEIGHT = 44f;

    GUIStyle zoneStyle;
    GUIStyle titleStyle;

    struct Zone
    {
        public string name;
        public float x;
        public float y;
        public float w;
        public float h;
        public Color color;

        public Zone(
            string name,
            float x,
            float y,
            float w,
            float h,
            Color color)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.color = color;
        }
    }

    Zone[] zones;

    void Awake()
    {
        BuildZones();
    }

    void Start()
    {
        GameObject p =
            GameObject.Find("Player");

        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            visible = !visible;

        if (player == null)
        {
            GameObject p =
                GameObject.Find("Player");

            if (p != null)
                player = p.transform;
        }
    }

    void BuildZones()
    {
        zones = new Zone[]
        {
            // Cour centrale
            new Zone(
                "COUR",
                24f,18f,18f,10f,
                new Color(.52f,.65f,.50f,.92f)
            ),

            // Préau
            new Zone(
                "PREAU",
                13f,27f,8f,5f,
                new Color(.60f,.67f,.58f,.92f)
            ),

            // Vie scolaire
            new Zone(
                "VIE",
                21f,27f,5f,3f,
                new Color(.72f,.76f,.77f,.95f)
            ),

            // CDI
            new Zone(
                "CDI",
                26f,21f,10f,3f,
                new Color(.88f,.72f,.20f,.95f)
            ),

            // Auditorium
            new Zone(
                "AUD.",
                47f,19f,5f,6f,
                new Color(.68f,.71f,.72f,.95f)
            ),

            // Self élèves
            new Zone(
                "SELF",
                49f,14f,10f,3f,
                new Color(.78f,.80f,.81f,.95f)
            ),

            // Gymnase
            new Zone(
                "GYM",
                7f,34f,10f,7f,
                new Color(.74f,.76f,.77f,.95f)
            ),

            // Arts + Musique
            new Zone(
                "ARTS",
                10f,12f,10f,5f,
                new Color(.86f,.69f,.20f,.95f)
            )
        };
    }

    void PrepareStyles()
    {
        if (zoneStyle != null)
            return;

        zoneStyle =
            new GUIStyle(GUI.skin.label);

        zoneStyle.fontSize = 10;
        zoneStyle.alignment =
            TextAnchor.MiddleCenter;

        zoneStyle.normal.textColor =
            Color.black;

        titleStyle =
            new GUIStyle(GUI.skin.label);

        titleStyle.fontSize = 11;
        titleStyle.fontStyle =
            FontStyle.Bold;

        titleStyle.alignment =
            TextAnchor.MiddleCenter;

        titleStyle.normal.textColor =
            Color.white;
    }

    void OnGUI()
    {
        if (!visible)
            return;

        PrepareStyles();

        float width =
            Mathf.Min(
                270f,
                Screen.width * .25f
            );

        float height =
            width * MAP_HEIGHT / MAP_WIDTH;

        float margin = 18f;

        Rect panel =
            new Rect(
                Screen.width-width-margin,
                margin,
                width,
                height+22f
            );

        // Fond transparent
        Fill(
            panel,
            new Color(
                .03f,.04f,.05f,.78f
            )
        );

        GUI.Label(
            new Rect(
                panel.x,
                panel.y,
                panel.width,
                20f
            ),
            "PLAN DU COLLEGE",
            titleStyle
        );

        Rect map =
            new Rect(
                panel.x+7f,
                panel.y+22f,
                panel.width-14f,
                height-7f
            );

        Fill(
            map,
            new Color(
                .82f,.79f,.69f,.90f
            )
        );

        // Axes principaux / circulation
        DrawZone(
            map,
            21f,15f,
            24f,18f,
            new Color(
                .68f,.65f,.56f,.65f
            ),
            ""
        );

        DrawZone(
            map,
            45f,11f,
            8f,18f,
            new Color(
                .68f,.65f,.56f,.65f
            ),
            ""
        );

        DrawZone(
            map,
            8f,8f,
            20f,11f,
            new Color(
                .68f,.65f,.56f,.55f
            ),
            ""
        );

        // Zones principales
        foreach (Zone zone in zones)
        {
            DrawZone(
                map,
                zone.x,
                zone.y,
                zone.w,
                zone.h,
                zone.color,
                zone.name
            );
        }

        // Bordure
        Border(
            map,
            new Color(1f,1f,1f,.65f),
            1f
        );

        // Joueur
        if (player != null)
        {
            float mapX =
                player.position.x / 2f
                + 32f;

            float mapY =
                player.position.z / 2f
                + 22f;

            float px =
                map.x
                + mapX / MAP_WIDTH
                * map.width;

            float py =
                map.y
                + mapY / MAP_HEIGHT
                * map.height;

            // halo
            Fill(
                new Rect(
                    px-5f,
                    py-5f,
                    10f,
                    10f
                ),
                Color.black
            );

            // joueur
            Fill(
                new Rect(
                    px-3f,
                    py-3f,
                    6f,
                    6f
                ),
                new Color(
                    1f,.12f,.10f,1f
                )
            );
        }
    }

    void DrawZone(
        Rect map,
        float x,
        float y,
        float w,
        float h,
        Color color,
        string label)
    {
        Rect r =
            new Rect(
                map.x
                + x / MAP_WIDTH
                * map.width,

                map.y
                + y / MAP_HEIGHT
                * map.height,

                w / MAP_WIDTH
                * map.width,

                h / MAP_HEIGHT
                * map.height
            );

        Fill(r,color);

        Border(
            r,
            new Color(
                .15f,.15f,.15f,.55f
            ),
            1f
        );

        if (!string.IsNullOrEmpty(label))
        {
            GUI.Label(
                r,
                label,
                zoneStyle
            );
        }
    }

    static void Fill(
        Rect rect,
        Color color)
    {
        Color old =
            GUI.color;

        GUI.color = color;

        GUI.DrawTexture(
            rect,
            Texture2D.whiteTexture
        );

        GUI.color = old;
    }

    static void Border(
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
