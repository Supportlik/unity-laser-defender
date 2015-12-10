using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

    public float damage;

    public float GetDamage() {
        return damage;
    }

    public void Hit() {
        Destroy(gameObject);
    }
}
