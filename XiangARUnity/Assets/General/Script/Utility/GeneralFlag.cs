using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFlag
{
    public class Regex {
        public const string UniversalSyntaxRex = @"^.{2,20}$";
    }

    public class ObeserverEvent {
        public const string AppStart = "event@start_app";

        //After lang is pick
        public const string TourStart = "event@start_tour";
        public const string AppEnd = "event@end_app";
    }

    public enum Language {
        En, Ch
    }

    public class ARTour {

    }

    public enum ARTourTheme {
        Spring, Summer, Autumn, Winter, None
    }

    public class Questionaire {
        public const string ThemeSpring = "area01";
        public const string ThemeSummer = "area02";
        public const string ThemeAutumn = "area03";
        public const string ThemeWinter = "area04";
    }

    public static readonly Dictionary<ARTourTheme, string> ThemeKeyLookUpTable = new Dictionary<ARTourTheme, string> {
        {ARTourTheme.Spring, Questionaire.ThemeSpring },
        {ARTourTheme.Summer, Questionaire.ThemeSummer },
        {ARTourTheme.Autumn, Questionaire.ThemeAutumn },
        {ARTourTheme.Winter, Questionaire.ThemeWinter }
    };
}