using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComicGenerator : MonoBehaviour
{
    [Header("UI 引用")]
    public TMP_Dropdown carTypeDropdown;      // 汽车类型选择
    public TMP_InputField speedInputField;    // 速度输入（km/h）
    public Button generateComicButton;        // “生成四格漫画”按钮

    [Header("提示词预设")]
    [TextArea(5, 15)]
    public string basePrompt = @"生成一张横向四格黑白漫画，软纸片质感、马赛克风格，内容是汽车通过减速带：";

    [Header("接入千问文生图")]
    public QwenAPIManager qwenImageAPI;
    public RawImage comicPreview;             // 用来显示生成的整张四格图

    // 当前选择
    string selectedCarType = "普通汽车";
    float selectedSpeed = 60f;

    void Start()
    {
        // 绑定 UI 事件
        if (carTypeDropdown != null)
        {
            carTypeDropdown.onValueChanged.AddListener(OnCarTypeChanged);
            // 初始化一次
            if(carTypeDropdown.options.Count > 0)
            {
                selectedCarType = carTypeDropdown.options[carTypeDropdown.value].text;
            }
        }

        if (speedInputField != null)
        {
            speedInputField.text = selectedSpeed.ToString();
            speedInputField.onEndEdit.AddListener(OnSpeedChanged);
        }

        if (generateComicButton != null)
        {
            generateComicButton.onClick.AddListener(OnGenerateComicClicked);
        }

        if (qwenImageAPI != null)
        {
            qwenImageAPI.OnImageGenerated += OnImageGenerated;
            qwenImageAPI.OnError += OnImageError;
        }
    }

    void OnCarTypeChanged(int index)
    {
        if (carTypeDropdown == null) return;
        if (index >= 0 && index < carTypeDropdown.options.Count)
        {
            selectedCarType = carTypeDropdown.options[index].text;
            Debug.Log($"选择汽车类型：{selectedCarType}");
        }
    }

    void OnSpeedChanged(string text)
    {
        if (float.TryParse(text, out var v) && v > 0)
        {
            selectedSpeed = v;
        }
        else
        {
            // 恢复上一次有效值
            if (speedInputField != null)
                speedInputField.text = selectedSpeed.ToString();
        }
        Debug.Log($"选择速度：{selectedSpeed} km/h");
    }

    void OnGenerateComicClicked()
    {
        if (qwenImageAPI == null)
        {
            Debug.LogError("QwenAPIManager 未设置，请在 Inspector 中拖入 QwenAPIManager 组件");
            return;
        }

        string fullPrompt = BuildPrompt(selectedCarType, selectedSpeed);
        Debug.Log("最终提示词：\n" + fullPrompt);

        // 调用千问文生图
        qwenImageAPI.GenerateImage(fullPrompt);
    }

    string BuildPrompt(string carType, float speed)
    {
        string speedLabel;
        if (speed < 50f)
            speedLabel = "低速（安全）";
        else if (speed <= 100f)
            speedLabel = "中速（危险）";
        else
            speedLabel = "高速（极危险）";

        string speedEffect;
        if (speed < 50f)
        {
            speedEffect = "纸片汽车缓慢通过减速带，汽车正侧面画面，纸片车身只产生轻微褶皱，好像风轻轻掠过纸面。";
        }
        else if (speed <= 100f)
        {
            speedEffect = "纸片汽车以较快速度冲向减速带，汽车正侧面画面，车身明显起伏，褶皱加深，边缘被空气吹得抖动。";
        }
        else
        {
            speedEffect = "纸片汽车以极高速度冲过减速带，汽车正侧面画面，在巨大空气阻力和撞击下开始燃烧、撕裂，最终粉碎成纸屑。要表现出纸片汽车整个碎裂。";
        }

        return
$@"{basePrompt}

角色与设定：
- 汽车类型：{carType}（纸片风格）
- 速度：{speed} km/h（{speedLabel}）

四格分镜要求（横向排列）：
1. 第 1 格：远景，道路上有醒目的减速带标志，一辆纸片质感的{carType}从远处驶来，画面安静，速度文字标注“{speed} km/h”。
2. 第 2 格：车辆刚压到减速带，车轮接触减速带，特写画面，纸片车身开始出现褶皱和轻微变形。
3. 第 3 格：{speedEffect}
4. 第 4 格：结果画面：如果速度安全，纸片车完整驶离减速带，只留下轻微褶皱；如果速度过快，则只剩下散落的纸片残骸，特写残骸画面，背景仍是同一条道路。

整体画风：
- 黑白马赛克风格，简笔画风格
- 强烈软纸片质感
- 四格边框清晰，漫画排版感明显。";
    }

    // --- 已更新为带有详细调试日志的版本 ---
    void OnImageGenerated(Texture2D tex)
    {
        Debug.Log($"[ComicGenerator] 收到千问生成的图片，尺寸: {tex?.width}x{tex?.height}");

        if (comicPreview == null)
        {
            Debug.LogError("[ComicGenerator] comicPreview 未设置！请在 Inspector 中绑定 RawImage");
            return;
        }

        if (tex == null)
        {
            Debug.LogError("[ComicGenerator] 收到的 Texture2D 为 null");
            return;
        }

        comicPreview.texture = tex;
        Debug.Log("[ComicGenerator] 图片已赋值给 comicPreview.texture");

        // 确保 RawImage 的 GameObject 是激活的
        if (!comicPreview.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("[ComicGenerator] comicPreview 的 GameObject 未激活！");
        }
    }

    void OnImageError(string err)
    {
        Debug.LogError("生成图片失败：" + err);
    }

    void OnDestroy()
    {
        if (qwenImageAPI != null)
        {
            qwenImageAPI.OnImageGenerated -= OnImageGenerated;
            qwenImageAPI.OnError -= OnImageError;
        }
        if (carTypeDropdown != null)
        {
            carTypeDropdown.onValueChanged.RemoveListener(OnCarTypeChanged);
        }
        if (speedInputField != null)
        {
            speedInputField.onEndEdit.RemoveListener(OnSpeedChanged);
        }
        if (generateComicButton != null)
        {
            generateComicButton.onClick.RemoveListener(OnGenerateComicClicked);
        }
    }
}