using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Questionaire;

public class QuestionaireSample : MonoBehaviour
{

    private QBuilder qBuilder;

    private List<KeyCode> keycodes;
    private int keycodesLength;

    private Ticket currentTicket;

    // Start is called before the first frame update
    void Start()
    {
        qBuilder = new QBuilder().OnComplete(OnBuilderReady);
        keycodes = new List<KeyCode>{ KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };
        keycodesLength = keycodes.Count;
    }

    void OnBuilderReady() {
        currentTicket = qBuilder.StartFromBeginning();
        RenderStats(currentTicket);
    }

    void RenderStats(Ticket ticket) {
        currentTicket = ticket;

        Debug.Log(ticket.eventStats.MainValue);
        Debug.Log(ticket.eventStats.Tag);

        RenderChoiceStat(ticket);
        RenderAnimationImageStat(ticket);
        RenderEndStat(ticket);
    }

    private void RenderChoiceStat(Ticket ticket) {
        if (ticket.eventStats.Tag == ParameterFlag.EventTag.Question)
        {
            int index = 0;
            foreach (ChoiceStats choice in ticket.choiceStats)
            {
                Debug.Log(ticket.eventStats.NextStop + " => " + (index + 1) + " => " + choice.MainValue);
                index++;
            }
        }
    }

    private void RenderAnimationImageStat(Ticket ticket)
    {
        if (ticket.eventStats.Tag == ParameterFlag.EventTag.Animation || ticket.eventStats.Tag == ParameterFlag.EventTag.Image)
        {
            Debug.Log("Animation OR Image ID " + ticket.eventStats.MainValue);

            var nextTicket = qBuilder.ProcessTicket(ticket);
            RenderStats(nextTicket);
        }
    }

    private void RenderEndStat(Ticket ticket)
    {
        if (ticket.eventStats.Tag == ParameterFlag.EventTag.End)
        {
            Debug.Log("END");

            List<string> AllFailMessage = qBuilder.GetFailMessageList();
            for (int i = 0; i < AllFailMessage.Count; i++)
                Debug.Log("Fail - " + AllFailMessage[i]);
        }
    }

    void Update() {
        for (int i = 0; i < keycodesLength; i++) {
            if (Input.GetKeyDown(keycodes[i])) {
                SubmitChoiceWithIndex(i);
            }
        }
    }

    private void SubmitChoiceWithIndex(int index) {
        var nextTicket = qBuilder.ProcessChoice(currentTicket, currentTicket.choiceStats[index]);
        RenderStats(nextTicket);
    }
}
