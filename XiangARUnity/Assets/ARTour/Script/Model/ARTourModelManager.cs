using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Model;

public class ARTourModelManager : ModelManager
{
    public override void SetUp()
    {
        base.SetUp();

        models.Add(new ARTourModel());
    }
}
