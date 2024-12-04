using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOption : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Dropdown screenModeDropdown;
    public Slider masterVolumeSlider;

    private Resolution[] resolutions;

    void Start()
    {
        // �ػ� �ɼ� �ʱ�ȭ
        InitializeResolutions();

        // UI �ʱⰪ ����
        InitializeUI();

        // �����̴� �� ���� �� ȣ��� �̺�Ʈ ���
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    // �ػ� �ɼ� �ʱ�ȭ
    void InitializeResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        // �ػ� ��� �߰�
        List<string> resolutionOptions = new List<string>();
        foreach (Resolution resolution in resolutions)
        {
            if (Mathf.Approximately((float)resolution.width / resolution.height, 16f / 9f))
            {
                resolutionOptions.Add(resolution.width + " x " + resolution.height);
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);

        // ���� �ػ󵵸� �⺻������ ����
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    // UI �ʱ�ȭ
    void InitializeUI()
    {
        // ���� �ɼ� ����
        screenModeDropdown.ClearOptions();

        // ȭ�� ��� ��� �߰� (��ü ȭ��� â ��常 ����)
        List<string> screenModeOptions = new List<string>
        {
            "��ü ȭ��",
            "â ���"
        };
        screenModeDropdown.AddOptions(screenModeOptions);

        // ȭ�� ��� �ʱⰪ ����
        screenModeDropdown.value = Screen.fullScreen ? 0 : 1; // ��ü ȭ��: 0, â ���: 1
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);

        // ������ ���� �ʱⰪ ����
        masterVolumeSlider.value = AudioListener.volume;
    }

    // ���� �ػ��� �ε����� ��ȯ
    int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0; // �⺻��
    }

    // �ػ� ����
    void SetResolution(int index)
    {
        if (index >= 0 && index < resolutions.Length)
        {
            Resolution resolution = resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }

    // ȭ�� ��� ����
    void SetScreenMode(int index)
    {
        // index 0: ��ü ȭ��, 1: â ���
        bool isFullScreen = index == 0;

        // ���� ���õ� �ػ󵵿� ȭ�� ��� ����
        Resolution currentResolution = resolutions[resolutionDropdown.value];
        Screen.SetResolution(currentResolution.width, currentResolution.height, isFullScreen);
    }

    // ������ ���� ����
    void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
