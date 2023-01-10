using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ScoreManager : MonoBehaviour
{
    int curScore = 0;    // ���� ���ھ�
    public int curLevel = 1;    // ���� ����
    int clearLines;        // ������������ ���� Ŭ���� �ؾ��ϴ� "����" ���� ��
    public int linesPerLevel = 5;    // �������� �ϱ����� �ʿ��� "�⺻" ���� ��

    const int minLines = 1;    // ���� �� �ִ� �ּҶ��� ��
    const int maxLines = 4;    // ���� �� �ִ� �ִ���� ��

    // UI ���� ��ũ
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
        // �����ִ� ����
        if (linesText)
        {
            linesText.text = clearLines.ToString();
        }
        // ���� ����
        if (levelText)
        {
            levelText.text = curLevel.ToString();
        }
        // ���� ���ھ�
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
