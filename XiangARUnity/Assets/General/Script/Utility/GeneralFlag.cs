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
}
