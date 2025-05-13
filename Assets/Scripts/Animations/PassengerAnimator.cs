using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerAnimatorManager : MonoBehaviour
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
}
