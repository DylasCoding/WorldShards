using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    public enum CharacterRole { Player, Enemy };
    public CharacterRole role = CharacterRole.Player;
    public CharacterData characterData;
    private Animator animator;
    private Vector2 originalPosition;
    public int currentHealth;
    public bool isAttacking = false;

    //save skill, target
    private SkillData currentSkill;
    private CharController targetForDamage;

    void Start()
    {
        currentHealth = characterData.maxHealth;
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
        animator.runtimeAnimatorController = characterData.animatorController;
    }

    void Update()
    {

    }

    public void useSkill(int index, CharController target)
    {
        if (isAttacking) return;
        isAttacking = true;
        SkillData skill = GetSkill(index);
        if (skill == null) return;

        animator.SetInteger("SkillNumber", index);

        if (skill.isMovementSkill)
        {
            StartCoroutine(HandleMovementSkill(skill));
            StartCoroutine(WaitAnimationFinish(skill.skillAnimation.name));
        }
        // StartCoroutine(DealDamage(skill, target));
        this.targetForDamage = target;
        this.currentSkill = skill;
        StartCoroutine(WaitAnimationFinish(skill.skillAnimation.name));

    }

    private SkillData GetSkill(int index)
    {
        switch (index)
        {
            case 1: return characterData.basicAttack;
            case 2: return characterData.specialSkill1;
            case 3: return characterData.specialSkill2;
            default: return null;
        }
    }

    private IEnumerator HandleMovementSkill(SkillData skill)
    {

        // Move to target position if enemy
        if (role == CharacterRole.Enemy)
        {
            if (skill.isJumpSkill)
            {
                yield return new WaitForSeconds(skill.jumpTime);
                transform.position = new Vector2(originalPosition.x - skill.jumpForce.x, originalPosition.y - skill.jumpForce.y);
            }
            yield return new WaitForSeconds(skill.animationEventTime);
            transform.position = new Vector2(originalPosition.x - skill.targetPositionOffset.x,
                                        originalPosition.y - skill.targetPositionOffset.y);
        }
        else
        {
            if (skill.isJumpSkill)
            {
                yield return new WaitForSeconds(skill.jumpTime);
                transform.position = new Vector2(originalPosition.x + skill.jumpForce.x, originalPosition.y + skill.jumpForce.y);
            }
            yield return new WaitForSeconds(skill.animationEventTime);
            transform.position = new Vector2(originalPosition.x + skill.targetPositionOffset.x,
                                        originalPosition.y + skill.targetPositionOffset.y);
        }
    }

    private IEnumerator WaitAnimationFinish(string clipName)
    {
        float animationLength = GetAnimationClipLength(clipName);
        yield return new WaitForSeconds(animationLength);
        isAttacking = false;
        animator.SetInteger("SkillNumber", 0);
        OnMovementComplete();
    }

    public void OnMovementComplete()
    {
        transform.position = new Vector2(originalPosition.x, originalPosition.y);
    }

    private float GetAnimationClipLength(string clipName)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        return 0f;
    }

    // private IEnumerator DealDamage(SkillData skill, CharController target)
    // {
    //     yield return new WaitForSeconds(skill.animationEventTime);
    //     if (target != null)
    //     {
    //         int basicDamage = skill.damageIncrease + characterData.attackDamage;
    //         int totalDamage = basicDamage + (int)(basicDamage * skill.damageMultiplier);
    //         target.TakeDamage(totalDamage);
    //     }
    // }

    public void OnAnimationDealDamage()
    {
        if (targetForDamage != null)
        {
            int basicDamage = currentSkill.damageIncrease + characterData.attackDamage;
            int totalDamage = basicDamage + (int)(basicDamage * currentSkill.damageMultiplier);
            targetForDamage.TakeDamage(totalDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(characterData.characterName + " take " + damage + " damage! Hp: " + currentHealth);
        animator.SetBool("TakeDamage", true);

        if (currentHealth <= 0)
        {
            Die();
        }
        StartCoroutine(ResetTakeDamage());
    }

    private void Die()
    {
        Debug.Log(characterData.characterName + " was defeated!");
        gameObject.SetActive(false);
    }

    // reset animation when take damage
    private IEnumerator ResetTakeDamage()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("TakeDamage", false);
    }
}