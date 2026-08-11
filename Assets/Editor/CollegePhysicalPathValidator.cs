using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public static class CollegePhysicalPathValidator
{
    const float STEP = 0.30f;

    const float XMIN = -64f;
    const float XMAX =  64f;

    const float ZMIN = -44f;
    const float ZMAX =  44f;

    const float PLAYER_HEIGHT = 1.70f;
    const float PLAYER_RADIUS = 0.28f;

    const float MAX_STEP_HEIGHT = 0.34f;

    static int pass = 0;
    static int fail = 0;

    static StringBuilder report =
        new StringBuilder();

    class Node
    {
        public Vector3 p;
        public int gx;
        public int gz;
    }

    static List<Node> nodes =
        new List<Node>();

    static Dictionary<long,List<int>> cells =
        new Dictionary<long,List<int>>();

    static readonly string[] clueNames =
    {
        "Indice_1_Vie_Scolaire",
        "Indice_2_CDI",
        "Indice_3_Auditorium",
        "Indice_4_Gymnase",
        "Indice_5_Arts_Musique",
        "Indice_6_Self"
    };

    [MenuItem("College Treasure/Tester parcours physique")]
    public static void Run()
    {
        pass = 0;
        fail = 0;

        report.Clear();
        nodes.Clear();
        cells.Clear();

        Log("");
        Log("================================================");
        Log(" COLLEGE TREASURE 3D");
        Log(" TEST DU PARCOURS PHYSIQUE");
        Log("================================================");
        Log("");

        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        if (!File.Exists(scenePath))
        {
            Fail("Scene introuvable.");
            Finish();
            return;
        }

        EditorSceneManager.OpenScene(scenePath);

        Physics.SyncTransforms();

        GameObject player =
            GameObject.Find("Player");

        if (player == null)
        {
            Fail("Player introuvable.");
            Finish();
            return;
        }

        Pass("Player present.");

        // ------------------------------------------------
        // Vérification des objets
        // ------------------------------------------------

        List<GameObject> clues =
            new List<GameObject>();

        foreach (string name in clueNames)
        {
            GameObject clue =
                GameObject.Find(name);

            if (clue == null)
            {
                Fail(name + " introuvable.");
            }
            else
            {
                Pass(name + " present.");
            }

            clues.Add(clue);
        }

        GameObject treasure =
            GameObject.Find("Tresor_Final");

        if (treasure == null)
        {
            Fail("Tresor_Final introuvable.");
        }
        else
        {
            Pass("Tresor_Final present.");
        }

        if (fail > 0)
        {
            Finish();
            return;
        }

        // ------------------------------------------------
        // Construction automatique de la carte praticable
        // ------------------------------------------------

        Log("");
        Log("Generation de la carte de navigation...");
        Log("Cela peut prendre quelques secondes.");

        BuildNavigationMap();

        Log("");
        Log(
            "Points praticables trouves : "
            + nodes.Count
        );

        if (nodes.Count < 100)
        {
            Fail(
                "Trop peu de points praticables detectes."
            );

            Finish();
            return;
        }

        Pass("Carte physique generee.");

        // ------------------------------------------------
        // Point de départ
        // ------------------------------------------------

        int current =
            FindNearestNode(
                player.transform.position,
                3f
            );

        if (current < 0)
        {
            Fail(
                "Impossible de placer le joueur "
                + "sur la carte praticable."
            );

            Finish();
            return;
        }

        Log("");
        Log(
            "Depart analyse : "
            + nodes[current].p
        );

        // ------------------------------------------------
        // Départ -> indices 1 à 6
        // ------------------------------------------------

        for (int i=0; i<clues.Count; i++)
        {
            Log("");
            Log("----------------------------------------------");
            Log(
                "PARCOURS "
                + (i == 0 ? "DEPART" : "INDICE " + i)
                + " -> INDICE "
                + (i+1)
            );

            GameObject target =
                clues[i];

            PathResult result =
                Search(
                    current,
                    target.transform.position,
                    2.6f
                );

            if (result.success)
            {
                Pass(
                    "Indice "
                    + (i+1)
                    + " physiquement accessible."
                );

                Log(
                    "Distance estimee : "
                    + result.distance.ToString("0.0")
                    + " m"
                );

                Log(
                    "Point d'arrivee : "
                    + nodes[result.target].p
                );

                Log(
                    "Points explores : "
                    + result.visited
                );

                current =
                    result.target;
            }
            else
            {
                Fail(
                    "Indice "
                    + (i+1)
                    + " INACCESSIBLE physiquement."
                );

                Log(
                    "Distance minimale restante : "
                    + result.closestDistance
                        .ToString("0.00")
                    + " m"
                );

                if (result.closest >= 0)
                {
                    Log(
                        "Point accessible le plus proche : "
                        + nodes[result.closest].p
                    );
                }

                Finish();
                return;
            }
        }

        // ------------------------------------------------
        // Indice 6 -> Trésor
        // ------------------------------------------------

        Log("");
        Log("----------------------------------------------");
        Log("PARCOURS INDICE 6 -> TRESOR");

        PathResult treasureResult =
            Search(
                current,
                treasure.transform.position,
                3.1f
            );

        if (treasureResult.success)
        {
            Pass(
                "Tresor final physiquement accessible."
            );

            Log(
                "Distance estimee : "
                + treasureResult.distance
                    .ToString("0.0")
                + " m"
            );
        }
        else
        {
            Fail(
                "Tresor final INACCESSIBLE."
            );

            Log(
                "Distance restante : "
                + treasureResult.closestDistance
                    .ToString("0.00")
                + " m"
            );
        }

        Finish();
    }

    // ====================================================
    // CONSTRUCTION DES POINTS PRATICABLES
    // ====================================================

    static void BuildNavigationMap()
    {
        int nx =
            Mathf.RoundToInt(
                (XMAX-XMIN)/STEP
            );

        int nz =
            Mathf.RoundToInt(
                (ZMAX-ZMIN)/STEP
            );

        for (int gx=0; gx<=nx; gx++)
        {
            float x =
                XMIN + gx*STEP;

            for (int gz=0; gz<=nz; gz++)
            {
                float z =
                    ZMIN + gz*STEP;

                RaycastHit[] hits =
                    Physics.RaycastAll(
                        new Vector3(x,8.5f,z),
                        Vector3.down,
                        10f,
                        ~0,
                        QueryTriggerInteraction.Ignore
                    );

                Array.Sort(
                    hits,
                    delegate(
                        RaycastHit a,
                        RaycastHit b)
                    {
                        return
                            b.point.y
                            .CompareTo(a.point.y);
                    }
                );

                List<float> usedHeights =
                    new List<float>();

                foreach (RaycastHit hit in hits)
                {
                    if (hit.normal.y < .70f)
                        continue;

                    float groundY =
                        hit.point.y;

                    if (
                        groundY < -.3f
                        ||
                        groundY > 4.6f
                    )
                        continue;

                    bool duplicate = false;

                    foreach (float h in usedHeights)
                    {
                        if (
                            Mathf.Abs(h-groundY)
                            < .12f
                        )
                        {
                            duplicate = true;
                            break;
                        }
                    }

                    if (duplicate)
                        continue;

                    if (
                        !HasSupport(
                            x,
                            groundY,
                            z
                        )
                    )
                        continue;

                    Vector3 standing =
                        new Vector3(
                            x,
                            groundY+.04f,
                            z
                        );

                    if (
                        !HasClearance(
                            standing
                        )
                    )
                        continue;

                    usedHeights.Add(groundY);

                    Node node =
                        new Node();

                    node.p =
                        standing;

                    node.gx = gx;
                    node.gz = gz;

                    int index =
                        nodes.Count;

                    nodes.Add(node);

                    long key =
                        Key(gx,gz);

                    if (!cells.ContainsKey(key))
                    {
                        cells[key] =
                            new List<int>();
                    }

                    cells[key].Add(index);
                }
            }
        }
    }

    // ====================================================
    // SUPPORT AU SOL
    // évite de prendre le sommet d'un mur pour un sol
    // ====================================================

    static bool HasSupport(
        float x,
        float y,
        float z)
    {
        Vector2[] offsets =
        {
            new Vector2(0f,0f),
            new Vector2(.20f,0f),
            new Vector2(-.20f,0f),
            new Vector2(0f,.20f),
            new Vector2(0f,-.20f)
        };

        int supported = 0;

        foreach (Vector2 o in offsets)
        {
            RaycastHit hit;

            if (
                Physics.Raycast(
                    new Vector3(
                        x+o.x,
                        y+.45f,
                        z+o.y
                    ),
                    Vector3.down,
                    out hit,
                    .85f,
                    ~0,
                    QueryTriggerInteraction.Ignore
                )
            )
            {
                if (
                    hit.normal.y > .60f
                    &&
                    Mathf.Abs(
                        hit.point.y-y
                    ) <= .28f
                )
                {
                    supported++;
                }
            }
        }

        return supported >= 3;
    }

    // ====================================================
    // VOLUME DU JOUEUR LIBRE ?
    // ====================================================

    static bool HasClearance(
        Vector3 feet)
    {
        Vector3 bottom =
            feet
            + Vector3.up
            * (PLAYER_RADIUS+.07f);

        Vector3 top =
            feet
            + Vector3.up
            * (
                PLAYER_HEIGHT
                - PLAYER_RADIUS
            );

        Collider[] overlaps =
            Physics.OverlapCapsule(
                bottom,
                top,
                PLAYER_RADIUS,
                ~0,
                QueryTriggerInteraction.Ignore
            );

        foreach (Collider col in overlaps)
        {
            if (IsIgnored(col))
                continue;

            return false;
        }

        return true;
    }

    // ====================================================
    // MOUVEMENT ENTRE DEUX POINTS
    // ====================================================

    static bool CanMove(
        Vector3 a,
        Vector3 b)
    {
        // Une vraie marche Unity peut opposer une face
        // verticale au bas du CharacterController.
        // Le stepOffset permet néanmoins de la franchir.
        // On vérifie donc la hauteur de marche puis
        // les obstacles au niveau du corps.

        float dy =
            Mathf.Abs(b.y-a.y);

        if (dy > MAX_STEP_HEIGHT)
            return false;

        float horizontalDistance =
            Vector2.Distance(
                new Vector2(a.x,a.z),
                new Vector2(b.x,b.z)
            );

        if (
            horizontalDistance
            > STEP * 1.6f
        )
            return false;

        float baseY =
            Mathf.Max(a.y,b.y);

        // Test au niveau des genoux / bassin
        if (
            BodyBlocked(
                new Vector3(
                    a.x,
                    baseY + .55f,
                    a.z
                ),
                new Vector3(
                    b.x,
                    baseY + .55f,
                    b.z
                )
            )
        )
            return false;

        // Test au niveau du torse
        if (
            BodyBlocked(
                new Vector3(
                    a.x,
                    baseY + 1.20f,
                    a.z
                ),
                new Vector3(
                    b.x,
                    baseY + 1.20f,
                    b.z
                )
            )
        )
            return false;

        return true;
    }

    static bool BodyBlocked(
        Vector3 from,
        Vector3 to)
    {
        Vector3 delta =
            to-from;

        float distance =
            delta.magnitude;

        if (distance < .01f)
            return false;

        RaycastHit[] hits =
            Physics.SphereCastAll(
                from,
                .18f,
                delta.normalized,
                distance,
                ~0,
                QueryTriggerInteraction.Ignore
            );

        foreach (
            RaycastHit hit
            in hits)
        {
            if (IsIgnored(hit.collider))
                continue;

            return true;
        }

        return false;
    }

    // ====================================================
    // PORTES / INDICES / JOUEUR
    // ne bloquent pas la recherche
    // ====================================================

    static bool IsIgnored(
        Collider col)
    {
        if (col == null)
            return true;

        // Les marches servent de sol.
        // Elles ne doivent pas etre considerees comme
        // des murs lors du calcul du passage du joueur.
        Transform t = col.transform;

        while (t != null)
        {
            if (
                t.name.StartsWith("Escalier_")
                ||
                t.name.StartsWith("Arts_Step_")
            )
                return true;

            t = t.parent;
        }

        if (col.isTrigger)
            return true;

        if (
            col.GetComponentInParent<
                DoorController
            >() != null
        )
            return true;

        if (
            col.GetComponentInParent<
                ClueObject
            >() != null
        )
            return true;

        if (
            col.GetComponentInParent<
                TreasureObject
            >() != null
        )
            return true;

        if (
            col.GetComponentInParent<
                FPSController
            >() != null
        )
            return true;

        return false;
    }

    // ====================================================
    // RECHERCHE BFS
    // ====================================================

    class PathResult
    {
        public bool success;
        public int target = -1;
        public int closest = -1;

        public float distance = 0f;

        public float closestDistance =
            float.MaxValue;

        public int visited = 0;
    }

    static PathResult Search(
        int start,
        Vector3 target,
        float targetRadius)
    {
        PathResult result =
            new PathResult();

        int count =
            nodes.Count;

        bool[] seen =
            new bool[count];

        int[] previous =
            new int[count];

        for (int i=0;i<count;i++)
            previous[i] = -1;

        Queue<int> queue =
            new Queue<int>();

        queue.Enqueue(start);
        seen[start] = true;

        result.closest = start;

        result.closestDistance =
            Vector3.Distance(
                nodes[start].p,
                target
            );

        while (queue.Count > 0)
        {
            int current =
                queue.Dequeue();

            result.visited++;

            float targetDistance =
                Vector3.Distance(
                    nodes[current].p,
                    target
                );

            if (
                targetDistance
                < result.closestDistance
            )
            {
                result.closestDistance =
                    targetDistance;

                result.closest =
                    current;
            }

            if (
                targetDistance
                <= targetRadius
            )
            {
                result.success = true;
                result.target = current;

                result.distance =
                    PathLength(
                        start,
                        current,
                        previous
                    );

                return result;
            }

            Node node =
                nodes[current];

            for (int dx=-1;dx<=1;dx++)
            {
                for (int dz=-1;dz<=1;dz++)
                {
                    if (
                        dx==0
                        &&
                        dz==0
                    )
                        continue;

                    long key =
                        Key(
                            node.gx+dx,
                            node.gz+dz
                        );

                    List<int> candidates;

                    if (
                        !cells.TryGetValue(
                            key,
                            out candidates
                        )
                    )
                        continue;

                    foreach (
                        int next
                        in candidates)
                    {
                        if (seen[next])
                            continue;

                        if (
                            !CanMove(
                                node.p,
                                nodes[next].p
                            )
                        )
                            continue;

                        seen[next] = true;

                        previous[next] =
                            current;

                        queue.Enqueue(next);
                    }
                }
            }
        }

        return result;
    }

    static float PathLength(
        int start,
        int target,
        int[] previous)
    {
        float distance = 0f;

        int current =
            target;

        while (
            current != start
            &&
            current >= 0
        )
        {
            int p =
                previous[current];

            if (p < 0)
                break;

            distance +=
                Vector3.Distance(
                    nodes[current].p,
                    nodes[p].p
                );

            current = p;
        }

        return distance;
    }

    // ====================================================
    // POINT PRATICABLE LE PLUS PROCHE
    // ====================================================

    static int FindNearestNode(
        Vector3 position,
        float maxDistance)
    {
        int best = -1;

        float bestDistance =
            maxDistance;

        for (int i=0;i<nodes.Count;i++)
        {
            float d =
                Vector3.Distance(
                    position,
                    nodes[i].p
                );

            if (d < bestDistance)
            {
                bestDistance = d;
                best = i;
            }
        }

        return best;
    }

    static long Key(
        int x,
        int z)
    {
        return
            ((long)x << 32)
            ^ (uint)z;
    }

    // ====================================================
    // RAPPORT
    // ====================================================

    static void Pass(string text)
    {
        pass++;

        string line =
            "[PASS] " + text;

        Debug.Log(line);
        report.AppendLine(line);
    }

    static void Fail(string text)
    {
        fail++;

        string line =
            "[FAIL] " + text;

        Debug.LogError(line);
        report.AppendLine(line);
    }

    static void Log(string text)
    {
        Debug.Log(text);
        report.AppendLine(text);
    }

    static void Finish()
    {
        Log("");
        Log("================================================");
        Log(" RESULTAT DU PARCOURS PHYSIQUE");
        Log("================================================");

        Log(
            "PASS : " + pass
        );

        Log(
            "FAIL : " + fail
        );

        Log("");

        if (fail == 0)
        {
            Log(
                ">>> PARCOURS COMPLET PHYSIQUEMENT ACCESSIBLE <<<"
            );
        }
        else
        {
            Log(
                ">>> PROBLEME DE PARCOURS DETECTE <<<"
            );
        }

        string directory =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "Logs"
            );

        Directory.CreateDirectory(
            directory
        );

        string output =
            Path.Combine(
                directory,
                "physical-path-tests.txt"
            );

        File.WriteAllText(
            output,
            report.ToString()
        );

        Debug.Log(
            "Rapport : " + output
        );
    }
}
