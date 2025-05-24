using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class CarMovementController : MonoBehaviour
{
    [SerializeField] private List<Transform> m_carSegments;
    private List<Vector3> m_previousPositions;

    private Vector3 m_offset;
    private float m_zCoord;
    private bool m_isDragging;

    private Vector3 m_dragStartWorldPos;
    private Vector3Int m_lastGridPos;

    private readonly string m_roadblockLayer = "roadblock";
    private Vector3 m_rayUpwardOffset = new(0, 0.3f, 0);

    private Camera m_camera;


    private void Start()
    {
        m_camera = Camera.main;
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
                    Ray ray = m_camera.ScreenPointToRay(touchPos);
                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
                    {
                        m_zCoord = m_camera.WorldToScreenPoint(transform.position).z;
                        m_dragStartWorldPos = GetWorldPosFromScreen(touchPos);
                        m_offset = transform.position - m_dragStartWorldPos;
                        m_isDragging = true;

                        m_lastGridPos = GetWorldToGrid(transform.position);
                    }
                    break;

                case TouchPhase.Moved:
                    if (m_isDragging)
                    {
                        Vector3 worldPos = GetWorldPosFromScreen(touchPos);
                        Vector3 targetPos = worldPos + m_offset;
                        targetPos.y = transform.position.y;

                        var gridPos = GetWorldToGrid(targetPos);

                        if (gridPos != m_lastGridPos)
                        {
                            Vector3 moveDirection = (GetGridToWorldPos(gridPos) - transform.position).normalized;

                            // Move only when the car doesn't hit roadblock 
                            if (!Physics.Raycast(transform.position + m_rayUpwardOffset, moveDirection, 1.0f, LayerMask.GetMask(m_roadblockLayer)))
                            {
                                var snappedPos = GetGridToWorldPos(gridPos);
                                transform.DOMove(snappedPos, 0.2f);
                                UpdateCarSegements(gridPos - m_lastGridPos);
                                m_lastGridPos = gridPos;
                            }
                            else
                            {
                                Debug.Log("Blocked in direction: " + moveDirection);
                            }
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    m_isDragging = false;
                    break;
            }
        }
    }

    Vector3 GetWorldPosFromScreen(Vector2 screenPos)
    {
        Vector3 pos = screenPos;
        pos.z = m_zCoord;
        return m_camera.ScreenToWorldPoint(pos);
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
        m_previousPositions = new List<Vector3>();
        foreach (var segment in m_carSegments)
        {
            m_previousPositions.Add(segment.position);
        }

        // Move Head.
        Vector3 newHeadPos = GetGridToWorldPos(m_lastGridPos + direction);
        m_carSegments[0].DOMove(newHeadPos, 0.2f);

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

        m_carSegments[0].DORotate(headRotation, 0.2f);

        for (int i = 1; i < m_carSegments.Count; i++)
        {
            if (m_carSegments.Count > 2 && i == m_carSegments.Count - 1)
            {
                m_carSegments[i].DOMove(m_previousPositions[i - 1], 0.2f);
                m_carSegments[i].DORotate(m_carSegments[i - 2].eulerAngles, 0.2f).SetEase(Ease.InQuad);
                break;
            }

            var targetPos = m_previousPositions[i - 1];

            var lookDirection = m_previousPositions[i - 1] - m_previousPositions[i];
            var targetRotation = Quaternion.LookRotation(lookDirection != Vector3.zero ? lookDirection : Vector3.zero);

            m_carSegments[i].DOMove(targetPos, 0.2f);
            m_carSegments[i].DORotateQuaternion(targetRotation, 0.2f).SetEase(Ease.InSine);
        }
    }
}
