using Expect.StaticAsset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixLionUtility 
{

    public static bool IsGivenToolAllowToProceed(string tool_name, int current_progress) {

        switch (tool_name) {
            case StringAsset.LionRepairing.ToolID_1:
                return current_progress >= 0;
            case StringAsset.LionRepairing.ToolID_2:
                return current_progress >= 1;
            case StringAsset.LionRepairing.ToolID_3:
                return current_progress >= 2;
        }

        return false;
    }
}
