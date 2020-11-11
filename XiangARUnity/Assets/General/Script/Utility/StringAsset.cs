using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

            public const string QuestionarieCorrectMsg = "恭喜, 你答對了";
            public const string QuestionarieWrongMsg = "真可惜, 你答錯了";

            public const string TourProceedToQuiz = "進行測試";
            public const string TourQuizIsDone = "測試結束";

            public const string EndGameTitle = "分數結算";
            public const string EndGameStatement = "你完成了四個關卡的問題, 總分為";
            public const string EndGameSubStatement = "請停留在此頁面, \n將行動裝置拿給課堂老師登記成績";
            public const string EndGameButton = "完成成績登記";

            public const string ShowAreaScoreStatement = "你在本關獲得了";
            public const string ShowAreaScoreSubStatement = "";
            public const string ShowAreaScoreButton = "確定";

            public const string ConfirmText = "正確";
            public const string MistakeText = "錯誤";
        }

        public class LionRepairing {
            public const string ToolID_1 = "tool_01";
            public const string ToolID_2 = "tool_02";
            public const string ToolID_3 = "tool_03";

            public const string UnavilableTipTool_1 = "";
            public const string UnavilableTipTool_2 = "需要先用過Tool_1 清潔";
            public const string UnavilableTipTool_3 = "需要先用過Tool_2 清潔";
            public const string ToolUsedMessage = "已完成";

            public const string DoneFixedTitle = "情理環節結束";
            public const string DoneFixedContent = "清除工作完成";
            public const string DoneFixedButton = "重新體驗";
        }

        public static string GetGradeString(int level) {
            if (level == 0) return ARTour.LowGrade;
            if (level == 1) return ARTour.MiddleGrade;
            return ARTour.HighGrade;
        }

        public enum GetToolStatusType { 
            Available, AlreadyUsed, Unavilable
        }

        public static Dictionary<string, string> UnavilableTipTable = new Dictionary<string, string>() {
            { LionRepairing.ToolID_1 , LionRepairing.UnavilableTipTool_1},
            { LionRepairing.ToolID_2 , LionRepairing.UnavilableTipTool_2},
            {LionRepairing.ToolID_3, LionRepairing.UnavilableTipTool_3}
        };

        private static Dictionary<string, ToolSRP.ToolEnum> ToolIDToEnumTable = new Dictionary<string, ToolSRP.ToolEnum> {
            { LionRepairing.ToolID_1 , ToolSRP.ToolEnum.Tool_1},
            { LionRepairing.ToolID_2 , ToolSRP.ToolEnum.Tool_2},
            {LionRepairing.ToolID_3, ToolSRP.ToolEnum.Tool_3}
        };

        public static ToolSRP.ToolEnum GetToolEnumByID(string id) {
            if (ToolIDToEnumTable.TryGetValue(id, out ToolSRP.ToolEnum toolEnum)) { 
                return toolEnum;
            }

            return ToolSRP.ToolEnum.Tool_1;
        }

    }

}