using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CharFemaleRandom : CharRandom
{
	public class RandomFaceFemaleInfo
	{
		public float[] shapeValue = new float[CharDefine.cf_headshapename.Length];

		public int headNo;

		public int baseTexNo;

		public int detailTexNo;

		public float detailWeight;

		public virtual void Load(TextAsset ta)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.Write(ta.bytes, 0, ta.bytes.Length);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					for (int i = 0; i < shapeValue.Length; i++)
					{
						shapeValue[i] = Mathf.Clamp(binaryReader.ReadSingle(), 0f, 1f);
					}
					headNo = binaryReader.ReadInt32();
					baseTexNo = binaryReader.ReadInt32();
					detailTexNo = binaryReader.ReadInt32();
					detailWeight = binaryReader.ReadSingle();
				}
			}
		}
	}

	public class RandomBodyFemaleInfo
	{
		public float[] shapeValue = new float[CharDefine.cf_bodyshapename.Length - 1];

		public virtual void Load(TextAsset ta)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.Write(ta.bytes, 0, ta.bytes.Length);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					for (int i = 0; i < shapeValue.Length; i++)
					{
						shapeValue[i] = Mathf.Clamp(binaryReader.ReadSingle(), 0f, 1f);
					}
				}
			}
		}
	}

	public static int cnt;

	public static void RandParameter(CharFemale female, int personalityType)
	{
		CharFileInfoParameterFemale charFileInfoParameterFemale = female.chaFile.parameterInfo as CharFileInfoParameterFemale;
		int length = Enum.GetValues(typeof(CharFileInfoParameterFemale.Characteristic)).Length;
		charFileInfoParameterFemale.characteristic = UnityEngine.Random.Range(0, length);
		List<int> list = new List<int>();
		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(4);
		list.Add(5);
		list.Add(7);
		list.Add(8);
		list.Add(10);
		list.Add(11);
		if (Game.GetAchievementRelease(7))
		{
			list.Add(3);
		}
		if (Game.GetAchievementRelease(8))
		{
			list.Add(6);
		}
		if (Game.GetAchievementRelease(9))
		{
			list.Add(9);
		}
		if (Game.GetAchievementRelease(10))
		{
			list.Add(12);
		}
		if (Game.GetAchievementRelease(11))
		{
			list.Add(13);
		}
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
			switch (personalityType)
			{
			case 0:
				if (item.Value.type == 0)
				{
					list2.Add(item.Value.Id);
				}
				break;
			case 1:
				if (item.Value.type == 1)
				{
					list2.Add(item.Value.Id);
				}
				break;
			default:
				list2.Add(item.Value.Id);
				break;
			}
		}
		int[] array2 = list2.ToArray();
		charFileInfoCustomFemale.personality = array2[UnityEngine.Random.Range(0, array2.Length)];
		charFileInfoCustomFemale.voicePitch = UnityEngine.Random.Range(0.94f, 1.06f);
	}

	public static void RandBody(CharFemale female, List<RandomFaceFemaleInfo> lstRandFace, List<RandomBodyFemaleInfo> lstRandSBody, List<RandomBodyFemaleInfo> lstRandNBody, List<RandomBodyFemaleInfo> lstRandLBody, int skinColor, int height, int bustSize, int bodyType, int hairType, bool tattoo, bool elf)
	{
		int index = UnityEngine.Random.Range(0, lstRandFace.Count);
		Array.Copy(lstRandFace[index].shapeValue, female.customInfo.shapeValueFace, lstRandFace[index].shapeValue.Length);
		female.customInfo.headId = lstRandFace[index].headNo;
		female.customInfo.texFaceId = lstRandFace[index].baseTexNo;
		female.customInfo.texFaceDetailId = lstRandFace[index].detailTexNo;
		female.customInfo.faceDetailWeight = lstRandFace[index].detailWeight;
		int num = bodyType;
		if (bodyType == 3)
		{
			num = ((UnityEngine.Random.Range(0, 100) >= 5) ? ((UnityEngine.Random.Range(0, 100) >= 5) ? 1 : 2) : 0);
		}
		switch (num)
		{
		case 0:
		{
			int index4 = UnityEngine.Random.Range(0, lstRandSBody.Count);
			Array.Copy(lstRandSBody[index4].shapeValue, female.customInfo.shapeValueBody, lstRandSBody[index4].shapeValue.Length);
			break;
		}
		case 1:
		{
			int index3 = UnityEngine.Random.Range(0, lstRandNBody.Count);
			Array.Copy(lstRandNBody[index3].shapeValue, female.customInfo.shapeValueBody, lstRandNBody[index3].shapeValue.Length);
			break;
		}
		default:
		{
			int index2 = UnityEngine.Random.Range(0, lstRandLBody.Count);
			Array.Copy(lstRandLBody[index2].shapeValue, female.customInfo.shapeValueBody, lstRandLBody[index2].shapeValue.Length);
			break;
		}
		}
		switch (height)
		{
		case 0:
			female.customInfo.shapeValueBody[0] = UnityEngine.Random.Range(0.1f, 0.35f);
			break;
		case 1:
			female.customInfo.shapeValueBody[0] = UnityEngine.Random.Range(0.35f, 0.65f);
			break;
		case 2:
			female.customInfo.shapeValueBody[0] = UnityEngine.Random.Range(0.65f, 0.9f);
			break;
		default:
			female.customInfo.shapeValueBody[0] = UnityEngine.Random.Range(0.1f, 0.9f);
			break;
		}
		switch (bustSize)
		{
		case 0:
			female.customInfo.shapeValueBody[1] = UnityEngine.Random.Range(0.25f, 0.35f);
			break;
		case 1:
			female.customInfo.shapeValueBody[1] = UnityEngine.Random.Range(0.35f, 0.55f);
			break;
		case 2:
			female.customInfo.shapeValueBody[1] = UnityEngine.Random.Range(1.15f, 1.32f);
			break;
		default:
			female.customInfo.shapeValueBody[1] = UnityEngine.Random.Range(0.25f, 0.65f);
			break;
		}
		if (elf)
		{
			female.customInfo.shapeValueFace[64] = UnityEngine.Random.Range(0.1f, 0.9f);
			female.customInfo.shapeValueFace[65] = UnityEngine.Random.Range(0.8f, 0.9f);
		}
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
			float[,] array = new float[21, 8]
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
		bool flag = false;
		Dictionary<int, ListTypeFbx> femaleFbxList = female.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairB);
		List<int> list = new List<int>();
		switch (hairType)
		{
		case 0:
			foreach (KeyValuePair<int, ListTypeFbx> item in femaleFbxList)
			{
				if (item.Value.Type == 2)
				{
					list.Add(item.Key);
				}
			}
			break;
		case 1:
			foreach (KeyValuePair<int, ListTypeFbx> item2 in femaleFbxList)
			{
				if ((item2.Value.Type == 0 || item2.Value.Type == 1) && "1" == item2.Value.Etc[1])
				{
					list.Add(item2.Key);
				}
			}
			break;
		case 2:
			foreach (KeyValuePair<int, ListTypeFbx> item3 in femaleFbxList)
			{
				if (item3.Value.Type == 4)
				{
					list.Add(item3.Key);
				}
			}
			break;
		case 3:
			foreach (KeyValuePair<int, ListTypeFbx> item4 in femaleFbxList)
			{
				if (item4.Value.Type == 3)
				{
					list.Add(item4.Key);
				}
			}
			break;
		case 4:
			foreach (KeyValuePair<int, ListTypeFbx> item5 in femaleFbxList)
			{
				if (item5.Value.Type == 5)
				{
					list.Add(item5.Key);
				}
			}
			break;
		default:
			foreach (KeyValuePair<int, ListTypeFbx> item6 in femaleFbxList)
			{
				if (!("0" == item6.Value.Etc[1]))
				{
					list.Add(item6.Key);
				}
			}
			break;
		}
		int index5 = UnityEngine.Random.Range(0, list.Count);
		if ("1" == femaleFbxList[list[index5]].Etc[0])
		{
			flag = true;
		}
		female.customInfo.hairId[0] = list[index5];
		if (!flag)
		{
			Dictionary<int, ListTypeFbx> femaleFbxList2 = female.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairF);
			int index6 = UnityEngine.Random.Range(1, femaleFbxList2.Count);
			female.customInfo.hairId[1] = femaleFbxList2.ElementAt(index6).Key;
			if (UnityEngine.Random.Range(0, 100) < 50)
			{
				Dictionary<int, ListTypeFbx> femaleFbxList3 = female.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairS);
				int index7 = UnityEngine.Random.Range(1, femaleFbxList3.Count);
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
		female.customInfo.hairColor[0].hsvDiffuse.Copy(new HsvColor(num2, num3, num4));
		female.customInfo.hairColor[0].hsvSpecular.Copy(new HsvColor(num5, num6, num7));
		female.customInfo.hairColor[0].specularIntensity = num8;
		female.customInfo.hairColor[0].specularSharpness = num9;
		female.customInfo.hairColor[1].hsvDiffuse.Copy(new HsvColor(num2, num3, num4));
		female.customInfo.hairColor[1].hsvSpecular.Copy(new HsvColor(num5, num6, num7));
		female.customInfo.hairColor[1].specularIntensity = num8;
		female.customInfo.hairColor[1].specularSharpness = num9;
		female.customInfo.hairColor[2].hsvDiffuse.Copy(new HsvColor(num2, num3, num4));
		female.customInfo.hairColor[2].hsvSpecular.Copy(new HsvColor(num5, num6, num7));
		female.customInfo.hairColor[2].specularIntensity = num8;
		female.customInfo.hairColor[2].specularSharpness = num9;
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
		switch (skinColor)
		{
		case 0:
			num11 = UnityEngine.Random.Range(0, 2);
			female.customInfo.skinColor.hsvDiffuse = new HsvColor(array2[num11, 0], array2[num11, 1], array2[num11, 2]);
			female.customInfo.skinColor.hsvSpecular = new HsvColor(array2[num11, 3], array2[num11, 4], array2[num11, 5]);
			female.customInfo.skinColor.specularIntensity = array2[num11, 6];
			female.customInfo.skinColor.specularSharpness = array2[num11, 7];
			break;
		case 1:
			num11 = UnityEngine.Random.Range(2, 4);
			female.customInfo.skinColor.hsvDiffuse = new HsvColor(array2[num11, 0], array2[num11, 1], array2[num11, 2]);
			female.customInfo.skinColor.hsvSpecular = new HsvColor(array2[num11, 3], array2[num11, 4], array2[num11, 5]);
			female.customInfo.skinColor.specularIntensity = array2[num11, 6];
			female.customInfo.skinColor.specularSharpness = array2[num11, 7];
			break;
		case 2:
			num11 = UnityEngine.Random.Range(4, 6);
			female.customInfo.skinColor.hsvDiffuse = new HsvColor(array2[num11, 0], array2[num11, 1], array2[num11, 2]);
			female.customInfo.skinColor.hsvSpecular = new HsvColor(array2[num11, 3], array2[num11, 4], array2[num11, 5]);
			female.customInfo.skinColor.specularIntensity = array2[num11, 6];
			female.customInfo.skinColor.specularSharpness = array2[num11, 7];
			break;
		default:
		{
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
			break;
		}
		}
		if (tattoo)
		{
			if (UnityEngine.Random.Range(0, 100) < 10)
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
		}
		else
		{
			female.customInfo.texTattoo_fId = 0;
		}
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
		int index9 = UnityEngine.Random.Range(0, list3.Count);
		female.customInfo.matEyebrowId = list3[index9];
		female.customInfo.eyebrowColor.hsvDiffuse.Copy(new HsvColor(num2, num3, num4 * 0.7f));
		female.customInfo.eyebrowColor.hsvSpecular.Copy(new HsvColor(num5, num6, num7));
		female.customInfo.eyebrowColor.specularIntensity = 0f;
		female.customInfo.eyebrowColor.specularSharpness = 0f;
		bool flag2 = (UnityEngine.Random.Range(0, 100) < 95) ? true : false;
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
		HsvColor src = new HsvColor(UnityEngine.Random.Range(0f, 359f), 0.25f, 0.5f);
		HsvColor src2 = new HsvColor(0f, 0f, 0.8f);
		female.customInfo.eyeLColor.hsvDiffuse.Copy(src);
		female.customInfo.eyeLColor.hsvSpecular.Copy(src2);
		if (!flag2)
		{
			src = new HsvColor(UnityEngine.Random.Range(0f, 359f), 0.25f, 0.5f);
			src2 = new HsvColor(0f, 0f, 0.8f);
		}
		female.customInfo.eyeRColor.hsvDiffuse.Copy(src);
		female.customInfo.eyeRColor.hsvSpecular.Copy(src2);
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
			int index11 = UnityEngine.Random.Range(0, list5.Count);
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
		Dictionary<int, ListTypeTexture> femaleTextureList4 = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_lip);
		int index13 = UnityEngine.Random.Range(0, femaleTextureList4.Count);
		female.femaleCustomInfo.texLipId = femaleTextureList4.ElementAt(index13).Key;
		float[,] array6 = new float[3, 3]
		{
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
		if (UnityEngine.Random.Range(0, 100) < 20)
		{
			Dictionary<int, ListTypeTexture> femaleTextureList5 = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_mole);
			int index14 = UnityEngine.Random.Range(0, femaleTextureList5.Count);
			female.femaleCustomInfo.texMoleId = femaleTextureList5.ElementAt(index14).Key;
		}
		else
		{
			female.femaleCustomInfo.texMoleId = 0;
		}
		Dictionary<int, ListTypeMaterial> femaleMaterialList3 = female.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyelashes);
		int index15 = UnityEngine.Random.Range(0, femaleMaterialList3.Count);
		female.femaleCustomInfo.matEyelashesId = femaleMaterialList3.ElementAt(index15).Key;
		female.femaleCustomInfo.eyelashesColor.hsvDiffuse.Copy(new HsvColor(num2, num3, num4 * 0.7f));
		female.femaleCustomInfo.eyelashesColor.hsvSpecular.Copy(new HsvColor(num5, num6, num7));
		female.femaleCustomInfo.eyelashesColor.specularIntensity = 0f;
		female.femaleCustomInfo.eyelashesColor.specularSharpness = 0f;
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
		if (UnityEngine.Random.Range(0, 100) < 10)
		{
			Dictionary<int, ListTypeTexture> femaleTextureList6 = female.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_detail_b);
			int index17 = UnityEngine.Random.Range(0, femaleTextureList6.Count);
			female.customInfo.texBodyDetailId = femaleTextureList6.ElementAt(index17).Key;
			female.customInfo.bodyDetailWeight = UnityEngine.Random.Range(0.1f, 0.2f);
		}
		else
		{
			female.customInfo.texBodyDetailId = 0;
		}
		if (tattoo)
		{
			if (UnityEngine.Random.Range(0, 100) < 10)
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
		}
		else
		{
			female.customInfo.texTattoo_bId = 0;
		}
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
		Dictionary<int, ListTypeMaterial> femaleMaterialList5 = female.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_nip);
		int index20 = UnityEngine.Random.Range(0, femaleMaterialList5.Count);
		female.femaleCustomInfo.matNipId = femaleMaterialList5.ElementAt(index20).Key;
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
		Dictionary<int, ListTypeMaterial> femaleMaterialList6 = female.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_underhair);
		int index21 = UnityEngine.Random.Range(0, femaleMaterialList6.Count);
		female.femaleCustomInfo.matUnderhairId = femaleMaterialList6.ElementAt(index21).Key;
		female.femaleCustomInfo.underhairColor.hsvDiffuse.Copy(new HsvColor(num2, num3, num4));
		female.femaleCustomInfo.underhairColor.hsvSpecular.Copy(new HsvColor(num5, num6, num7));
		female.femaleCustomInfo.underhairColor.specularIntensity = num8;
		female.femaleCustomInfo.underhairColor.specularSharpness = num9;
		float t = 1f - female.femaleCustomInfo.shapeValueBody[1];
		t = Mathf.Lerp(0.2f, 0.8f, t);
		female.femaleCustomInfo.areolaSize = t;
		float t2 = female.femaleCustomInfo.shapeValueBody[1];
		t2 = Mathf.Lerp(0.3f, 0.7f, t2);
		female.femaleCustomInfo.bustSoftness = t2;
		float t3 = female.femaleCustomInfo.shapeValueBody[1];
		t3 = Mathf.Lerp(0.3f, 0.7f, t3);
		female.femaleCustomInfo.bustWeight = t3;
	}

	public static void RandClothes(CharFemale female, bool accessory, bool megane)
	{
		CharFileInfoClothes.Accessory[] array = new CharFileInfoClothes.Accessory[10];
		for (int i = 0; i < 10; i++)
		{
			array[i] = new CharFileInfoClothes.Accessory();
		}
		int num = 0;
		int num2 = 30;
		if ((accessory && UnityEngine.Random.Range(0, 100) < num2) || megane)
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
}
