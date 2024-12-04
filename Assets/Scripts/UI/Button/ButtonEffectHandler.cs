using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEffectHandler : MonoBehaviour
{
    private Vector3 originalScale;

    private void Start()
    {
        // �ʱ� ũ�⸦ ����
        originalScale = transform.localScale;
    }

    public void OnButtonClick()
    {
        // �ִϸ��̼� ���� ���� ��ư ũ�⸦ �ʱ� ũ��� ����
        transform.localScale = originalScale;

        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(originalScale * 0.95f, 0.1f))
            .Append(transform.DOScale(originalScale * 1.05f, 0.1f))
            .Append(transform.DOScale(originalScale, 0.1f));

        seq.Play();
    }
}