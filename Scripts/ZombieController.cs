using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private float detectionRange;
    [SerializeField] private float zombieSpeed;
    [SerializeField] private float forgetPlayer;
    private GameObject player;
    private Rigidbody2D rb;
    private Animator anim;
    private enum ZombieMovement {idle, walk};


    private bool isDetected;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        ZombieMovement zombieState = ZombieMovement.idle;

        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance < detectionRange)
        {
            isDetected = true;
            if (transform.position.x > player.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(new Vector2(0, 1) * 180);
                zombieState = ZombieMovement.walk;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, zombieSpeed * Time.deltaTime);

            } else if (transform.position.x < player.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(new Vector2(0, 1) * 0);
                zombieState = ZombieMovement.walk;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, zombieSpeed * Time.deltaTime);
            }


        } else
        {
            zombieState = ZombieMovement.idle;
        }

        if (isDetected && distance < forgetPlayer)
        {
            if (transform.position.x > player.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(new Vector2(0, 1) * 180);
                zombieState = ZombieMovement.walk;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, zombieSpeed * Time.deltaTime);

            }
            else if (transform.position.x < player.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(new Vector2(0, 1) * 0);
                zombieState = ZombieMovement.walk;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, zombieSpeed * Time.deltaTime);
            }
        } else
        {
            isDetected = false;
        }

        anim.SetInteger("zombieState", (int)zombieState);


        
    }
}
