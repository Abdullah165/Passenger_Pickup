using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Transform m_container;
    [SerializeField] private GameObject m_winText;
    [SerializeField] private GameObject m_loseText;
    [SerializeField] private Button m_nextLevel;
    [SerializeField] private Button m_tryAgain;

    [SerializeField] private TimerUI m_timerUI;

    // Start is called before the first frame update
    void Start()
    {
        m_container.gameObject.SetActive(false);

        GameOverManager.Instance.OnGameOver += Instance_OnGameOver;

        m_timerUI.OnTimerOut += TimerUI_OnTimerOut;

        m_nextLevel.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });

        m_tryAgain.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    private void TimerUI_OnTimerOut(object sender, System.EventArgs e)
    {
        m_container.gameObject.SetActive(true);
        m_container.localScale = Vector3.zero;
        m_container.DOScale(1, 0.2f);

        m_loseText.SetActive(true);
        m_winText.SetActive(false);

        m_tryAgain.gameObject.SetActive(true);
        m_nextLevel.gameObject.SetActive(false);
    }

    private void Instance_OnGameOver(object sender, System.EventArgs e)
    {
        m_container.gameObject.SetActive(true);         
        m_container.localScale = Vector3.zero;
        m_container.DOScale(1, 0.2f);

        m_winText.SetActive(true);
        m_loseText.SetActive(false);

        m_nextLevel.gameObject.SetActive(true);
        m_tryAgain.gameObject.SetActive(false);
    }
}
