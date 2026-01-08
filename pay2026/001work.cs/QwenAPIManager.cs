using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class QwenAPIManager : MonoBehaviour
{
    [Header("千问配置")]
    public string apiKey = "sk-b8ccd86f5079415b8a4259f09055b9ed"; // 替换为你的API Key
    
    private string apiUrl = "https://dashscope.aliyuncs.com/api/v1/services/aigc/multimodal-generation/generation";

    public Action<Texture2D> OnImageGenerated;
    public Action<string> OnError;

    public void GenerateImage(string prompt)
    {
        StartCoroutine(GenerateImageCoroutine(prompt));
    }

    IEnumerator GenerateImageCoroutine(string prompt)
    {
        Debug.Log("[QwenAPI] 开始生成图片 (同步模式)...");
        
        var requestData = new
        {
            model = "wan2.6-t2i",
            input = new
            {
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = new[] { new { text = prompt } }
                    }
                }
            },
            parameters = new
            {
                // --- 修改点在这里 ---
                size = "1024*1024" // 改为一个在有效范围内的尺寸
            }
        };

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            Debug.Log($"[QwenAPI] 发送请求到: {apiUrl}");
            
            yield return request.SendWebRequest();

            string responseText = request.downloadHandler.text;
            Debug.Log($"[QwenAPI] 收到响应: {responseText}");

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"API调用失败: {request.responseCode} {request.error}\n响应: {responseText}";
                Debug.LogError($"[QwenAPI] {errorMsg}");
                OnError?.Invoke(errorMsg);
                yield break;
            }

            string imageUrl = ParseImageUrlFromResponse(responseText);
            if (string.IsNullOrEmpty(imageUrl))
            {
                string errorMsg = "无法从响应中解析图片URL";
                Debug.LogError($"[QwenAPI] {errorMsg}\n响应: {responseText}");
                OnError?.Invoke(errorMsg);
                yield break;
            }

            yield return DownloadImage(imageUrl);
        }
    }

    string ParseImageUrlFromResponse(string json)
    {
        try
        {
            var root = JObject.Parse(json);
            var output = root["output"];
            if (output == null) return null;

            var choices = output["choices"] as JArray;
            if (choices == null || choices.Count == 0) return null;

            var message = choices[0]?["message"];
            if (message == null) return null;

            var content = message["content"] as JArray;
            if (content == null || content.Count == 0) return null;

            var imageNode = content[0]?["image"];
            return imageNode?.ToString();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[QwenAPI] 解析响应时出错: {ex.Message}");
            return null;
        }
    }

    IEnumerator DownloadImage(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            OnError?.Invoke("图片URL为空");
            yield break;
        }

        Debug.Log($"[QwenAPI] 开始下载图片: {imageUrl}");
        
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"下载图片失败: {request.error}";
                Debug.LogError($"[QwenAPI] {errorMsg}");
                OnError?.Invoke(errorMsg);
                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            if (texture != null)
            {
                Debug.Log($"[QwenAPI] 图片下载成功，尺寸: {texture.width}x{texture.height}");
                OnImageGenerated?.Invoke(texture);
            }
            else
            {
                string errorMsg = "下载的图片数据为null";
                Debug.LogError($"[QwenAPI] {errorMsg}");
                OnError?.Invoke(errorMsg);
            }
        }
    }

    void OnDestroy()
    {
        Debug.Log("[QwenAPI] QwenAPIManager 被销毁");
    }
}