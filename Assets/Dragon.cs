using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;

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
            Destroy(gameObject, 5f); // Usuñ obiekt po 2 sekundach
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("damage");
            }
        }
    }

}
