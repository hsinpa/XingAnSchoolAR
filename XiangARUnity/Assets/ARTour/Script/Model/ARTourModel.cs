using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Model;
using System.Linq;
using Questionaire;

public class ARTourModel : Model
{
    private QBuilder qBuilder;

    public ARTourModel() {
        qBuilder = new QBuilder().OnComplete(OnBuilderReady);
    }

    public Ticket EnterQuestionaire(string key) {
        ShuffleData();
        return qBuilder.StartFromEventKey(key);
    }

    public Ticket SubmitChoiceWithIndex(Ticket questionTicket, int index)
    {
        return qBuilder.ProcessChoice(questionTicket, questionTicket.choiceStats[index]);
    }

    public int GetVariable(string key) {
        return qBuilder.qmodel.FindTraitValue(key);
    }

    private void OnBuilderReady()
    {
        ShuffleData();
    }

    private void ShuffleData() {
        qBuilder.EditParameter(ParameterFlag.QuestionaireParameter.RandomValue, "=", UnityEngine.Random.Range(1, 5));
        qBuilder.EditParameter(ParameterFlag.QuestionaireParameter.ClassLevel, "=", PlayerPrefs.GetInt(GeneralFlag.Playerpref.Level, 0));
    }



    public void Reset() {

    }

    [System.Serializable]
    public struct TourDataTextStruct {
        public TextAsset textAsset;
        public string title;
        public string _id;
    }

}
