using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class CarMovementController : MonoBehaviour
{
    [SerializeField] private List<Transform> m_CarSegments;
    private List<Vector3> m_PreviousPositions;

    private Vector3 m_Offset;
    private float m_ZCoord;
    private bool m_IsDragging;

    private Vector3 m_DragStartWorldPos;
    private Vector3Int m_LastGridPos;

    private Camera m_Camera;


    private void Start()
    {
        m_Camera = Camera.main;
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
                    Ray ray = m_Camera.ScreenPointToRay(touchPos);
                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
                    {
                        m_ZCoord = m_Camera.WorldToScreenPoint(transform.position).z;
                        m_DragStartWorldPos = GetWorldPosFromScreen(touchPos);
                        m_Offset = transform.position - m_DragStartWorldPos;
                        m_IsDragging = true;

                        m_LastGridPos = GetWorldToGrid(transform.position);
                    }
                    break;

                case TouchPhase.Moved:
                    if (m_IsDragging)
                    {
                        Vector3 worldPos = GetWorldPosFromScreen(touchPos);
                        Vector3 targetPos = worldPos + m_Offset;
                        targetPos.y = transform.position.y;

                        var gridPos = GetWorldToGrid(targetPos);

                        if (gridPos != m_LastGridPos)
                        {
                            var snappedPos = GetGridToWorldPos(gridPos);
                            transform.DOMove(snappedPos, 0.2f);
                            UpdateCarSegements(gridPos - m_LastGridPos);
                            m_LastGridPos = gridPos;
                        }
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
        return m_Camera.ScreenToWorldPoint(pos);
    }

    Vector3Int GetWorldToGrid(Vector3 worldPos)
    {
        return Vector3Int.RoundToInt(worldPos);
    }

    Vector3 GetGridToWorldPos(Vector3 gridPos)
    {
        return gridPos;
    }

    void UpdateCarSegements(Vector3Int direction)
    {
        m_PreviousPositions = new List<Vector3>();
        foreach (var segment in m_CarSegments)
        {
            m_PreviousPositions.Add(segment.position);
        }

        // Move Head.
        Vector3 newHeadPos = GetGridToWorldPos(m_LastGridPos + direction);
        m_CarSegments[0].DOMove(newHeadPos, 0.2f);

        Vector3 headRotation = Vector3.zero;
        if (direction == Vector3Int.left)
        {
            headRotation = new Vector3(0, -90, 0);
        }
        else if (direction == Vector3Int.right)
        {
            headRotation = new Vector3(0, 90, 0);
        }
        else if (direction == Vector3Int.forward)
        {
            headRotation = new Vector3(0, 0, 0);
        }
        else if (direction == Vector3Int.back)
        {
            headRotation = new Vector3(0, 180, 0);
        }

        m_CarSegments[0].DORotate(headRotation, 0.2f);

        for (int i = 1; i < m_CarSegments.Count; i++)
        {
            if(i == m_CarSegments.Count - 1)
            {
                m_CarSegments[i].DOMove(m_PreviousPositions[i - 1], 0.2f);
                m_CarSegments[i].DORotate(m_CarSegments[i - 2].eulerAngles, 0.2f);
                break;
            }
            var targetPos = m_PreviousPositions[i - 1];

            var lookDirection = m_PreviousPositions[i - 1] - m_PreviousPositions[i];
            var targetRotation = Quaternion.LookRotation(lookDirection != Vector3.zero ? lookDirection : Vector3.zero);

            m_CarSegments[i].DOMove(targetPos,0.2f);
            m_CarSegments[i].DORotateQuaternion(targetRotation,0.2f);
        }
    }
}
