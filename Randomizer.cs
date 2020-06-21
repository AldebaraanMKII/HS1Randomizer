using Manager;
using System;  /////
using System.Collections;  /////
using System.Collections.Generic;  /////
using System.IO;  /////
using System.Linq;   ////
using System.Text;   /////
using System.Reflection;  /////
using UnityEngine;  /////
using UnityEngine.UI;  /////
//using IllusionPlugin;


namespace HS1_Randomizer
{
    public class Main : MonoBehaviour, IllusionPlugin.IPlugin
    {
		
		public CharFemale female = FindObjectOfType<CharFemale>();

        public class HS1RandomizeAll
        {
            public void Update()
            {
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    female.customInfo.shapeValueBody[0] = UnityEngine.Random.Range(0.6f, 0.95f);
                    female.customInfo.shapeValueBody[1] = UnityEngine.Random.Range(0.6f, 0.9f);
                }
            }
	    }	
  }
}









