using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    public int damage;
    public float speed;
    public float destroyTime;

    private Rigidbody2D rigid;

    public virtual void Initialize(GameObject target, int damage, float speed, float destroyTime)
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
    
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Debug.Log(collision.gameObject.tag);
            switch (collision.gameObject.tag)
            {
                case ("Ground"):
                case ("Platform"):
                    Destroy(gameObject);
                    break;
                case ("Enemy"):
                    break;
            }
        }
    }
}
