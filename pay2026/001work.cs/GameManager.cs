using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI元素")]
    public TMP_Dropdown carTypeDropdown;   // 汽车类型下拉菜单
    public TMP_InputField speedInputField; // 速度输入框

    private string selectedCarType = "普通汽车";
    private float selectedSpeed = 60f;

    void Start()
    {
        // 初始化下拉选项
        if (carTypeDropdown != null)
        {
            carTypeDropdown.ClearOptions();
            carTypeDropdown.AddOptions(new List<string> { "car", "sportscar" });
            carTypeDropdown.value = 0;
            carTypeDropdown.RefreshShownValue();
            selectedCarType = carTypeDropdown.options[0].text;

            carTypeDropdown.onValueChanged.AddListener(OnCarTypeChanged);
        }

        if (speedInputField != null)
        {
            speedInputField.text = selectedSpeed.ToString();
            speedInputField.onEndEdit.AddListener(OnSpeedChanged);
        }
    }

    void OnCarTypeChanged(int index)
    {
        if (carTypeDropdown != null && index >= 0 && index < carTypeDropdown.options.Count)
        {
            selectedCarType = carTypeDropdown.options[index].text;
            Debug.Log($"选择汽车类型: {selectedCarType}");
        }
    }

    void OnSpeedChanged(string speedText)
    {
        if (float.TryParse(speedText, out float speed) && speed > 0)
        {
            selectedSpeed = speed;
            Debug.Log($"设置速度: {selectedSpeed} km/h");
        }
        else
        {
            Debug.LogWarning("速度输入无效！");
            if (speedInputField != null)
                speedInputField.text = selectedSpeed.ToString();
        }
    }

    // 提供给 ComicGenerator 使用：拿到当前选择生成好的提示词
    public string BuildSimulationPrompt()
    {
        string speedEffect;

        if (selectedSpeed < 50f)
        {
            speedEffect = "速度较慢，纸片汽车微微褶皱，像风掠过纸面，产生轻微的波浪效果和柔和的摆动。";
        }
        else if (selectedSpeed >= 50f && selectedSpeed <= 332f)
        {
            speedEffect = "速度中等，纸片汽车褶皱明显，有明显的空气阻力效果，纸片边缘开始抖动和变形。";
        }
        else
        {
            speedEffect = "速度过快，纸片汽车因为与空气阻力剧烈摩擦而开始燃烧，逐渐粉碎、分解，产生火花和碎片飞散的效果。";
        }

        return $"{basePrompt}\n\n" +
               $"当前设置：\n" +
               $"- 汽车类型：{selectedCarType}\n" +
               $"- 速度：{selectedSpeed} km/h\n" +
               $"- 预期效果：{speedEffect}";
    }

    void OnDestroy()
    {
        if (carTypeDropdown != null)
            carTypeDropdown.onValueChanged.RemoveListener(OnCarTypeChanged);

        if (speedInputField != null)
            speedInputField.onEndEdit.RemoveListener(OnSpeedChanged);
    }

    // 注意：basePrompt 仍然可以放在这里或放到别的脚本里
    [Header("预设提示词")]
    [TextArea(5, 15)]
    public string basePrompt = @"生成一个四格漫画，每一个镜头角度必须不同，展示纸片汽车通过减速带的物理效果。不要重复使用相同的镜头角度。
背景：已有背景上有马赛克黑白风格的纸片汽车。
视觉效果要求：
- 速度较小时（<50 km/h）：纸片汽车微微褶皱，像风掠过纸面，产生轻微的波浪效果
- 速度中等时（50-332km/h）：纸片汽车褶皱更明显，有明显的空气阻力效果
- 速度过快时（>332 km/h）：纸片汽车因为与空气阻力摩擦而燃烧、粉碎、分解
风格：马赛克黑白风格，纸片质感，物理效果真实。";
}