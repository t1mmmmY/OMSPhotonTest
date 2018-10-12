using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour 
{
    public int health { get; set; }
    Animator patientAnimator;
    AudioSource deadSound;

    public bool isDead { get; private set; }

    void Start()
    {
        isDead = false;
        health = 100;
        patientAnimator = GetComponent<Animator>();
        deadSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (health <= 0 && !isDead)
        {
            Dead();
        }
    }

    public void SetHealth(int healthChange)
    {
        health += healthChange;   
    }

    void Dead()
    {
        isDead = true;
        patientAnimator.SetTrigger("Dead");
        deadSound.Play();
    }
}
