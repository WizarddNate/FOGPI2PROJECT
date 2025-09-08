using UnityEngine;

public class Pistol : Weapon
{
    public override void Use()
    {
        Debug.Log("Player shot 9mm!! Bang Bang!!");
    }

    public void Equipped()
    {
        Debug.Log("Equipped: " + gameObject.name);
    }
}
