using System.Collections;
using UnityEngine;

public class PassengerController : MonoBehaviour
{
    private const string k_orangeCarLayer = "OrangeCar";
    private const string k_blueCarLayer = "BlueCar";
    private const string k_redCarLayer = "RedCar";

    private float m_maxDistance = 1.5f;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 origin = transform.position + transform.up * 0.15f;
        Vector3 direction = -transform.forward;
        float rayLength = m_maxDistance;

        RaycastHit hit;

        int orangeCarLayer = LayerMask.NameToLayer(k_orangeCarLayer);
        int blueCarLayer = LayerMask.NameToLayer(k_blueCarLayer);
        int redCarLayer = LayerMask.NameToLayer(k_redCarLayer);
        int layerMask = LayerMask.GetMask(k_orangeCarLayer, k_blueCarLayer,k_redCarLayer);

        if (Physics.Raycast(origin, direction, out hit, rayLength, layerMask))
        {
            if (hit.collider.gameObject.layer == orangeCarLayer)
            {
                if (gameObject.CompareTag("OrangeMan"))
                {
                    m_maxDistance = 0f;
                    GetComponent<CapsuleCollider>().enabled = false;
                    StartCoroutine(HandleDelayBetweenPassengers(PassengerSeatingManager.PassegnersType.OrangeMan));
                }
            }

            if (hit.collider.gameObject.layer == blueCarLayer)
            {
                if (gameObject.CompareTag("BlueMan"))
                {
                    m_maxDistance = 0f;
                    GetComponent<CapsuleCollider>().enabled = false;
                    StartCoroutine(HandleDelayBetweenPassengers(PassengerSeatingManager.PassegnersType.BlueMan));
                }
            }

            if (hit.collider.gameObject.layer == redCarLayer)
            {
                if (gameObject.CompareTag("RedMan"))
                {
                    m_maxDistance = 0f;
                    GetComponent<CapsuleCollider>().enabled = false;
                    StartCoroutine(HandleDelayBetweenPassengers(PassengerSeatingManager.PassegnersType.RedMan));
                }
            }
        }
    }

    IEnumerator HandleDelayBetweenPassengers(PassengerSeatingManager.PassegnersType passegnersType)
    {
        yield return new WaitForSeconds(0.2f);
        PassengerSeatingManager.Instance.HandlePassengerEntry(passegnersType, transform);
        m_animator.Play("Sitting Idle");
    }
}
