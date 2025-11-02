using UnityEngine;

public class IntroUIController : MonoBehaviour
{
    public GameObject introUI;
    public float displayDuration = 2f;

    private void Start()
    {
        if (introUI != null)
        {
            introUI.SetActive(true);
            Invoke(nameof(HideIntroUI), displayDuration);
        }
    }

    private void HideIntroUI()
    {
        introUI.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
