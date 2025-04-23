using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ActionPanelManager : MonoBehaviour
{
    [SerializeField] private Image _playerActionImage;
    [SerializeField] private Image _enemyActionImage;
    [SerializeField] private Sprite _defaultActionSprite;
    [SerializeField] private float _moveDistance = 150f;
    [SerializeField] private float _moveDuration = .2f;

    private AudioManager _audioManager;
    private SkillData _skillData;

    private void Start()
    {
        gameObject.SetActive(true);
    }

    public void ShowPanel(SkillData skillData, CharController.CharacterRole characterRole, AudioManager audioManager, Action onComplete = null)
    {
        // Active Panel
        gameObject.SetActive(true);

        _audioManager = audioManager;
        _skillData = skillData;

        Sprite sprite = skillData.SkillActionImage;

        // Gán sprite dựa trên role
        if (characterRole == CharController.CharacterRole.Player)
        {
            _playerActionImage.sprite = sprite; // Player
            _playerActionImage.gameObject.SetActive(true); // Player
            _enemyActionImage.gameObject.SetActive(false); // Enemy
        }
        else
        {
            _enemyActionImage.sprite = sprite; // Enemy
            _playerActionImage.gameObject.SetActive(false); // Player
            _enemyActionImage.gameObject.SetActive(true); // Enemy
        }

        // Bắt đầu chuyển động
        StartCoroutine(MoveImages(onComplete));
    }

    private IEnumerator MoveImages(Action onComplete)
    {
        _audioManager.EnqueueSFX(_skillData.audioClip);
        // save first position
        Vector3 image1StartPos = _playerActionImage.rectTransform.anchoredPosition;
        Vector3 image2StartPos = _enemyActionImage.rectTransform.anchoredPosition;

        // calculate end position
        Vector3 image1From = image1StartPos + new Vector3(-_moveDistance, 0, 0); // Trái sang phải
        Vector3 image1To = image1StartPos;
        Vector3 image2From = image2StartPos + new Vector3(_moveDistance, 0, 0); // Phải sang trái
        Vector3 image2To = image2StartPos;

        // Đặt vị trí bắt đầu
        _playerActionImage.rectTransform.anchoredPosition = image1From;
        _enemyActionImage.rectTransform.anchoredPosition = image2From;

        // Di chuyển bằng Lerp
        float elapsedTime = 0f;
        while (elapsedTime < _moveDuration)
        {
            elapsedTime += Time.deltaTime * 1.25f;
            float t = elapsedTime / _moveDuration;
            float smoothT = 1f - Mathf.Pow(2f, -10f * t);

            // Di chuyển image1 từ trái sang phải
            _playerActionImage.rectTransform.anchoredPosition = Vector3.Lerp(image1From, image1To, smoothT);
            // Di chuyển image2 từ phải sang trái
            _enemyActionImage.rectTransform.anchoredPosition = Vector3.Lerp(image2From, image2To, smoothT);

            yield return null;
        }

        // Đảm bảo vị trí cuối cùng chính xác
        _playerActionImage.rectTransform.anchoredPosition = image1To;
        _enemyActionImage.rectTransform.anchoredPosition = image2To;

        while (_audioManager.IsSFXPlaying())
        {
            yield return null;
        }

        gameObject.SetActive(false);

        // Gọi callback khi hoàn thành
        onComplete?.Invoke();
    }
}