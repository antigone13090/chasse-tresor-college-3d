using System.Collections.Generic;
using UnityEngine;

namespace CollegeTreasure.Campaign
{
    public sealed class CampaignPlayerInteractor : MonoBehaviour
    {
        public float interactionDistance = 5f;

        Camera playerCamera;
        ICampaignInteractable focused;
        int keyboardSelection = -1;

        public string CurrentPrompt
        {
            get { return focused == null ? string.Empty : focused.InteractionPrompt; }
        }

        void Start()
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        void Update()
        {
            if (Time.timeScale <= 0f)
                return;

            focused = FindAimedInteractable();

            if (Input.GetKeyDown(KeyCode.Tab))
                focused = SelectWithKeyboard();

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                && focused != null
                && focused.InteractionEnabled)
            {
                focused.Interact(this);
            }
        }

        ICampaignInteractable FindAimedInteractable()
        {
            if (playerCamera == null)
                return focused != null && focused.InteractionEnabled ? focused : null;

            RaycastHit hit;
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (!Physics.Raycast(ray, out hit, interactionDistance))
                return focused != null && focused.InteractionEnabled ? focused : null;

            MonoBehaviour[] behaviours = hit.collider.GetComponentsInParent<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in behaviours)
            {
                ICampaignInteractable interactable = behaviour as ICampaignInteractable;
                if (interactable != null && interactable.InteractionEnabled)
                    return interactable;
            }

            return null;
        }

        ICampaignInteractable SelectWithKeyboard()
        {
            CampaignActionObject[] all = Object.FindObjectsByType<CampaignActionObject>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None);

            List<CampaignActionObject> reachable = new List<CampaignActionObject>();
            foreach (CampaignActionObject action in all)
            {
                float distance = Vector3.Distance(transform.position, action.transform.position);
                if (action.InteractionEnabled && distance <= interactionDistance + 2f)
                    reachable.Add(action);
            }

            reachable.Sort((left, right) =>
                Vector3.Distance(transform.position, left.transform.position).CompareTo(
                    Vector3.Distance(transform.position, right.transform.position)));

            if (reachable.Count == 0)
                return null;

            keyboardSelection = (keyboardSelection + 1) % reachable.Count;
            return reachable[keyboardSelection];
        }
    }
}
