using UnityEngine;
using System.Collections.Generic;

namespace Gift2
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private float detectionRadius = 3f;
        [SerializeField] private LayerMask interactableLayer;

        private Interactable currentSelected;

        public Interactable GetSelectedObject()
        {
            return currentSelected;
        }

        private void Update()
        {
            FindAndSelectClosestInteractable();
        }

        private void FindAndSelectClosestInteractable()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, interactableLayer);

            if (colliders.Length == 0)
            {
                if (currentSelected != null)
                {
                    currentSelected.Deselect();
                    currentSelected = null;
                }
                return;
            }

            List<Interactable> interactables = new List<Interactable>();
            foreach (var col in colliders)
            {
                Interactable interactable = col.GetComponent<Interactable>();
                if (interactable != null)
                    interactables.Add(interactable);
            }

            if (interactables.Count == 0)
            {
                if (currentSelected != null)
                {
                    currentSelected.Deselect();
                    currentSelected = null;
                }
                return;
            }

            Interactable closest = null;
            float minDistance = float.MaxValue;
            Vector2 currentPos = transform.position;

            foreach (var interactable in interactables)
            {
                float distance = Vector2.Distance(currentPos, interactable.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = interactable;
                }
            }

            if (closest != currentSelected)
            {
                if (currentSelected != null)
                    currentSelected.Deselect();
                currentSelected = closest;
                currentSelected.Select();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}