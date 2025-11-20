using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections; // Needed for Coroutines

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Coroutine movementCoroutine;

    [Header("Hover Settings")]
    [SerializeField] private float moveAmount = 15f; // Distance to move right (in pixels/units)
    [SerializeField] private float moveDuration = 0.15f; // Time taken for the movement

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 targetPosition = originalPosition + new Vector3(moveAmount, 0, 0);
        
        // Stop any current movement before starting a new one
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(MoveButton(targetPosition));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Stop any current movement before starting to move back
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(MoveButton(originalPosition));
    }

    // Coroutine to handle the smooth movement
    private IEnumerator MoveButton(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = rectTransform.localPosition;

        while (elapsedTime < moveDuration)
        {
            // Calculate the current position using Lerp for smooth interpolation
            rectTransform.localPosition = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the button reaches the exact target position at the end
        rectTransform.localPosition = targetPosition;
    }
}