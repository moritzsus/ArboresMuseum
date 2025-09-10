using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetectiveEndUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject root;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button mainMenuButton;

    private bool nextClicked = false;

    private void Start()
    {
        nextButton.onClick.AddListener(OnClickNext);
        replayButton.onClick.AddListener(OnClickReplay);

        // Falls der MainMenu-Button existiert
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnClickMainMenu);

        // Button-Status zu Beginn setzen
        if (mainMenuButton != null)
            mainMenuButton.gameObject.SetActive(false);
    }

    private void OnClickNext()
    {
        nextClicked = true;

        int game1 = GameSettings.Instance.GetPointsForGame(0);
        int game2 = GameSettings.Instance.GetPointsForGame(1);
        int game3 = GameSettings.Instance.GetPointsForGame(2);
        int game4 = GameSettings.Instance.GetPointsForGame(3);
        int totalPoints = game1 + game2 + game3 + game4;

        DialogueManager dm = DialogueManager.Instance;
        if (dm != null && dm.endTitle != null && dm.endText != null)
        {
            dm.endTitle.text = "Spielabschluss";

            dm.endText.text = "Du hast alle Spiele abgeschlossen und dabei diese Punkte gesammelt:\n";
            dm.endText.text += $"Puzzle: {game1} Punkte\n";
            dm.endText.text += $"Farbspiel: {game2} Punkte\n";
            dm.endText.text += $"Quiz: {game3} Punkte\n";
            dm.endText.text += $"Detektivspiel: {game4} Punkte\n";
            dm.endText.text += $"Insgesamt hast du {totalPoints} Punkte gesammelt!\n";
            dm.endText.text += "Du kannst jetzt entweder zum Museum zurückkehren oder zum Hauptmenü.";
        }

        nextButton.gameObject.SetActive(false);
        if (mainMenuButton != null)
            mainMenuButton.gameObject.SetActive(true);

        if (replayButton != null)
            replayButton.GetComponentInChildren<TMPro.TextMeshProUGUI>()?.SetText("Museum");
    }

    private void OnClickReplay()
    {
        if (!nextClicked)
            SceneManager.LoadScene("DetectiveGame", LoadSceneMode.Single);
        else
            SceneManager.LoadScene("Museum", LoadSceneMode.Single);
    }

    private void OnClickMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
