using DefaultNamespace;
using UnityEngine;

namespace SlivaRtfJam.Scripts.Guns
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float liveTime = 5;
        private float damage;
        private Beat beatType;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private new Rigidbody2D rigidbody;
        private bool isHit = false;
        private bool needImmediateDestroy;

        public float Damage => damage;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            needImmediateDestroy = GetComponent<TrailRenderer>() == null;
        }

        public void LaunchProjectile(Vector3 direction, float speed, float damage, Beat beatType = Beat.Enemy)
        {
            this.beatType = beatType;
            this.damage = damage;
            rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
            Destroy(gameObject, liveTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Healthable healthable))
            {
                // if (TryGetComponent(out Healthable healthable))
                if (other.CompareTag("Player") && beatType is Beat.Player or Beat.Both
                   )
                {
                    healthable.TakeDamage(damage);
                    HideObj();
                }
                else if (other.TryGetComponent(out Enemy enemy) && beatType is Beat.Enemy or Beat.Both)
                {
                    if (!enemy.IsDestroyingSelf)
                    {
                        healthable.TakeDamage(damage);
                        HideObj();
                    }
                }
            }
            else if (other.GetComponent<BorderGenerator>() && beatType is Beat.Enemy)
            {
                HideObj();
            }
        }

        private void HideObj()
        {
            if (needImmediateDestroy)
            {
                Destroy(gameObject);
            }
            else
            {
                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector2.zero;
                spriteRenderer.gameObject.SetActive(false);
            }
        }
    }

    public enum Beat
    {
        Player,
        Enemy,
        Both
    }
}