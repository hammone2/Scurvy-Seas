using UnityEngine;

public class AiShip : MonoBehaviour
{

    private ShipMovement ship;

    private void Awake()
    {
        ship = GetComponent<ShipMovement>();
    }

    void Start()
    {
        ship.HandleThrust(0.5f);
    }
}
