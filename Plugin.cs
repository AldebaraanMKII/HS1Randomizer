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


namespace HS1_RandomizerPlugin
{
    public class Plugin : MonoBehaviour, IllusionPlugin.IPlugin
    {

        //public RandomPhotoCtrlPanel randPhotoCtrl;
        //
        // public CharFemale female = FindObjectOfType;
		// public static CharFemale charFemale = null;
		
		public CharFemale female = FindObjectOfType<CharFemale>();
	    //levelManager = GameObject.FindObjectOfType<LevelManager>();
        //var motionReceiver = FindObjectOfType<OpponentMotionReceiver>();
		//public static void femaleCum()
		//{
		//	CharFemale[] array = Object.FindObjectsOfType<CharFemale>();
		//	foreach (CharFemale charFemale in array)
		//	{
		//		HParticleCtrl hParticleCtrl = new HParticleCtrl();
		//		hParticleCtrl.Init("list/h/", charFemale.chaBody.objBone);
		//		hParticleCtrl.Play(0);
		//	}
		//}
		
        //
        //protected bool saveCard;
        ////////////////////////////////////////////////

        //public virtual void Init(CharFemale _female)
        //{
        //    female = _female;
        //}
        ////////////////////////////////////////////////
        public string Name { get { return "HS1_Randomizer"; } }

        public string Version { get { return "0.1"; } }

        public void OnApplicationQuit()
        {
        }

        public void OnApplicationStart()
        {
            //_log.Info("Starting HS1_Randomizer...");
        }

        public void OnFixedUpdate()
        {
        }

        public void OnLateUpdate()
        {
        }


        public void OnLevelWasInitialized(int level)
        {
            ////(check for maker scene)
            var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (sceneName == "CustomScene")
            {
                //create new GameObject() + add MonoBehaviour to it
                var newObject = new GameObject();
                newObject.AddComponent<Randomizer.HS1_Randomizer.HS1RandomizeAll>();
            }
        }

        //   var customMainMenu = GameObject.Find("CustomMainMenu");
        //   _partManager = customMainMenu.AddComponent<PartManager>();
       
        public void OnLevelWasLoaded(int level)
        {
            // if (_partManager != null)
            // {
            //    _partManager.Unload();
            //    _partManager = null;
            // }
        }
       
        ////////////////////////////////////////////////
        public void OnUpdate()
        {
            //if (_partManager != null)
            //{	
        }
    ////////////////////////////////////////////////	
  }
}









