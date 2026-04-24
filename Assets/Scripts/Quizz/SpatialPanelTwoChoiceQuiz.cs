using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpatialPanelTwoChoiceQuiz : MonoBehaviour
{
    [System.Serializable]
    public class AnswerItem
    {
        public string answerText;
        public Sprite answerImage;
        public bool isCorrect;

        [TextArea(1, 4)]
        public string explain;
    }

    [System.Serializable]
    public class QuizItem
    {
        [TextArea(2, 5)]
        public string questionText;

        public Sprite questionImage;
        public List<AnswerItem> answers = new List<AnswerItem>();
    }

    [Header("Quiz Data")]
    [SerializeField] private List<QuizItem> questions = new List<QuizItem>();

    [Header("UI - Main")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image questionImageView;
    [SerializeField] private Button optionAButton;
    [SerializeField] private Button optionBButton;
    [SerializeField] private TextMeshProUGUI optionAText;
    [SerializeField] private Image optionAImageView;
    [SerializeField] private TextMeshProUGUI optionBText;
    [SerializeField] private Image optionBImageView;

    [Header("UI - Optional")]
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI finalResultText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button restartButton;

    [Header("Spatial Panel Scroll")]
    [Tooltip("Assign the ScrollRect of your Spatial Panel content.")]
    [SerializeField] private ScrollRect panelScrollRect;

    [Header("Result Colors")]
    [SerializeField] private Color correctColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    [SerializeField] private Color wrongColor = new Color(0.9f, 0.2f, 0.2f, 1f);
    [SerializeField] private Color defaultColor = Color.white;

    private int currentQuestionIndex;
    private int score;
    private bool answeredCurrentQuestion;

    private void Start()
    {
        WireEvents();

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        if (restartButton != null)
            restartButton.gameObject.SetActive(false);

        SetFinalResultVisible(false, string.Empty);

        if (questions == null || questions.Count == 0)
        {
            SetMainInteractable(false);
            SetFeedback("No question data. Add quiz items in the Inspector.");
            SetProgress("Quiz 0/0");
            return;
        }

        currentQuestionIndex = 0;
        score = 0;
        ShowQuestion(currentQuestionIndex);
    }

    private void WireEvents()
    {
        if (optionAButton != null)
        {
            optionAButton.onClick.RemoveListener(OnClickOptionA);
            optionAButton.onClick.AddListener(OnClickOptionA);
        }

        if (optionBButton != null)
        {
            optionBButton.onClick.RemoveListener(OnClickOptionB);
            optionBButton.onClick.AddListener(OnClickOptionB);
        }

        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(NextQuestion);
            nextButton.onClick.AddListener(NextQuestion);
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(RestartQuiz);
            restartButton.onClick.AddListener(RestartQuiz);
        }
    }

    private void OnClickOptionA()
    {
        SubmitAnswer(0);
    }

    private void OnClickOptionB()
    {
        SubmitAnswer(1);
    }

    public void SubmitAnswer(int selectedOption)
    {
        if (answeredCurrentQuestion)
            return;

        if (!IsQuestionIndexValid(currentQuestionIndex))
            return;

        QuizItem q = questions[currentQuestionIndex];
        if (q.answers == null || selectedOption < 0 || selectedOption >= q.answers.Count)
            return;

        AnswerItem selectedAnswer = q.answers[selectedOption];
        answeredCurrentQuestion = true;

        bool isCorrect = selectedAnswer.isCorrect;
        if (isCorrect)
            score++;

        HighlightAnswerResult(q, selectedOption);

        string result = isCorrect ? "Correct" : "Wrong";
        string explanationPart = string.IsNullOrWhiteSpace(selectedAnswer.explain) ? string.Empty : "\n" + selectedAnswer.explain;
        SetFeedback(result + explanationPart);

        if (nextButton != null)
            nextButton.gameObject.SetActive(true);

        SetMainInteractable(false);
    }

    public void NextQuestion()
    {
        if (questions == null || questions.Count == 0)
            return;

        currentQuestionIndex++;

        if (currentQuestionIndex >= questions.Count)
        {
            ShowFinal();
            return;
        }

        ShowQuestion(currentQuestionIndex);
    }

    public void RestartQuiz()
    {
        if (questions == null || questions.Count == 0)
            return;

        currentQuestionIndex = 0;
        score = 0;
        answeredCurrentQuestion = false;
        SetFinalResultVisible(false, string.Empty);
        ShowQuestion(currentQuestionIndex);
    }

    private void ShowQuestion(int index)
    {
        if (!IsQuestionIndexValid(index))
            return;

        QuizItem q = questions[index];
        answeredCurrentQuestion = false;

        if (questionText != null)
            questionText.text = q.questionText;

        SetImageSprite(questionImageView, q.questionImage);

        bool hasValidAnswers = q.answers != null && q.answers.Count >= 2;
        if (hasValidAnswers)
        {
            if (optionAText != null)
                optionAText.text = q.answers[0].answerText;

            SetImageSprite(optionAImageView, q.answers[0].answerImage);

            if (optionBText != null)
                optionBText.text = q.answers[1].answerText;

            SetImageSprite(optionBImageView, q.answers[1].answerImage);
        }
        else
        {
            if (optionAText != null)
                optionAText.text = "Need at least 2 answers";

            SetImageSprite(optionAImageView, null);

            if (optionBText != null)
                optionBText.text = "Need at least 2 answers";

            SetImageSprite(optionBImageView, null);
        }

        ResetAnswerColors();
        SetFinalResultVisible(false, string.Empty);

        SetFeedback("Choose one answer.");
        SetProgress("Question " + (index + 1) + "/" + questions.Count + " | Score: " + score);
        SetMainInteractable(hasValidAnswers);

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        if (restartButton != null)
            restartButton.gameObject.SetActive(false);

        ResetScrollToTop();
    }

    private void ShowFinal()
    {
        if (questionText != null)
            questionText.text = "Quiz finished";

        SetImageSprite(questionImageView, null);

        if (optionAText != null)
            optionAText.text = "-";

        SetImageSprite(optionAImageView, null);

        if (optionBText != null)
            optionBText.text = "-";

        SetImageSprite(optionBImageView, null);

        float percent = questions.Count > 0 ? (score * 100f) / questions.Count : 0f;
        SetFeedback("Final score: " + score + "/" + questions.Count + " (" + percent.ToString("0.#") + "%)");
        SetFinalResultVisible(true, "Result: " + percent.ToString("0.#") + "% correct");
        SetProgress("Completed");
        SetMainInteractable(false);
        ResetAnswerColors();

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        if (restartButton != null)
            restartButton.gameObject.SetActive(true);

        ResetScrollToTop();
    }

    private void SetMainInteractable(bool value)
    {
        if (optionAButton != null)
            optionAButton.interactable = value;

        if (optionBButton != null)
            optionBButton.interactable = value;
    }

    private void SetFeedback(string message)
    {
        if (feedbackText != null)
            feedbackText.text = message;
    }

    private void SetProgress(string message)
    {
        if (progressText != null)
            progressText.text = message;
    }

    private void SetFinalResultVisible(bool visible, string message)
    {
        if (finalResultText == null)
            return;

        finalResultText.text = message;
        finalResultText.gameObject.SetActive(visible);
    }

    private void SetImageSprite(Image target, Sprite sprite)
    {
        if (target == null)
            return;

        bool hasSprite = sprite != null;
        target.sprite = sprite;
        target.enabled = hasSprite;
    }

    private void HighlightAnswerResult(QuizItem question, int selectedIndex)
    {
        int correctIndex = -1;
        for (int i = 0; i < question.answers.Count; i++)
        {
            if (question.answers[i].isCorrect)
            {
                correctIndex = i;
                break;
            }
        }

        if (correctIndex >= 0)
            SetAnswerColor(correctIndex, correctColor);

        if (selectedIndex != correctIndex)
            SetAnswerColor(selectedIndex, wrongColor);
    }

    private void ResetAnswerColors()
    {
        SetAnswerColor(0, defaultColor);
        SetAnswerColor(1, defaultColor);
    }

    private void SetAnswerColor(int answerIndex, Color color)
    {
        if (answerIndex == 0 && optionAButton != null)
        {
            Image image = optionAButton.targetGraphic as Image;
            if (image != null)
                image.color = color;
        }
        else if (answerIndex == 1 && optionBButton != null)
        {
            Image image = optionBButton.targetGraphic as Image;
            if (image != null)
                image.color = color;
        }
    }

    private void ResetScrollToTop()
    {
        if (panelScrollRect == null)
            return;

        Canvas.ForceUpdateCanvases();
        panelScrollRect.verticalNormalizedPosition = 1f;
        panelScrollRect.horizontalNormalizedPosition = 0f;
    }

    private bool IsQuestionIndexValid(int index)
    {
        return questions != null && index >= 0 && index < questions.Count;
    }
}
