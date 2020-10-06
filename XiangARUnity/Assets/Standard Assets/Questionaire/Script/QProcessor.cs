using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Questionaire
{

    public class QProcessor
    {

        QModel _qmodel;
        QDataParser.ParseResult _rawParseResult;

        public QProcessor(QModel qmodel, QDataParser.ParseResult parseResult)
        {
            this._qmodel = qmodel;
            this._rawParseResult = parseResult;
        }

        public Ticket ProcessChoice(Ticket ticket, ChoiceStats pick_choice) {
            _qmodel.RecordChoice(pick_choice, ticket.eventStats);

            return ProcessNextEvent(pick_choice.NextStep);
        }

        public Ticket ProcessTicket(Ticket ticket) {

            return ProcessNextEvent(ticket.eventStats.NextStop);
        }

        public Ticket ProcessNextEvent(string event_id) {
            Ticket ticket = GetTicketFromID(event_id);



            if (ticket.valid) {

                if (ticket.eventStats.Tag == ParameterFlag.EventTag.Examination) {
                    return ProcessExamination(ticket);
                }

                bool hasPass = this._qmodel.CheckConstraint(ticket.eventStats.Constraint);

                if ((ticket.eventStats.Tag == ParameterFlag.EventTag.Question && ticket.choiceStats.Count <= 0) || !hasPass) {
                    return ProcessNextEvent(ticket.eventStats.FallbackStop);
                }
            }

            return ticket;
        }

        private Ticket GetTicketFromID(string id)
        {
            int EventStatsIndex = this._rawParseResult.EventStats.FindIndex(x => x._ID == id);
            var ticket = new Ticket();

            if (EventStatsIndex < 0) return ticket;

            EventStats eventStat = this._rawParseResult.EventStats[EventStatsIndex];

            ticket.eventStats = eventStat;

            if (eventStat.Tag == ParameterFlag.EventTag.Question)
            {
                ticket = GetChoiceTagTicket(ticket, eventStat);
            }

            ticket.index = EventStatsIndex;

            return ticket;
        }

        private Ticket GetTicketByIndex(int index) {
            var ticket = new Ticket();

            if (this._rawParseResult.EventStats.Count > index) return ticket;

            EventStats eventStat = this._rawParseResult.EventStats[index];

            ticket.eventStats = eventStat;

            if (eventStat.Tag == ParameterFlag.EventTag.Question)
            {
                ticket = GetChoiceTagTicket(ticket, eventStat);
            }

            ticket.index = index;

            return ticket;
        }


        #region Process Examination 
        private Ticket ProcessExamination(Ticket ticket) {

            List<ChoiceStats> choiceSet = _rawParseResult.ExaminationStats.FindAll(x => x.ChoiceID == ticket.eventStats.NextStop);

            foreach (var set in choiceSet) {
                //If Constraint check Fail
                if (_qmodel.CheckConstraint(set.Constraint)) {

                    _qmodel.RecordChoice(set, ticket.eventStats);

                    return ProcessNextEvent(set.NextStep);
                }
            }

            return ProcessNextEvent(ticket.eventStats.FallbackStop);
        }
        #endregion 

        #region Process Choice 
        private Ticket GetChoiceTagTicket(Ticket ticket, EventStats eventStat)
        {
            ticket.eventStats = eventStat;

            ticket.choiceStats = GetChoiceSet(eventStat.NextStop);

            return ticket;
        }

        private List<ChoiceStats> GetChoiceSet(string choice_id)
        {
            List<ChoiceStats> choiceSet = _rawParseResult.ChoiceStats.FindAll(x => x.ChoiceID == choice_id);

            return FilterSelectedChoice(choiceSet);
        }

        private List<ChoiceStats> FilterSelectedChoice(List<ChoiceStats> choice_set)
        {
            List<ChoiceStats> filterSet = new List<ChoiceStats>();
            int choiceLength = choice_set.Count;
            for (int i = 0; i < choiceLength; i++)
            {
                bool isSelected = this._qmodel.IsChoiceSelected(choice_set[i].ChoiceID, choice_set[i].UniqueID);
                bool noConstraint = this._qmodel.CheckConstraint(choice_set[i].Constraint);

                if (!isSelected && noConstraint)
                    filterSet.Add(choice_set[i]);
            }

            return filterSet;
        }
        #endregion


    }
}