using UnityEngine;

public class GradeSelectController : MonoBehaviour
{
    public GameObject gradeSelectPanel; // 年級選擇面板

    void Start()
    {
        // 遊戲一開始先隱藏年級選擇面板
        gradeSelectPanel.SetActive(false);
    }

    // 按下「開始遊戲」
    public void OnStartGameClicked()
    {
        gradeSelectPanel.SetActive(true);
    }

    // 按下「返回」
    public void OnBackClicked()
    {
        gradeSelectPanel.SetActive(false);
    }

    // 選擇年級
    public void OnSelectGrade(string gradeChinese)
    {
        string gradeEnglish = "";

        switch (gradeChinese)
        {
            case "國小":
                gradeEnglish = "elementary school";
                break;
            case "國中":
                gradeEnglish = "junior high school";
                break;
            case "高中":
                gradeEnglish = "high school";
                break;
            default:
                gradeEnglish = "unknown";
                break;
        }

        Debug.Log("選擇了年級：" + gradeChinese + " / " + gradeEnglish);

        // TODO: 在這裡進入對應場景，例如：
        // SceneManager.LoadScene(gradeEnglishSceneName);
    }
}
