using UnityEngine;

namespace CollegeTreasure.Campaign
{
    public sealed class CampaignWorldBuilder
    {
        readonly Transform root;
        readonly CampaignLevelDefinition definition;

        public CampaignWorldBuilder(Transform parent, CampaignLevelDefinition levelDefinition)
        {
            root = parent;
            definition = levelDefinition;
        }

        public GameObject CreateBlock(
            string name,
            Vector3 position,
            Vector3 scale,
            Color color,
            PrimitiveType primitive = PrimitiveType.Cube)
        {
            GameObject block = GameObject.CreatePrimitive(primitive);
            block.name = name;
            block.transform.SetParent(root, false);
            block.transform.position = position;
            block.transform.localScale = scale;
            block.GetComponent<Renderer>().material = CreateMaterial(color);
            return block;
        }

        public CampaignActionObject CreateAction(
            CampaignPuzzleBase owner,
            string actionId,
            string label,
            Vector3 position,
            Vector3 scale,
            Color color,
            PrimitiveType primitive = PrimitiveType.Cube)
        {
            GameObject block = CreateBlock(actionId, position, scale, color, primitive);
            CampaignActionObject action = block.AddComponent<CampaignActionObject>();
            action.Configure(owner, actionId, label, color);
            CreateLabel(block.transform, label, Vector3.up * (scale.y * 0.6f + 0.35f), 0.22f, Color.white);
            return action;
        }

        public TextMesh CreateWorldLabel(
            string name,
            string text,
            Vector3 position,
            float size,
            Color color)
        {
            GameObject labelObject = new GameObject(name);
            labelObject.transform.SetParent(root, false);
            labelObject.transform.position = position;
            labelObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            TextMesh textMesh = labelObject.AddComponent<TextMesh>();
            textMesh.text = text;
            textMesh.fontSize = 64;
            textMesh.characterSize = size;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.color = color;
            return textMesh;
        }

        public GameObject CreateGate(string name, Vector3 position, Vector3 scale)
        {
            return CreateBlock(name, position, scale, definition.accentColor * 0.65f);
        }

        public void CreateRoom(Vector2 size, float height, Color floorColor, Color wallColor)
        {
            CreateBlock("Sol", new Vector3(0f, -0.25f, 0f), new Vector3(size.x, 0.5f, size.y), floorColor);
            CreateBlock("MurNord", new Vector3(0f, height * 0.5f, size.y * 0.5f), new Vector3(size.x, height, 0.5f), wallColor);
            CreateBlock("MurSud", new Vector3(0f, height * 0.5f, -size.y * 0.5f), new Vector3(size.x, height, 0.5f), wallColor);
            CreateBlock("MurOuest", new Vector3(-size.x * 0.5f, height * 0.5f, 0f), new Vector3(0.5f, height, size.y), wallColor);
            CreateBlock("MurEst", new Vector3(size.x * 0.5f, height * 0.5f, 0f), new Vector3(0.5f, height, size.y), wallColor);

            GameObject lightObject = new GameObject("EclairagePrincipal");
            lightObject.transform.SetParent(root, false);
            lightObject.transform.position = new Vector3(0f, height - 0.5f, 0f);
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.range = Mathf.Max(size.x, size.y);
            light.intensity = 1.6f;
            light.color = Color.Lerp(Color.white, definition.primaryColor, 0.18f);
        }

        public static GameObject CreatePlayer(Vector3 position)
        {
            GameObject player = new GameObject("CampaignPlayer");
            player.transform.position = position;
            CharacterController characterController = player.AddComponent<CharacterController>();
            characterController.height = 1.75f;
            characterController.radius = 0.3f;
            characterController.center = new Vector3(0f, 0.88f, 0f);

            GameObject cameraObject = new GameObject("Player Camera");
            cameraObject.transform.SetParent(player.transform, false);
            cameraObject.transform.localPosition = new Vector3(0f, 1.58f, 0f);
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.nearClipPlane = 0.05f;
            cameraObject.AddComponent<AudioListener>();

            player.AddComponent<CampaignFirstPersonController>();
            player.AddComponent<CampaignPlayerInteractor>();
            return player;
        }

        static Material CreateMaterial(Color color)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
                shader = Shader.Find("Universal Render Pipeline/Lit");

            Material material = new Material(shader);
            material.color = color;
            return material;
        }

        void CreateLabel(
            Transform parent,
            string text,
            Vector3 offset,
            float size,
            Color color)
        {
            GameObject labelObject = new GameObject("Etiquette");
            labelObject.transform.SetParent(parent, false);
            labelObject.transform.localPosition = new Vector3(0f, offset.y / Mathf.Max(0.01f, parent.localScale.y), -0.55f);
            labelObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            labelObject.transform.localScale = new Vector3(
                1f / Mathf.Max(0.01f, parent.localScale.x),
                1f / Mathf.Max(0.01f, parent.localScale.y),
                1f / Mathf.Max(0.01f, parent.localScale.z));
            TextMesh mesh = labelObject.AddComponent<TextMesh>();
            mesh.text = text;
            mesh.fontSize = 48;
            mesh.characterSize = size;
            mesh.anchor = TextAnchor.MiddleCenter;
            mesh.alignment = TextAlignment.Center;
            mesh.color = color;
        }
    }
}
