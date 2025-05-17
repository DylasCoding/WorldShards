using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharController : MonoBehaviour
{
    public enum CharacterRole { Player, Enemy };
    public CharacterRole role = CharacterRole.Player;
    public CharacterData characterData;
    private Animator _animator;
    private Vector2 _originalPosition;
    public bool isAttacking = false;

    [Header("Damage Spawner")]
    [SerializeField] private DamageSpawner _damageSpawner;

    [Header("Effect Prefab")]
    [SerializeField] private SkillEffectHandler _skillEffectHandler;

    [Header("Buff")]
    [SerializeField] private BattleManager _battleManager;


    [Header("Health")]
    private int _currentHealth;
    private int _maxHealth;

    //save skill, target
    private SkillData _currentSkill;
    private CharController _targetForDamage;
    private int _level = 1;

    [Header("UI")]
    [SerializeField] private GameObject _healthBarPrefab;

    private void Start()
    {
        // _maxHealth = characterData.maxHealth;
        // _currentHealth = _maxHealth;
        _animator = GetComponent<Animator>();
        _originalPosition = transform.position;
        _animator.runtimeAnimatorController = characterData.animatorController;

        // HealthUIUpdate(_currentHealth, _maxHealth);

        //update position
        MoveToOriginalPosition();
    }

    private void MoveToOriginalPosition()
    {
        _originalPosition = transform.position;
    }

    public void UpdateCharacterData(CharacterData newCharacterData, int newHealth, int level)
    {
        MoveToOriginalPosition();
        characterData = newCharacterData;
        _currentHealth = newHealth;
        _level = level;
        _maxHealth = newCharacterData.maxHealth + level * newCharacterData.incrementalHealth;

        //update UI
        HealthUIUpdate(_currentHealth, _maxHealth);

        if (_animator == null)
            _animator = GetComponent<Animator>();

        _animator.runtimeAnimatorController = characterData.animatorController;
        isAttacking = false;
        _animator.SetInteger("SkillNumber", 0);
        transform.position = _originalPosition;
    }

    public void useSkill(int index, int level, CharController target, ActionPanelManager panelManager)
    {
        if (isAttacking) return;
        isAttacking = true;

        SkillData skill = GetSkill(index);
        if (skill == null) return;

        // Gọi ShowPanel và truyền callback
        panelManager.ShowPanel(skill, role, () => ContinueSkillLogic(index, level, target, skill));
    }

    private void ContinueSkillLogic(int index, int level, CharController target, SkillData skill)
    {
        _animator.SetInteger("SkillNumber", index);
        AudioManager.Instance.PlaySFX(skill.hitAudioClip);

        if (skill.isMovementSkill)
        {
            StartCoroutine(HandleMovementSkill(skill));
        }

        this._targetForDamage = target;
        this._currentSkill = skill;
        this._level = level;

        if (skill.isBuffSkill)
        {
            Debug.Log("Buff skill used: " + skill.skillName);
            Debug.Log("Name of target: " + target.characterData.characterName);
            _targetForDamage.TakeDamage(0);
        }

        StartCoroutine(WaitAnimationFinish(skill.skillAnimation.name));
    }

    public SkillData GetSkill(int index)
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
        Vector2 targetPosition = new Vector2(currentPosition.x + skill.targetPositionOffset.x, currentPosition.y + skill.targetPositionOffset.y);
        if (role == CharacterRole.Enemy)
        {
            if (skill.isJumpSkill)
            {
                targetPosition = new Vector2(_originalPosition.x - skill.jumpForce.x, _originalPosition.y + skill.jumpForce.y);
                yield return StartCoroutine(SweepToTarget(targetPosition, skill.jumpTime));
            }
            targetPosition = new Vector2(_originalPosition.x - skill.targetPositionOffset.x,
                                        _originalPosition.y + skill.targetPositionOffset.y);
            yield return StartCoroutine(SweepToTarget(targetPosition, skill.animationEventTime));
        }
        else
        {
            if (skill.isJumpSkill)
            {
                targetPosition = new Vector2(_originalPosition.x + skill.jumpForce.x, _originalPosition.y + skill.jumpForce.y);
                yield return StartCoroutine(SweepToTarget(targetPosition, skill.jumpTime));
            }
            targetPosition = new Vector2(_originalPosition.x + skill.targetPositionOffset.x,
                                        _originalPosition.y + skill.targetPositionOffset.y);
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
        _animator.SetInteger("SkillNumber", 0);
        OnMovementComplete();
        // isAttacking = false;
    }

    public void OnMovementComplete()
    {
        // transform.position = new Vector2(originalPosition.x, originalPosition.y);
        Vector2 nowPosition = new Vector2(_originalPosition.x, _originalPosition.y);
        StartCoroutine(SweepToTarget(nowPosition, 0.5f));
    }

    private float GetAnimationClipLength(string clipName)
    {
        RuntimeAnimatorController ac = _animator.runtimeAnimatorController;
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
        if (_targetForDamage != null)
        {
            // int basicDamage = _currentSkill.damageIncrease + characterData.attackDamage;
            // int totalDamage = basicDamage + (int)(basicDamage * _currentSkill.damageMultiplier);
            DamageCalculator damageCalculator = new DamageCalculator();

            Debug.Log($"Character Type: {characterData.characterType}, Target Type: {_targetForDamage.characterData.characterType}");

            int totalDamage = damageCalculator.CalculateDamage(_currentSkill.damageIncrease, _level, characterData, characterData.characterType, _targetForDamage.characterData, _currentSkill);

            if (_currentSkill.isArrowSkill || _currentSkill.isRangeSkill)
            {
                Vector3 actorPosition = transform.position;
                if (_currentSkill.isSpellCastSkill)
                    StartCoroutine(ExecuteSkillEffects(_currentSkill, _targetForDamage, actorPosition, totalDamage));
                else
                    StartCoroutine(_skillEffectHandler.RangedAttackEffect(_currentSkill, _targetForDamage, actorPosition, totalDamage));
            }
            else
            {
                StartCoroutine(_skillEffectHandler.MeleeAttackEffect(_currentSkill.skillEffectPrefab, _targetForDamage, totalDamage));
            }
            // _targetForDamage.TakeDamage(totalDamage);
        }
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public CharacterData GetCharacterData()
    {
        return characterData;
    }
    public void HealthUIUpdate(int currentHealth, int maxHealth)
    {
        if (_healthBarPrefab != null)
        {
            HealthUI healthUI = _healthBarPrefab.GetComponent<HealthUI>();
            if (healthUI != null)
            {
                // Debug.Log($"{characterData.characterName} - Updating HP: {currentHealth}/{maxHealth}");
                healthUI.UpdateHealthBar(currentHealth, maxHealth);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            int defense = characterData.defense + _level * characterData.incrementalDefense;
            damage = Mathf.Max(damage - defense, 0); // Ensure damage is not negative
            _currentHealth -= damage;
            _animator.SetBool("TakeDamage", true);

            HealthUIUpdate(_currentHealth, _maxHealth);
            SpawnDamagePopup(damage);
            StartCoroutine(ReturnInitialState());
        }

        if (_currentHealth <= 0)
        {
            _animator.SetBool("TakeDamage", true);
            Die();
        }

        //next turn
        isAttacking = false;

        Debug.Log(characterData.characterName + " took " + damage + " damage! Remaining health: " + _currentHealth);

        _battleManager.EndTurn();
    }

    private void SpawnDamagePopup(int damage)
    {
        if (_damageSpawner != null)
        {
            Vector2 spawnPosition = new Vector2(_originalPosition.x, _originalPosition.y + 1f);
            _damageSpawner.SpawnDamagePopup(spawnPosition, damage);
        }
    }

    private void Die()
    {
        Debug.Log(characterData.characterName + " was defeated!");
    }

    // reset animation when take damage
    private IEnumerator ReturnInitialState()
    {
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool("TakeDamage", false);
    }

    private IEnumerator ExecuteSkillEffects(SkillData skillData, CharController target, Vector3 actorPosition, int totalDamage)
    {
        // Chờ SpellCastEffect hoàn thành
        yield return StartCoroutine(_skillEffectHandler.SpellCastEffect(skillData, target, actorPosition));

        // Sau khi SpellCastEffect hoàn thành, gọi RangedAttackEffect
        yield return StartCoroutine(_skillEffectHandler.RangedAttackEffect(skillData, target, actorPosition, totalDamage));
    }
}