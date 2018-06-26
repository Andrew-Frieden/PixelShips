using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    public MiningDemoShip miningDemoShip;

	// Use this for initialization
	void Start () {
        miningDemoShip = new MiningDemoShip(1, "USS BINKERS", ShipClass.YACHT, "MilesTheMighty");

        miningDemoShip.addItem(new CargoItem(1, 2));
        miningDemoShip.addItem(new CargoItem(2, 8));
        miningDemoShip.addItem(new CargoItem(3, 2));

        miningDemoShip.removeitem(new CargoItem(2, 5));

        miningDemoShip.addItem(new CargoItem(5, 1));
        miningDemoShip.addItem(new CargoItem(4, 1));

        Debug.Log(miningDemoShip.getStringManifest());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
