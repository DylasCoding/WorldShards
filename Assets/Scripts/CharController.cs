using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        if (skill.isMovementSkill) { StartCoroutine(HandleMovementSkill(skill)); }

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
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = new Vector2(currentPosition.x + skill.targetPositionOffset.x,
                                        currentPosition.y + skill.targetPositionOffset.y);
        if (role == CharacterRole.Enemy)
        {
            if (skill.isJumpSkill)
            {
                targetPosition = new Vector2(originalPosition.x - skill.jumpForce.x, originalPosition.y - skill.jumpForce.y);
                yield return StartCoroutine(SweepToTarget(targetPosition, skill.jumpTime));
            }
            targetPosition = new Vector2(originalPosition.x - skill.targetPositionOffset.x,
                                        originalPosition.y - skill.targetPositionOffset.y);
            yield return StartCoroutine(SweepToTarget(targetPosition, skill.animationEventTime));
        }
        else
        {
            if (skill.isJumpSkill)
            {
                targetPosition = new Vector2(originalPosition.x + skill.jumpForce.x, originalPosition.y + skill.jumpForce.y);
                yield return StartCoroutine(SweepToTarget(targetPosition, skill.jumpTime));
                Debug.Log("jump" + targetPosition + " " + skill.jumpTime);
            }
            targetPosition = new Vector2(originalPosition.x + skill.targetPositionOffset.x,
                                        originalPosition.y + skill.targetPositionOffset.y);
            yield return StartCoroutine(SweepToTarget(targetPosition, skill.animationEventTime));
        }
    }

    private IEnumerator SweepToTarget(Vector2 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector2 currentPosition = transform.position;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * 1.25f;
            float t = elapsedTime / duration;
            // float smoothT = 1f - Mathf.Pow(1f - t, 3);
            float smoothT = 1f - Mathf.Pow(2f, -10f * t);
            transform.position = Vector2.Lerp(currentPosition, targetPosition, smoothT);
            yield return null;
        }
        transform.position = targetPosition;
    }

    private IEnumerator WaitAnimationFinish(string clipName)
    {
        float animationLength = GetAnimationClipLength(clipName);
        yield return new WaitForSeconds(animationLength);
        animator.SetInteger("SkillNumber", 0);
        OnMovementComplete();
        isAttacking = false;
    }

    public void OnMovementComplete()
    {
        // transform.position = new Vector2(originalPosition.x, originalPosition.y);
        Vector2 nowPosition = new Vector2(originalPosition.x, originalPosition.y);
        StartCoroutine(SweepToTarget(nowPosition, 0.5f));
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
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("TakeDamage", false);
    }
}