using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_timerText;
    [SerializeField] private float m_timeForOneRound;


    // Update is called once per frame
    void Update()
    {
        if (m_timeForOneRound > 0)
        {
            m_timeForOneRound -= Time.deltaTime;
        }
        else
        {
            m_timeForOneRound = 0;
        }

        DisplayTime(m_timeForOneRound);
    }

    public void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(m_timeForOneRound / 60);
        float seconds = Mathf.FloorToInt(m_timeForOneRound % 60);
        m_timerText.text = string.Format("{0:D2}:{1:D2}", (int)minutes, (int)seconds);

    }
}
