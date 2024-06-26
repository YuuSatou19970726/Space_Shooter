using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f;
    public float rotate_Speed = 50f;

    public bool canShoot;
    public bool canRotate;
    private bool canMove = true;

    public float bound_X = -11f;

    public Transform attack_Point;
    public GameObject bulletPrefab;

    private Animator anim;
    private AudioSource explosionSound;

    void Awake()
    {
        anim = GetComponent<Animator>();
        explosionSound = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (canRotate)
        {
            if (Random.Range(0, 2) > 0)
            {
                rotate_Speed = Random.Range(rotate_Speed, rotate_Speed + 20f);
                rotate_Speed *= -1f;
            }
            else
            {
                rotate_Speed = Random.Range(rotate_Speed, rotate_Speed + 20f);
            }
        }

        if (canShoot)
            Invoke(nameof(StartShooting), Random.Range(1f, 3f));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        RotateEnemy();
    }

    void Move()
    {
        if (canMove)
        {
            Vector3 temp = transform.position;
            temp.x -= speed * Time.deltaTime;
            transform.position = temp;

            if (temp.x < bound_X)
                gameObject.SetActive(false);
        }
    }

    void RotateEnemy()
    {
        if (canRotate)
        {
            transform.Rotate(new Vector3(0f, 0f, rotate_Speed * Time.deltaTime), Space.World);
        }
    }

    void StartShooting()
    {
        GameObject bullet = Instantiate(bulletPrefab, attack_Point.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().is_EnemyBullet = true;

        if (canShoot)
            Invoke(nameof(StartShooting), Random.Range(1f, 3f));
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == Tags.BULLET_TAG)
        {
            canMove = false;

            if (canShoot)
            {
                canShoot = false;
                CancelInvoke(nameof(StartShooting));
            }

            Invoke(nameof(TurnOffGameObject), 3f);

            // play explosion sound
            explosionSound.Play();

            anim.Play(AnimationTags.DESTROY_ANIM);
        }
    }
}
