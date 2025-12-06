using UnityEngine;
using UnityEngine.UI;

public class ToastController : MonoBehaviour
{
    public float animationDuration = 0.5f;
    public float displayDuration = 2f;
    
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private float timer;
    private bool isShowing;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        timer = 0f;
        isShowing = true;
        transform.localScale = Vector3.zero;
        if (canvasGroup != null) canvasGroup.alpha = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < animationDuration)
        {
            // Scale Up and Fade In
            float t = timer / animationDuration;
            // Use ease out back for pop effect
            float scale = Mathf.Lerp(0f, 1f, t); 
            transform.localScale = Vector3.one * scale;
            canvasGroup.alpha = t;
        }
        else if (timer < displayDuration)
        {
            // Stay visible
            transform.localScale = Vector3.one;
            canvasGroup.alpha = 1f;
        }
        else if (timer < displayDuration + animationDuration)
        {
            // Fade Out
            float t = (timer - displayDuration) / animationDuration;
            canvasGroup.alpha = 1f - t;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
