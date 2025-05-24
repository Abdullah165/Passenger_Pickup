using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueExit") && gameObject.CompareTag("BlueCar"))
        {
            var passengerType = PassengerSeatingManager.PassegnersType.BlueMan;

            if (PassengerSeatingManager.Instance.IsCarFull(passengerType))
            {
                CarShrinkManager.Instance.ShrinkCar((CarShrinkManager.CarType)passengerType);
            }
        }

        if (other.CompareTag("OrangeExit") && gameObject.CompareTag("OrangeCar"))
        {
            var passengerType = PassengerSeatingManager.PassegnersType.OrangeMan;

            if (PassengerSeatingManager.Instance.IsCarFull(passengerType))
            {
                CarShrinkManager.Instance.ShrinkCar((CarShrinkManager.CarType)passengerType);
            }
        }

        if (other.CompareTag("RedExit") && gameObject.CompareTag("RedCar"))
        {
            var passengerType = PassengerSeatingManager.PassegnersType.RedMan;

            if (PassengerSeatingManager.Instance.IsCarFull(passengerType))
            {
                CarShrinkManager.Instance.ShrinkCar((CarShrinkManager.CarType)passengerType);
            }
        }
    }
}
