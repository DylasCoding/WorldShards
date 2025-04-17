using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillEffectHandler : MonoBehaviour
{
    public IEnumerator MeleeAttackEffect(GameObject SkillEffect, CharController target, int totalDamage)
    {
        bool flowControl = CheckNullComponent(SkillEffect, target);
        if (!flowControl)
        {
            yield break;
        }

        FlipEffect(SkillEffect, target);

        GameObject effect = Instantiate(SkillEffect, target.transform.position, Quaternion.identity);

        Animator animator = effect.GetComponent<Animator>();
        float animationLength = GetAnimationLength(animator);

        Destroy(effect, animationLength);

        target.TakeDamage(totalDamage);
    }

    public IEnumerator RangedAttackEffect(SkillData skillData, CharController target, Vector3 actorLocation, int totalDamage)
    {
        bool flowControl = CheckNullComponent(skillData.RangePrefab, target);
        if (!flowControl)
        {
            yield break;
        }

        // Tiếp tục với hiệu ứng bắn skill
        GameObject effect = Instantiate(skillData.RangePrefab, actorLocation, Quaternion.identity);

        Animator animator = effect.GetComponent<Animator>();
        float animationLength = GetAnimationLength(animator);
        if (skillData.isArrowSkill)
            effect.transform.rotation = Quaternion.Euler(0, 0, 90);

        FlipEffect(effect, target);

        float elapsedTime = 0f;
        Vector3 startPosition = actorLocation;
        while (elapsedTime < animationLength)
        {
            float t = elapsedTime / animationLength;
            effect.transform.position = Vector3.Lerp(startPosition, target.transform.position, t);

            if (skillData.isArrowSkill)
                effect.transform.rotation = Quaternion.Euler(0, 0, 90);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(effect);

        target.TakeDamage(totalDamage);
    }

    public IEnumerator SpellCastEffect(SkillData skillData, CharController target, Vector3 actorLocation)
    {
        Debug.Log("SpellCastEffect called ");
        bool flowControl = CheckNullComponent(skillData.spellPrefab, target);
        if (!flowControl)
        {
            yield break;
        }

        GameObject effect = Instantiate(skillData.spellPrefab, actorLocation, Quaternion.identity);

        // Quay hiệu ứng
        SpriteRenderer spriteRenderer = effect.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            float rotationDuration = .5f;
            float elapsedTime = 0f;
            Quaternion startRotation = spriteRenderer.transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0, 0, 90);

            while (elapsedTime < rotationDuration)
            {
                float t = elapsedTime / rotationDuration;
                spriteRenderer.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            spriteRenderer.transform.rotation = targetRotation;
        }

        Destroy(effect);
    }

    private void FlipEffect(GameObject effect, CharController target)
    {
        if (target.role == CharController.CharacterRole.Player)
        {
            Vector3 scale = effect.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * -1; // Đảm bảo lật sang trái
            effect.transform.localScale = scale;
        }
        else
        {
            Vector3 scale = effect.transform.localScale;
            scale.x = Mathf.Abs(scale.x); // Đảm bảo không lật (trạng thái gốc)
            effect.transform.localScale = scale;
        }
    }

    private static bool CheckNullComponent(GameObject SkillEffect, CharController target)
    {
        if (target == null)
        {
            Debug.LogError("Target is null");
            return false;
        }
        if (SkillEffect == null)
        {
            Debug.LogError("SkillData is null");
            return false;
        }

        return true;
    }

    private float GetAnimationLength(Animator animator)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        if (controller == null || controller.animationClips.Length == 0)
        {
            return 0f;
        }

        // Vì chỉ có một animation mặc định, lấy clip đầu tiên
        AnimationClip clip = controller.animationClips[0];
        if (clip == null)
        {
            return 0f;
        }

        return clip.length;
    }
}