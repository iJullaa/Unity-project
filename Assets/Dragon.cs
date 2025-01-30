using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dragon : MonoBehaviour
{
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;

    private float lastAttackTime = 0f;
    public float attackDelay = 5f;

    public GameObject WinCond;

    private void Start()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = HP;
            healthBar.value = HP;
        }
        if (WinCond == null)
        {
            WinCond = GameObject.Find("WinCond");
        }
    }

    private void Update()
    {
        healthBar.value = HP;
    }
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("die");
            }
            Destroy(gameObject, 2f);
            WinCond.SetActive(true);
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("damage");
            }
        }
    }

    public void PerformAttack(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(10); // Zadaj obra¿enia graczowi
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Time.time - lastAttackTime > attackDelay)
        {
            PerformAttack(other);
            lastAttackTime = Time.time;
        }
    }
}
