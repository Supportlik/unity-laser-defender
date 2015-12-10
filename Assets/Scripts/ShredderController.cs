using UnityEngine;
using System.Collections;

public class ShredderController : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D collider) {
        Destroy(collider.gameObject);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.lossyScale.x, transform.lossyScale.y));
    }
}
