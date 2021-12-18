using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject topPlayerMenu;

    [SerializeField] TextMeshProUGUI topPlayerList;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void TopPlayerClicked()
    {
        mainMenu.SetActive(false);
        topPlayerMenu.SetActive(true);

        TopPlayerText();

    }

    void TopPlayerText()
    {
        string[] top3Name = GameManager.Instance.topPlayers;
        int[] top3Score = GameManager.Instance.topScore;

        if(top3Name != null && top3Score != null)
        {
            string fullText = null;
            for (int i = 0; i < top3Name.Length; i++)
            {
                fullText += (i + 1) + ". " + top3Name[i] + " - " + top3Score[i] + "\n";
            }

            topPlayerList.SetText(fullText);
        }
    }

    public void BackButtonClicked()
    {
        topPlayerMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ExitGame()
    {
        GameManager.Instance.SaveData();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

    }
}
