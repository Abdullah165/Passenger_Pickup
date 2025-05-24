using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PassengerSeatingManager : MonoBehaviour
{
    public static PassengerSeatingManager Instance { get; private set; }

    public event EventHandler OnAllowedPassengersGetIn;

    public enum PassegnersType
    {
        BlueMan,
        OrangeMan,
        RedMan,
    }

    [Serializable]
    public class PassegersGroup
    {
        public PassegnersType type;
        public List<Transform> Seats;
    }

    [SerializeField] private List<PassegersGroup> m_passegersGroups;

    private void Awake()
    {
        Instance = this;
    }


    public void HandlePassengerEntry(PassegnersType passegnersType, Transform passenger)
    {
        var passengerGroup = m_passegersGroups[(int)passegnersType];

        if (passengerGroup.Seats.Count == 0)
            return;

        var seat = passengerGroup.Seats[0];

        passengerGroup.Seats.RemoveAt(0);

        passenger.DOMove(seat.position, 0.3f).OnComplete(() =>
        {
            passenger.SetParent(seat);
            passenger.localPosition = Vector3.zero;


            OnAllowedPassengersGetIn?.Invoke(this, EventArgs.Empty);
        });

    }

    public bool IsCarFull(PassegnersType passegnersType)
    {
        var passengerGroup = m_passegersGroups[(int)passegnersType];

        return passengerGroup.Seats.Count <= 0;
    }
}
