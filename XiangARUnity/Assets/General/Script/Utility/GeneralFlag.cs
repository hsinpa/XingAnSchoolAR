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
        public const string QuizStart = "event@start_quiz";
        public const string ThemeChange = "event@theme_change";

        public const string AppEnd = "event@end_app";
    }

    public enum Language {
        En, Ch
    }

    public class ARTour {
        public class QuestionType {
            public const string Continue = "Continue";
            public const string End = "End";
            public const string Question = "Question";
            public const string Examination = "Examination";
        }

        public class ImageTracker {
            public const string ThemeSpring = "ar_theme_spring";
            public const string ThemeSummer = "ar_theme_summer";
            public const string ThemeAutumn = "ar_theme_autumn";
            public const string ThemeWinter = "ar_theme_winter";
        }
    }

    public enum ARTourTheme {
        Spring, Summer, Autumn, Winter, None
    }

    public class Playerpref {
        public const string Level = "pp@level";
    }

    public class Questionaire {
        public const string ThemeSpring = "area01";
        public const string ThemeSummer = "area02";
        public const string ThemeAutumn = "area03";
        public const string ThemeWinter = "area04";

        //Question Key
        public const string QThemeSpring = "area01_01";
        public const string QThemeSummer = "area02_01";
        public const string QThemeAutumn = "area03_01";
        public const string QThemeWinter = "area04_01";
    }

    public static readonly Dictionary<ARTourTheme, string> ThemeKeyLookUpTable = new Dictionary<ARTourTheme, string> {
        {ARTourTheme.Spring, Questionaire.ThemeSpring },
        {ARTourTheme.Summer, Questionaire.ThemeSummer },
        {ARTourTheme.Autumn, Questionaire.ThemeAutumn },
        {ARTourTheme.Winter, Questionaire.ThemeWinter }
    };

    public static readonly Dictionary<ARTourTheme, string> QThemeLookUpTable = new Dictionary<ARTourTheme, string> {
        {ARTourTheme.Spring, Questionaire.QThemeSpring },
        {ARTourTheme.Summer, Questionaire.QThemeSummer },
        {ARTourTheme.Autumn, Questionaire.QThemeAutumn },
        {ARTourTheme.Winter, Questionaire.QThemeWinter }
    };

    public static readonly Dictionary<string, ARTourTheme> SThemeLookUpTable = new Dictionary<string, ARTourTheme> {
        {ARTour.ImageTracker.ThemeSpring, ARTourTheme.Spring },
        {ARTour.ImageTracker.ThemeSummer, ARTourTheme.Summer },
        {ARTour.ImageTracker.ThemeAutumn, ARTourTheme.Autumn },
        {ARTour.ImageTracker.ThemeWinter, ARTourTheme.Winter },
    };

}