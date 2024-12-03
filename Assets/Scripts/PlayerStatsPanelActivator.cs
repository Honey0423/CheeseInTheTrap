using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsPanelActivator : MonoBehaviour
{
    public GameObject targetPanel; // Ȱ��ȭ/��Ȱ��ȭ�� ������Ʈ
    public KeyCode toggleKey = KeyCode.Tab; // ��ۿ� ����� Ű (�⺻��: Tab)

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (targetPanel != null)
            {
                // Ȱ��ȭ ���¸� �ݴ�� ����
                targetPanel.SetActive(!targetPanel.activeSelf);
            }
            else
            {
                Debug.LogWarning("Target Panel is not assigned!");
            }
        }
    }
}
