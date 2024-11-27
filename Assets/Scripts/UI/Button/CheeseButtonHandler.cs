using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheeseButtonHandler : MonoBehaviour
{
    public PanelHandler popupWindow;

    public void OnButtonClick()
    {
        Vector3 originalScale = transform.localScale;

        // ��ư �ִϸ��̼�
        var buttonSeq = DOTween.Sequence();
        buttonSeq.Append(transform.DOScale(originalScale * 0.95f, 0.1f))
                 .Append(transform.DOScale(originalScale * 1.05f, 0.1f))
                 .Append(transform.DOScale(originalScale, 0.1f))
                 .OnComplete(() =>
                 {
                     // �˾� â ǥ��
                     popupWindow.Show();

                     // ��ư ��Ȱ��ȭ �ִϸ��̼�
                     var hideSeq = DOTween.Sequence();
                     hideSeq.Append(transform.DOScale(originalScale * 0.2f, 0.2f))
                            .OnComplete(() =>
                            {
                                gameObject.SetActive(false); // ��ư ��Ȱ��ȭ
                            });
                 });

        buttonSeq.Play();
    }
}

