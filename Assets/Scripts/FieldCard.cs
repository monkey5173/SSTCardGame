using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCard : MonoBehaviour
{
    private int cost;               // 코스트
    private int attackPower;        // 공격력
    private int maxHealth;          // 최대 체력
    private int currentHealth;      // 현재 체력

    private bool canAttack = false;     // 공격 가능 여부(턴에 따라)

    // 필드에 출전했을 때만 활성화 해야 함(초기엔 활성화 OFF)
    private void Awake()
    {
        enabled = false;
    }

    // 필드에 출전했을 때 값을 받아와 공격력, 체력을 설정한다.
    public void Init(int costValue, int attack, int health)
    {
        cost = costValue;
        attackPower = attack;
        maxHealth = health;
        currentHealth = maxHealth;
        enabled = true;
        canAttack = false;
    }

    // 공격이 가능해졌을 때 호출하는 함수
    public void EnableAttack()
    {
        canAttack = true;
    }

    // 공격 가능 여부를 판단할 때 호출하는 함수
    public bool CanAttack()
    {
        return canAttack;
    }

    // 상대 카드를 공격할 때 호출하는 함수
    public void Attack(FieldCard target)
    {
        if (!canAttack)
        {
            return;
        }

        Debug.Log($"상대 카드를 {attackPower} 의 데미지로 공격했습니다.");

        target.TakeDamage(attackPower);
        TakeDamage(target.attackPower);
    }

    // 데미지를 입을 때 호출하는 함수
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 카드가 죽었을 경우 호출하는 함수
    public void Die()
    {
        CardFieldManager fieldManager = FindObjectOfType<CardFieldManager>();
        fieldManager.RemoveCard(this);

        Destroy(gameObject);
    }
}
