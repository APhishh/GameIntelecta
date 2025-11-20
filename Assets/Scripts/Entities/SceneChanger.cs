using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad = "NextScene";
    [SerializeField] private float delayBeforeLoad = 0f;

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;       // A full-screen black Image
    [SerializeField] private float fadeDuration = 1f;

    private bool triggered = false;

    private void Start()
    {
        if (fadeImage != null)
        {
            // Start transparent → fade in
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;

            StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;
        if (!collision.CompareTag("Player")) return;

        triggered = true;

        StartCoroutine(SceneTransitionRoutine());
    }

    private System.Collections.IEnumerator SceneTransitionRoutine()
    {
        if (delayBeforeLoad > 0)
            yield return new WaitForSeconds(delayBeforeLoad);

        // Fade out first
        if (fadeImage != null)
            yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(sceneToLoad);
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = 1f - (t / fadeDuration);

            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;

            yield return null;
        }

        // Ensure fully transparent
        Color end = fadeImage.color;
        end.a = 0f;
        fadeImage.color = end;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = (t / fadeDuration);

            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;

            yield return null;
        }

        // Ensure fully black
        Color end = fadeImage.color;
        end.a = 1f;
        fadeImage.color = end;
    }
}
