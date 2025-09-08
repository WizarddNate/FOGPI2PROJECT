using UnityEngine;

public class Shotgun : Weapon
{
    public override void Use()
    {
        Debug.Log("Player used a shotgun!! Pow Pow!!");
    }

    public void Equipped()
    {
        Debug.Log("Equipped: " + gameObject.name);
    }
}
