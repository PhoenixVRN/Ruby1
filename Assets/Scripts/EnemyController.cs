using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    bool broken = true;

    Rigidbody2D _rigidbody2D;
    float timer;
    int direction = 1;
    Animator animator;
    public ParticleSystem smokeEffect;

    AudioSource audioSource;
    public AudioClip collectedClipDamag;
    public AudioClip collectedClipFix;

    public RubyController Ruby;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
       
    }

    void Update()
    {
        if (!broken)
        {
            return;
        }
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {

        if (!broken)
        {
            return;
        }
        Vector2 position = _rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction; ;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);

        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        _rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);

            player.PlaySound(collectedClipDamag);
        }
    }
     public void Fix()
    {
        broken = false;
        _rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
        audioSource.PlayOneShot(collectedClipFix);
        smokeEffect.Stop();
        Ruby.DetectingEnemy();
       
    }
}
