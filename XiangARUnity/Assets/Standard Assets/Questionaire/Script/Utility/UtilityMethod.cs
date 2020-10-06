using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Questionaire.Utility {
    public class UtilityMethod
    {

        public static int BasicMathOperator(string p_operator, int p_value, int self_value)
        {
            switch (p_operator)
            {
                case "+":
                    self_value += p_value;
                    break;
                case "-":
                    self_value -= p_value;
                    break;

                case "*":
                    self_value *= p_value;
                    break;

                case "=":
                    self_value = p_value;
                    break;

            }

            return self_value;
        }

        public static bool AnalyzeStringOperator(string p_operator, int p_value, int self_value)
        {
            bool isValid = false;
            switch (p_operator)
            {
                case ">":
                    isValid = self_value > p_value;
                    break;
                case ">=":
                    isValid = self_value >= p_value;
                    break;

                case "<=":
                    isValid = self_value <= p_value;
                    break;

                case "<":
                    isValid = self_value < p_value;
                    break;

                case "==":
                    isValid = self_value == p_value;
                    break;

                case "!=":
                    isValid = self_value != p_value;
                    break;
            }
            return isValid;
        }
    }

}
