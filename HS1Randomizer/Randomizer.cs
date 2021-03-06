﻿using Manager;
using IllusionPlugin;
using System;  /////
using System.Collections;
using System.Collections.Generic;  /////
using System.Linq;   ////
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using RandomSearch;
using System.Reflection;
using SimpleJSON;



public class HSCharRandom : IPlugin
{
    public void OnApplicationQuit() { }
    public void OnApplicationStart() { }
    public void OnFixedUpdate() { }
    public void OnLateUpdate() { }
    public void OnUpdate() { }
    public void OnLevelWasInitialized(int level) { }

    public string Name => nameof(HSCharRandom);
    public string Version => "1.1";

    public void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name != "CustomScene") return;

        var newObject = new GameObject();
        newObject.AddComponent<HS1RandomizeAll>();
    }
}

public class HS1RandomizeAll : MonoBehaviour
{
    private CharFemale female;
    public RandomPhotoCtrlPanel randPhotoCtrl;
    protected bool saveCard;
    protected int coordinateType;

    string Key1;
    string Key2;
    string Key3;
    string Key4;
    string Key5;

    public Vector2 currentkey;

    protected List<CharFemaleRandom.RandomFaceFemaleInfo> lstRandFaceF = new List<CharFemaleRandom.RandomFaceFemaleInfo>();

    void Awake()
    {
        StartCoroutine(WaitForCharsToExist());
    }

    IEnumerator WaitForCharsToExist()
    {
        CustomControl cc = null;
        yield return new WaitUntil(() =>
            (cc = FindObjectOfType<CustomControl>()) != null
        );
        female = cc.chainfo as CharFemale;
        LoadAssets();

        ////GetExecutingAssembly finds the path where THIS code is running
        var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string configFilename = "HS1Randomizer.json";
        var configPath = Path.Combine(dllPath, configFilename);


        //// from https://answers.unity.com/questions/1473952/how-to-write-and-read-json-in-unity.html  
        //// https://github.com/Bunny83/SimpleJSON
        string jsonString = File.ReadAllText(configPath);
        JSONNode data = JSON.Parse(jsonString);
        JSONNode keycodes = data["KeyCodes"];

        Key1 = keycodes["RandomizeAllKey"];
        Key2 = keycodes["RandomizeFaceKey"];
        Key3 = keycodes["RandomizeBodyKey"];
        Key4 = keycodes["RandomizeFaceBodyKey"];
        Key5 = keycodes["RandomizeClothesKey"];

        // Key1 = "[1]";
        // Key2 = "[2]";
        // Key3 = "[3]";
        // Key4 = "[4]";
        // Key5 = "[5]";
    }

    void Update()
    {
        //////////////////////////////////////// All
        if (Input.GetKeyDown(Key1))
        {
            RandomiseCharAll();
            //female.Reload();
        }
        //////////////////////////////////////// Only face
        if (Input.GetKeyDown(Key2))
        {
            RandomiseCharPersonality(female);
            RandomiseCharFace(lstRandFaceF);
            female.chaFile.ChangeCoordinateType((CharDefine.CoordinateType)coordinateType);
            female.Reload();
        }
        //////////////////////////////////////// Only body
        if (Input.GetKeyDown(Key3))
        {
            RandomiseCharBody();
            female.chaFile.ChangeCoordinateType((CharDefine.CoordinateType)coordinateType);
            female.Reload();
        }
        //////////////////////////////////////// Body/Face
        if (Input.GetKeyDown(Key4))
        {
            RandomiseCharBodyFace();
            female.chaFile.ChangeCoordinateType((CharDefine.CoordinateType)coordinateType);
            female.Reload();
        }
        //////////////////////////////////////// Clothes and accessories
        if (Input.GetKeyDown(Key5))
        {
            RandomiseCharClothing();
            female.chaFile.ChangeCoordinateType((CharDefine.CoordinateType)coordinateType);
            female.Reload();
        }
        //////////////////////////////////////// Save card
        //if (Input.GetKeyDown(Key6))
        //{
        //    CustomMenu.SubMenuBase.ExecuteSaveNew();
        //}
        ////////////////////////////////////////
		
        //////////////////////////////////////// Export card data to preset
        //if (Input.GetKeyDown(Key7))
        //{
        //    CustomMenu.SubMenuBase.ExecuteSaveNew();
        //}
        ////////////////////////////////////////
    }

    void RandomiseCharAll()
    {
        if (female == null) return;
        RandomiseCharPersonality(female);
        RandomiseCharFace(lstRandFaceF);
        RandomiseCharBody();
        RandomiseCharClothing();

        female.chaFile.ChangeCoordinateType((CharDefine.CoordinateType)coordinateType);
        female.Reload();
        //UpdateCharaInfo();
        saveCard = false;
        PersonalityIdInfo personalityInfo = female.ListInfo.GetPersonalityInfo(female.chaFile.customInfo.personality);
        int ptn = personalityInfo.randAnmPtn[UnityEngine.Random.Range(0, 3)];
        randPhotoCtrl.ChangeCustomAnm(ptn);
        female.ChangeEyesPtn(personalityInfo.randEyesPtn);
        female.ChangeMouthPtn(personalityInfo.randMouthPtn);
        Singleton<Voice>.Instance.StopAll();
        Transform transform = Singleton<Voice>.Instance.Play(female.chaFile.customInfo.personality, personalityInfo.assetBundleName, personalityInfo.randomVoice, female.chaFile.customInfo.voicePitch);
        if ((bool)transform)
        {
            female.SetVoice(transform);
        }
        return;
        // female.chaCustom.UpdateShapeBodyValueFromCustomInfo();
    }


    void LoadAssets()
    {
        AssetBundleLoadAssetOperation assetBundleLoadAssetOperation = null;
        string assetBundleName = "custom/face_randf.unity3d";
        assetBundleLoadAssetOperation = AssetBundleManager.LoadAllAsset(assetBundleName, typeof(TextAsset));
        if (assetBundleLoadAssetOperation != null && !assetBundleLoadAssetOperation.IsEmpty())
        {
            TextAsset[] allAssets = assetBundleLoadAssetOperation.GetAllAssets<TextAsset>();
            TextAsset[] array = allAssets;
            foreach (TextAsset ta in array)
            {
                CharFemaleRandom.RandomFaceFemaleInfo randomFaceFemaleInfo = new CharFemaleRandom.RandomFaceFemaleInfo();
                randomFaceFemaleInfo.Load(ta);
                lstRandFaceF.Add(randomFaceFemaleInfo);
            }
        }
        AssetBundleManager.UnloadAssetBundle(assetBundleName);
    }


