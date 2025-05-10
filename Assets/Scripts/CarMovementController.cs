using UnityEngine;
using DG.Tweening;

public class CarMovementController : MonoBehaviour
{
    private Vector3 m_Offset;
    private float m_ZCoord;
    private bool m_IsDragging;

    private Vector3 m_DragStartWorldPos;
    private Camera m_Cam;


    private void Start()
    {
        m_Cam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Ray ray = m_Cam.ScreenPointToRay(touchPos);
                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
                    {
                        m_ZCoord = m_Cam.WorldToScreenPoint(transform.position).z;
                        m_DragStartWorldPos = GetWorldPosFromScreen(touchPos);
                        m_Offset = transform.position - m_DragStartWorldPos;
                        m_IsDragging = true;
                    }
                    break;

                case TouchPhase.Moved:
                    if (m_IsDragging)
                    {
                        Vector3 worldPos = GetWorldPosFromScreen(touchPos);
                        Vector3 targetPos = worldPos + m_Offset;
                        targetPos.y = transform.position.y;
                        transform.DOMove(targetPos, 0.2f);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    m_IsDragging = false;
                    break;
            }
        }
    }

    Vector3 GetWorldPosFromScreen(Vector2 screenPos)
    {
        Vector3 pos = screenPos;
        pos.z = m_ZCoord;
        return m_Cam.ScreenToWorldPoint(pos);
    }

}
