using UnityEngine;
using UnityEngine.UI; // 如果你的按钮是 UI Button

public class GameExit : MonoBehaviour
{
    [Header("UI 引用 (可选)")]
    public Button exitButton; // 你可以将退出按钮拖到这里，脚本会自动绑定事件

    void Start()
    {
        // 如果在 Inspector 中设置了按钮，就自动为它添加点击事件
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(QuitGame);
        }
    }

    /// <summary>
    /// 这个方法是公开的，所以你也可以直接在按钮的 OnClick() 事件中手动选择它
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("退出游戏按钮被点击！");

        // 根据不同的平台执行不同的退出操作
#if UNITY_EDITOR
        // 如果是在 Unity 编辑器中运行，就停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 如果是编译后的游戏，就直接退出应用程序
        Application.Quit();
#endif
    }

    void OnDestroy()
    {
        // 在对象销毁时移除事件监听，这是一个好习惯
        if (exitButton != null)
        {
            exitButton.onClick.RemoveListener(QuitGame);
        }
    }
}