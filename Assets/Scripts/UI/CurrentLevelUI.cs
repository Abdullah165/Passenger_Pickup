using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentLevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        m_currentLevel.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
    }
}
