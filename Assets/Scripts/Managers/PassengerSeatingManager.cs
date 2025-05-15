using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;


public class PassengerSeatingManager : MonoBehaviour
{
    public static PassengerSeatingManager Instance { get; private set; }

    public enum PassegnersType
    {
        Blue,
        Orange,
        Red,
        Green,
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


    public void HandlePassengerEntry(PassegnersType passegnersType)
    {
        var passengerGroup = m_passegersGroups[(int)passegnersType];

        Sequence passengerEntrySequence = DOTween.Sequence();

        for (int i = 0; i < passengerGroup.Seats.Count; i++)
        {
            var passenger = passengerGroup.Passengers[i];
            var passengerAnimator = passengerGroup.PassengersAnimator[i];
            var seat = passengerGroup.Seats[i];

            passengerEntrySequence.AppendCallback(() =>
            {
                passenger.DOMove(seat.position, 0.2f).OnComplete(() =>
                {
                    passenger.SetParent(seat);
                    passenger.localPosition = Vector3.zero;
                    passengerAnimator.Play("Sitting Idle");
                });
            });

            passengerEntrySequence.AppendInterval(0.2f); // small delay between passengers
        }

        // The car is full off.
        passengerEntrySequence.OnComplete(() =>
        {
            CarShrinkManager.Instance.ShrinkCar((CarShrinkManager.CarType)passegnersType);
        });
    }
}
