using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ScoreManager : MonoBehaviour
{
    int curScore = 0;    // 현재 스코어
    public int curLevel = 1;    // 현재 레벨
    int clearLines;        // 다음레벨업을 위해 클리어 해야하는 "현재" 라인 수
    public int linesPerLevel = 5;    // 레벨업을 하기위해 필요한 "기본" 라인 수

    const int minLines = 1;    // 지울 수 있는 최소라인 수
    const int maxLines = 4;    // 지울 수 있는 최대라인 수

    // UI 변수 링크
    public Text linesText;
    public Text levelText;
    public TextMeshProUGUI scoreText;

    StringBuilder sb;
    readonly int[] lineBaseScore = new int[] { 40, 100, 300, 1200};
    private void Awake()
    {
        sb = new StringBuilder();
        sb.Remove(0, sb.Length);
    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        curLevel = 1;
        clearLines = linesPerLevel * curLevel;
        UpdateUIText();
    }

    void UpdateUIText()
    {
        // 남아있는 라인
        if (linesText)
        {
            linesText.text = clearLines.ToString();
        }
        // 현재 레벨
        if (levelText)
        {
            levelText.text = curLevel.ToString();
        }
        // 현재 스코어
        if (scoreText)
        {
            //scoreText.text = curScore.ToString();
        }
    }

    public void LevelUp()
    {
        curLevel++;
        clearLines = linesPerLevel * curLevel;
    }

    public void ScoreLines(int n) 
    {
        n = Mathf.Clamp(n, minLines, maxLines);
        curScore += lineBaseScore[n - 1] * curLevel;
        clearLines += n;
        if(clearLines <= 0)
        {
            UpdateUIText();
        }
    }

    string PadZero(int num, int padDigits)
    {
        string nStr = num.ToString(); 
        sb.Remove(0, sb.Length);
        for(int i = 0; i < padDigits - nStr.Length; i++)  
        {
            sb.Append("0");
        }
        sb.Append(num);

        return sb.ToString();
    }
}
