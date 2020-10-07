using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Questionaire.Utility;

namespace Questionaire
{
    public class QModel
    {

        private Dictionary<string, int> ScoreDict;
        private List<ChoiceRecord> SelecteChoiceRecord;

        public QModel() {
            ScoreDict = new Dictionary<string, int>();
            SelecteChoiceRecord = new List<ChoiceRecord>();
        }

        /// <summary>
        /// True => Pass, False => didn't meet requirement
        /// </summary>
        /// <param name="p_raw_constraint"></param>
        /// <returns></returns>
        public bool CheckConstraint(string p_raw_constraint)
        {
            bool isValid = false;

            //if EMPTY == true 
            if (string.IsNullOrEmpty(p_raw_constraint)) return true;

            // string[] p_constraints =  p_raw_constraint.Split('&');
            string strDelimitor = "[&|]";

            string[] p_constraints = Regex.Split(p_raw_constraint, strDelimitor);
            string[] lines = Regex.Matches(p_raw_constraint, strDelimitor).
                                        Cast<Match>()
                                        .Select(m => m.Value)
                                        .ToArray();

            for (int i = 0; i < p_constraints.Length; i++)
            {
                string constraint = p_constraints[i];

                string[] c_varaibles = constraint.Split(' ');
                string trait_id = c_varaibles[0];
                string operator_string = c_varaibles[1];
                int c_value = int.Parse(c_varaibles[2]);

                //This one is very slow ==>>
                int m_value = FindTraitValue(trait_id);

                isValid = Utility.UtilityMethod.AnalyzeStringOperator(operator_string, c_value, m_value);

                Debug.Log("Constraint " + operator_string + ", " + c_value + ", " + m_value);

                //first element and the constraint fail, break loop
                if (i <= 0 && isValid == false)
                {
                    break;

                }
                else if (i > 0)
                {
                    string logicalSign = lines[i - 1];
                    switch (logicalSign)
                    {
                        //Break loop if a constraint check fail
                        case "&":
                            if (isValid == false) break;
                            break;

                        //Return true if isvalid passed
                        case "|":
                            if (isValid) return isValid;
                            break;
                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// Add effect variable to save dictionary
        /// </summary>
        /// <param name="p_raw_effect"></param>
        public void ImplementEffect(string p_raw_effect)
        {
            if (string.IsNullOrEmpty(p_raw_effect)) return;

            try
            {
                string[] attrGroup = p_raw_effect.Split('&');
                foreach (string s_attr in attrGroup)
                {
                    string[] attrValue = s_attr.Split(' ');
                    string trait_id = attrValue[0];
                    string operator_mark = attrValue[1];
                    int intValue = int.Parse(attrValue[2]);


                    if (ScoreDict.TryGetValue(trait_id, out int score))
                    {
                        ScoreDict[trait_id] = UtilityMethod.BasicMathOperator(operator_mark, intValue, score);
                    }
                    else
                    {
                        ScoreDict.Add(trait_id, UtilityMethod.BasicMathOperator(operator_mark, intValue, score));
                    }
                }
            }
            catch {
                Debug.LogError("AnalyzeEffect Parse Error");
            }
        }

        public int FindTraitValue(string trait_id) {
            if (ScoreDict.TryGetValue(trait_id, out int score)) {

                Debug.Log("trait_id " + trait_id + ", score " + score);
                return score;
            }

            return 0;
        }

        public bool IsChoiceSelected(string choice_id, string unique_id) {
            return SelecteChoiceRecord.Count(x => x.selectedChoice.ChoiceID == choice_id && x.selectedChoice.UniqueID == unique_id) > 0;
        }

        //Insert a record to dictionary
        public void RecordChoice(ChoiceStats selectedChoice, EventStats choiceEvent)
        {
            ImplementEffect(selectedChoice.Effect);

            if (!IsChoiceSelected(selectedChoice.ChoiceID, selectedChoice.UniqueID)) {
                ChoiceRecord choiceRecord = new ChoiceRecord();
                choiceRecord.selectedChoice = selectedChoice;
                choiceRecord.choiceEvent = choiceEvent;

                SelecteChoiceRecord.Add(choiceRecord);
            }
        }

        public List<string> GetRecordFailMessage() {
            List<string> failList = new List<string>();

            int choiceLength = SelecteChoiceRecord.Count;

            //Add Choice / Examination
            for (int i = 0; i < choiceLength; i++)
            {
                if (!string.IsNullOrEmpty(SelecteChoiceRecord[i].selectedChoice.Extra)) {
                    failList.Add(SelecteChoiceRecord[i].selectedChoice.Extra);
                }
            }


            return failList;
        }

        public struct ChoiceRecord {
            public ChoiceStats selectedChoice;
            public EventStats choiceEvent;
        }

        public void Reset() {
            ScoreDict.Clear();
            SelecteChoiceRecord.Clear();
        }

    }
}