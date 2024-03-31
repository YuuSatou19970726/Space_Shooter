using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    public float min_Y = -4.3f, max_Y = 4.3f;

    [SerializeField]
    private GameObject player_Bullet;

    [SerializeField]
    private Transform attack_Point;

    public float attack_Timer = 0.35f;
    private float current_Attack_Timer;
    private bool canAttack;

    private AudioSource laserAudio;

    void Awake()
    {
        laserAudio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        current_Attack_Timer = attack_Timer;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Attack();
    }

    void MovePlayer()
    {
        if (Input.GetAxisRaw(Axis.VERTICAL_AXIS) > 0f)
        {
            Vector3 temp = transform.position;
            temp.y += speed * Time.deltaTime;

            if (temp.y > max_Y)
                temp.y = max_Y;

            transform.position = temp;
        }
        else if (Input.GetAxisRaw(Axis.VERTICAL_AXIS) < 0f)
        {
            Vector3 temp = transform.position;
            temp.y -= speed * Time.deltaTime;

            if (temp.y < min_Y)
                temp.y = min_Y;

            transform.position = temp;
        }
    }

    void Attack()
    {
        attack_Timer += Time.deltaTime;

        if (attack_Timer > current_Attack_Timer)
            canAttack = true;

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (canAttack)
            {
                canAttack = false;
                attack_Timer = 0f;

                Instantiate(player_Bullet, attack_Point.position, Quaternion.identity);

                // play the sound FX
                laserAudio.Play();
            }
        }
    }
}
