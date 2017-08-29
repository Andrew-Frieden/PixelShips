using PixelSpace.Models.SharedModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// interacts with the AWS api to ping for updates and submit actions
/// </summary>
public class VerseController {

    public bool SubmitShipAction(ShipAction action)
    {
        var dbi = action.ToDbi();

        throw new NotImplementedException();
    }
}
