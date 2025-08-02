using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash_Screen_Controller : MonoBehaviour
{
    public CanvasGroup canvasGroup;  // Assign in Inspector
    public string nextSceneName = "Main_Scene";
    public float fadeDuration = 1f;
    public float displayTime = 2f;

    void Start()
    {
        StartCoroutine(PlaySplash());
    }

    System.Collections.IEnumerator PlaySplash()
    {
        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1;

        // Wait
        float timer = 0f;
        while (timer < displayTime)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) // Skip on tap
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = 1 - (t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;

        // Load next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
