using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponManager))]
public class WeaponManagerEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WeaponManager wm = (WeaponManager)target;

        if (GUILayout.Button("Prev"))
        {
            wm.PrevWeapon();
        }

        if (GUILayout.Button("Next"))
        {
            wm.NextWeapon();
        }

        if (GUILayout.Button("Use"))
        {
            wm.Use();
        }
    }
}

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> weapons;
    public Weapon currentWeapon;
    private int weaponIndex = 0;

    public void Use()
    {
        if (currentWeapon)
            currentWeapon.Use();
    }

    public void PrevWeapon()
    {
        //if there are no weapons, or theres only one weapon, then leave prev weapon
        if (weapons.Count <= 1)
            return;

        weaponIndex--;

        if (weaponIndex < 0)
            weaponIndex = weapons.Count - 1;
        currentWeapon = weapons[weaponIndex];
        currentWeapon.equipped.Invoke();
    }

    public void NextWeapon()
    {
        if (weapons.Count <= 1)
            return;
        weaponIndex++;

        if (weaponIndex >= weapons.Count)
            weaponIndex = 0;
        currentWeapon = weapons[weaponIndex];
    }
}