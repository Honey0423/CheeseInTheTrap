using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class DoorOpen : MonoBehaviour
{
    public float moveSpeed = 2f; // Grid�� �̵��ϴ� �ӵ�
    public float targetYOffset = 50f; // ��ǥ y ��ǥ ��·�
    public Text messageText; // UI �ؽ�Ʈ ������Ʈ�� �����մϴ�.

    private Transform gridTransform;

    private void Start()
    {
        // Grid��� �ڽ� ������Ʈ�� ã���ϴ�.
        gridTransform = transform.Find("Grid");
        if (gridTransform != null)
        {
            Open();
        }
        else
        {
            Debug.LogError("Grid child not found!");
        }
    }

    void Open()
    {
        ShowMessage("���� ���Ƚ��ϴ�!\nŻ���ϼ���!");
        StartCoroutine(OpenDoorCoroutine());
    }

    IEnumerator OpenDoorCoroutine()
    {
        float initialY = gridTransform.localPosition.y;
        float targetY = initialY + targetYOffset;

        // y ��ǥ�� õõ�� ���
        while (gridTransform.localPosition.y < targetY)
        {
            Vector3 newPosition = gridTransform.localPosition;
            newPosition.y += moveSpeed * Time.deltaTime;
            newPosition.y = Mathf.Min(newPosition.y, targetY); // ��ǥ ��ǥ�� �ʰ����� �ʵ��� ����
            gridTransform.localPosition = newPosition;

            yield return null; // ���� �����ӱ��� ���
        }
    }
    void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message; // �޽��� ����
            messageText.gameObject.SetActive(true); // �޽��� ǥ��

            // ���� �ð� �� �޽����� ����ϴ�.
            StartCoroutine(HideMessageAfterDelay(3f)); // 3�� �� ����
        }
    }

    IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageText.gameObject.SetActive(false);
    }
}
