using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // 添加场景管理命名空间

public class NewBehaviourScript : MonoBehaviour
{
    [Header("场景设置")]
    public string nextSceneName = "作业飞跃减速带";  // 下一个场景的名称
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 点击Start按钮开始游戏
    public void OnStartButtonClick()
    {
        StartGame();
    }

    // 开始游戏的方法
    void StartGame()
    {
        Debug.Log("游戏开始！准备跳转到场景：" + nextSceneName);
        
        // 跳转到下一个场景
        LoadNextScene();
    }
    
    // 加载下一个场景
    void LoadNextScene()
    {
        // 方式1：通过场景名称加载（推荐）
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("场景名称未设置！请在Inspector中设置nextSceneName");
        }
        
        // 方式2：通过场景索引加载（备选方案）
        // SceneManager.LoadScene(1); // 加载Build Settings中索引为1的场景
    }
}