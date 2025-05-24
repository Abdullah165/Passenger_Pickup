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

        Vector3 headStartPosition = headPart.position;
        Vector3 headFallTarget = headStartPosition + Vector3.down * 3f;

        carShrinkSequence.AppendCallback(() =>
        {
            Vector3 currentRotation = headPart.eulerAngles;
            float randomX = UnityEngine.Random.Range(10f, 37f);

            headPart.DORotate(new Vector3(randomX, currentRotation.y, currentRotation.z), 0.3f);
            headPart.DOMoveY(headFallTarget.y, 0.4f).SetEase(Ease.InQuad);
        });

        carShrinkSequence.AppendInterval(0.05f); // Make a delay before other parts begin

        // Animate other car segments
        for (int i = 1; i < car.CarBodyParts.Count; i++)
        {
            var currentPart = car.CarBodyParts[i];

            carShrinkSequence.AppendCallback(() =>
            {
                Vector3 currentRotation = currentPart.eulerAngles;
                float randomX = UnityEngine.Random.Range(10f, 37f);

                currentPart.DOMove(headStartPosition, 0.3f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    currentPart.DORotate(new Vector3(randomX, currentRotation.y, currentRotation.z), 0.3f);
                    currentPart.DOMoveY(headFallTarget.y, 0.4f).SetEase(Ease.InQuad);
                });
            });

            carShrinkSequence.AppendInterval(0.1f); 
        }

        carShrinkSequence.OnComplete(() =>
        {
            GameOverManager.Instance.SetCarFull((GameOverManager.CarType)carType);
        });
    }

}
