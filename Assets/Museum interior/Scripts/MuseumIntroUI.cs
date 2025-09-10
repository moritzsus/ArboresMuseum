using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MuseumIntroUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private Button startButton;

    private bool showing;

    private void Awake()
    {
        if (!GameSettings.Instance)
        {
            var go = new GameObject("GameSettings");
            go.AddComponent<GameSettings>();
        }
    }

    private void Start()
    {
        if (!GameSettings.Instance.MuseumIntroSeen)
            ShowIntro();
        else
            HideImmediate();
    }

    private void ShowIntro()
    {
        showing = true;
        if (root) root.SetActive(true);

        CursorGuard.Instance.SetNeedsCursor(true);

        Time.timeScale = 0f;

        if (infoText)
        {
            var mode = GameSettings.Instance != null ? GameSettings.Instance.Mode : GameMode.Explore;
            infoText.text = (mode == GameMode.Play)
                ? "Du hast den Spielen-Modus gew‰hlt. Das bedeutet, dass zu Beginn nur ein Raum begehbar ist. Die Anderen kannst du freischalten, indem du das jeweilige Minispiel im Raum abschlieﬂt. Versuche dir die Werke und die zugehˆrigen Informationen gut einzupr‰gen; das kann in den Minispielen von Vorteil sein. Du kannst bereits abgeschlossene Spiele erneut spielen, allerdings z‰hlen die Punkte nur vom ersten erfolgreichen Durchgang. Viel Spaﬂ beim spielerischen Erkunden des Museums zu Arbores Consanguinitatis."
                : "Du hast den Erkunden-Modus gew‰hlt. Bewege dich frei und ohne Stress durch alle R‰ume des Museums und lerne Neues zu Arbores Consanguinitatis. Viel Spaﬂ!";
        }

        if (startButton)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnClickStart);
        }
    }

    private void OnClickStart()
    {
        GameSettings.Instance.MuseumIntroSeen = true;
        Hide();
    }

    private void Hide()
    {
        if (!showing) return;
        showing = false;

        if (startButton) startButton.onClick.RemoveListener(OnClickStart);
        CursorGuard.Instance.SetNeedsCursor(false);
        Time.timeScale = 1f;
        if (root) root.SetActive(false);
    }

    private void HideImmediate()
    {
        showing = false;
        if (root) root.SetActive(false);
    }

    private void OnDisable()
    {
        if (showing)
        {
            CursorGuard.Instance.SetNeedsCursor(false);
            Time.timeScale = 1f;
        }
    }
}
