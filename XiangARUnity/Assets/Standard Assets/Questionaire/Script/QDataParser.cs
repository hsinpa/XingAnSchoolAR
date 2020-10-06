using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Questionaire.Utility;
using System.Linq;

namespace Questionaire
{
    public class QDataParser
    {
        string[] RawEventJSONArray;
        string RawChoiceJSON;
        string RawExaminationJSON;
       
        public QDataParser(string[] EventStatsPath, string ChoicePath, string ExaminationPath)
        {
            int eventStatCount = EventStatsPath.Length;

            RawEventJSONArray = new string[eventStatCount];

            for (int i = 0; i < eventStatCount; i++) {
                RawEventJSONArray[i] = Resources.Load<TextAsset>(EventStatsPath[i]).text;
            }

            RawChoiceJSON = Resources.Load<TextAsset>(ChoicePath).text;
            RawExaminationJSON = Resources.Load<TextAsset>(ExaminationPath).text;
        }

        public async Task<ParseResult> Parse() {
            ParseResult parseResult = new ParseResult();

            if (RawEventJSONArray == null || this.RawChoiceJSON == null || this.RawExaminationJSON == null) return parseResult;

            await Task.Run(() =>
            {
                parseResult.EventStats = ParseStatsArray<EventStats>(RawEventJSONArray);
                parseResult.ChoiceStats = ParseStats<ChoiceStats>(RawChoiceJSON);
                parseResult.ExaminationStats = ParseStats<ChoiceStats>(RawExaminationJSON);
            });

            Reset();

            return (parseResult);
        }

        private List<T> ParseStatsArray<T>(string[] RawStatArray) where T : struct
        {
            List<T> StatArray = new List<T>();

            int eventStatCount = RawStatArray.Length;

            for (int i = 0; i < eventStatCount; i++)
            {
                StatArray.AddRange(JsonHelper.FromJson<T>(RawStatArray[i]).ToList());
            }

            return StatArray;
        }

        private List<T> ParseStats<T>(string RawStatString) where T : struct
        {
            List<T> StatArray = new List<T>();
            StatArray.AddRange(JsonHelper.FromJson<T>(RawStatString).ToList());

            return StatArray;
        }

        public struct ParseResult {
            public List<EventStats> EventStats;
            public List<ChoiceStats> ChoiceStats;
            public List<ChoiceStats> ExaminationStats;
        }

        private void Reset() {
            RawEventJSONArray = null;
            RawChoiceJSON = null;
            RawExaminationJSON = null;
        }

    }
}