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
        GreenMan,
    }

    [Serializable]
    public class PassegersGroup
    {
        public PassegnersType type;
        public List<Transform> Seats;
        public List<Transform> Passengers;
        public List<Animator> PassengersAnimator;
    }

    [SerializeField] private List<PassegersGroup> m_passegersGroups;

    private void Awake()
    {
        Instance = this;
    }


    public void HandlePassengerEntry(PassegnersType passegnersType, Transform passenger)
    {
        var passengerGroup = m_passegersGroups[(int)passegnersType];

        if (passengerGroup.Passengers.Count == 0 || passengerGroup.Seats.Count == 0)
            return;

        // 1. Reserve seat and animator immediately
        var seat = passengerGroup.Seats[0];
        var passengerAnimator = passengerGroup.PassengersAnimator[0];

        passengerGroup.Seats.RemoveAt(0);
        passengerGroup.Passengers.RemoveAt(0);
        passengerGroup.PassengersAnimator.RemoveAt(0);

        passenger.DOMove(seat.position, 0.3f).OnComplete(() =>
        {
            passenger.SetParent(seat);
            passenger.localPosition = Vector3.zero;
            //passengerAnimator.Play("Sitting Idle");

            if (passengerGroup.Passengers.Count <= 0)
            {
                CarShrinkManager.Instance.ShrinkCar((CarShrinkManager.CarType)passegnersType);
            }

            OnAllowedPassengersGetIn?.Invoke(this, EventArgs.Empty);
        });

    }
}
