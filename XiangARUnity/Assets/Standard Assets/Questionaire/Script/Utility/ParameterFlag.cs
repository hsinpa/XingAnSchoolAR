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
        }

        public class QuestionaireParameter {
            public const string RandomValue = "random_value";
            public const string ClassLevel = "level";
            public const string Score = "score";
        }

        public class DatabasePath {
            public const string StampingEventStats = "Database/EventDatabase";
            public const string ChoiceStats = "Database/ChoiceDatabse";
            public const string ExaminationStats = "Database/ExaminationDatabase";
        }

        public class Style {
            public static readonly Color Correct = new Color(0.08410467f, 0.3962264f, 0.08977961f);
            public static readonly Color Wrong = new Color(0.6603774f, 0.264774f, 0.3467458f);
        }

    }
}
