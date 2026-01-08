using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // 用于 Button 和 RawImage
using UnityEngine.Networking;
using Newtonsoft.Json.Linq; // 需要项目中已安装 Newtonsoft.Json 包

public class SimpleImageGenerator : MonoBehaviour
{
    [Header("UI 引用")]
    public Button startButton;      // 将你的“开始”按钮拖到这里
    public RawImage displayImage;   // 将用于显示图片的 RawImage 拖到这里
    public Text statusText;         // (可选) 用于显示状态的文本控件

    [Header("API 配置")]
    public string apiKey = "sk-b8ccd86f5079415b8a4259f09055b9ed"; // ！！！！在这里填入你自己的 DashScope API Key

    [Header("生成内容")]
    [TextArea(3, 10)]
    public string prompt = "生成一张横向四格黑白漫画，纸片质感、马赛克风格，内容是汽车通过减速带："; // 你想要生成的图片描述

    // 这是我们最终确定的、支持同步返回的正确 API 地址
    private string apiUrl = "https://dashscope.aliyuncs.com/api/v1/services/aigc/multimodal-generation/generation";

    void Start()
    {
        if (startButton != null)
        {
            // 绑定按钮的点击事件
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        else
        {
            Debug.LogError("错误：'开始'按钮没有在 Inspector 中设置！");
        }
    }

    // 当按钮被点击时，这个方法会被调用
    public void OnStartButtonClicked()
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey.Contains("xxxx"))
        {
            Debug.LogError("错误：请在 Inspector 中填入你的 API Key！");
            if(statusText != null) statusText.text = "错误: API Key 未设置。";
            return;
        }
        // 开始执行生成图片的整个流程
        StartCoroutine(GenerateAndDisplayImage());
    }

    IEnumerator GenerateAndDisplayImage()
    {
        // --- 步骤 1: 进入加载状态 ---
        if (startButton != null) startButton.interactable = false;
        if (statusText != null) statusText.text = "正在生成图片...";
        if (displayImage != null) displayImage.texture = null; // 清空上一张图片

        // --- 步骤 2: 构建 API 请求体 ---
        var requestData = new
        {
            model = "wan2.6-t2i",
            input = new { messages = new[] { new { role = "user", content = new[] { new { text = prompt } } } } },
            parameters = new { size = "1024*1024" } // 一个有效的图片尺寸
        };
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        // --- 步骤 3: 发送 API 请求 ---
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            yield return request.SendWebRequest();

            string responseText = request.downloadHandler.text;
            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"API 调用失败: {request.responseCode} {request.error}\n响应: {responseText}";
                Debug.LogError(errorMsg);
                if (statusText != null) statusText.text = "错误: " + errorMsg;
                if (startButton != null) startButton.interactable = true;
                yield break; // 提前终止协程
            }

            // --- 步骤 4: 解析响应，获取图片 URL ---
            string imageUrl = ParseImageUrlFromResponse(responseText);
            if (string.IsNullOrEmpty(imageUrl))
            {
                string errorMsg = "无法从 API 响应中解析出图片 URL。";
                Debug.LogError(errorMsg + "\n响应: " + responseText);
                if (statusText != null) statusText.text = "错误: " + errorMsg;
                if (startButton != null) startButton.interactable = true;
                yield break;
            }

            // --- 步骤 5: 下载图片并显示 ---
            if (statusText != null) statusText.text = "正在下载图片...";
            yield return DownloadAndDisplayImage(imageUrl);
        }
    }

    string ParseImageUrlFromResponse(string json)
    {
        try
        {
            // 使用 JObject.SelectToken 精准定位到图片 URL
            var root = JObject.Parse(json);
            var imageNode = root.SelectToken("output.choices[0].message.content[0].image");
            return imageNode?.ToString();
        }
        catch (Exception ex)
        {
            Debug.LogError($"解析 JSON 时出错: {ex.Message}");
            return null;
        }
    }

    IEnumerator DownloadAndDisplayImage(string imageUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"下载图片失败: {request.error}";
                Debug.LogError(errorMsg);
                if (statusText != null) statusText.text = "错误: " + errorMsg;
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                if (displayImage != null)
                {
                    displayImage.texture = texture; // 将下载好的图片赋给 RawImage
                    if (statusText != null) statusText.text = "图片加载完成！";
                }
                else
                {
                    Debug.LogError("错误：用于显示图片的 RawImage 没有在 Inspector 中设置！");
                }
            }
        }
        // 无论成功失败，都重新启用按钮
        if (startButton != null) startButton.interactable = true;
    }
}