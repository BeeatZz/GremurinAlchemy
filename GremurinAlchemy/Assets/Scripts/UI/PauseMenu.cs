using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("PauseMneu")]
    public GameObject uiPanel;

    [Header("loadingsCreen")]
    public GameObject objectToCheck;

    public SceneTransition sceneFader;
    private void Start()
    {
        uiPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!objectToCheck.activeSelf)
            {
                uiPanel.SetActive(true);
            }
        }
    }

    public void Continue()
    {
        uiPanel.SetActive(false);

    }
    public void QuitGame()
    {
        sceneFader.TransitionToScene(0);
    }

}
