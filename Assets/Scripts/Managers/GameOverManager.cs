using System;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    public event EventHandler OnGameOver;

    public enum CarType
    {
        Blue,
        Orange,
        Red,
    }

    [Serializable]
    public class Car
    {
        public CarType CarType;
        public bool IsCarFull;
    }

    [Tooltip("Insert how many cars we have at the current level.")]
    [SerializeField] private List<Car> m_cars;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCarFull(CarType carType)
    {
        int index = (int)carType;

        if (index < 0 || index >= m_cars.Count) return;

        var car = m_cars[index];
        car.IsCarFull = true;

        if (AreAllCarsFull())
        {
            OnGameOver?.Invoke(this, EventArgs.Empty);
        }
    }


    private bool AreAllCarsFull()
    {
        foreach (var car in m_cars)
        {
            if (!car.IsCarFull)
                return false;
        }
        return true;
    }

}
