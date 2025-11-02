using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public GameObject fadeCanvas;
    public float fadeDuration = 1f;

    private void Start()
    {
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0);
    }

    public void TransitionToScene(int sceneIndex)
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(FadeAndLoad(sceneIndex));
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    private IEnumerator FadeAndLoad(int sceneIndex)
    {
        fadeCanvas.SetActive(true);
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(t / fadeDuration));
            yield return null;
        }

       
        SceneManager.LoadScene(sceneIndex);
    }
}
