using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;
    [SerializeField] float radius;
    [SerializeField] Vector3 offset;
    private bool onGround;

    public bool OnGround { get => onGround; }

    private void Update()
    {
        onGround = Physics2D.OverlapCircle(transform.position + offset, radius, groundMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}
