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
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.width + " x " + resolution.height));
        }

        // ���� �ػ󵵸� �⺻������ ����
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    // UI �ʱ�ȭ
    void InitializeUI()
    {
        // ���� �ɼ� ����
        screenModeDropdown.ClearOptions();

        // ȭ�� ��� �ʱⰪ ����
        screenModeDropdown.value = (int)GetCurrentScreenMode();
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);

        // ȭ�� ��� ��� �߰�
        screenModeDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "��ü ȭ��",
            "â ���",
            "����� â ���"
        });

        // ������ ���� �ʱⰪ ����
        masterVolumeSlider.value = AudioListener.volume;
    }

    // ���� �ػ��� �ε��� ��ȯ
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
        return 0;
    }

    // ���� ȭ�� ��� ��ȯ
    FullScreenMode GetCurrentScreenMode()
    {
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            return FullScreenMode.FullScreenWindow;
        else if (Screen.fullScreenMode == FullScreenMode.Windowed)
            return FullScreenMode.Windowed;
        else
            return FullScreenMode.ExclusiveFullScreen;
    }

    // �ػ� ����
    void SetResolution(int index)
    {
        if (index >= 0 && index < resolutions.Length)
        {
            Resolution resolution = resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        }
    }

    // ȭ�� ��� ����
    void SetScreenMode(int index)
    {
        FullScreenMode mode = (FullScreenMode)index;
        Screen.fullScreenMode = mode;
    }

    // ������ ���� ����
    void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
