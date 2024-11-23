using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider; // ����� Slider
    public float duration = 30f; // Slider�� ä������ �� �ɸ��� �ð�
    private float elapsedTime = 0f; // ��� �ð�
    private bool isIncreasing = false; // Slider�� �ö󰡴� ����
    private bool isDecreasing = false; // Slider�� �������� ����
    private bool isCompleted = false; // Slider�� �Ϸ�� ����
    public delegate void SliderCompleted(); // Slider �Ϸ� �̺�Ʈ
    public event SliderCompleted OnSliderCompleted;

    private CanvasGroup canvasGroup; // CanvasGroup�� ����� ���� ó��

    void Start()
    {
        if (slider == null)
        {
            Debug.LogError("Slider�� ������� �ʾҽ��ϴ�.");
            return;
        }

        slider.value = 0f; // �ʱⰪ ����
        slider.maxValue = 1f; // �ִ밪 ����

        // CanvasGroup �߰� �Ǵ� ����
        canvasGroup = slider.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = slider.gameObject.AddComponent<CanvasGroup>();
        }

        HideSlider(); // ���� �� Slider ����
    }

    void Update()
    {
        if (isIncreasing)
        {
            if (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                slider.value = elapsedTime / duration;
                ShowSlider(); // ���� �߿��� Slider ǥ��
            }
            else
            {
                CompleteSlider();
            }
        }
        else if (isDecreasing)
        {
            if (slider.value > 0)
            {
                slider.value -= Time.deltaTime / duration;
                // Slider�� ���� �߿��� ��� ������ ���� ����
            }
            else
            {
                StopDecreasing();
            }
        }
    }

    public void StartIncreasing()
    {
        if (isCompleted)
        {
            Debug.Log($"{gameObject.name} slider is already completed.");
            return; // �Ϸ�� ���¶�� Ȱ��ȭ ����
        }

        isIncreasing = true;
        isDecreasing = false;
        elapsedTime = slider.value * duration; // ���� ���� ���¿��� �簳
        ShowSlider(); // Slider ǥ��
        Debug.Log($"{gameObject.name} slider is now increasing.");
    }

    public void StartDecreasing()
    {
        if (isCompleted)
        {
            Debug.Log($"{gameObject.name} slider is already completed.");
            return; // �Ϸ�� ���¶�� ��Ȱ��ȭ ����
        }

        isDecreasing = true;
        isIncreasing = false;
        HideSlider(); // Slider ����
    }

    private void CompleteSlider()
    {
        isIncreasing = false;
        isCompleted = true; // �Ϸ� ���·� ����
        HideSlider(); // �Ϸ� �� Slider ����

        OnSliderCompleted?.Invoke(); // �Ϸ� �̺�Ʈ ȣ��
        Debug.Log($"{gameObject.name} slider completed!");
    }

    private void StopDecreasing()
    {
        isDecreasing = false;
        HideSlider(); // Slider ����
    }

    private void ShowSlider()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1; // Slider ǥ��
            canvasGroup.blocksRaycasts = true; // ��ȣ�ۿ� ���
        }
    }

    private void HideSlider()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0; // Slider ����
            canvasGroup.blocksRaycasts = false; // ��ȣ�ۿ� ����
        }
    }

    public bool IsCompleted()
    {
        return isCompleted; // isCompleted ���� �� ��ȯ
    }
}
