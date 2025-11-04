using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    public float handleRange = 50f;
    public CanvasGroup canvasGroup; // For fading in/out
    
    private Vector2 inputVector;
    private PlayerController player;
    private Canvas canvas;
    private RectTransform canvasRectTransform;
    
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        canvas = GetComponentInParent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        
        // Hide joystick at start
        HideJoystick();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        // Show joystick at touch position
        ShowJoystickAtPosition(eventData.position);
        OnDrag(eventData);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
        
        if (player != null)
        {
            player.SetMoveInput(Vector2.zero);
        }
        
        // Hide joystick when touch ends
        HideJoystick();
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            eventData.position,
            eventData.pressEventCamera,
            out position))
        {
            position.x = (position.x / joystickBackground.sizeDelta.x);
            position.y = (position.y / joystickBackground.sizeDelta.y);
            
            inputVector = new Vector2(position.x * 2, position.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            
            joystickHandle.anchoredPosition = new Vector2(
                inputVector.x * handleRange,
                inputVector.y * handleRange
            );
            
            if (player != null)
            {
                player.SetMoveInput(inputVector);
            }
        }
    }
    
    private void ShowJoystickAtPosition(Vector2 screenPosition)
    {
        // Convert screen position to canvas position
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            screenPosition,
            canvas.worldCamera,
            out localPoint
        );
        
        // Position the joystick background at touch point
        joystickBackground.anchoredPosition = localPoint;
        
        // Reset handle to center
        joystickHandle.anchoredPosition = Vector2.zero;
        
        // Show joystick
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            joystickBackground.gameObject.SetActive(true);
        }
    }
    
    private void HideJoystick()
    {
        // Hide joystick
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
        else
        {
            joystickBackground.gameObject.SetActive(false);
        }
    }
}