    void RandomiseCharFace(List<CharFemaleRandom.RandomFaceFemaleInfo> lstRandFace)
    {
        if (female == null) return;
        //4
        //26
        //9
        int index = UnityEngine.Random.Range(0, 4);
        int index22 = UnityEngine.Random.Range(0, 26);
        int index23 = UnityEngine.Random.Range(0, 9);
        //Array.Copy(lstRandFace[index].shapeValue, female.customInfo.shapeValueFace, lstRandFace[index].shapeValue.Length);
        female.customInfo.headId = lstRandFace[index].headNo;
        female.customInfo.texFaceId = lstRandFace[index22].baseTexNo;
        female.customInfo.texFaceDetailId = lstRandFace[index23].detailTexNo;
        // female.customInfo.faceDetailWeight = lstRandFace[index].detailWeight;
        female.customInfo.faceDetailWeight = UnityEngine.Random.Range(0.1f, 0.8f);


        var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string configFilename = "HS1Randomizer.json";
        var configPath = Path.Combine(dllPath, configFilename);

        //// from https://answers.unity.com/questions/1473952/how-to-write-and-read-json-in-unity.html  
        //// https://github.com/Bunny83/SimpleJSON
        string jsonString = File.ReadAllText(configPath);
        JSONNode data = JSON.Parse(jsonString);
        JSONNode FacePresets = data["FacePresets"];
        JSONNode FaceSettings = data["FaceSettings"];

        ////gets a random preset from the list
        var randompreset = FacePresets[UnityEngine.Random.Range(0, FacePresets.Count)];

        float min0 = randompreset["overall_face_breadth"]["min"];
        float min1 = randompreset["upper_face_depth"]["min"];
        float min2 = randompreset["face_height"]["min"];
        float min3 = randompreset["lower_face_depth"]["min"];
        float min4 = randompreset["lower_face_width"]["min"];
        float min5 = randompreset["jaw_width"]["min"];
        float min6 = randompreset["jaw_height"]["min"];
        float min7 = randompreset["jaw_depth"]["min"];
        float min8 = randompreset["jaw_angle"]["min"];
        float min9 = randompreset["neck_droop"]["min"];
        float min10 = randompreset["chin_width"]["min"];
        float min11 = randompreset["chin_height"]["min"];
        float min12 = randompreset["chin_depth"]["min"];
        float min13 = randompreset["lower_cheek_height"]["min"];
        float min14 = randompreset["lower_cheek_depth"]["min"];
        float min15 = randompreset["lower_cheek_width"]["min"];
        float min16 = randompreset["upper_cheek_height"]["min"];
        float min17 = randompreset["upper_cheek_depth"]["min"];
        float min18 = randompreset["upper_cheek_width"]["min"];
        float min19 = randompreset["eyebrow_height"]["min"];
        float min20 = randompreset["eyebrow_spacing"]["min"];
        float min21 = randompreset["eyebrow_angle"]["min"];
        float min22 = randompreset["inner_arching"]["min"];
        float min23 = randompreset["outer_arching"]["min"];
        float min24 = randompreset["eye_height"]["min"];
        float min25 = randompreset["eye_spacing"]["min"];
        float min26 = randompreset["eye_depth"]["min"];
        float min27 = randompreset["eye_width"]["min"];
        float min28 = randompreset["eye_openness"]["min"];
        float min29 = randompreset["eye_angle 1"]["min"];
        float min30 = randompreset["eye_angle 2"]["min"];
        float min31 = randompreset["inner_corner_distance"]["min"];
        float min32 = randompreset["outer_corner_distance"]["min"];
        float min33 = randompreset["inner_corner_height"]["min"];
        float min34 = randompreset["outer_corner_height"]["min"];
        float min35 = randompreset["eyelid_shape_1"]["min"];
        float min36 = randompreset["eyelid_shape_2"]["min"];
        float min37 = randompreset["pupil_pos"]["min"];
        float min38 = randompreset["pupil_width"]["min"];
        float min39 = randompreset["pupil_height"]["min"];
        float min40 = randompreset["nose_height"]["min"];
        float min41 = randompreset["nose_projection"]["min"];
        float min42 = randompreset["nose_angle"]["min"];
        float min43 = randompreset["nose_size"]["min"];
        float min44 = randompreset["bridge_height"]["min"];
        float min45 = randompreset["bridge_width"]["min"];
        float min46 = randompreset["bridge_shape"]["min"];
        float min47 = randompreset["nose_width"]["min"];
        float min48 = randompreset["nostril_height"]["min"];
        float min49 = randompreset["nostril_length"]["min"];
        float min50 = randompreset["nostril_inner width"]["min"];
        float min51 = randompreset["nostril_outer width"]["min"];
        float min52 = randompreset["tip_height"]["min"];
        float min53 = randompreset["tip_length"]["min"];
        float min54 = randompreset["tip_size"]["min"];
        float min55 = randompreset["mouth_height"]["min"];
        float min56 = randompreset["mouth_width"]["min"];
        float min57 = randompreset["lip thickness"]["min"];
        float min58 = randompreset["mouth projection"]["min"];
        float min59 = randompreset["upper lip thickness"]["min"];
        float min60 = randompreset["lower lip thickness"]["min"];
        float min61 = randompreset["corner shape"]["min"];
        float min62 = randompreset["ear size"]["min"];
        float min63 = randompreset["ear angle"]["min"];
        float min64 = randompreset["ear rotation"]["min"];
        float min65 = randompreset["upper ear shape"]["min"];
        float min66 = randompreset["lower ear shape"]["min"];
              
        float max0 = randompreset["overall_face_breadth"]["max"];
        float max1 = randompreset["upper_face_depth"]["max"];
        float max2 = randompreset["face_height"]["max"];
        float max3 = randompreset["lower_face_depth"]["max"];
        float max4 = randompreset["lower_face_width"]["max"];
        float max5 = randompreset["jaw_width"]["max"];
        float max6 = randompreset["jaw_height"]["max"];
        float max7 = randompreset["jaw_depth"]["max"];
        float max8 = randompreset["jaw_angle"]["max"];
        float max9 = randompreset["neck_droop"]["max"];
        float max10 = randompreset["chin_width"]["max"];
        float max11 = randompreset["chin_height"]["max"];
        float max12 = randompreset["chin_depth"]["max"];
        float max13 = randompreset["lower_cheek_height"]["max"];
        float max14 = randompreset["lower_cheek_depth"]["max"];
        float max15 = randompreset["lower_cheek_width"]["max"];
        float max16 = randompreset["upper_cheek_height"]["max"];
        float max17 = randompreset["upper_cheek_depth"]["max"];
        float max18 = randompreset["upper_cheek_width"]["max"];
        float max19 = randompreset["eyebrow_height"]["max"];
        float max20 = randompreset["eyebrow_spacing"]["max"];
        float max21 = randompreset["eyebrow_angle"]["max"];
        float max22 = randompreset["inner_arching"]["max"];
        float max23 = randompreset["outer_arching"]["max"];
        float max24 = randompreset["eye_height"]["max"];
        float max25 = randompreset["eye_spacing"]["max"];
        float max26 = randompreset["eye_depth"]["max"];
        float max27 = randompreset["eye_width"]["max"];
        float max28 = randompreset["eye_openness"]["max"];
        float max29 = randompreset["eye_angle 1"]["max"];
        float max30 = randompreset["eye_angle 2"]["max"];
        float max31 = randompreset["inner_corner_distance"]["max"];
        float max32 = randompreset["outer_corner_distance"]["max"];
        float max33 = randompreset["inner_corner_height"]["max"];
        float max34 = randompreset["outer_corner_height"]["max"];
        float max35 = randompreset["eyelid_shape_1"]["max"];
        float max36 = randompreset["eyelid_shape_2"]["max"];
        float max37 = randompreset["pupil_pos"]["max"];
        float max38 = randompreset["pupil_width"]["max"];
        float max39 = randompreset["pupil_height"]["max"];
        float max40 = randompreset["nose_height"]["max"];
        float max41 = randompreset["nose_projection"]["max"];
        float max42 = randompreset["nose_angle"]["max"];
        float max43 = randompreset["nose_size"]["max"];
        float max44 = randompreset["bridge_height"]["max"];
        float max45 = randompreset["bridge_width"]["max"];
        float max46 = randompreset["bridge_shape"]["max"];
        float max47 = randompreset["nose_width"]["max"];
        float max48 = randompreset["nostril_height"]["max"];
        float max49 = randompreset["nostril_length"]["max"];
        float max50 = randompreset["nostril_inner width"]["max"];
        float max51 = randompreset["nostril_outer width"]["max"];
        float max52 = randompreset["tip_height"]["max"];
        float max53 = randompreset["tip_length"]["max"];
        float max54 = randompreset["tip_size"]["max"];
        float max55 = randompreset["mouth_height"]["max"];
        float max56 = randompreset["mouth_width"]["max"];
        float max57 = randompreset["lip thickness"]["max"];
        float max58 = randompreset["mouth projection"]["max"];
        float max59 = randompreset["upper lip thickness"]["max"];
        float max60 = randompreset["lower lip thickness"]["max"];
        float max61 = randompreset["corner shape"]["max"];
        float max62 = randompreset["ear size"]["max"];
        float max63 = randompreset["ear angle"]["max"];
        float max64 = randompreset["ear rotation"]["max"];
        float max65 = randompreset["upper ear shape"]["max"];
        float max66 = randompreset["lower ear shape"]["max"];


         female.customInfo.shapeValueFace[0] = UnityEngine.Random.Range(min0, max0);
         female.customInfo.shapeValueFace[1] = UnityEngine.Random.Range(min1, max1);
         female.customInfo.shapeValueFace[2] = UnityEngine.Random.Range(min2, max2);
         female.customInfo.shapeValueFace[3] = UnityEngine.Random.Range(min3, max3);
         female.customInfo.shapeValueFace[4] = UnityEngine.Random.Range(min4, max4);
         female.customInfo.shapeValueFace[5] = UnityEngine.Random.Range(min5, max5);
         female.customInfo.shapeValueFace[6] = UnityEngine.Random.Range(min6, max6);
         female.customInfo.shapeValueFace[7] = UnityEngine.Random.Range(min7, max7);
         female.customInfo.shapeValueFace[8] = UnityEngine.Random.Range(min8, max8);
         female.customInfo.shapeValueFace[9] = UnityEngine.Random.Range(min9, max9);
        female.customInfo.shapeValueFace[10] = UnityEngine.Random.Range(min10, max10);
        female.customInfo.shapeValueFace[11] = UnityEngine.Random.Range(min11, max11);
        female.customInfo.shapeValueFace[12] = UnityEngine.Random.Range(min12, max12);
        female.customInfo.shapeValueFace[13] = UnityEngine.Random.Range(min13, max13);
        female.customInfo.shapeValueFace[14] = UnityEngine.Random.Range(min14, max14);
        female.customInfo.shapeValueFace[15] = UnityEngine.Random.Range(min15, max15);
        female.customInfo.shapeValueFace[16] = UnityEngine.Random.Range(min16, max16);
        female.customInfo.shapeValueFace[17] = UnityEngine.Random.Range(min17, max17);
        female.customInfo.shapeValueFace[18] = UnityEngine.Random.Range(min18, max18);
        female.customInfo.shapeValueFace[19] = UnityEngine.Random.Range(min19, max19);
        female.customInfo.shapeValueFace[20] = UnityEngine.Random.Range(min20, max20);
        female.customInfo.shapeValueFace[21] = UnityEngine.Random.Range(min21, max21);
        female.customInfo.shapeValueFace[22] = UnityEngine.Random.Range(min22, max22);
        female.customInfo.shapeValueFace[23] = UnityEngine.Random.Range(min23, max23);
        female.customInfo.shapeValueFace[24] = UnityEngine.Random.Range(min24, max24);
        female.customInfo.shapeValueFace[25] = UnityEngine.Random.Range(min25, max25);
        female.customInfo.shapeValueFace[26] = UnityEngine.Random.Range(min26, max26);
        female.customInfo.shapeValueFace[27] = UnityEngine.Random.Range(min27, max27);
        female.customInfo.shapeValueFace[28] = UnityEngine.Random.Range(min28, max28);
        female.customInfo.shapeValueFace[29] = UnityEngine.Random.Range(min29, max29);
        female.customInfo.shapeValueFace[30] = UnityEngine.Random.Range(min30, max30);
        female.customInfo.shapeValueFace[31] = UnityEngine.Random.Range(min31, max31);
        female.customInfo.shapeValueFace[32] = UnityEngine.Random.Range(min32, max32);
        female.customInfo.shapeValueFace[33] = UnityEngine.Random.Range(min33, max33);
        female.customInfo.shapeValueFace[34] = UnityEngine.Random.Range(min34, max34);
        female.customInfo.shapeValueFace[35] = UnityEngine.Random.Range(min35, max35);
        female.customInfo.shapeValueFace[36] = UnityEngine.Random.Range(min36, max36);
        female.customInfo.shapeValueFace[37] = UnityEngine.Random.Range(min37, max37);
        female.customInfo.shapeValueFace[38] = UnityEngine.Random.Range(min38, max38);
        female.customInfo.shapeValueFace[39] = UnityEngine.Random.Range(min39, max39);
        female.customInfo.shapeValueFace[40] = UnityEngine.Random.Range(min40, max40);
        female.customInfo.shapeValueFace[41] = UnityEngine.Random.Range(min41, max41);
        female.customInfo.shapeValueFace[42] = UnityEngine.Random.Range(min42, max42);
        female.customInfo.shapeValueFace[43] = UnityEngine.Random.Range(min43, max43);
        female.customInfo.shapeValueFace[44] = UnityEngine.Random.Range(min44, max44);
        female.customInfo.shapeValueFace[45] = UnityEngine.Random.Range(min45, max45);
        female.customInfo.shapeValueFace[46] = UnityEngine.Random.Range(min46, max46);
        female.customInfo.shapeValueFace[47] = UnityEngine.Random.Range(min47, max47);
        female.customInfo.shapeValueFace[48] = UnityEngine.Random.Range(min48, max48);
        female.customInfo.shapeValueFace[49] = UnityEngine.Random.Range(min49, max49);
        female.customInfo.shapeValueFace[50] = UnityEngine.Random.Range(min50, max50);
        female.customInfo.shapeValueFace[51] = UnityEngine.Random.Range(min51, max51);
        female.customInfo.shapeValueFace[52] = UnityEngine.Random.Range(min52, max52);
        female.customInfo.shapeValueFace[53] = UnityEngine.Random.Range(min53, max53);
        female.customInfo.shapeValueFace[54] = UnityEngine.Random.Range(min54, max54);
        female.customInfo.shapeValueFace[55] = UnityEngine.Random.Range(min55, max55);
        female.customInfo.shapeValueFace[56] = UnityEngine.Random.Range(min56, max56);
        female.customInfo.shapeValueFace[57] = UnityEngine.Random.Range(min57, max57);
        female.customInfo.shapeValueFace[58] = UnityEngine.Random.Range(min58, max58);
        female.customInfo.shapeValueFace[59] = UnityEngine.Random.Range(min59, max59);
        female.customInfo.shapeValueFace[60] = UnityEngine.Random.Range(min60, max60);
        female.customInfo.shapeValueFace[61] = UnityEngine.Random.Range(min61, max61);
        female.customInfo.shapeValueFace[62] = UnityEngine.Random.Range(min62, max62);
        female.customInfo.shapeValueFace[63] = UnityEngine.Random.Range(min63, max63);
        female.customInfo.shapeValueFace[64] = UnityEngine.Random.Range(min64, max64);
        female.customInfo.shapeValueFace[65] = UnityEngine.Random.Range(min65, max65);
        female.customInfo.shapeValueFace[66] = UnityEngine.Random.Range(min66, max66);

        //////////////////////////////// Hair color
        float num2 = 0f;
        float num3 = 0f;
        float num4 = 0f;
        float num5 = 0f;
        float num6 = 0f;
        float num7 = 0f;
        float num8 = 0f;
        float num9 = 0f;
        // predefined
        int hair_color_predefined_preset_chance = FaceSettings["hair_color_predefined_preset_chance"];
        if (UnityEngine.Random.Range(0, 100) < hair_color_predefined_preset_chance)
        {
		    ///////////// Hair color presets
            JSONNode HairColors = data["HairColors"];
            ////gets a random preset from the list
            var randomhaircolor = HairColors[UnityEngine.Random.Range(0, HairColors.Count)];
			
            num2 = randomhaircolor["BaseColorHue"];
            num3 = randomhaircolor["BaseColorSaturation"];
            //num4 = randomhaircolor["BaseColorValue"];
		    
            // float BaseColorValueMin = FaceSettings["hair_color_brightness_min"];
            // float BaseColorValueMax = FaceSettings["hair_color_brightness_max"];
            float BaseColorValueMin = FaceSettings["hair_color_brightness"]["min"];
            float BaseColorValueMax = FaceSettings["hair_color_brightness"]["max"];
            num4 = UnityEngine.Random.Range(BaseColorValueMin, BaseColorValueMax);

            num5 = randomhaircolor["ShineColorHue"];
            num6 = randomhaircolor["ShineColorSaturation"];
            //num7 = randomhaircolor["ShineColorValue"];
		    
            // float ShineColorValueMin = FaceSettings["hair_color_shine_brightness_min"];
            // float ShineColorValueMax = FaceSettings["hair_color_shine_brightness_max"];
            float ShineColorValueMin = FaceSettings["hair_color_shine_brightness"]["min"];
            float ShineColorValueMax = FaceSettings["hair_color_shine_brightness"]["max"];
            num7 = UnityEngine.Random.Range(ShineColorValueMin, ShineColorValueMax);
			
            num8 = randomhaircolor["ColorIntensity"];
            num9 = randomhaircolor["ColorSoftness"];
			
        }
        else
        {
          num2 = UnityEngine.Random.Range(0f, 359f);
          num3 = UnityEngine.Random.Range(0f, 1f);
          //num4 = UnityEngine.Random.Range(0.3f, 0.6f);
          float BaseColorValueMin = FaceSettings["hair_color_brightness"]["min"];
          float BaseColorValueMax = FaceSettings["hair_color_brightness"]["max"];
          num4 = UnityEngine.Random.Range(BaseColorValueMin, BaseColorValueMax);
		  
          num5 = num2;
          num6 = num3;
          //num7 = num4 + 0.15f;
          float ShineColorValueMin = FaceSettings["hair_color_shine_brightness"]["min"];
          float ShineColorValueMax = FaceSettings["hair_color_shine_brightness"]["max"];
          num7 = UnityEngine.Random.Range(ShineColorValueMin, ShineColorValueMax);
		  
          num8 = 0.6f;
          num9 = 0.6f;
		
        }


        //////////////////////////////// hairType (Back)
        bool flag = false;
        Dictionary<int, ListTypeFbx> femaleFbxList = female.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairB);
        List<int> list = new List<int>();


