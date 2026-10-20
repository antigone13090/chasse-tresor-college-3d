using System.Collections.Generic;
using UnityEngine;

namespace CollegeTreasure.Campaign
{
    public sealed class CampaignPlayerInteractor : MonoBehaviour
    {
        public float interactionDistance = 5f;

        Camera playerCamera;
        ICampaignInteractable focused;
        ICampaignInteractable keyboardFocused;
        int keyboardSelection = -1;

        public string CurrentPrompt
        {
            get { return focused == null || !CanReach(focused) ? string.Empty : focused.InteractionPrompt; }
        }

        void Start()
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        void Update()
        {
            if (Time.timeScale <= 0f)
                return;

            if (playerCamera == null)
                playerCamera = GetComponentInChildren<Camera>();

            ICampaignInteractable aimed = FindAimedInteractable();
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                keyboardFocused = SelectWithKeyboard();
            }

            if (keyboardFocused != null && !CanReach(keyboardFocused))
                keyboardFocused = null;

            focused = keyboardFocused ?? aimed;

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                && focused != null && CanReach(focused))
            {
                focused.Interact(this);
            }
        }

        bool CanReach(ICampaignInteractable candidate)
        {
            if (candidate == null || !candidate.InteractionEnabled)
                return false;

            Transform target = candidate.InteractionTransform;
            if (target == null || Vector3.Distance(transform.position, target.position) > interactionDistance)
                return false;

            if (playerCamera == null)
                playerCamera = GetComponentInChildren<Camera>();
            Vector3 origin = playerCamera != null ? playerCamera.transform.position : transform.position;
            Vector3 direction = target.position - origin;
            float distance = direction.magnitude;

            if (distance <= 0.01f)
                return true;

            RaycastHit hit;
            if (!Physics.Raycast(origin, direction / distance, out hit, distance + 0.1f))
                return false;

            MonoBehaviour[] behaviours = hit.collider.GetComponentsInParent<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in behaviours)
                if (ReferenceEquals(behaviour, candidate))
                    return true;

            return false;
        }

#if UNITY_EDITOR
        // Physics regression checks only: real Tab/Return input still needs PlayMode coverage.
        public bool CanReachForEditorTest(ICampaignInteractable candidate)
        {
            return CanReach(candidate);
        }

        public void SelectForEditorTest()
        {
            keyboardFocused = SelectWithKeyboard();
            focused = keyboardFocused;
        }

        public string FocusedActionIdForEditorTest
        {
            get
            {
                CampaignActionObject action = focused as CampaignActionObject;
                return action == null ? null : action.ActionId;
            }
        }
#endif

        ICampaignInteractable FindAimedInteractable()
        {
            if (playerCamera == null)
                return null;

            RaycastHit hit;
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (!Physics.Raycast(ray, out hit, interactionDistance))
                return null;

            MonoBehaviour[] behaviours = hit.collider.GetComponentsInParent<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in behaviours)
            {
                ICampaignInteractable interactable = behaviour as ICampaignInteractable;
                if (interactable != null && CanReach(interactable))
                    return interactable;
            }

            return null;
        }

        ICampaignInteractable SelectWithKeyboard()
        {
            CampaignActionObject[] all = Object.FindObjectsByType<CampaignActionObject>(
                FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            List<CampaignActionObject> reachable = new List<CampaignActionObject>();
            foreach (CampaignActionObject action in all)
                if (CanReach(action))
                    reachable.Add(action);

            reachable.Sort((left, right) =>
            {
                int cmp = Vector3.Distance(transform.position, left.transform.position)
                    .CompareTo(Vector3.Distance(transform.position, right.transform.position));
                return cmp != 0 ? cmp : string.CompareOrdinal(left.ActionId, right.ActionId);
            });

            if (reachable.Count == 0)
            {
                keyboardSelection = -1;
                return null;
            }

            keyboardSelection = (keyboardSelection + 1) % reachable.Count;
            return reachable[keyboardSelection];
        }
    }
}
