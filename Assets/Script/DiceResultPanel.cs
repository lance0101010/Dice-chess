using UnityEngine;
using TMPro;
using System.Collections;

public class DiceResultPanel : MonoBehaviour
{
    public TextMeshProUGUI directionText;
    public TextMeshProUGUI stepText;
    public TextMeshProUGUI timerText;

    private System.Action onCountdownFinished;

    public void ShowResult(string directionName, int steps, float countdownTime, System.Action onFinish)
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        directionText.text = $"��V�G{directionName}";
        stepText.text = $"�B�ơG{steps}";
        onCountdownFinished = onFinish;
        StartCoroutine(Countdown(countdownTime));
    }

    private IEnumerator Countdown(float time)
    {
        float t = time;
        while (t > 0)
        {
            timerText.text = $"�Ѿl {Mathf.CeilToInt(t)} ��";
            yield return new WaitForSeconds(1f);
            t -= 1f;
        }

        timerText.text = "���椤...";
        gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        onCountdownFinished?.Invoke();
    }
}
