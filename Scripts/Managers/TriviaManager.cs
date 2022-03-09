using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TriviaManager : Singleton<TriviaManager>
{
    public GameModel Configuration;

    private string currentCategory;

    private List<int>askedQuestionIndex=new List<int>();


    public QuestionModel GetQuestionForCategory(string categoryName)

    {

        CategoryModel categoryModel=Configuration.Categories.FirstOrDefault(category=>category.CategoryName==categoryName);
        if(categoryModel!=null)
        {
            int randomIndex=Random.Range(0,categoryModel.Questions.Count);
            while(categoryModel.Questions.Count>askedQuestionIndex.Count && askedQuestionIndex.Contains(randomIndex))
            randomIndex=Random.Range(0,categoryModel.Questions.Count);

            askedQuestionIndex.Add(randomIndex);

            return categoryModel.Questions[randomIndex];
        }

        return null;

    }

    public void SetCurrentCategory(string categoryName)
    {
        currentCategory=categoryName;
        askedQuestionIndex.Clear();
    }

    public string GetCurrentCategory()
    {
        return currentCategory;
    }
}
