using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CarShrinkManager : MonoBehaviour
{
    public static CarShrinkManager Instance { get; private set; }

    public enum CarType
    {
        Blue,
        Orange,
        Red,
    }

    [Serializable]
    public class CarParts
    {
        public CarType CarType;
        public List<Transform> CarBodyParts;
    }

    [SerializeField] private List<CarParts> m_carPartsList;

    private void Awake()
    {
        Instance = this;
    }

    public void ShrinkCar(CarType carType)
    {
        var car = m_carPartsList[(int)carType];

        Sequence carShrinkSequence = DOTween.Sequence();

        var headPart = car.CarBodyParts[0];

        for (int i = 0; i < car.CarBodyParts.Count; i++)
        {
            if (car.CarBodyParts[i] == headPart) continue;

            var currentPart = car.CarBodyParts[i];

            carShrinkSequence.AppendCallback(() =>
            {
                currentPart.DOMove(headPart.position, 0.3f).OnComplete(() =>
                {
                    currentPart.DOScale(0, 0.2f);
                });
            });

            carShrinkSequence.AppendInterval(0.05f);
        }

        carShrinkSequence.OnComplete(() =>
        {
            headPart.DOScale(0, 0.2f);
            GameOverManager.Instance.SetCarFull((GameOverManager.CarType)carType);
        });
    }
}
