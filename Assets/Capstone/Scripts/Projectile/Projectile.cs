using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    public float damage;
    public float speed;
    public float destroyTime;

    private Rigidbody2D rigid;

    public virtual void Initialize(GameObject target, float damage, float speed, float destroyTime)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        this.destroyTime = destroyTime;
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        LaunchMethod();
    }

    protected virtual void LaunchMethod()
    {
        rigid.velocity = transform.right * speed;
        Destroy(gameObject, destroyTime);
    }
}
