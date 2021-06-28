using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;


public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public int health { get { return currentHeath; } }

    public int CountBrokenEnemy {
        get
        {
            return countBrokenEnemy;
        }
        set 
        {
            countBrokenEnemy = value; 
        } }

    public float speed;                                 // скорость персонажа.
    private volatile int countBrokenEnemy;              // количество роботов которых надо починить.
    public AudioClip audioTrowCog;
    public float timeInvincible = 2.0f;                 // врем€ неу€звимости после получени€ дамаги.

    public GameObject projectilePrefab;
    public ParticleSystem damageEffect;
    public ParticleSystem CollectibleEffect;

    int currentHeath;
    float invincibleTimer;
    bool invincible; // маркер неу€звимости    

    Rigidbody2D rigidbody2DL;
    AudioSource audioSource;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    void Start()
    {
        currentHeath = LoadBuffer.currentHealthRuby;
        CountBrokenEnemy = LoadBuffer.countBrokenRobot_L1;
        Debug.Log("Ѕыло при старте" + CountBrokenEnemy);
        rigidbody2DL = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

   
    void FixedUpdate()
    {
if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);   
        }

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
        if (CountBrokenEnemy == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        LoadBuffer.currentHealthRuby = currentHeath;
        Debug.Log("ѕри дамаге " + currentHeath);
        HealthEnergi();
 //       Debug.Log(currentHeath + "/" + maxHealth);
    }

    public void HealthEnergi()
    {
        UIHealthBar.instance.SetValue(currentHeath / (float)maxHealth);
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2DL.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        audioSource.PlayOneShot(audioTrowCog);
        animator.SetTrigger("Launch");
    }
     public void DetectingEnemy()
    {
        Debug.Log("Ѕыло "+CountBrokenEnemy);     
            CountBrokenEnemy--;       
        Debug.Log("стало " + CountBrokenEnemy);
    }
    public void PlaySound (AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void LoadConstsant(int currentHit)
    {
        LoadBuffer.currentHealthRuby = currentHit;
    }
}
