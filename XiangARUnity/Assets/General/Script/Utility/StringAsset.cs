using UnityEngine;

namespace Expect.StaticAsset {
    
    public class StringAsset
    {

        public class ARTour {
            public const string ChooseLangTitle = "導覽語言";
            public const string ChooseLangContent = "選擇合適的導覽語言";
            public const string LangEN = "英文";
            public const string LangCH = "中文";

            public const string ChooseGradeTitle = "學級選擇";
            public const string ChooseGradeContent = "選擇合適的難度等級";
            public const string LowGrade = "低年級";
            public const string MiddleGrade = "中年級";
            public const string HighGrade = "高年級";

            public const string QuestionaireSubmitBtn = "提交";
            public const string QuestionaireContinueBtn = "繼續";
            public const string AddScoreText = "+{0}分";

            public const string TourProceedToQuiz = "進行測試";
            public const string TourQuizIsDone = "測試結束";
        }

        public static string GetGradeString(int level) {
            if (level == 0) return ARTour.LowGrade;
            if (level == 1) return ARTour.MiddleGrade;
            return ARTour.HighGrade;
        }

    }

}