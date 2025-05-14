using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BluePassage") && gameObject.CompareTag("BlueCar"))
        {
            PassengerSeatingManager.Instance.HandlePassengerEntry(PassengerSeatingManager.PassegnersType.Blue);
        }

        if (other.gameObject.CompareTag("OrangePassage") && gameObject.CompareTag("OrangeCar"))
        {
            PassengerSeatingManager.Instance.HandlePassengerEntry(PassengerSeatingManager.PassegnersType.Orange);
        }

        if (other.gameObject.CompareTag("RedPassage") && gameObject.CompareTag("RedCar"))
        {
            PassengerSeatingManager.Instance.HandlePassengerEntry(PassengerSeatingManager.PassegnersType.Red);
        }
    }
}