        //switch (hairType)
        //{
        //default:
        foreach (KeyValuePair<int, ListTypeFbx> item6 in femaleFbxList)
        {
            if (!("0" == item6.Value.Etc[1]))
            {
                list.Add(item6.Key);
            }
        }
        //break;
        //}
		//////////////////////////////// hairType (Back)
        // int index5 = UnityEngine.Random.Range(0, list.Count);
        //46 base
        //53 coiffure            
        int index5 = UnityEngine.Random.Range(0, 99);
        if ("1" == femaleFbxList[list[index5]].Etc[0])
        {
            flag = true;
        }
        female.customInfo.hairId[0] = list[index5];

        if (!flag)
        {
            //////////////////////////////// hairType (Front)
            Dictionary<int, ListTypeFbx> femaleFbxList2 = female.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairF);
            // int index6 = UnityEngine.Random.Range(1, femaleFbxList2.Count);
            //41 base
            //46 coiffure
            //27 useless stuff
            int index6 = UnityEngine.Random.Range(1, 88);
            female.customInfo.hairId[1] = femaleFbxList2.ElementAt(index6).Key;

            //////////////////////////////// hairType (Side)
            if (UnityEngine.Random.Range(0, 100) < 50)
            {
                Dictionary<int, ListTypeFbx> femaleFbxList3 = female.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairS);
                // int index7 = UnityEngine.Random.Range(1, femaleFbxList3.Count);
                //7 base 
                //10 crap
                //23 coiffure
                int index7 = UnityEngine.Random.Range(1, 40);
                if (index7 >= 8 && index7 <= 17)
                {
                    index7 = index7 + 10;
                }
                female.customInfo.hairId[2] = femaleFbxList3.ElementAt(index7).Key;
            }
            else
            {
                female.customInfo.hairId[2] = 0;
            }
        }
        else
        {
            female.customInfo.hairId[1] = 0;
            female.customInfo.hairId[2] = 0;
        }
        //////////////////////////////// Hair color back
        female.customInfo.hairColor[0].hsvDiffuse.Copy(new HsvColor(num2, num3, num4));
        female.customInfo.hairColor[0].hsvSpecular.Copy(new HsvColor(num5, num6, num7));
        female.customInfo.hairColor[0].specularIntensity = num8;
        female.customInfo.hairColor[0].specularSharpness = num9;
        //////////////////////////////// Hair color front	 
        female.customInfo.hairColor[1].hsvDiffuse.Copy(new HsvColor(num2, num3, num4));
        female.customInfo.hairColor[1].hsvSpecular.Copy(new HsvColor(num5, num6, num7));
        female.customInfo.hairColor[1].specularIntensity = num8;
        female.customInfo.hairColor[1].specularSharpness = num9;
        //////////////////////////////// Hair color side
        female.customInfo.hairColor[2].hsvDiffuse.Copy(new HsvColor(num2, num3, num4));
        female.customInfo.hairColor[2].hsvSpecular.Copy(new HsvColor(num5, num6, num7));
        female.customInfo.hairColor[2].specularIntensity = num8;
        female.customInfo.hairColor[2].specularSharpness = num9;

        //////////////////////////////// Fingernails color		
        JSONNode BodySettings = data["BodySettings"];
        int firgernail_paint_chance = BodySettings["firgernail_paint_chance"];
        if (UnityEngine.Random.Range(0, 100) < firgernail_paint_chance)
        {
            int firgernail_paint_match_hair_color_chance = BodySettings["firgernail_paint_match_hair_color_chance"];
            if (UnityEngine.Random.Range(0, 100) < firgernail_paint_match_hair_color_chance)
            {
              female.femaleCustomInfo.nailColor.hsvDiffuse.Copy(new HsvColor(num2, num3, num4));
              female.femaleCustomInfo.nailColor.hsvSpecular.Copy(new HsvColor(num5, num6, num7));
              female.femaleCustomInfo.nailColor.specularIntensity = num8;
              female.femaleCustomInfo.nailColor.specularSharpness = num9;
            }
			else
			{
              num2 = UnityEngine.Random.Range(0f, 359f);
              num3 = UnityEngine.Random.Range(0f, 1f);
              num4 = UnityEngine.Random.Range(0.3f, 0.6f);
              num5 = num2;
              num6 = num3;
              num7 = num4 + 0.15f;
              num8 = 0.6f;
              num9 = 0.6f;
			}
        }
	    else
	    {
        //female.customInfo.skinColor.hsvDiffuse = new HsvColor(BaseColorHue, BaseColorSaturation, BaseColorValue);
        //female.customInfo.skinColor.hsvSpecular = new HsvColor(ShineColorHue, ShineColorSaturation, ShineColorValue);
        //female.customInfo.skinColor.specularIntensity = ColorIntensity;
        //female.customInfo.skinColor.specularSharpness = ColorSoftness;
          female.femaleCustomInfo.nailColor.hsvDiffuse = female.customInfo.skinColor.hsvDiffuse;
          female.femaleCustomInfo.nailColor.hsvSpecular = female.customInfo.skinColor.hsvSpecular;
          female.femaleCustomInfo.nailColor.specularIntensity = female.customInfo.skinColor.specularIntensity;
          female.femaleCustomInfo.nailColor.specularSharpness = female.customInfo.skinColor.specularSharpness;
	    }



        //////////////////////////////// eyebrows
        Dictionary<int, ListTypeMaterial> femaleMaterialList = female.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyebrow);
        List<int> list3 = new List<int>();
        foreach (KeyValuePair<int, ListTypeMaterial> item8 in femaleMaterialList)
        {
            int num13 = int.Parse(item8.Value.Etc[0]);
            for (int j = 0; j < num13; j++)
            {
                list3.Add(item8.Key);
            }
        }
        // int index9 = UnityEngine.Random.Range(0, list3.Count);
        int index9 = UnityEngine.Random.Range(0, 31);
        female.customInfo.matEyebrowId = list3[index9];
        female.customInfo.eyebrowColor.hsvDiffuse.Copy(new HsvColor(num2, num3, num4 * 0.7f));
        female.customInfo.eyebrowColor.hsvSpecular.Copy(new HsvColor(num5, num6, num7));
        female.customInfo.eyebrowColor.specularIntensity = 0f;
        female.customInfo.eyebrowColor.specularSharpness = 0f;

        //////////////////////////////// eyes
        Dictionary<int, ListTypeMaterial> femaleMaterialList2 = female.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyeball);
        List<int> list4 = new List<int>();
        foreach (KeyValuePair<int, ListTypeMaterial> item9 in femaleMaterialList2)
        {
            int num14 = int.Parse(item9.Value.Etc[0]);
            for (int k = 0; k < num14; k++)
            {
                list4.Add(item9.Key);
            }
        }
        int index10 = UnityEngine.Random.Range(0, list4.Count);
        female.customInfo.matEyeLId = list4[index10];
        female.customInfo.matEyeRId = list4[index10];
        //////////////////////////////// eye color left
        // HsvColor src = new HsvColor(UnityEngine.Random.Range(0f, 359f), 0.25f, 0.5f);
        int eye_color_match_hair_color_chance = FaceSettings["eye_color_match_hair_color_chance"];
        if (UnityEngine.Random.Range(0, 100) < eye_color_match_hair_color_chance)
        {
            HsvColor src = new HsvColor(num2, num3, num4);
            HsvColor src2 = new HsvColor(num5, num6, num7);
            female.customInfo.eyeLColor.hsvDiffuse.Copy(src);
            female.customInfo.eyeLColor.hsvSpecular.Copy(src2);
            female.customInfo.eyeRColor.hsvDiffuse.Copy(src);
            female.customInfo.eyeRColor.hsvSpecular.Copy(src2);
        }
        else
        {
            // int eye_color_predefined_preset_chance = FaceSettings["eye_color_predefined_preset_chance"];
            // if (UnityEngine.Random.Range(0, 100) < eye_color_predefined_preset_chance)
            // {
                ///////////// Eye color presets
                // JSONNode EyeColors = data["EyeColors"];
                ////gets a random preset from the list
                // var randomeyecolor = EyeColors[UnityEngine.Random.Range(0, EyeColors.Count)];

                // float BaseColorHue = randomeyecolor["BaseColorHue"];
                // float BaseColorSaturation = randomeyecolor["BaseColorSaturation"];
                // float BaseColorValue = randomeyecolor["BaseColorValue"];
				
                // float BaseColorValueMin = FaceSettings["eye_color_brightness"]["min"];
                // float BaseColorValueMax = FaceSettings["eye_color_brightness"]["max"];
                // float BaseColorValue = UnityEngine.Random.Range(BaseColorValueMin, BaseColorValueMax);

                // float ColorIntensity = randomeyecolor["ColorIntensity"];
                // float ColorSoftness = randomeyecolor["ColorSoftness"];
				
                // HsvColor src = new HsvColor(BaseColorHue, BaseColorSaturation, BaseColorValue);
                // HsvColor src2 = new HsvColor(0f, 0f, 0.8f);
                // female.customInfo.eyeLColor.hsvDiffuse.Copy(src);
                // female.customInfo.eyeLColor.hsvSpecular.Copy(src2);
                /////////////////////////////////// heterochromia chance (5% default)		
                // int heterochromia_chance = FaceSettings["heterochromia_chance"];
                // if (UnityEngine.Random.Range(0, 100) < heterochromia_chance)
                // {
                    // src = new HsvColor(UnityEngine.Random.Range(0f, 359f), UnityEngine.Random.Range(0.25f, 0.90f), UnityEngine.Random.Range(0.7f, 0.90f));
                    // src2 = new HsvColor(0f, 0f, 0.8f);
                // }
                //////////////////////////// eye color right
                // female.customInfo.eyeRColor.hsvDiffuse.Copy(src);
                // female.customInfo.eyeRColor.hsvSpecular.Copy(src2);
            // }
            // else
            // {
                HsvColor src = new HsvColor(UnityEngine.Random.Range(0f, 359f), UnityEngine.Random.Range(0.25f, 0.90f), UnityEngine.Random.Range(0.7f, 0.90f));
                HsvColor src2 = new HsvColor(0f, 0f, 0.8f);
                female.customInfo.eyeLColor.hsvDiffuse.Copy(src);
                female.customInfo.eyeLColor.hsvSpecular.Copy(src2);
                /////////////////////////////////// heterochromia chance (5% default)		
                int heterochromia_chance = FaceSettings["heterochromia_chance"];
                if (UnityEngine.Random.Range(0, 100) < heterochromia_chance)
                {
                    // src = new HsvColor(UnityEngine.Random.Range(0f, 359f), 0.25f, 0.5f);
                    src = new HsvColor(UnityEngine.Random.Range(0f, 359f), UnityEngine.Random.Range(0.25f, 0.90f), UnityEngine.Random.Range(0.7f, 0.90f));
                    src2 = new HsvColor(0f, 0f, 0.8f);
                }
                //////////////////////////////// eye color right
                female.customInfo.eyeRColor.hsvDiffuse.Copy(src);
                female.customInfo.eyeRColor.hsvSpecular.Copy(src2);
            
        }

        //////////////////////////////// eye shadow
        int eye_shadow_chance = FaceSettings["eye_shadow_chance"];
        if (UnityEngine.Random.Range(0, 100) < eye_shadow_chance)
        {
            Dictionary<int, ListTypeTexture> femaleTextureList2 = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_eyeshadow);
            List<int> list5 = new List<int>();
            foreach (KeyValuePair<int, ListTypeTexture> item10 in femaleTextureList2)
            {
                int randomRate2 = item10.Value.RandomRate;
                for (int l = 0; l < randomRate2; l++)
                {
                    list5.Add(item10.Key);
                }
            }
            // int index11 = UnityEngine.Random.Range(0, list5.Count);
            int index11 = UnityEngine.Random.Range(0, 10);
            female.femaleCustomInfo.texEyeshadowId = list5[index11];
			
		    ///////////// Eye Shadow color presets
            JSONNode EyeShadowColors = data["EyeShadowColors"];
            ////gets a random preset from the list
            var randomeyeshadowcolor = EyeShadowColors[UnityEngine.Random.Range(0, EyeShadowColors.Count)];
		    
            float BaseColorHue = randomeyeshadowcolor["BaseColorHue"];
            float BaseColorSaturation = randomeyeshadowcolor["BaseColorSaturation"];
            float BaseColorValue = randomeyeshadowcolor["BaseColorValue"];
			
            female.femaleCustomInfo.eyeshadowColor.hsvDiffuse = new HsvColor(BaseColorHue, BaseColorSaturation, BaseColorValue);
            female.femaleCustomInfo.eyeshadowColor.alpha = UnityEngine.Random.Range(0.2f, 0.5f);
        }
        else
        {
            female.femaleCustomInfo.texEyeshadowId = 0;
        }

        //////////////////////////////// cheeks
        int cheeks_detail_chance = FaceSettings["cheeks_detail_chance"];
        if (UnityEngine.Random.Range(0, 100) < cheeks_detail_chance)
        {
            Dictionary<int, ListTypeTexture> femaleTextureList3 = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_cheek);
            int index12 = UnityEngine.Random.Range(0, femaleTextureList3.Count);
            female.femaleCustomInfo.texCheekId = femaleTextureList3.ElementAt(index12).Key;
            female.femaleCustomInfo.cheekColor.alpha = UnityEngine.Random.Range(0.2f, 0.5f);
        }
        else
        {
            female.femaleCustomInfo.texCheekId = 0;
        }

        //////////////////////////////// lips
        Dictionary<int, ListTypeTexture> femaleTextureList4 = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_lip);
        // int index13 = UnityEngine.Random.Range(0, femaleTextureList4.Count);
        int lipstick_chance = FaceSettings["lipstick_chance"];
        if (UnityEngine.Random.Range(0, 100) < lipstick_chance)
        {
            int index13 = UnityEngine.Random.Range(1, 4);
            female.femaleCustomInfo.texLipId = femaleTextureList4.ElementAt(index13).Key;

		    ///////////// Lipstick color presets
            JSONNode LipstickColors = data["LipstickColors"];
            ////gets a random preset from the list
            var randomlipstickcolor = LipstickColors[UnityEngine.Random.Range(0, LipstickColors.Count)];
		    
            float BaseColorHue = randomlipstickcolor["BaseColorHue"];
            float BaseColorSaturation = randomlipstickcolor["BaseColorSaturation"];
            float BaseColorValue = randomlipstickcolor["BaseColorValue"];
			
            female.femaleCustomInfo.lipColor.hsvDiffuse = new HsvColor(BaseColorHue, BaseColorSaturation, BaseColorValue);
            female.femaleCustomInfo.lipColor.alpha = UnityEngine.Random.Range(0.2f, 0.5f);
        }
        // if no just set it to no lipstick
        else
        {
            female.femaleCustomInfo.texLipId = femaleTextureList4.ElementAt(0).Key;
        }
        //////////////////////////////// moles
        int mole_chance = FaceSettings["mole_chance"];
        if (UnityEngine.Random.Range(0, 100) < mole_chance)
        {
            Dictionary<int, ListTypeTexture> femaleTextureList5 = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_mole);
            int index14 = UnityEngine.Random.Range(0, femaleTextureList5.Count);
            female.femaleCustomInfo.texMoleId = femaleTextureList5.ElementAt(index14).Key;
        }
        else
        {
            female.femaleCustomInfo.texMoleId = 0;
        }

        //////////////////////////////// eyelashes
        Dictionary<int, ListTypeMaterial> femaleMaterialList3 = female.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyelashes);
        // int index15 = UnityEngine.Random.Range(0, femaleMaterialList3.Count); // needs to be edited or else it will load mods custom eyelashes which fucks up the faces
        int index15 = UnityEngine.Random.Range(0, 5);
        female.femaleCustomInfo.matEyelashesId = femaleMaterialList3.ElementAt(index15).Key;
        female.femaleCustomInfo.eyelashesColor.hsvDiffuse.Copy(new HsvColor(num2, num3, num4 * 0.7f));
        female.femaleCustomInfo.eyelashesColor.hsvSpecular.Copy(new HsvColor(num5, num6, num7));
        female.femaleCustomInfo.eyelashesColor.specularIntensity = 0f;
        female.femaleCustomInfo.eyelashesColor.specularSharpness = 0f;

        //////////////////////////////// eye highlight
        Dictionary<int, ListTypeMaterial> femaleMaterialList4 = female.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyehi);
        List<int> list6 = new List<int>();
        foreach (KeyValuePair<int, ListTypeMaterial> item11 in femaleMaterialList4)
        {
            int num17 = int.Parse(item11.Value.Etc[0]);
            for (int m = 0; m < num17; m++)
            {
                list6.Add(item11.Key);
            }
        }
        int index16 = UnityEngine.Random.Range(0, list6.Count);
        female.femaleCustomInfo.matEyeHiId = list6[index16];
        female.customInfo.texBodyId = 0;

        //////////////////////////////// underhair
        Dictionary<int, ListTypeMaterial> femaleMaterialList6 = female.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_underhair);
        int index21 = UnityEngine.Random.Range(0, femaleMaterialList6.Count);
        female.femaleCustomInfo.matUnderhairId = femaleMaterialList6.ElementAt(index21).Key;
        //////////////////////////////// underhair color
        female.femaleCustomInfo.underhairColor.hsvDiffuse.Copy(new HsvColor(num2, num3, num4));
        female.femaleCustomInfo.underhairColor.hsvSpecular.Copy(new HsvColor(num5, num6, num7));
        female.femaleCustomInfo.underhairColor.specularIntensity = num8;
        female.femaleCustomInfo.underhairColor.specularSharpness = num9;

        //////////////////////////////// elf ears	
        int elf_ears_chance = FaceSettings["elf_ears_chance"];
        if (UnityEngine.Random.Range(0, 100) < elf_ears_chance)
        {
            female.customInfo.shapeValueFace[64] = UnityEngine.Random.Range(0.1f, 0.9f);
            female.customInfo.shapeValueFace[65] = UnityEngine.Random.Range(0.8f, 0.9f);
        }

        //female.chaCustom.UpdateShapeBodyValueFromCustomInfo();
    }

    void RandomiseCharBody()
    {
        if (female == null) return;

        var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string configFilename = "HS1Randomizer.json";
        var configPath = Path.Combine(dllPath, configFilename);

        //// from https://answers.unity.com/questions/1473952/how-to-write-and-read-json-in-unity.html  
        //// https://github.com/Bunny83/SimpleJSON
        string jsonString = File.ReadAllText(configPath);
        JSONNode data = JSON.Parse(jsonString);
        JSONNode BodyPresets = data["BodyPresets"];

        ////gets a random preset from the list
        var randompreset = BodyPresets[UnityEngine.Random.Range(0, BodyPresets.Count)];


        //////// automate this
        //int slider = 0;
        //var presetobject = 0;
        //while (presetobject <= 33)
        //{
		//	if (presetobject == 0)
		//	{
		//		presetobject++; // skip name
		//	}
		//	else
		//	{   float min = randompreset[presetobject]["min"];
		//		float max = randompreset[presetobject]["max"];
        //        female.customInfo.shapeValueBody[slider] = UnityEngine.Random.Range(min, max);
		//		slider++;
		//		presetobject++;
		//	}
		//}
        ////////////

        float min1 = randompreset["height"]["min"];
        float min2 = randompreset["bust_size"]["min"];
        float min3 = randompreset["breast_height"]["min"];
        float min4 = randompreset["breast_direction"]["min"];
        float min5 = randompreset["breast_spacing"]["min"];
        float min6 = randompreset["breast_angle"]["min"];
        float min7 = randompreset["breast_lenght"]["min"];
        float min8 = randompreset["areola_puffiness"]["min"];
        float min9 = randompreset["nipple_width"]["min"];
        float min10 = randompreset["head_size"]["min"];
        float min11 = randompreset["neck_width"]["min"];
        float min12 = randompreset["neck_thickness"]["min"];
        float min13 = randompreset["thorax_width"]["min"];
        float min14 = randompreset["thorax_thickness"]["min"];
        float min15 = randompreset["chest_width"]["min"];
        float min16 = randompreset["chest_thickness"]["min"];
        float min17 = randompreset["waist_width"]["min"];
        float min18 = randompreset["waist_thickness"]["min"];
        float min19 = randompreset["waist_height"]["min"];
        float min20 = randompreset["pelvis_width"]["min"];
        float min21 = randompreset["pelvis_thickness"]["min"];
        float min22 = randompreset["hips_width"]["min"];
        float min23 = randompreset["hips_thickness"]["min"];
        float min24 = randompreset["butt"]["min"];
        float min25 = randompreset["butt_angle"]["min"];
        float min26 = randompreset["thighs"]["min"];
        float min27 = randompreset["legs"]["min"];
        float min28 = randompreset["calves"]["min"];
        float min29 = randompreset["ankles"]["min"];
        float min30 = randompreset["shoulders"]["min"];
        float min31 = randompreset["upper_arms"]["min"];
        float min32 = randompreset["lower_arms"]["min"];
		
        float min33 = randompreset["areolaSize"]["min"];
        float min34 = randompreset["bustSoftness"]["min"];
        float min35 = randompreset["bustWeight"]["min"];
		
        float max1 = randompreset["height"]["max"];
        float max2 = randompreset["bust_size"]["max"];
        float max3 = randompreset["breast_height"]["max"];
        float max4 = randompreset["breast_direction"]["max"];
        float max5 = randompreset["breast_spacing"]["max"];
        float max6 = randompreset["breast_angle"]["max"];
        float max7 = randompreset["breast_lenght"]["max"];
        float max8 = randompreset["areola_puffiness"]["max"];
        float max9 = randompreset["nipple_width"]["max"];
        float max10 = randompreset["head_size"]["max"];
        float max11 = randompreset["neck_width"]["max"];
        float max12 = randompreset["neck_thickness"]["max"];
        float max13 = randompreset["thorax_width"]["max"];
        float max14 = randompreset["thorax_thickness"]["max"];
        float max15 = randompreset["chest_width"]["max"];
        float max16 = randompreset["chest_thickness"]["max"];
        float max17 = randompreset["waist_width"]["max"];
        float max18 = randompreset["waist_thickness"]["max"];
        float max19 = randompreset["waist_height"]["max"];
        float max20 = randompreset["pelvis_width"]["max"];
        float max21 = randompreset["pelvis_thickness"]["max"];
        float max22 = randompreset["hips_width"]["max"];
        float max23 = randompreset["hips_thickness"]["max"];
        float max24 = randompreset["butt"]["max"];
        float max25 = randompreset["butt_angle"]["max"];
        float max26 = randompreset["thighs"]["max"];
        float max27 = randompreset["legs"]["max"];
        float max28 = randompreset["calves"]["max"];
        float max29 = randompreset["ankles"]["max"];
        float max30 = randompreset["shoulders"]["max"];
        float max31 = randompreset["upper_arms"]["max"];
        float max32 = randompreset["lower_arms"]["max"];
		
        float max33 = randompreset["areolaSize"]["max"];
        float max34 = randompreset["bustSoftness"]["max"];
        float max35 = randompreset["bustWeight"]["max"];
		
        //// applies the values
        female.customInfo.shapeValueBody[0] = UnityEngine.Random.Range(min1, max1);
        female.customInfo.shapeValueBody[1] = UnityEngine.Random.Range(min2, max2);
        female.customInfo.shapeValueBody[2] = UnityEngine.Random.Range(min3, max3);
        female.customInfo.shapeValueBody[3] = UnityEngine.Random.Range(min4, max4);
        female.customInfo.shapeValueBody[4] = UnityEngine.Random.Range(min5, max5);
        female.customInfo.shapeValueBody[5] = UnityEngine.Random.Range(min6, max6);
        female.customInfo.shapeValueBody[6] = UnityEngine.Random.Range(min7, max7);
        female.customInfo.shapeValueBody[7] = UnityEngine.Random.Range(min8, max8);
        female.customInfo.shapeValueBody[8] = UnityEngine.Random.Range(min9, max9);
        female.customInfo.shapeValueBody[9] = UnityEngine.Random.Range(min10, max10);
        female.customInfo.shapeValueBody[10] = UnityEngine.Random.Range(min11, max11);
        female.customInfo.shapeValueBody[11] = UnityEngine.Random.Range(min12, max12);
        female.customInfo.shapeValueBody[12] = UnityEngine.Random.Range(min13, max13);
        female.customInfo.shapeValueBody[13] = UnityEngine.Random.Range(min14, max14);
        female.customInfo.shapeValueBody[14] = UnityEngine.Random.Range(min15, max15);
        female.customInfo.shapeValueBody[15] = UnityEngine.Random.Range(min16, max16);
        female.customInfo.shapeValueBody[16] = UnityEngine.Random.Range(min17, max17);
        female.customInfo.shapeValueBody[17] = UnityEngine.Random.Range(min18, max18);
        female.customInfo.shapeValueBody[18] = UnityEngine.Random.Range(min19, max19);
        female.customInfo.shapeValueBody[19] = UnityEngine.Random.Range(min20, max20);
        female.customInfo.shapeValueBody[20] = UnityEngine.Random.Range(min21, max21);
        female.customInfo.shapeValueBody[21] = UnityEngine.Random.Range(min22, max22);
        female.customInfo.shapeValueBody[22] = UnityEngine.Random.Range(min23, max23);
        female.customInfo.shapeValueBody[23] = UnityEngine.Random.Range(min24, max24);
        female.customInfo.shapeValueBody[24] = UnityEngine.Random.Range(min25, max25);
        female.customInfo.shapeValueBody[25] = UnityEngine.Random.Range(min26, max26);
        female.customInfo.shapeValueBody[26] = UnityEngine.Random.Range(min27, max27);
        female.customInfo.shapeValueBody[27] = UnityEngine.Random.Range(min28, max28);
        female.customInfo.shapeValueBody[28] = UnityEngine.Random.Range(min29, max29);
        female.customInfo.shapeValueBody[29] = UnityEngine.Random.Range(min30, max30);
        female.customInfo.shapeValueBody[30] = UnityEngine.Random.Range(min31, max31);
        female.customInfo.shapeValueBody[31] = UnityEngine.Random.Range(min32, max32);
		
        female.femaleCustomInfo.areolaSize = UnityEngine.Random.Range(min33, max33);
        female.femaleCustomInfo.bustSoftness = UnityEngine.Random.Range(min34, max34);
        female.femaleCustomInfo.bustWeight = UnityEngine.Random.Range(min35, max35);
		
		
        ////////////////////////////////skincolor
        int num11 = 0;
        float[,] array2 = new float[6, 8]
        {
        {
            13f,
            0.08f,
            0.73f,
            0f,
            0f,
            0.8f,
            0.6f,
            0.6f
        },
        {
            15f,
            0.07f,
            0.71f,
            0f,
            0f,
            0.82f,
            0.2f,
            0.9f
        },
        {
            16f,
            0.11f,
            0.72f,
            0f,
            0.04f,
            0.9f,
            0.21f,
            0.76f
        },
        {
            14f,
            0.1f,
            0.71f,
            0f,
            0.02f,
            0.9f,
            0.5f,
            0.5f
        },
        {
            18f,
            0.18f,
            0.7f,
            0f,
            0.04f,
            0.9f,
            0.47f,
            0.72f
        },
        {
            16f,
            0.15f,
            0.73f,
            7f,
            0.16f,
            0.76f,
            0.16f,
            0.62f
        }
        };
        //switch (skinColor)
        //{
        //default:
        //{
        int[] array3 = new int[14]
        {
            0,
            1,
            2,
            2,
            2,
            2,
            2,
            3,
            3,
            3,
            3,
            3,
            4,
            5
        };
		
		
		///////////// Skincolor presets
        JSONNode SkinColors = data["SkinColors"];
        ////gets a random preset from the list
        var randomskincolor = SkinColors[UnityEngine.Random.Range(0, SkinColors.Count)];
		
        float BaseColorHue = randomskincolor["BaseColorHue"];
        float BaseColorSaturation = randomskincolor["BaseColorSaturation"];
        float BaseColorValue = randomskincolor["BaseColorValue"];
		
        float ShineColorHue = randomskincolor["ShineColorHue"];
        float ShineColorSaturation = randomskincolor["ShineColorSaturation"];
        float ShineColorValue = randomskincolor["ShineColorValue"];
		
        float ColorIntensity = randomskincolor["ColorIntensity"];
        float ColorSoftness = randomskincolor["ColorSoftness"];
		
        female.customInfo.skinColor.hsvDiffuse = new HsvColor(BaseColorHue, BaseColorSaturation, BaseColorValue);
        female.customInfo.skinColor.hsvSpecular = new HsvColor(ShineColorHue, ShineColorSaturation, ShineColorValue);
        female.customInfo.skinColor.specularIntensity = ColorIntensity;
        female.customInfo.skinColor.specularSharpness = ColorSoftness;
		
        // num11 = array3[UnityEngine.Random.Range(0, array3.Length)];
        // female.customInfo.skinColor.hsvDiffuse = new HsvColor(array2[num11, 0], array2[num11, 1], array2[num11, 2]);
        // female.customInfo.skinColor.hsvSpecular = new HsvColor(array2[num11, 3], array2[num11, 4], array2[num11, 5]);
        // female.customInfo.skinColor.specularIntensity = array2[num11, 6];
        // female.customInfo.skinColor.specularSharpness = array2[num11, 7];
        //	break;
        //}
        //}

        //////////////////////////////tattoo
        // if (tattoo)
        // {
        JSONNode BodySettings = data["BodySettings"];
        int tattoo_chance = BodySettings["tattoo_chance"];
        if (UnityEngine.Random.Range(0, 100) < tattoo_chance)
        {
            Dictionary<int, ListTypeTexture> femaleTextureList = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_tattoo_f);
            List<int> list2 = new List<int>();
            foreach (KeyValuePair<int, ListTypeTexture> item7 in femaleTextureList)
            {
                int randomRate = item7.Value.RandomRate;
                for (int i = 0; i < randomRate; i++)
                {
                    list2.Add(item7.Key);
                }
            }
            int index8 = UnityEngine.Random.Range(0, list2.Count);
            female.customInfo.texTattoo_fId = list2[index8];
            float[,] array4 = new float[2, 3]
            {
                    {
                        0f,
                        0.2f,
                        0.2f
                    },
                    {
                        0f,
                        0.5f,
                        0.5f
                    }
            };
            int num12 = UnityEngine.Random.Range(0, array4.GetLength(0));
            female.customInfo.tattoo_fColor.hsvDiffuse = new HsvColor(array4[num12, 0], array4[num12, 1], array4[num12, 2]);
        }
        else
        {
            female.customInfo.texTattoo_fId = 0;
        }
        // }
        // else
        // {
        // female.customInfo.texTattoo_fId = 0;
        // }

        //////////////////////////////// body Detail
        int body_detail_chance = BodySettings["body_detail_chance"];
        if (UnityEngine.Random.Range(0, 100) < body_detail_chance)
        {
            Dictionary<int, ListTypeTexture> femaleTextureList6 = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_detail_b);
            int index17 = UnityEngine.Random.Range(0, femaleTextureList6.Count);
            female.customInfo.texBodyDetailId = femaleTextureList6.ElementAt(index17).Key;
            female.customInfo.bodyDetailWeight = UnityEngine.Random.Range(0.3f, 0.9f);
        }
        else
        {
            female.customInfo.texBodyDetailId = 0;
        }

        //////////////////////////////// tattoo
        // if (tattoo)
        // {

        // int tattoo_chance = BodySettings["tattoo_chance"];
        if (UnityEngine.Random.Range(0, 100) < tattoo_chance)
        {
            Dictionary<int, ListTypeTexture> femaleTextureList7 = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_tattoo_b);
            List<int> list7 = new List<int>();
            foreach (KeyValuePair<int, ListTypeTexture> item12 in femaleTextureList7)
            {
                int randomRate3 = item12.Value.RandomRate;
                for (int n = 0; n < randomRate3; n++)
                {
                    list7.Add(item12.Key);
                }
            }
            int index18 = UnityEngine.Random.Range(0, list7.Count);
            female.customInfo.texTattoo_bId = list7[index18];
            float[,] array7 = new float[3, 3]
            {
                    {
                        200f,
                        0.3f,
                        0.3f
                    },
                    {
                        0f,
                        0.2f,
                        0.2f
                    },
                    {
                        0f,
                        1f,
                        0.3f
                    }
            };
            int num18 = UnityEngine.Random.Range(0, array7.GetLength(0));
            female.customInfo.tattoo_bColor.hsvDiffuse = new HsvColor(array7[num18, 0], array7[num18, 1], array7[num18, 2]);
        }
        else
        {
            female.customInfo.texTattoo_bId = 0;
        }
        // }
        // else
        // {
        // female.customInfo.texTattoo_bId = 0;
        // }

        //////////////////////////////// sunburn
        int sunburn_chance = BodySettings["sunburn_chance"];
        if (UnityEngine.Random.Range(0, 100) < sunburn_chance)
        {
            Dictionary<int, ListTypeTexture> femaleTextureList8 = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_sunburn);
            List<int> list8 = new List<int>();
            foreach (KeyValuePair<int, ListTypeTexture> item13 in femaleTextureList8)
            {
                int randomRate4 = item13.Value.RandomRate;
                for (int num19 = 0; num19 < randomRate4; num19++)
                {
                    list8.Add(item13.Key);
                }
            }
            int index19 = UnityEngine.Random.Range(0, list8.Count);
            female.femaleCustomInfo.texSunburnId = list8[index19];
            HsvColor hsvColor = new HsvColor(female.customInfo.skinColor.hsvDiffuse.H, female.customInfo.skinColor.hsvDiffuse.S, female.customInfo.skinColor.hsvDiffuse.V);
            hsvColor.V = Mathf.Min(1f, hsvColor.V + 0.1f);
            female.femaleCustomInfo.sunburnColor.hsvDiffuse = hsvColor;
        }
        else
        {
            female.femaleCustomInfo.texSunburnId = 0;
        }

        //////////////////////////////// nipple
        Dictionary<int, ListTypeMaterial> femaleMaterialList5 = female.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_nip);
        int index20 = UnityEngine.Random.Range(0, femaleMaterialList5.Count);
        female.femaleCustomInfo.matNipId = femaleMaterialList5.ElementAt(index20).Key;

        //////////////////////////////// nipple color
        float hue = 4f;
        float num20 = 0f;
        float num21 = 0f;
        switch (num11)
        {
            case 0:
            case 1:
                {
                    num20 = 0.15f;
                    num21 = 0.65f;
                    for (int num23 = 0; num23 < 10; num23++)
                    {
                        num21 -= 0.01f;
                        if (UnityEngine.Random.Range(0, 100) < 60)
                        {
                            break;
                        }
                    }
                    break;
                }
            case 2:
            case 3:
                {
                    num20 = 0.17f;
                    num21 = 0.65f;
                    for (int num24 = 0; num24 < 10; num24++)
                    {
                        num21 -= 0.01f;
                        if (UnityEngine.Random.Range(0, 100) < 60)
                        {
                            break;
                        }
                    }
                    break;
                }
            default:
                {
                    num20 = 0.18f;
                    num21 = 0.6f;
                    for (int num22 = 0; num22 < 5; num22++)
                    {
                        num21 -= 0.01f;
                        if (UnityEngine.Random.Range(0, 100) < 60)
                        {
                            break;
                        }
                    }
                    break;
                }
        }
        female.femaleCustomInfo.nipColor.hsvDiffuse = new HsvColor(hue, num20, num21);

        //////////////////////////////// breast values	
        //float t = 1f - female.femaleCustomInfo.shapeValueBody[1];
        //t = Mathf.Lerp(0.2f, 0.8f, t);
        //female.femaleCustomInfo.areolaSize = t;
        //
        //float t2 = female.femaleCustomInfo.shapeValueBody[1];
        //t2 = Mathf.Lerp(0.3f, 0.7f, t2);
        //female.femaleCustomInfo.bustSoftness = t2;
        //
        //float t3 = female.femaleCustomInfo.shapeValueBody[1];
        //t3 = Mathf.Lerp(0.3f, 0.7f, t3);
        //female.femaleCustomInfo.bustWeight = t3;

        //female.chaCustom.UpdateShapeBodyValueFromCustomInfo();
    }


    void RandomiseCharBodyFace()
    {
        if (female == null) return;
        RandomiseCharPersonality(female);
        RandomiseCharFace(lstRandFaceF);
        RandomiseCharBody();
    }

    void RandomiseCharClothing()
    {
        if (female == null) return;
        RandClothes2(female);
    }


    public static void RandomiseCharPersonality(CharFemale female)
    {
        CharFileInfoParameterFemale charFileInfoParameterFemale = female.chaFile.parameterInfo as CharFileInfoParameterFemale;
        int length = Enum.GetValues(typeof(CharFileInfoParameterFemale.Characteristic)).Length;
        charFileInfoParameterFemale.characteristic = UnityEngine.Random.Range(0, length);
        List<int> list = new List<int>();
        list.Add(0);
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Add(4);
        list.Add(5);
        list.Add(6);
        list.Add(7);
        list.Add(8);
        list.Add(9);
        list.Add(10);
        list.Add(11);
        list.Add(12);
        list.Add(13);
        //if (Game.GetAchievementRelease(7))
        //{
        //	list.Add(3);
        //}
        //if (Game.GetAchievementRelease(8))
        //{
        //	list.Add(6);
        //}
        //if (Game.GetAchievementRelease(9))
        //{
        //	list.Add(9);
        //}
        //if (Game.GetAchievementRelease(10))
        //{
        //	list.Add(12);
        //}
        //if (Game.GetAchievementRelease(11))
        //{
        //	list.Add(13);
        //}
        int[] array = list.ToArray();
        charFileInfoParameterFemale.impression = array[UnityEngine.Random.Range(0, array.Length)];
        int length2 = Enum.GetValues(typeof(CharFileInfoParameterFemale.Attribute)).Length;
        charFileInfoParameterFemale.attribute = UnityEngine.Random.Range(0, length2);
        CharFileInfoCustomFemale charFileInfoCustomFemale = female.chaFile.customInfo as CharFileInfoCustomFemale;
        // charFileInfoCustomFemale.name = female.ListInfo.GetRandName(1);
		
		//////////// new random name (western)
        var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string configFilename = "HS1Randomizer.json";
        var configPath = Path.Combine(dllPath, configFilename);

        string jsonString = File.ReadAllText(configPath);
        JSONNode data = JSON.Parse(jsonString);
        JSONNode female_names = data["female_names"];
        JSONNode family_names = data["family_names"];

        string FirstName = female_names[UnityEngine.Random.Range(0, female_names.Count)];
        string SecondName = family_names[UnityEngine.Random.Range(0, family_names.Count)];
        charFileInfoCustomFemale.name = (FirstName + " " + SecondName);
		////////////
		
        Dictionary<int, PersonalityIdInfo> personalityIdInfo = female.ListInfo.GetPersonalityIdInfo();
        List<int> list2 = new List<int>();
        foreach (KeyValuePair<int, PersonalityIdInfo> item in personalityIdInfo)
        {
            //switch (personalityType)
            //{
            //case 0:
            //	if (item.Value.type == 0)
            //	{
            //		list2.Add(item.Value.Id);
            //	}
            //	break;
            //case 1:
            //	if (item.Value.type == 1)
            //	{
            //		list2.Add(item.Value.Id);
            //	}
            //	break;
            //default:
            list2.Add(item.Value.Id);
            //	break;
            //}
        }
        int[] array2 = list2.ToArray();
        charFileInfoCustomFemale.personality = array2[UnityEngine.Random.Range(0, array2.Length)];
        charFileInfoCustomFemale.voicePitch = UnityEngine.Random.Range(0.94f, 1.06f);
    }


	public static void RandClothes2(CharFemale female)
	{
		CharFileInfoClothes.Accessory[] array = new CharFileInfoClothes.Accessory[10];
		for (int i = 0; i < 10; i++)
		{
			array[i] = new CharFileInfoClothes.Accessory();
		}
		
        var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string configFilename = "HS1Randomizer.json";
        var configPath = Path.Combine(dllPath, configFilename);

        string jsonString = File.ReadAllText(configPath);
        JSONNode data = JSON.Parse(jsonString);
        JSONNode BodySettings = data["ClothSettings"];
        int accessory_chance = BodySettings["accessory_chance"];
        int glasses_chance = BodySettings["glasses_chance"];
		int num = 0;
		int num2 = accessory_chance;
		
        bool accessory = false;
		if (UnityEngine.Random.Range(0, 100) < accessory_chance) 
		{
		  accessory = true;
		}
		
		if (UnityEngine.Random.Range(0, 100) < glasses_chance) 
		{
			Dictionary<int, ListTypeFbx> accessoryFbxList = female.ListInfo.GetAccessoryFbxList(CharaListInfo.TypeAccessoryFbx.ca_f_megane);
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, ListTypeFbx> item in accessoryFbxList)
			{
				int num3 = int.Parse(item.Value.SiruABPath);
				for (int j = 0; j < num3; j++)
				{
					list.Add(item.Key);
				}
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			array[num].type = 2;
			array[num].id = list[index];
			float[,] array2 = new float[7, 3]
			{
				{
					0f,
					0f,
					0f
				},
				{
					0f,
					0f,
					0f
				},
				{
					0f,
					0f,
					0f
				},
				{
					0f,
					0.6f,
					0.3f
				},
				{
					34f,
					0.55f,
					0.24f
				},
				{
					249f,
					0.67f,
					0.18f
				},
				{
					121f,
					1f,
					0.1f
				}
			};
			int num4 = UnityEngine.Random.Range(0, array2.GetLength(0));
			array[num].color.hsvDiffuse = new HsvColor(array2[num4, 0], array2[num4, 1], array2[num4, 2]);
			num4 = UnityEngine.Random.Range(0, array2.GetLength(0));
			array[num].color2.hsvDiffuse = new HsvColor(array2[num4, 0], array2[num4, 1], array2[num4, 2]);
			num++;
			num2 -= 5;
		}
		
		if (num == 0 && accessory && UnityEngine.Random.Range(0, 100) < 5)
		{
			Dictionary<int, ListTypeFbx> accessoryFbxList2 = female.ListInfo.GetAccessoryFbxList(CharaListInfo.TypeAccessoryFbx.ca_f_face);
			List<int> list2 = new List<int>();
			foreach (KeyValuePair<int, ListTypeFbx> item2 in accessoryFbxList2)
			{
				int num5 = int.Parse(item2.Value.SiruABPath);
				for (int k = 0; k < num5; k++)
				{
					list2.Add(item2.Key);
				}
			}
			int index2 = UnityEngine.Random.Range(0, list2.Count);
			array[num].type = 3;
			array[num].id = list2[index2];
			num++;
		}
		if (accessory && UnityEngine.Random.Range(0, 100) < num2)
		{
			Dictionary<int, ListTypeFbx> accessoryFbxList3 = female.ListInfo.GetAccessoryFbxList(CharaListInfo.TypeAccessoryFbx.ca_f_hand);
			List<int> list3 = new List<int>();
			foreach (KeyValuePair<int, ListTypeFbx> item3 in accessoryFbxList3)
			{
				int num6 = int.Parse(item3.Value.SiruABPath);
				for (int l = 0; l < num6; l++)
				{
					list3.Add(item3.Key);
				}
			}
			int index3 = UnityEngine.Random.Range(0, list3.Count);
			array[num].type = 10;
			array[num].id = list3[index3];
			float[,] array3 = new float[11, 3]
			{
				{
					0f,
					0.8f,
					0.8f
				},
				{
					45f,
					0.8f,
					0.8f
				},
				{
					90f,
					0.8f,
					0.8f
				},
				{
					200f,
					0.8f,
					0.8f
				},
				{
					280f,
					0.8f,
					0.8f
				},
				{
					0f,
					0.4f,
					0.7f
				},
				{
					35f,
					0.4f,
					0.7f
				},
				{
					145f,
					0.4f,
					0.7f
				},
				{
					205f,
					0.4f,
					0.7f
				},
				{
					260f,
					0.4f,
					0.7f
				},
				{
					295f,
					0.4f,
					0.7f
				}
			};
			int num7 = UnityEngine.Random.Range(0, array3.GetLength(0));
			array[num].color.hsvDiffuse = new HsvColor(array3[num7, 0], array3[num7, 1], array3[num7, 2]);
			num7 = UnityEngine.Random.Range(0, array3.GetLength(0));
			array[num].color2.hsvDiffuse = new HsvColor(array3[num7, 0], array3[num7, 1], array3[num7, 2]);
			num++;
			num2 -= 5;
		}
		if (accessory && UnityEngine.Random.Range(0, 100) < num2)
		{
			float[,] array4 = new float[11, 3]
			{
				{
					0f,
					0.8f,
					0.8f
				},
				{
					45f,
					0.8f,
					0.8f
				},
				{
					90f,
					0.8f,
					0.8f
				},
				{
					200f,
					0.8f,
					0.8f
				},
				{
					280f,
					0.8f,
					0.8f
				},
				{
					0f,
					0.4f,
					0.7f
				},
				{
					35f,
					0.4f,
					0.7f
				},
				{
					145f,
					0.4f,
					0.7f
				},
				{
					205f,
					0.4f,
					0.7f
				},
				{
					260f,
					0.4f,
					0.7f
				},
				{
					295f,
					0.4f,
					0.7f
				}
			};
			int num8 = UnityEngine.Random.Range(0, array4.GetLength(0));
			int num9 = UnityEngine.Random.Range(0, array4.GetLength(0));
			Dictionary<int, ListTypeFbx> accessoryFbxList4 = female.ListInfo.GetAccessoryFbxList(CharaListInfo.TypeAccessoryFbx.ca_f_ear);
			List<int> list4 = new List<int>();
			foreach (KeyValuePair<int, ListTypeFbx> item4 in accessoryFbxList4)
			{
				int num10 = int.Parse(item4.Value.SiruABPath);
				for (int m = 0; m < num10; m++)
				{
					list4.Add(item4.Key);
				}
			}
			int index4 = UnityEngine.Random.Range(0, list4.Count);
			array[num].type = 1;
			array[num].id = list4[index4];
			array[num].parentKey = "AP_Earring_L";
			array[num].color.hsvDiffuse = new HsvColor(array4[num8, 0], array4[num8, 1], array4[num8, 2]);
			array[num].color2.hsvDiffuse = new HsvColor(array4[num9, 0], array4[num9, 1], array4[num9, 2]);
			num++;
			array[num].type = 1;
			array[num].id = list4[index4];
			array[num].parentKey = "AP_Earring_R";
			array[num].color.hsvDiffuse = new HsvColor(array4[num8, 0], array4[num8, 1], array4[num8, 2]);
			array[num].color2.hsvDiffuse = new HsvColor(array4[num9, 0], array4[num9, 1], array4[num9, 2]);
			num++;
			num2 -= 10;
		}
		if (accessory && UnityEngine.Random.Range(0, 100) < num2)
		{
			Dictionary<int, ListTypeFbx> accessoryFbxList5 = female.ListInfo.GetAccessoryFbxList(CharaListInfo.TypeAccessoryFbx.ca_f_neck);
			List<int> list5 = new List<int>();
			foreach (KeyValuePair<int, ListTypeFbx> item5 in accessoryFbxList5)
			{
				int num11 = int.Parse(item5.Value.SiruABPath);
				for (int n = 0; n < num11; n++)
				{
					list5.Add(item5.Key);
				}
			}
			int index5 = UnityEngine.Random.Range(0, list5.Count);
			array[num].type = 4;
			array[num].id = list5[index5];
			float[,] array5 = new float[11, 3]
			{
				{
					0f,
					0.8f,
					0.8f
				},
				{
					45f,
					0.8f,
					0.8f
				},
				{
					90f,
					0.8f,
					0.8f
				},
				{
					200f,
					0.8f,
					0.8f
				},
				{
					280f,
					0.8f,
					0.8f
				},
				{
					0f,
					0.4f,
					0.7f
				},
				{
					35f,
					0.4f,
					0.7f
				},
				{
					145f,
					0.4f,
					0.7f
				},
				{
					205f,
					0.4f,
					0.7f
				},
				{
					260f,
					0.4f,
					0.7f
				},
				{
					295f,
					0.4f,
					0.7f
				}
			};
			int num12 = UnityEngine.Random.Range(0, array5.GetLength(0));
			array[num].color.hsvDiffuse = new HsvColor(array5[num12, 0], array5[num12, 1], array5[num12, 2]);
			num12 = UnityEngine.Random.Range(0, array5.GetLength(0));
			array[num].color2.hsvDiffuse = new HsvColor(array5[num12, 0], array5[num12, 1], array5[num12, 2]);
			num++;
		}
		if (accessory && UnityEngine.Random.Range(0, 100) < 10)
		{
			Dictionary<int, ListTypeFbx> accessoryFbxList6 = female.ListInfo.GetAccessoryFbxList(CharaListInfo.TypeAccessoryFbx.ca_f_head);
			List<int> list6 = new List<int>();
			foreach (KeyValuePair<int, ListTypeFbx> item6 in accessoryFbxList6)
			{
				int num13 = int.Parse(item6.Value.SiruABPath);
				for (int num14 = 0; num14 < num13; num14++)
				{
					list6.Add(item6.Key);
				}
			}
			int index6 = UnityEngine.Random.Range(0, list6.Count);
			array[num].type = 0;
			array[num].id = list6[index6];
			if ("1" == accessoryFbxList6[list6[index6]].SiruPrefab)
			{
				array[num].color.hsvDiffuse.Copy(new HsvColor(female.customInfo.hairColor[0].hsvDiffuse.H, female.customInfo.hairColor[0].hsvDiffuse.S, female.customInfo.hairColor[0].hsvDiffuse.V));
			}
			else
			{
				array[num].color.hsvDiffuse = new HsvColor(20f, 0.6f, 0.7f);
				array[num].color2.hsvDiffuse = new HsvColor(90f, 0.6f, 0.7f);
			}
			num++;
		}
		Dictionary<int, RandomClothesInfo> randomClothesInfoF = female.ListInfo.GetRandomClothesInfoF();
		RandomClothesInfo value = null;
		if (randomClothesInfoF.TryGetValue(0, out value))
		{
			CharFileInfoClothesFemale charFileInfoClothesFemale = new CharFileInfoClothesFemale();
			int index7 = UnityEngine.Random.Range(0, value.lstFileName.Count);
			charFileInfoClothesFemale.Load(value.abName, value.lstFileName[index7]);
			for (int num15 = 0; num15 < 10; num15++)
			{
				charFileInfoClothesFemale.accessory[num15].Copy(array[num15]);
			}
			female.chaFile.coordinateInfo.SetInfo(CharDefine.CoordinateType.type00, charFileInfoClothesFemale);
		}
		if (randomClothesInfoF.TryGetValue(1, out value))
		{
			CharFileInfoClothesFemale charFileInfoClothesFemale2 = new CharFileInfoClothesFemale();
			int index8 = UnityEngine.Random.Range(0, value.lstFileName.Count);
			charFileInfoClothesFemale2.Load(value.abName, value.lstFileName[index8]);
			for (int num16 = 0; num16 < 10 && array[num16].type == 2; num16++)
			{
				charFileInfoClothesFemale2.accessory[num16].Copy(array[num16]);
			}
			female.chaFile.coordinateInfo.SetInfo(CharDefine.CoordinateType.type01, charFileInfoClothesFemale2);
		}
		if (randomClothesInfoF.TryGetValue(2, out value))
		{
			CharFileInfoClothesFemale charFileInfoClothesFemale3 = new CharFileInfoClothesFemale();
			int index9 = UnityEngine.Random.Range(0, value.lstFileName.Count);
			charFileInfoClothesFemale3.Load(value.abName, value.lstFileName[index9]);
			for (int num17 = 0; num17 < 10 && array[num17].type == 2; num17++)
			{
				charFileInfoClothesFemale3.accessory[num17].Copy(array[num17]);
			}
			female.chaFile.coordinateInfo.SetInfo(CharDefine.CoordinateType.type02, charFileInfoClothesFemale3);
		}
	}

    //public virtual void ExecuteSaveNew()
    //{
    //	if (!(null == chaInfo) && chaInfo.chaFile != null)
    //	{
    //		string empty = string.Empty;
    //		empty = ((chaInfo.Sex != 0) ? ("charaF_" + DateTime.Now.ToString("yyyyMMddHHmmssfff")) : ("charaM_" + DateTime.Now.ToString("yyyyMMddHHmmssfff")));
    //		if (chaInfo.Sex == 1)
    //		{
    //			CharFileInfoParameterFemale charFileInfoParameterFemale = chaInfo.chaFile.parameterInfo as CharFileInfoParameterFemale;
    //			charFileInfoParameterFemale.InitParameter = true;
    //			Singleton<Info>.Instance.InitState(charFileInfoParameterFemale, chaInfo.customInfo.personality, _isForce: true);
    //		}
    //		customControl.CustomSaveCharaAssist(empty);
    //		FileInfo fileInfo = new FileInfo();
    //		fileInfo.no = 0;
    //		fileInfo.time = DateTime.Now;
    //		fileInfo.FileName = empty;
    //		fileInfo.FullPath = chaInfo.chaFile.ConvertCharaFilePath(empty);
    //		fileInfo.CharaName = customInfo.name;
    //		fileInfo.personality = customInfo.personality;
    //		fileInfo.limited = false;
    //		lstFileInfo.Add(fileInfo);
    //		CreateListObject();
    //		UpdateSort();
    //	}
    //}
    //
    //

    ////////////////////////////////////////////////////////////////////////////////////////////////

}




