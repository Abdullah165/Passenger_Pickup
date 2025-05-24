using System;
using System.Collections.Generic;
using UnityEngine;

public class PassageController : MonoBehaviour
{
    [Serializable]
    public class TagMaterialPair
    {
        public string PassengerType;
        public string Tag;
        public Material Material;
    }

    [SerializeField] private List<Transform> m_passengers;

    [SerializeField] private List<TagMaterialPair> m_tagMaterialPairs;

    private Dictionary<string, (string Tag, Material Material)> m_tagToMaterialDic;

    public bool isActive;

    private void Awake()
    {
        m_tagToMaterialDic = new Dictionary<string, (string, Material)>();

        foreach (var item in m_tagMaterialPairs)
        {
            if (!m_tagToMaterialDic.ContainsKey(item.PassengerType))
            {
                m_tagToMaterialDic.Add(item.PassengerType, (item.Tag, item.Material));
            }
        }
    }

    private void Start()
    {
        PassengerSeatingManager.Instance.OnAllowedPassengersGetIn += Instance_OnAllowedPassengersGetIn;
    }

    private void Instance_OnAllowedPassengersGetIn(object sender, EventArgs e)
    {
        if (isActive)
        {
            RearrangePassengers();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Change the passage's material depending on the passenger type.
        if (m_tagToMaterialDic.ContainsKey(other.gameObject.tag))
        {
            (string tag, _) = m_tagToMaterialDic[other.gameObject.tag];
            gameObject.tag = tag;


            if (TryGetComponent<MeshRenderer>(out var meshRenderer))
            {
                Material[] mats = meshRenderer.materials;

                if (mats.Length > 1)
                {
                    (_, Material mat) = m_tagToMaterialDic[other.gameObject.tag];
                    mats[1] = mat;
                    meshRenderer.materials = mats;
                }
            }
        }

        if (other.gameObject.CompareTag("OrangeCar") || other.gameObject.CompareTag("BlueCar") || other.gameObject.CompareTag("RedCar"))
        {
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OrangeCar") || other.CompareTag("BlueCar") || other.gameObject.CompareTag("RedCar"))
        {
            isActive = false;
        }
    }

    private void RearrangePassengers()
    {
        m_passengers.Remove(m_passengers[0]);

        for (int i = 0; i < m_passengers.Count; i++)
        {
            float spacing = 0.5f;
            var passenger = m_passengers[i];

            Vector3 moveDir = -passenger.forward;
            passenger.position += moveDir * spacing;
            //passenger.DOMove(passenger.position + moveDir * spacing, 0.05f);
        }
    }
}
