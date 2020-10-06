using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Questionaire
{
    public class ParameterFlag
    {

        public class EventTag {
            public const string Animation = "Animation";
            public const string End = "End";
            public const string Image = "Image";
            public const string Examination = "Examination";
            public const string Question = "Question";
        }

        public class StaticEventID {
            public const string Menu = "area01_01";
            public const string RandomValue = "random_value";
            public const string ClassLevel = "class_level";
        }

        public class DatabasePath {
            public const string StampingEventStats = "Database/EventDatabase";
            public const string ChoiceStats = "Database/ChoiceDatabse";
            public const string ExaminationStats = "Database/ExaminationDatabase";
        }

    }
}
