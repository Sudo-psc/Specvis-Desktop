using System;

[Serializable]
public class IshiharaUserResponse
{
    public string plateID;
    public string userAnswer;
    public string correctAnswer; // Store for easy comparison/review
    public bool isCorrect;

    public IshiharaUserResponse(string plateID, string userAnswer, string correctAnswer)
    {
        this.plateID = plateID;
        this.userAnswer = userAnswer;
        this.correctAnswer = correctAnswer;
        this.isCorrect = userAnswer.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase);
    }
}
