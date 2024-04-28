using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class statsGraph : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private RectTransform lineY;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private TMP_Text yMax;

    [Header("Colours")]
    [SerializeField] private Color gameModeColor;
    [SerializeField] private Color practiceModeColor;
    private void Start()
    {
        PlotGraph();
    }
    private void PlotGraph()
    {
        List<ScoreData> scores = ScoreManager.Instance.scores;
        float xIncrement = graphContainer.rect.width / (scores.Count + 1);
        float maxGraphHeight = graphContainer.sizeDelta.y;
        float maxTotalScore = 0f;
        float minTotalScore = 0f;

        foreach (ScoreData score in scores)
        {
            if (score.totalScore > maxTotalScore)
            {
                maxTotalScore = score.totalScore;
            }
            if (score.totalScore < minTotalScore)
            {
                minTotalScore = score.totalScore;
            }
            Debug.Log("Score: " + score.totalScore);
            Debug.Log("Max Total Score: " + maxTotalScore);
            Debug.Log("Min Total Score: " + minTotalScore);

        }
        for (int i = 0; i < scores.Count; i++)
        {
            float bufferMaxTotalScore = maxTotalScore + ((maxTotalScore - minTotalScore) * 0.2f);
            float bufferMinTotalScore = minTotalScore - ((maxTotalScore - minTotalScore) * 0.2f);
            ScoreData score = scores[i];
            float xPosition = (i + 1) * xIncrement;
            float yPosition = ((score.totalScore - bufferMinTotalScore) / (bufferMaxTotalScore - bufferMinTotalScore)) * graphContainer.rect.height;

            Vector2 graphPosition = new Vector2(xPosition, yPosition);
            Vector2 anchoredPosition = new Vector2(graphPosition.x - graphContainer.rect.width / 2f, graphPosition.y - graphContainer.rect.height / 2f);
            PlotPoint(anchoredPosition, score.gameMode == "game" ? gameModeColor : practiceModeColor);
        }

        int lineCount = 3;
        for (int j = 0; j <= lineCount; j++)
        {
            float bufferMaxTotalScore = maxTotalScore + ((maxTotalScore - minTotalScore) * 0.2f);
            float bufferMinTotalScore = minTotalScore - ((maxTotalScore - minTotalScore) * 0.2f);
            float normalizedValue = j * 1f / lineCount;
            float yPosition = Mathf.Lerp(minTotalScore, maxTotalScore, normalizedValue);
            float yGraphPosition = ((yPosition - bufferMinTotalScore) / (bufferMaxTotalScore - bufferMinTotalScore)) * graphContainer.rect.height;

            RectTransform axis = Instantiate(lineY);
            axis.SetParent(graphContainer, false);
            axis.gameObject.SetActive(true);
            axis.anchoredPosition = new Vector2(-3f, yGraphPosition);

            TMP_Text yMax1 = Instantiate(yMax, graphContainer);
            yMax1.rectTransform.anchoredPosition = new Vector2((-graphContainer.rect.width / 2f) - 30f, yGraphPosition - (graphContainer.rect.height / 2f));

            yMax1.text = Mathf.Round(yPosition).ToString();
            Debug.Log("line value is " + yPosition);
        }
    }
    private void PlotPoint(Vector2 anchoredPosition, Color color)
    {
        GameObject point = Instantiate(pointPrefab, graphContainer);
        RectTransform pointRectTransform = point.GetComponent<RectTransform>();
        pointRectTransform.anchoredPosition = anchoredPosition;
        SpriteRenderer pointSpriteRenderer = point.GetComponent<SpriteRenderer>();
        if (pointSpriteRenderer != null)
        {
            pointSpriteRenderer.color = color;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the point prefab.");
        }
    }
}