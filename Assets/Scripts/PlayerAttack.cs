using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 1f;
    public LayerMask attackMask;
    public Vector3 attackOffset;

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        Collider2D hitObject = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (hitObject)
        {
            Enemy enemy = hitObject.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.Hit();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        Gizmos.DrawWireSphere(pos, attackRange);
    }
}
