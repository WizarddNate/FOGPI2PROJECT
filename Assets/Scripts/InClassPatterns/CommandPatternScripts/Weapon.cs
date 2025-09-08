using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public UnityEvent equipped;

    public UnityEvent reload;
    public virtual void Use() {}
}
