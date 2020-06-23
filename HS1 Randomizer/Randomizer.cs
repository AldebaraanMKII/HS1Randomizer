using Manager;
using IllusionPlugin;
using System;  /////
using System.Collections;
using System.Collections.Generic;  /////
using System.Linq;   ////
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using RandomSearch;


public class HSCharRandom : IPlugin
{
    public void OnApplicationQuit() { }
    public void OnApplicationStart() { }
    public void OnFixedUpdate() { }
    public void OnLateUpdate() { }
    public void OnUpdate() { }
    public void OnLevelWasInitialized(int level) { }

    public string Name => nameof(HSCharRandom);
    public string Version => "0.1";

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
    }

    void Update()
    {
        //////////////////////////////////////// All
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            RandomiseCharAll();
            //female.Reload();
        }
        //////////////////////////////////////// Only face
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            RandomiseCharFace(lstRandFaceF);
            female.chaFile.ChangeCoordinateType((CharDefine.CoordinateType)coordinateType);
            female.Reload();
        }
        //////////////////////////////////////// Only body
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            RandomiseCharBody();
            female.chaFile.ChangeCoordinateType((CharDefine.CoordinateType)coordinateType);
            female.Reload();
        }
        //////////////////////////////////////// Body/Face
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            RandomiseCharBodyFace();
            female.chaFile.ChangeCoordinateType((CharDefine.CoordinateType)coordinateType);
            female.Reload();
        }
        //////////////////////////////////////// Clothes and accessories
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            RandomiseCharClothing();
            female.chaFile.ChangeCoordinateType((CharDefine.CoordinateType)coordinateType);
            female.Reload();
        }
        //////////////////////////////////////// Save card
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            CustomMenu.SubMenuBase.ExecuteSaveNew();
        }
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

        ////////////////////////////////Overall Face Breadth,
        female.customInfo.shapeValueFace[0] = UnityEngine.Random.Range(0.35f, 0.45f);
        ////////////////////////////////Upper face depth,
        female.customInfo.shapeValueFace[1] = UnityEngine.Random.Range(0.3f, 0.45f);
        ////////////////////////////////Face height,
        female.customInfo.shapeValueFace[2] = UnityEngine.Random.Range(0.4f, 0.52f);
        ////////////////////////////////Lower face depth,
        female.customInfo.shapeValueFace[3] = UnityEngine.Random.Range(0.45f, 0.6f);
        ////////////////////////////////Lower face width,
        female.customInfo.shapeValueFace[4] = UnityEngine.Random.Range(0.1f, 0.4f);

        ////////////////////////////////Jaw Width
        female.customInfo.shapeValueFace[5] = UnityEngine.Random.Range(0.2f, 0.35f);
        ////////////////////////////////Jaw Height
        female.customInfo.shapeValueFace[6] = UnityEngine.Random.Range(0.2f, 0.5f);
        ////////////////////////////////Jaw Depth
        female.customInfo.shapeValueFace[7] = UnityEngine.Random.Range(0.2f, 0.35f);
        ////////////////////////////////Jaw Angle
        female.customInfo.shapeValueFace[8] = UnityEngine.Random.Range(0.2f, 0.5f);

        ////////////////////////////////Neck Droop
        female.customInfo.shapeValueFace[9] = UnityEngine.Random.Range(0.5f, 0.9f);
        ////////////////////////////////Chin Width
        female.customInfo.shapeValueFace[10] = UnityEngine.Random.Range(0.2f, 0.6f);
        ////////////////////////////////Chin Height
        female.customInfo.shapeValueFace[11] = UnityEngine.Random.Range(0.2f, 0.5f);
        ////////////////////////////////Chin Depth
        female.customInfo.shapeValueFace[12] = UnityEngine.Random.Range(0.05f, 0.3f);


        ////////////////////////////////Lower Cheek Height
        female.customInfo.shapeValueFace[13] = UnityEngine.Random.Range(0.1f, 0.3f);
        ////////////////////////////////Lower Cheek Depth
        female.customInfo.shapeValueFace[14] = UnityEngine.Random.Range(0.5f, 0.8f);
        ////////////////////////////////Lower Cheek Width
        female.customInfo.shapeValueFace[15] = UnityEngine.Random.Range(0.5f, 0.8f);

        ////////////////////////////////Upper Cheek Height
        female.customInfo.shapeValueFace[16] = UnityEngine.Random.Range(0.2f, 0.6f);
        ////////////////////////////////Upper Cheek Depth
        female.customInfo.shapeValueFace[17] = UnityEngine.Random.Range(0.3f, 0.45f);
        ////////////////////////////////Upper Cheek Width
        female.customInfo.shapeValueFace[18] = UnityEngine.Random.Range(0.2f, 0.5f);


        ////////////////////////////////Eyebrow Spacing
        female.customInfo.shapeValueFace[20] = UnityEngine.Random.Range(0.2f, 0.4f);
        ////////////////////////////////Eyebrow Angle
        female.customInfo.shapeValueFace[21] = UnityEngine.Random.Range(0.2f, 0.9f);
        ////////////////////////////////Inner Arching
        female.customInfo.shapeValueFace[22] = UnityEngine.Random.Range(0.1f, 0.7f);
        ////////////////////////////////Outer Arching
        female.customInfo.shapeValueFace[23] = UnityEngine.Random.Range(0.2f, 0.5f);


        ////////////////////////////////Eye Height
        float Eye_Height = UnityEngine.Random.Range(0.25f, 0.5f);
        female.customInfo.shapeValueFace[24] = Eye_Height;
        ////////////////////////////////Eyebrow Height
        float Eyebrow_Height = UnityEngine.Random.Range(0.3f, 0.5f);
        female.customInfo.shapeValueFace[19] = Eye_Height - Eyebrow_Height;
        ////////////////////////////////Eye Spacing
        female.customInfo.shapeValueFace[25] = UnityEngine.Random.Range(0.1f, 0.25f);
        ////////////////////////////////Eye Depth
        female.customInfo.shapeValueFace[26] = UnityEngine.Random.Range(0.1f, 0.5f);
        ////////////////////////////////Eye Width
        female.customInfo.shapeValueFace[27] = UnityEngine.Random.Range(0.6f, 0.8f);
        ////////////////////////////////Eye Openness
        female.customInfo.shapeValueFace[28] = UnityEngine.Random.Range(0.5f, 0.7f);
        ////////////////////////////////Eye Angle 1
        female.customInfo.shapeValueFace[29] = UnityEngine.Random.Range(0.45f, 0.55f);
        ////////////////////////////////Eye Angle 2
        female.customInfo.shapeValueFace[30] = UnityEngine.Random.Range(0.4f, 0.7f);

        ////////////////////////////////Inner Corner Distance
        female.customInfo.shapeValueFace[31] = UnityEngine.Random.Range(0.5f, 0.8f);
        ////////////////////////////////Outer Corner Distance
        female.customInfo.shapeValueFace[32] = UnityEngine.Random.Range(0.5f, 0.7f);
        ////////////////////////////////Inner Corner Height
        female.customInfo.shapeValueFace[33] = UnityEngine.Random.Range(0.5f, 0.8f);
        ////////////////////////////////Outer Corner Height
        female.customInfo.shapeValueFace[34] = UnityEngine.Random.Range(0.3f, 0.8f);

        ////////////////////////////////Eyelid Shape 1
        female.customInfo.shapeValueFace[35] = UnityEngine.Random.Range(0.9f, 1.3f);
        ////////////////////////////////Eyelid Shape 2
        female.customInfo.shapeValueFace[36] = UnityEngine.Random.Range(0.1f, 0.2f);
        ////////////////////////////////Pupil Pos
        female.customInfo.shapeValueFace[37] = UnityEngine.Random.Range(0.4f, 0.5f);
        ////////////////////////////////Pupil Width
        female.customInfo.shapeValueFace[38] = UnityEngine.Random.Range(0.45f, 0.6f);
        ////////////////////////////////Pupil Height
        female.customInfo.shapeValueFace[39] = UnityEngine.Random.Range(0.50f, 0.6f);

        ////////////////////////////////Nose Height
        female.customInfo.shapeValueFace[40] = UnityEngine.Random.Range(0.5f, 0.7f);
        ////////////////////////////////Nose Projection
        female.customInfo.shapeValueFace[41] = UnityEngine.Random.Range(0.4f, 0.6f);
        ////////////////////////////////Nose Angle
        female.customInfo.shapeValueFace[42] = UnityEngine.Random.Range(0.5f, 0.6f);
        ////////////////////////////////Nose Size
        female.customInfo.shapeValueFace[43] = UnityEngine.Random.Range(0.3f, 0.5f);

        ////////////////////////////////Bridge Height
        female.customInfo.shapeValueFace[44] = UnityEngine.Random.Range(0.4f, 0.5f);
        ////////////////////////////////Bridge Width
        female.customInfo.shapeValueFace[45] = UnityEngine.Random.Range(0.3f, 0.5f);
        ////////////////////////////////Bridge Shape
        female.customInfo.shapeValueFace[46] = UnityEngine.Random.Range(0.3f, 0.6f);
        ////////////////////////////////Nose Width
        female.customInfo.shapeValueFace[47] = UnityEngine.Random.Range(0.3f, 0.7f);

        ////////////////////////////////Nostril Height
        female.customInfo.shapeValueFace[48] = UnityEngine.Random.Range(0.2f, 0.6f);
        ////////////////////////////////Nostril Length
        female.customInfo.shapeValueFace[49] = UnityEngine.Random.Range(0.2f, 0.4f);
        ////////////////////////////////Nostril Inner Width
        female.customInfo.shapeValueFace[50] = UnityEngine.Random.Range(0.2f, 0.5f);
        ////////////////////////////////Nostril Outer Width
        female.customInfo.shapeValueFace[51] = UnityEngine.Random.Range(0.3f, 0.8f);

        ////////////////////////////////Tip Length
        female.customInfo.shapeValueFace[52] = UnityEngine.Random.Range(0.4f, 0.65f);
        ////////////////////////////////Tip Height
        female.customInfo.shapeValueFace[53] = UnityEngine.Random.Range(0.3f, 0.45f);
        ////////////////////////////////Tip Size
        female.customInfo.shapeValueFace[54] = UnityEngine.Random.Range(0.4f, 0.6f);

        ////////////////////////////////Mouth Height
        female.customInfo.shapeValueFace[55] = UnityEngine.Random.Range(0.5f, 0.8f);
        ////////////////////////////////Mouth Width
        female.customInfo.shapeValueFace[56] = UnityEngine.Random.Range(0.5f, 0.8f);
        ////////////////////////////////Lip Thickness
        female.customInfo.shapeValueFace[57] = UnityEngine.Random.Range(0.1f, 0.3f);
        ////////////////////////////////Mouth Projection
        female.customInfo.shapeValueFace[58] = UnityEngine.Random.Range(0.3f, 0.5f);
        ////////////////////////////////Upper Lip Thickness
        female.customInfo.shapeValueFace[59] = UnityEngine.Random.Range(0.1f, 0.3f);
        ////////////////////////////////Lower Lip Thickness
        female.customInfo.shapeValueFace[60] = UnityEngine.Random.Range(0.1f, 0.3f);
        ////////////////////////////////Corner Shape
        female.customInfo.shapeValueFace[61] = UnityEngine.Random.Range(0.1f, 0.5f);

        ////////////////////////////////Ear Size
        female.customInfo.shapeValueFace[62] = UnityEngine.Random.Range(0.4f, 0.6f);
        ////////////////////////////////Ear Angle
        female.customInfo.shapeValueFace[63] = UnityEngine.Random.Range(0.3f, 0.5f);
        ////////////////////////////////Ear Rotation
        female.customInfo.shapeValueFace[64] = UnityEngine.Random.Range(0.1f, 0.4f);
        ////////////////////////////////Upper Ear Shape
        female.customInfo.shapeValueFace[65] = UnityEngine.Random.Range(0.3f, 0.4f);
        ////////////////////////////////Lower Ear Shape
        female.customInfo.shapeValueFace[66] = UnityEngine.Random.Range(0.2f, 0.5f);


        //////////////////////////////// numbers
        float num2 = 0f;
        float num3 = 0f;
        float num4 = 0f;
        float num5 = 0f;
        float num6 = 0f;
        float num7 = 0f;
        float num8 = 0f;
        float num9 = 0f;
        if (UnityEngine.Random.Range(0, 100) < 90)
        {
            float[,] array = new float[21, 8] // 21 is number of {} and 8 is item count
            {
            {
                6f,
                0f,
                0.45f,
                6f,
                0f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                6f,
                0f,
                0.45f,
                6f,
                0f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                6f,
                0f,
                0.45f,
                6f,
                0f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                6f,
                0f,
                0.45f,
                6f,
                0f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                6f,
                0f,
                0.45f,
                6f,
                0f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                6f,
                0f,
                0.45f,
                6f,
                0f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                6f,
                0.15f,
                0.5f,
                6f,
                0.15f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                6f,
                0.15f,
                0.5f,
                6f,
                0.15f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                6f,
                0.15f,
                0.5f,
                6f,
                0.15f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                6f,
                0.15f,
                0.5f,
                6f,
                0.15f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                6f,
                0.15f,
                0.5f,
                6f,
                0.15f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                6f,
                0.15f,
                0.5f,
                6f,
                0.15f,
                0.65f,
                0.6f,
                0.6f
            },
            {
                0f,
                0.27f,
                0.6f,
                0f,
                0.27f,
                0.75f,
                0.6f,
                0.6f
            },
            {
                0f,
                0.27f,
                0.6f,
                0f,
                0.27f,
                0.75f,
                0.6f,
                0.6f
            },
            {
                0f,
                0f,
                0.7f,
                0f,
                0f,
                0.85f,
                0.6f,
                0.6f
            },
            {
                50f,
                0.8f,
                0.7f,
                50f,
                0.8f,
                0.85f,
                0.6f,
                0.6f
            },
            {
                0f,
                0.8f,
                0.6f,
                0f,
                0.8f,
                0.75f,
                0.6f,
                0.6f
            },
            {
                36f,
                0.12f,
                0.88f,
                31f,
                0.24f,
                0.81f,
                0.6f,
                0.64f
            },
            {
                94f,
                0f,
                0.4f,
                108f,
                0.07f,
                0.44f,
                0.47f,
                0.87f
            },
            {
                252f,
                0.08f,
                0.39f,
                238f,
                0.08f,
                0.56f,
                0.66f,
                0.62f
            },
            {
                196f,
                0.08f,
                0.89f,
                0f,
                0f,
                0.89f,
                0.54f,
                0.71f
            }
            };
            int num10 = UnityEngine.Random.Range(0, array.GetLength(0));
            num2 = array[num10, 0];
            num3 = array[num10, 1];
            num4 = array[num10, 2];
            num5 = array[num10, 3];
            num6 = array[num10, 4];
            num7 = array[num10, 5];
            num8 = array[num10, 6];
            num9 = array[num10, 7];
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

        //////////////////////////////// eyebrow
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
        HsvColor src = new HsvColor(UnityEngine.Random.Range(0f, 359f), UnityEngine.Random.Range(0.25f, 0.90f), UnityEngine.Random.Range(0.6f, 0.90f));
        HsvColor src2 = new HsvColor(0f, 0f, 0.8f);
        female.customInfo.eyeLColor.hsvDiffuse.Copy(src);
        female.customInfo.eyeLColor.hsvSpecular.Copy(src2);
        /////////////////////////////////// heterochromia chance (5% default)		
        bool flag2 = (UnityEngine.Random.Range(0, 100) < 90) ? true : false;
        if (!flag2)
        {
            // src = new HsvColor(UnityEngine.Random.Range(0f, 359f), 0.25f, 0.5f);
            src = new HsvColor(UnityEngine.Random.Range(0f, 359f), UnityEngine.Random.Range(0.25f, 0.90f), UnityEngine.Random.Range(0.6f, 0.90f));
            src2 = new HsvColor(0f, 0f, 0.8f);
        }
        //////////////////////////////// eye color left
        female.customInfo.eyeRColor.hsvDiffuse.Copy(src);
        female.customInfo.eyeRColor.hsvSpecular.Copy(src2);

        //////////////////////////////// eye shadow
        if (UnityEngine.Random.Range(0, 100) < 20)
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
            float[,] array5 = new float[7, 3]
            {
            {
                355f,
                0.28f,
                0.6f
            },
            {
                350f,
                0.45f,
                0.55f
            },
            {
                20f,
                0.6f,
                0.6f
            },
            {
                290f,
                0.15f,
                0.15f
            },
            {
                245f,
                0.25f,
                0.6f
            },
            {
                190f,
                0.7f,
                0.4f
            },
            {
                150f,
                0.5f,
                0.4f
            }
            };
            int num15 = UnityEngine.Random.Range(0, array5.GetLength(0));
            female.femaleCustomInfo.eyeshadowColor.hsvDiffuse = new HsvColor(array5[num15, 0], array5[num15, 1], array5[num15, 2]);
            female.femaleCustomInfo.eyeshadowColor.alpha = UnityEngine.Random.Range(0.2f, 0.5f);
        }
        else
        {
            female.femaleCustomInfo.texEyeshadowId = 0;
        }

        //////////////////////////////// cheeks
        if (UnityEngine.Random.Range(0, 100) < 20)
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
        int index13 = UnityEngine.Random.Range(0, 4);
        female.femaleCustomInfo.texLipId = femaleTextureList4.ElementAt(index13).Key;
        float[,] array6 = new float[6, 3]
        {
        {
            0f,
            0.9f,
            0.5f
        },
        {
            0f,
            0.9f,
            0.5f
        },
        {
            0f,
            0.9f,
            0.5f
        },
        {
            0f,
            0.9f,
            0.5f
        },
        {
            310f,
            0.2f,
            0.7f
        },
        {
            340f,
            0.25f,
            0.7f
        }
        };
        int num16 = UnityEngine.Random.Range(0, array6.GetLength(0));
        female.femaleCustomInfo.lipColor.hsvDiffuse = new HsvColor(array6[num16, 0], array6[num16, 1], array6[num16, 2]);
        female.femaleCustomInfo.lipColor.alpha = UnityEngine.Random.Range(0.2f, 0.5f);

        //////////////////////////////// moles
        if (UnityEngine.Random.Range(0, 100) < 10)
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
        bool flag3 = (UnityEngine.Random.Range(0, 100) < 90) ? true : false;
        if (!flag3)
        {
            female.customInfo.shapeValueFace[64] = UnityEngine.Random.Range(0.1f, 0.9f);
            female.customInfo.shapeValueFace[65] = UnityEngine.Random.Range(0.8f, 0.9f);
        }

        //female.chaCustom.UpdateShapeBodyValueFromCustomInfo();
    }

    void RandomiseCharBody()
    {
        if (female == null) return;
        //////////////////////////////// height
        female.customInfo.shapeValueBody[0] = UnityEngine.Random.Range(0.6f, 0.95f);
        //////////////////////////////// bust size
        female.customInfo.shapeValueBody[1] = UnityEngine.Random.Range(0.6f, 0.8f);
        //////////////////////////////// Breast height,
        female.customInfo.shapeValueBody[2] = UnityEngine.Random.Range(0.1f, 0.4f);
        //////////////////////////////// Breast direction
        female.customInfo.shapeValueBody[3] = UnityEngine.Random.Range(0.4f, 0.9f);
        //////////////////////////////// Breast spacing,
        female.customInfo.shapeValueBody[4] = UnityEngine.Random.Range(0.1f, 0.5f);
        //////////////////////////////// Breast angle,
        female.customInfo.shapeValueBody[5] = UnityEngine.Random.Range(0.35f, 0.6f);
        //////////////////////////////// Breast lenght,
        female.customInfo.shapeValueBody[6] = UnityEngine.Random.Range(0.2f, 0.5f);
        //////////////////////////////// Areola puffiness,
        female.customInfo.shapeValueBody[7] = UnityEngine.Random.Range(0.2f, 0.8f);
        //////////////////////////////// Nipple width,
        female.customInfo.shapeValueBody[8] = UnityEngine.Random.Range(0.3f, 0.6f);

        //////////////////////////////// areolaSize
        female.femaleCustomInfo.areolaSize = UnityEngine.Random.Range(0.6f, 0.8f);
        //////////////////////////////// bustSoftness,
        female.femaleCustomInfo.bustSoftness = UnityEngine.Random.Range(0.6f, 0.8f);
        ////////////////////////////////bustWeight
        female.femaleCustomInfo.bustWeight = UnityEngine.Random.Range(0.8f, 1f);

        ////////////////////////////////Head size,
        female.customInfo.shapeValueBody[9] = UnityEngine.Random.Range(0.3f, 0.5f);
        ////////////////////////////////Neck width,
        female.customInfo.shapeValueBody[10] = UnityEngine.Random.Range(0.4f, 0.5f);
        ////////////////////////////////Neck thickness,
        female.customInfo.shapeValueBody[11] = UnityEngine.Random.Range(0.4f, 0.5f);
        ////////////////////////////////Thorax width,
        female.customInfo.shapeValueBody[12] = UnityEngine.Random.Range(0.5f, 0.6f);
        ////////////////////////////////Thorax thickness,
        female.customInfo.shapeValueBody[13] = UnityEngine.Random.Range(0.45f, 0.6f);
        ////////////////////////////////Chest width,
        female.customInfo.shapeValueBody[14] = UnityEngine.Random.Range(0.45f, 0.6f);
        ////////////////////////////////Chest thickness,
        female.customInfo.shapeValueBody[15] = UnityEngine.Random.Range(0.45f, 0.65f);
        ////////////////////////////////Waist width,
        female.customInfo.shapeValueBody[16] = UnityEngine.Random.Range(0.45f, 0.55f);
        ////////////////////////////////Waist thickness,
        female.customInfo.shapeValueBody[17] = UnityEngine.Random.Range(0.45f, 0.55f);
        ////////////////////////////////Waist height,
        female.customInfo.shapeValueBody[18] = UnityEngine.Random.Range(0.45f, 0.5f);
        ////////////////////////////////Pelvis width,
        female.customInfo.shapeValueBody[19] = UnityEngine.Random.Range(0.45f, 0.55f);
        ////////////////////////////////Pelvis thickness,
        female.customInfo.shapeValueBody[20] = UnityEngine.Random.Range(0.55f, 0.75f);
        ////////////////////////////////hips width,
        female.customInfo.shapeValueBody[21] = UnityEngine.Random.Range(0.55f, 0.75f);
        ////////////////////////////////hips thickness,
        female.customInfo.shapeValueBody[22] = UnityEngine.Random.Range(0.55f, 0.75f);

        ////////////////////////////////Butt,
        female.customInfo.shapeValueBody[23] = UnityEngine.Random.Range(0.8f, 1.2f);
        ////////////////////////////////Butt angle,
        female.customInfo.shapeValueBody[24] = UnityEngine.Random.Range(-0.2f, -0.15f);
        ////////////////////////////////thighs,
        female.customInfo.shapeValueBody[25] = UnityEngine.Random.Range(0.60f, 0.9f);
        ////////////////////////////////legs,
        female.customInfo.shapeValueBody[26] = UnityEngine.Random.Range(0.25f, 0.35f);
        ////////////////////////////////Calves,
        female.customInfo.shapeValueBody[27] = UnityEngine.Random.Range(0.6f, 0.8f);
        ////////////////////////////////ankles,
        female.customInfo.shapeValueBody[28] = UnityEngine.Random.Range(0.45f, 0.6f);
        ////////////////////////////////shoulders,
        female.customInfo.shapeValueBody[29] = UnityEngine.Random.Range(0.5f, 0.7f);
        ////////////////////////////////upper arms,
        female.customInfo.shapeValueBody[30] = UnityEngine.Random.Range(0.45f, 0.6f);
        ////////////////////////////////Lower arms,
        female.customInfo.shapeValueBody[31] = UnityEngine.Random.Range(0.45f, 0.6f);



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
        num11 = array3[UnityEngine.Random.Range(0, array3.Length)];
        female.customInfo.skinColor.hsvDiffuse = new HsvColor(array2[num11, 0], array2[num11, 1], array2[num11, 2]);
        female.customInfo.skinColor.hsvSpecular = new HsvColor(array2[num11, 3], array2[num11, 4], array2[num11, 5]);
        female.customInfo.skinColor.specularIntensity = array2[num11, 6];
        female.customInfo.skinColor.specularSharpness = array2[num11, 7];
        //	break;
        //}
        //}

        ////////////////////////////////tattoo
        //if (tattoo)
        //{
        //	if (UnityEngine.Random.Range(0, 100) < 10)
        //	{
        //		Dictionary<int, ListTypeTexture> femaleTextureList = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_tattoo_f);
        //		List<int> list2 = new List<int>();
        //		foreach (KeyValuePair<int, ListTypeTexture> item7 in femaleTextureList)
        //		{
        //			int randomRate = item7.Value.RandomRate;
        //			for (int i = 0; i < randomRate; i++)
        //			{
        //				list2.Add(item7.Key);
        //			}
        //		}
        //		int index8 = UnityEngine.Random.Range(0, list2.Count);
        //		female.customInfo.texTattoo_fId = list2[index8];
        //		float[,] array4 = new float[2, 3]
        //		{
        //			{
        //				0f,
        //				0.2f,
        //				0.2f
        //			},
        //			{
        //				0f,
        //				0.5f,
        //				0.5f
        //			}
        //		};
        //		int num12 = UnityEngine.Random.Range(0, array4.GetLength(0));
        //		female.customInfo.tattoo_fColor.hsvDiffuse = new HsvColor(array4[num12, 0], array4[num12, 1], array4[num12, 2]);
        //	}
        //	else
        //	{
        //		female.customInfo.texTattoo_fId = 0;
        //	}
        //}
        //else
        //{
        //	female.customInfo.texTattoo_fId = 0;
        //}

        //////////////////////////////// body Detail
        if (UnityEngine.Random.Range(0, 100) < 40)
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
        //if (tattoo)
        //{
        //	if (UnityEngine.Random.Range(0, 100) < 10)
        //	{
        //		Dictionary<int, ListTypeTexture> femaleTextureList7 = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_tattoo_b);
        //		List<int> list7 = new List<int>();
        //		foreach (KeyValuePair<int, ListTypeTexture> item12 in femaleTextureList7)
        //		{
        //			int randomRate3 = item12.Value.RandomRate;
        //			for (int n = 0; n < randomRate3; n++)
        //			{
        //				list7.Add(item12.Key);
        //			}
        //		}
        //		int index18 = UnityEngine.Random.Range(0, list7.Count);
        //		female.customInfo.texTattoo_bId = list7[index18];
        //		float[,] array7 = new float[3, 3]
        //		{
        //			{
        //				200f,
        //				0.3f,
        //				0.3f
        //			},
        //			{
        //				0f,
        //				0.2f,
        //				0.2f
        //			},
        //			{
        //				0f,
        //				1f,
        //				0.3f
        //			}
        //		};
        //		int num18 = UnityEngine.Random.Range(0, array7.GetLength(0));
        //		female.customInfo.tattoo_bColor.hsvDiffuse = new HsvColor(array7[num18, 0], array7[num18, 1], array7[num18, 2]);
        //	}
        //	else
        //	{
        //		female.customInfo.texTattoo_bId = 0;
        //	}
        //}
        //else
        //{
        //	female.customInfo.texTattoo_bId = 0;
        //}

        //////////////////////////////// sunburn
        if (UnityEngine.Random.Range(0, 100) < 20)
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
        RandomiseCharFace(lstRandFaceF);
        RandomiseCharBody();
    }

    void RandomiseCharClothing()
    {
        if (female == null) return;
        CharFemaleRandom.RandClothes(female, accessory: true, megane: false);
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
        charFileInfoCustomFemale.name = female.ListInfo.GetRandName(1);
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

    public void Load()
    {
        var config = Config.Load();
        
        var path = GetCurrentlyEditedBoneModFilePath();

        if (path == null)
            return;

        if (File.Exists(path))
        {
            // _log.Info("Loading: " + path);

            var bones = ReadBones(path);

            if (bones.Length == 0)
                return;

            if (HasGUI)
                Unload();

            StartCoroutine(CreateGUI(config, bones));

            HasGUI = true;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////
    //public static void RandBody2(CharFemale female, List<CharFemaleRandom.RandomFaceFemaleInfo> lstRandFace)
    //{
    //    int index = UnityEngine.Random.Range(0, lstRandFace.Count);
    //    // applies all face slider values
    //    // Array.Copy(lstRandFace[index].shapeValue, female.customInfo.shapeValueFace, lstRandFace[index].shapeValue.Length);
    //    female.customInfo.headId = lstRandFace[index].headNo;
    //    female.customInfo.texFaceId = lstRandFace[index].baseTexNo;
    //    female.customInfo.texFaceDetailId = lstRandFace[index].detailTexNo;
    //    female.customInfo.faceDetailWeight = lstRandFace[index].detailWeight;

    ////////////////////////////////////////////////////////////////



    //}
    ////////////////////////////////////////////////////////////////////////////////////////////////

}




