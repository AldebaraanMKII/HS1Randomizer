using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace RandomSearch
{
	public class RandomSearchControl : MonoBehaviour
	{
		[Serializable]
		public class Info
		{
			public Text textName;

			public Text textPersonality;

			public Text textPitch;

			public Text textCharacteristic;

			public Text textImpression;

			public Text textHAttribute;
		}

		[Serializable]
		public class Search
		{
			public Toggle[] tglSkinColor;

			public Toggle[] tglHeight;

			public Toggle[] tglBustSize;

			public Toggle[] tglBodyType;

			public Toggle[] tglHairType;

			public Toggle[] tglPersonality;

			public Toggle[] tglAccessory;

			public Toggle[] tglTattoo;

			public Toggle[] tglOption;
		}

		public Canvas cvsChangeScene;

		public RandomPhotoCtrlPanel randPhotoCtrl;

		public RandomPhotoCheck randPhotoCheck;

		protected List<CharFemaleRandom.RandomFaceFemaleInfo> lstRandFaceF = new List<CharFemaleRandom.RandomFaceFemaleInfo>();

		protected List<CharFemaleRandom.RandomBodyFemaleInfo> lstRandSBodyF = new List<CharFemaleRandom.RandomBodyFemaleInfo>();

		protected List<CharFemaleRandom.RandomBodyFemaleInfo> lstRandNBodyF = new List<CharFemaleRandom.RandomBodyFemaleInfo>();

		protected List<CharFemaleRandom.RandomBodyFemaleInfo> lstRandLBodyF = new List<CharFemaleRandom.RandomBodyFemaleInfo>();

		public bool checkMode;

		public bool photoMode;

		public Info info = new Info();

		public Search search = new Search();

		public Canvas cvsSearchUI;

		public Canvas cvsPhotoUI;

		public GameObject objCheck;

		public GameObject objCheckExit;

		public GameObject objCheckExitNoSave;

		public GameObject objCheckGameEnd;

		public GameObject obj3DEffect;

		public Image imgDefault;

		public Image imgOptional;

		public Toggle[] tglCoordinateType = new Toggle[3];

		protected int coordinateType;

		public Toggle[] tglClothesState = new Toggle[3];

		protected int clothesState;

		public Toggle[] tglAccessoryState = new Toggle[2];

		protected int accessoryState;

		protected bool saveCard;

		public SaveFrameAssist saveFrameAssist;

		public bool showCardFrontFrame = true;

		public bool showCardBackFrame = true;

		protected FolderAssist faBackground = new FolderAssist();

		protected bool changeCoordinateTypeOK = true;

		protected CameraControl cctrl;

		public CharFemale female;

		public virtual void Init(CharFemale _female)
		{
			female = _female;
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
			string assetBundleName2 = "custom/body_s_randf.unity3d";
			assetBundleLoadAssetOperation = AssetBundleManager.LoadAllAsset(assetBundleName2, typeof(TextAsset));
			if (assetBundleLoadAssetOperation != null && !assetBundleLoadAssetOperation.IsEmpty())
			{
				TextAsset[] allAssets2 = assetBundleLoadAssetOperation.GetAllAssets<TextAsset>();
				TextAsset[] array2 = allAssets2;
				foreach (TextAsset ta2 in array2)
				{
					CharFemaleRandom.RandomBodyFemaleInfo randomBodyFemaleInfo = new CharFemaleRandom.RandomBodyFemaleInfo();
					randomBodyFemaleInfo.Load(ta2);
					lstRandSBodyF.Add(randomBodyFemaleInfo);
				}
			}
			AssetBundleManager.UnloadAssetBundle(assetBundleName2);
			string assetBundleName3 = "custom/body_n_randf.unity3d";
			assetBundleLoadAssetOperation = AssetBundleManager.LoadAllAsset(assetBundleName3, typeof(TextAsset));
			if (assetBundleLoadAssetOperation != null && !assetBundleLoadAssetOperation.IsEmpty())
			{
				TextAsset[] allAssets3 = assetBundleLoadAssetOperation.GetAllAssets<TextAsset>();
				TextAsset[] array3 = allAssets3;
				foreach (TextAsset ta3 in array3)
				{
					CharFemaleRandom.RandomBodyFemaleInfo randomBodyFemaleInfo2 = new CharFemaleRandom.RandomBodyFemaleInfo();
					randomBodyFemaleInfo2.Load(ta3);
					lstRandNBodyF.Add(randomBodyFemaleInfo2);
				}
			}
			AssetBundleManager.UnloadAssetBundle(assetBundleName3);
			string assetBundleName4 = "custom/body_l_randf.unity3d";
			assetBundleLoadAssetOperation = AssetBundleManager.LoadAllAsset(assetBundleName4, typeof(TextAsset));
			if (assetBundleLoadAssetOperation != null && !assetBundleLoadAssetOperation.IsEmpty())
			{
				TextAsset[] allAssets4 = assetBundleLoadAssetOperation.GetAllAssets<TextAsset>();
				TextAsset[] array4 = allAssets4;
				foreach (TextAsset ta4 in array4)
				{
					CharFemaleRandom.RandomBodyFemaleInfo randomBodyFemaleInfo3 = new CharFemaleRandom.RandomBodyFemaleInfo();
					randomBodyFemaleInfo3.Load(ta4);
					lstRandLBodyF.Add(randomBodyFemaleInfo3);
				}
			}
			AssetBundleManager.UnloadAssetBundle(assetBundleName4);
			string folder = UserData.Path + "background";
			string[] searchPattern = new string[1]
			{
				"*.png"
			};
			faBackground.CreateFolderInfoEx(folder, searchPattern);
			if (faBackground.GetFileCount() != 0)
			{
				Sprite sprite = PngAssist.LoadSpriteFromFile(faBackground.lstFile[0].FullPath, 0, 0, new Vector2(0.5f, 0.5f));
				if ((bool)imgOptional)
				{
					if ((bool)(UnityEngine.Object)(object)imgOptional.sprite)
					{
						if ((bool)imgOptional.sprite.get_texture())
						{
							UnityEngine.Object.Destroy(imgOptional.sprite.get_texture());
						}
						UnityEngine.Object.Destroy((UnityEngine.Object)(object)imgOptional.sprite);
					}
					imgOptional.sprite = sprite;
				}
			}
			showCardFrontFrame = true;
			showCardBackFrame = true;
			randPhotoCtrl.Init();
			saveFrameAssist = SimpleSingleton<SaveFrameAssist>.Instance;
			saveFrameAssist.ForgetLastName();
			saveFrameAssist.CreateSaveFrameToHierarchy();
			ExitPhotoMode();
			CharFemaleRandom.RandParameter(female, 1);
			CharFemaleRandom.RandBody(female, lstRandFaceF, lstRandSBodyF, lstRandNBodyF, lstRandLBodyF, 1, 1, 1, 1, 5, tattoo: false, elf: false);
			CharFemaleRandom.RandClothes(female, accessory: true, megane: false);
			female.chaFile.ChangeCoordinateType(CharDefine.CoordinateType.type00);
			if (null == cctrl && (bool)Camera.main)
			{
				cctrl = Camera.main.GetComponent<CameraControl>();
			}
			if ((bool)cctrl)
			{
				cctrl.EnableResetKey = false;
			}
			StartCoroutine(StartCamera());
		}

		public virtual IEnumerator StartCamera()
		{
			yield return null;
			yield return null;
			randPhotoCtrl.ResetCamera(forceCalc: true);
			yield return null;
		}

		public virtual void Update()
		{
			if (string.Empty == Singleton<Scene>.Instance.AddSceneName && !checkMode)
			{
				if (Input.GetKeyDown(KeyCode.F1))
				{
					Singleton<Scene>.Instance.LoadReserv("Config", isLoad: false, isAsync: false, isFade: false, isOverlap: false, isLoadingImageDraw: false);
				}
				else if (Input.GetKeyDown(KeyCode.R))
				{
					if (photoMode)
					{
						randPhotoCtrl.OnResetCameraEx();
					}
					else
					{
						randPhotoCtrl.OnResetCamera();
					}
				}
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					OnClickEndGame();
					return;
				}
			}
			if (null == cctrl && (bool)Camera.main)
			{
				cctrl = Camera.main.GetComponent<CameraControl>();
			}
			if (null != cctrl)
			{
				float cameraSpeed = Manager.Config.EtcData.CameraSpeed;
				cctrl.xRotSpeed = Mathf.Lerp(0.5f, 4f, cameraSpeed);
				cctrl.yRotSpeed = Mathf.Lerp(0.5f, 4f, cameraSpeed);
				cctrl.moveSpeed = Mathf.Lerp(0.01f, 0.1f, cameraSpeed);
			}
			if (Input.GetMouseButtonDown(1))
			{
				if ((bool)objCheck)
				{
					objCheck.SetActive(value: false);
				}
				if ((bool)objCheckExit)
				{
					objCheckExit.SetActive(value: false);
				}
				if ((bool)objCheckExitNoSave)
				{
					objCheckExitNoSave.SetActive(value: false);
				}
				if ((bool)objCheckGameEnd)
				{
					objCheckGameEnd.SetActive(value: false);
				}
			}
		}

		public virtual void UpdateCharaInfo()
		{
			if (info != null)
			{
				if ((bool)(UnityEngine.Object)(object)info.textName)
				{
					info.textName.text = female.customInfo.name;
				}
				if ((bool)(UnityEngine.Object)(object)info.textPersonality)
				{
					PersonalityIdInfo personalityInfo = female.ListInfo.GetPersonalityInfo(female.customInfo.personality);
					info.textPersonality.text = personalityInfo.Name;
				}
				if ((bool)(UnityEngine.Object)(object)info.textPitch)
				{
					string[] array = new string[3]
					{
						"低い",
						"普通",
						"高い"
					};
					float num = Mathf.InverseLerp(0.94f, 1.06f, female.customInfo.voicePitch);
					int num2 = 0;
					num2 = ((!(num < 0.33f)) ? ((!(num > 0.66f)) ? 1 : 2) : 0);
					info.textPitch.text = array[num2];
				}
				CharFileInfoParameterFemale charFileInfoParameterFemale = female.parameterInfo as CharFileInfoParameterFemale;
				if ((bool)(UnityEngine.Object)(object)info.textCharacteristic)
				{
					string[] array2 = new string[9]
					{
						"なし",
						"綺麗好き",
						"ものぐさ",
						"疲れやすい",
						"疲れ知らず",
						"頻尿",
						"グラスハ\u30fcト",
						"不屈の精神",
						"欲求不満"
					};
					info.textCharacteristic.text = array2[charFileInfoParameterFemale.characteristic];
				}
				if ((bool)(UnityEngine.Object)(object)info.textImpression)
				{
					string[] array3 = new string[14]
					{
						"なし",
						"楽しい",
						"好きかも",
						"一目惚れ",
						"興味がある",
						"魅力的",
						"抱かれたい",
						"話づらい",
						"苦手",
						"嫌い",
						"指示されたい",
						"命令されたい",
						"逆らえない",
						"・・・・"
					};
					info.textImpression.text = array3[charFileInfoParameterFemale.impression];
				}
				if ((bool)(UnityEngine.Object)(object)info.textHAttribute)
				{
					string[] array4 = new string[13]
					{
						"なし",
						"性欲旺盛",
						"Ｓ",
						"Ｍ",
						"胸が敏感",
						"お尻が敏感",
						"股間が敏感",
						"キスに弱い",
						"されるのに弱い",
						"反応好き",
						"潔癖症",
						"Ｈに抵抗がある",
						"寂しがり屋"
					};
					info.textHAttribute.text = array4[charFileInfoParameterFemale.attribute];
				}
			}
		}

		public virtual void OnClickConfig()
		{
			Singleton<Scene>.Instance.LoadReserv("Config", isLoad: false, isAsync: false, isFade: false, isOverlap: false, isLoadingImageDraw: false);
		}

		public virtual void OnClickAllOFF()
		{
			Toggle[] tglSkinColor = search.tglSkinColor;
			foreach (Toggle toggle in tglSkinColor)
			{
				toggle.isOn = false;
			}
			if ((bool)search.tglSkinColor[3])
			{
				search.tglSkinColor[3].isOn = true;
			}
			Toggle[] tglHeight = search.tglHeight;
			foreach (Toggle toggle2 in tglHeight)
			{
				toggle2.isOn = false;
			}
			if ((bool)search.tglHeight[3])
			{
				search.tglHeight[3].isOn = true;
			}
			Toggle[] tglBustSize = search.tglBustSize;
			foreach (Toggle toggle3 in tglBustSize)
			{
				toggle3.isOn = false;
			}
			if ((bool)search.tglBustSize[3])
			{
				search.tglBustSize[3].isOn = true;
			}
			Toggle[] tglBodyType = search.tglBodyType;
			foreach (Toggle toggle4 in tglBodyType)
			{
				toggle4.isOn = false;
			}
			if ((bool)search.tglBodyType[3])
			{
				search.tglBodyType[3].isOn = true;
			}
			Toggle[] tglHairType = search.tglHairType;
			foreach (Toggle toggle5 in tglHairType)
			{
				toggle5.isOn = false;
			}
			if ((bool)search.tglHairType[5])
			{
				search.tglHairType[5].isOn = true;
			}
			Toggle[] tglPersonality = search.tglPersonality;
			foreach (Toggle toggle6 in tglPersonality)
			{
				toggle6.isOn = false;
			}
			if ((bool)search.tglPersonality[2])
			{
				search.tglPersonality[2].isOn = true;
			}
			Toggle[] tglAccessory = search.tglAccessory;
			foreach (Toggle toggle7 in tglAccessory)
			{
				toggle7.isOn = false;
			}
			if ((bool)search.tglAccessory[0])
			{
				search.tglAccessory[0].isOn = true;
			}
			Toggle[] tglTattoo = search.tglTattoo;
			foreach (Toggle toggle8 in tglTattoo)
			{
				toggle8.isOn = false;
			}
			if ((bool)search.tglTattoo[0])
			{
				search.tglTattoo[0].isOn = true;
			}
			Toggle[] tglOption = search.tglOption;
			foreach (Toggle toggle9 in tglOption)
			{
				toggle9.isOn = false;
			}
		}

		public virtual void OnClickSearch()
		{
			int personalityType = 0;
			for (int i = 0; i < search.tglPersonality.Length; i++)
			{
				if (search.tglPersonality[i].isOn)
				{
					personalityType = i;
					break;
				}
			}
			int skinColor = 0;
			for (int j = 0; j < search.tglSkinColor.Length; j++)
			{
				if (search.tglSkinColor[j].isOn)
				{
					skinColor = j;
					break;
				}
			}
			int height = 0;
			for (int k = 0; k < search.tglHeight.Length; k++)
			{
				if (search.tglHeight[k].isOn)
				{
					height = k;
					break;
				}
			}
			int bustSize = 0;
			for (int l = 0; l < search.tglBustSize.Length; l++)
			{
				if (search.tglBustSize[l].isOn)
				{
					bustSize = l;
					break;
				}
			}
			int bodyType = 0;
			for (int m = 0; m < search.tglBodyType.Length; m++)
			{
				if (search.tglBodyType[m].isOn)
				{
					bodyType = m;
					break;
				}
			}
			int hairType = 0;
			for (int n = 0; n < search.tglHairType.Length; n++)
			{
				if (search.tglHairType[n].isOn)
				{
					hairType = n;
					break;
				}
			}
			CharFemaleRandom.RandParameter(female, personalityType);
			CharFemaleRandom.RandBody(female, lstRandFaceF, lstRandSBodyF, lstRandNBodyF, lstRandLBodyF, skinColor, height, bustSize, bodyType, hairType, search.tglTattoo[0].isOn, search.tglOption[0].isOn);
			CharFemaleRandom.RandClothes(female, search.tglAccessory[0].isOn, search.tglOption[1].isOn);
			female.chaFile.ChangeCoordinateType((CharDefine.CoordinateType)coordinateType);
			female.Reload();
			UpdateCharaInfo();
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
		}

		public virtual void OnClickSave()
		{
			ChangePhotoMode();
		}

		public virtual void ExecuteSave()
		{
			string path = "charaF_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
			string path2 = female.chaFile.ConvertCharaFilePath(path);
			if (File.Exists(path2))
			{
			}
			female.chaFile.Save(path2);
			saveCard = true;
		}

		public virtual void OnClickChangeCoordinateType(int index)
		{
			if (changeCoordinateTypeOK)
			{
				coordinateType = index;
				female.chaFile.ChangeCoordinateType((CharDefine.CoordinateType)coordinateType);
				female.Reload();
			}
		}

		public virtual void OnClickChangeClothesState(int index)
		{
			clothesState = index;
			byte[,] array = new byte[3, 12]
			{
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				{
					2,
					2,
					0,
					0,
					0,
					0,
					2,
					2,
					0,
					0,
					0,
					2
				},
				{
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2,
					2
				}
			};
			for (int i = 0; i < 12; i++)
			{
				female.chaClothes.SetClothesState(i, array[clothesState, i]);
			}
		}

		public virtual void OnClickChangeAccessoryState(int index)
		{
			accessoryState = index;
			bool[] array = new bool[2]
			{
				true,
				false
			};
			female.chaClothes.SetAccessoryStateAll(array[accessoryState]);
		}

		public virtual void OnClickEndSearch()
		{
			if (!objCheck)
			{
				return;
			}
			objCheck.SetActive(value: true);
			if (saveCard)
			{
				if ((bool)objCheckExit)
				{
					objCheckExit.SetActive(value: true);
				}
			}
			else if ((bool)objCheckExitNoSave)
			{
				objCheckExitNoSave.SetActive(value: true);
			}
		}

		public virtual void OnCheckExit(int yesno)
		{
			if (yesno == 0)
			{
				if ((bool)objCheck)
				{
					objCheck.SetActive(value: false);
				}
				if ((bool)objCheckExit)
				{
					objCheckExit.SetActive(value: false);
				}
				if ((bool)objCheckExitNoSave)
				{
					objCheckExitNoSave.SetActive(value: false);
				}
			}
			else
			{
				Singleton<Character>.Instance.EndLoadAssetBundle();
				if ((bool)cvsChangeScene)
				{
					cvsChangeScene.gameObject.SetActive(value: true);
				}
				Singleton<Game>.Instance.isRoomConcierge = true;
				Singleton<Scene>.Instance.LoadReserv("Game", isLoad: true);
			}
		}

		public virtual void OnClickEndGame()
		{
			if ((bool)objCheck)
			{
				objCheck.SetActive(value: true);
				if ((bool)objCheckGameEnd)
				{
					objCheckGameEnd.SetActive(value: true);
				}
				Singleton<Scene>.Instance.isSkipGameExit = true;
			}
		}

		public virtual void OnCheckEndGame(int yesno)
		{
			if (yesno == 0)
			{
				if ((bool)objCheck)
				{
					objCheck.SetActive(value: false);
				}
				if ((bool)objCheckGameEnd)
				{
					objCheckGameEnd.SetActive(value: false);
				}
				Singleton<Scene>.Instance.isSkipGameExit = false;
			}
			else
			{
				Singleton<Character>.Instance.EndLoadAssetBundle();
				Singleton<Scene>.Instance.isSkipGameExit = false;
				Singleton<Scene>.Instance.GameEnd(_isCheck: false);
			}
		}

		public virtual void SetSetting()
		{
			changeCoordinateTypeOK = false;
			Toggle[] array = tglCoordinateType;
			foreach (Toggle toggle in array)
			{
				toggle.isOn = false;
			}
			tglCoordinateType[(int)female.chaFile.statusInfo.coordinateType].isOn = true;
			changeCoordinateTypeOK = true;
			Toggle[] array2 = tglClothesState;
			foreach (Toggle toggle2 in array2)
			{
				toggle2.isOn = false;
			}
			tglClothesState[randPhotoCtrl.cosStateNo].isOn = true;
			OnClickChangeClothesState(randPhotoCtrl.cosStateNo);
			Toggle[] array3 = tglAccessoryState;
			foreach (Toggle toggle3 in array3)
			{
				toggle3.isOn = false;
			}
			tglAccessoryState[randPhotoCtrl.acsStateNo].isOn = true;
			OnClickChangeAccessoryState(randPhotoCtrl.acsStateNo);
			if ((bool)obj3DEffect)
			{
				obj3DEffect.SetActive(value: true);
			}
			if ((bool)imgDefault)
			{
				imgDefault.enabled = true;
			}
			if ((bool)imgOptional)
			{
				imgOptional.enabled = false;
			}
			PersonalityIdInfo personalityInfo = female.ListInfo.GetPersonalityInfo(female.chaFile.customInfo.personality);
			int ptn = personalityInfo.randAnmPtn[UnityEngine.Random.Range(0, 3)];
			randPhotoCtrl.ChangeCustomAnm(ptn);
		}

		public virtual void ChangePhotoMode()
		{
			photoMode = true;
			if ((bool)cvsSearchUI)
			{
				cvsSearchUI.gameObject.SetActive(value: false);
			}
			if ((bool)cvsPhotoUI)
			{
				cvsPhotoUI.gameObject.SetActive(value: true);
			}
			if (null == cctrl && (bool)Camera.main)
			{
				cctrl = Camera.main.GetComponent<CameraControl>();
			}
			if (null != cctrl)
			{
				cctrl.NoCtrlCondition = (() => false);
			}
			if (saveFrameAssist != null)
			{
				saveFrameAssist.ShowSaveFrameBack(showCardBackFrame);
				saveFrameAssist.ShowSaveFrameFront(showCardFrontFrame);
			}
			randPhotoCtrl.SetSetting(clothesState, accessoryState);
		}

		public virtual void ExitPhotoMode()
		{
			photoMode = false;
			if ((bool)cvsSearchUI)
			{
				cvsSearchUI.gameObject.SetActive(value: true);
			}
			if ((bool)cvsPhotoUI)
			{
				cvsPhotoUI.gameObject.SetActive(value: false);
			}
			if (null == cctrl && (bool)Camera.main)
			{
				cctrl = Camera.main.GetComponent<CameraControl>();
			}
			if (null != cctrl)
			{
				cctrl.NoCtrlCondition = (() => false);
			}
			if (saveFrameAssist != null)
			{
				saveFrameAssist.ShowSaveFrameBack(visible: false);
				saveFrameAssist.ShowSaveFrameFront(visible: false);
			}
			SetSetting();
			female.ChangeEyesPtn(0);
			female.ChangeLookEyesPtn(1);
			female.ChangeEyesOpen(1f);
			female.ChangeMouthPtn(1);
			female.ChangeLookNeckPtn(3);
			female.ChangeMouthOpen(1f);
		}
	}
}
