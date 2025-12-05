namespace WebApplication1.Models;
using System.Text.Json.Serialization;
using WebApplication1.Models; // để nhận ra class User

public class Question
{
    public int QuestionId { get; set; }
    public string Content { get; set; } 
    public string? OptionA { get; set; }
    public string? OptionB { get; set; }
    public string? OptionC { get; set; }
    public string? OptionD { get; set; }
    public string Answer { get; set; }
    public int GameLevelId { get; set; }
    public GameLevel GameLevel { get; set; }



    public Question()
    {
       
    }

    public Question(int questionId, string content, string answer, string? optionA = null, string? optionB = null, string? optionC = null, string? optionD = null)
    {
        QuestionId = questionId;
        Content = content;
        Answer = answer;
        OptionA = optionA;
        OptionB = optionB;
        OptionC = optionC;
        OptionD = optionD;
    }
    

}