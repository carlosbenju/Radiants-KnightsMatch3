using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NavigationButtonsManager : MonoBehaviour
{
    MasterSceneManager masterSceneManager;

    private void Awake()
    {
        masterSceneManager = FindObjectOfType<MasterSceneManager>();
    }

    public void GoToMainMenu()
    {
        masterSceneManager.LoadScene("Main Menu Scene");
    }

    public void GoToStore()
    {
        masterSceneManager.LoadScene("StoreScene");
    }

    public void GoToLevel(TextMeshProUGUI text)
    {
        Int32.TryParse(text.text, out int level);

        masterSceneManager.LoadScene("Level " + level + " Scene");
    }

    public int GetButtonText(Button levelSelectionButton)
    {
        string levelText = levelSelectionButton.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text;

        if (Int32.TryParse(levelText, out int levelNumber))
        {
            return levelNumber;
        }

        return 0;
    }
}
