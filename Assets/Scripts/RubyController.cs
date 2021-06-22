using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public int health { get { return currentHeath; } }
    int currentHeath;
    public float speed;
    Rigidbody2D rigidbody2DL;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;

    public ParticleSystem damageEffect;
    public ParticleSystem CollectibleEffect;

    public float timeInvincible = 2.0f; // врем€ неу€звимости после дамаги
    bool invincible; // маркер неу€звимости
    float invincibleTimer;



    void Start()
    {
        rigidbody2DL = GetComponent<Rigidbody2D>();
        currentHeath = maxHealth;
        animator = GetComponent<Animator>();
    }

   
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 position = rigidbody2DL.position;
        position.x = position.x + horizontal*speed*Time.deltaTime;
        position.y = position.y + vertical * speed * Time.deltaTime;
       rigidbody2DL.MovePosition(position);

        if (invincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                invincible = false;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2DL.position + Vector2.up * 0.2f,
                lookDirection, 1.5f, LayerMask.GetMask("NPS"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }

        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (invincible)
                return;
            invincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            ParticleSystem damageParticle = Instantiate(damageEffect, rigidbody2DL.position + Vector2.up * 0.5f, Quaternion.identity);
        }
        if (amount > 0)
        {
            ParticleSystem collectParticle = Instantiate(CollectibleEffect, rigidbody2DL.position + Vector2.up * 0.5f, Quaternion.identity);
        }
        currentHeath = Mathf.Clamp(currentHeath + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHeath / (float)maxHealth);
        Debug.Log(currentHeath + "/" + maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2DL.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

 //       animator.SetTrigger("Launch");
    }
}
