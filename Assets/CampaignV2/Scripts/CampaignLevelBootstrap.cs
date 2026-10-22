using UnityEngine;

namespace CollegeTreasure.Campaign
{
    public sealed class CampaignLevelBootstrap : MonoBehaviour
    {
        public CampaignLevelDefinition definition;

        void Awake()
        {
            if (definition == null)
            {
                Debug.LogError("Definition de niveau V2 absente dans " + gameObject.scene.name + ".");
                return;
            }

            CampaignLevelController controller = gameObject.AddComponent<CampaignLevelController>();
            controller.Initialize(definition);
        }
    }
}
