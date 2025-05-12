using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BluePassage"))
        {
            PassengerSeatingManager.Instance.HandleBluePassengerEntry(PassengerSeatingManager.PassegnersType.Blue);
        }
    }
}
