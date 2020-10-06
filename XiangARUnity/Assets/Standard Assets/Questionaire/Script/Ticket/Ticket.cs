
using System.Collections;
using System.Collections.Generic;


namespace Questionaire
{
    public struct Ticket
    {
        public EventStats eventStats;

        public int index;

        //Only tag => Choice will have this variable
        public List<ChoiceStats> choiceStats;

        public string tag {
            get {
                return eventStats.Tag;
            }
        }

        public bool valid {
            get {
                return !string.IsNullOrEmpty(eventStats._ID);
            }
        }
    }
}