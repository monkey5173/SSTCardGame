using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCard : MonoBehaviour
{
    private int cost;               // �ڽ�Ʈ
    private int attackPower;        // ���ݷ�
    private int maxHealth;          // �ִ� ü��
    private int currentHealth;      // ���� ü��

    private bool canAttack = false;     // ���� ���� ����(�Ͽ� ����)

    // �ʵ忡 �������� ���� Ȱ��ȭ �ؾ� ��(�ʱ⿣ Ȱ��ȭ OFF)
    private void Awake()
    {
        enabled = false;
    }

    // �ʵ忡 �������� �� ���� �޾ƿ� ���ݷ�, ü���� �����Ѵ�.
    public void Init(int costValue, int attack, int health)
    {
        cost = costValue;
        attackPower = attack;
        maxHealth = health;
        currentHealth = maxHealth;
        enabled = true;
        canAttack = false;
    }

    // ������ ���������� �� ȣ���ϴ� �Լ�
    public void EnableAttack()
    {
        canAttack = true;
    }

    // ���� ���� ���θ� �Ǵ��� �� ȣ���ϴ� �Լ�
    public bool CanAttack()
    {
        return canAttack;
    }

    // ��� ī�带 ������ �� ȣ���ϴ� �Լ�
    public void Attack(FieldCard target)
    {
        if (!canAttack)
        {
            return;
        }

        Debug.Log($"��� ī�带 {attackPower} �� �������� �����߽��ϴ�.");

        target.TakeDamage(attackPower);
        TakeDamage(target.attackPower);
    }

    // �������� ���� �� ȣ���ϴ� �Լ�
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ī�尡 �׾��� ��� ȣ���ϴ� �Լ�
    public void Die()
    {
        CardFieldManager fieldManager = FindObjectOfType<CardFieldManager>();
        fieldManager.RemoveCard(this);

        Destroy(gameObject);
    }
}
