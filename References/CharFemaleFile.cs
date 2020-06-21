public class CharFemaleFile : CharFile
{
	public CharFileInfoCustomFemale femaleCustomInfo
	{
		get;
		private set;
	}

	public CharFileInfoClothesFemale femaleClothesInfo
	{
		get;
		private set;
	}

	public CharFileInfoCoordinateFemale femaleCoordinateInfo
	{
		get;
		private set;
	}

	public CharFileInfoStatusFemale femaleStatusInfo
	{
		get;
		private set;
	}

	public CharFileInfoParameterFemale femaleParameterInfo
	{
		get;
		private set;
	}

	public CharFemaleFile()
		: base("【HoneySelectCharaFemale】", "chara/female/")
	{
		base.customInfo = (femaleCustomInfo = new CharFileInfoCustomFemale());
		base.clothesInfo = (femaleClothesInfo = new CharFileInfoClothesFemale());
		base.coordinateInfo = (femaleCoordinateInfo = new CharFileInfoCoordinateFemale());
		base.statusInfo = (femaleStatusInfo = new CharFileInfoStatusFemale());
		base.parameterInfo = (femaleParameterInfo = new CharFileInfoParameterFemale());
	}

	public override bool LoadFromSBPR(string path)
	{
		CharSave_SexyBeachPR charSave_SexyBeachPR = new CharSave_SexyBeachPR(CharSave_SexyBeachPR.ConvertType.HoneySelect);
		if (!charSave_SexyBeachPR.LoadCharaFile(path))
		{
			return false;
		}
		CharSave_SexyBeachPR.SaveData savedata = charSave_SexyBeachPR.savedata;
		if (savedata.sex == 0)
		{
			Debug.LogWarning("キャラファイルの性別が違います");
			return false;
		}
		charaFileName = savedata.fileName;
		charaFilePNG = savedata.pngData;
		femaleCustomInfo.personality = savedata.personality;
		femaleCustomInfo.name = savedata.name;
		femaleCustomInfo.headId = savedata.headId;
		for (int i = 0; i < savedata.shapeBody.Length; i++)
		{
			femaleCustomInfo.shapeValueBody[i] = CharFile.ClampEx(savedata.shapeBody[i], 0f, 1f);
		}
		for (int j = 0; j < savedata.shapeFace.Length; j++)
		{
			femaleCustomInfo.shapeValueFace[j] = CharFile.ClampEx(savedata.shapeFace[j], 0f, 1f);
		}
		for (int k = 0; k < femaleCustomInfo.hairId.Length; k++)
		{
			femaleCustomInfo.hairId[k] = savedata.hairId[k];
			savedata.hairColor[k].CopyToHoneySelect(femaleCustomInfo.hairColor[k]);
			savedata.hairAcsColor[k].CopyToHoneySelect(femaleCustomInfo.hairAcsColor[k]);
		}
		femaleCustomInfo.hairType = savedata.hairType;
		femaleCustomInfo.texFaceId = savedata.texFaceId;
		savedata.skinColor.CopyToHoneySelect(femaleCustomInfo.skinColor);
		femaleCustomInfo.texEyeshadowId = savedata.texEyeshadowId;
		savedata.eyeshadowColor.CopyToHoneySelect(femaleCustomInfo.eyeshadowColor);
		femaleCustomInfo.texCheekId = savedata.texCheekId;
		savedata.cheekColor.CopyToHoneySelect(femaleCustomInfo.cheekColor);
		femaleCustomInfo.texLipId = savedata.texLipId;
		savedata.lipColor.CopyToHoneySelect(femaleCustomInfo.lipColor);
		femaleCustomInfo.texTattoo_fId = savedata.texTattoo_fId;
		savedata.tattoo_fColor.CopyToHoneySelect(femaleCustomInfo.tattoo_fColor);
		femaleCustomInfo.texMoleId = savedata.texMoleId;
		savedata.moleColor.CopyToHoneySelect(femaleCustomInfo.moleColor);
		femaleCustomInfo.matEyebrowId = savedata.matEyebrowId;
		savedata.eyebrowColor.CopyToHoneySelect(femaleCustomInfo.eyebrowColor);
		femaleCustomInfo.matEyelashesId = savedata.matEyelashesId;
		savedata.eyelashesColor.CopyToHoneySelect(femaleCustomInfo.eyelashesColor);
		femaleCustomInfo.matEyeLId = savedata.matEyeLId;
		savedata.eyeLColor.CopyToHoneySelect(femaleCustomInfo.eyeLColor);
		femaleCustomInfo.matEyeRId = savedata.matEyeRId;
		savedata.eyeRColor.CopyToHoneySelect(femaleCustomInfo.eyeRColor);
		femaleCustomInfo.matEyeHiId = savedata.matEyeHiId;
		savedata.eyeHiColor.CopyToHoneySelect(femaleCustomInfo.eyeHiColor);
		savedata.eyeWColor.CopyToHoneySelect(femaleCustomInfo.eyeWColor);
		femaleCustomInfo.texFaceDetailId = 0;
		femaleCustomInfo.faceDetailWeight = 0.5f;
		femaleCustomInfo.texBodyId = 0;
		femaleCustomInfo.texSunburnId = savedata.texSunburnId;
		savedata.sunburnColor.CopyToHoneySelect(femaleCustomInfo.sunburnColor);
		femaleCustomInfo.texTattoo_bId = savedata.texTattoo_bId;
		savedata.tattoo_bColor.CopyToHoneySelect(femaleCustomInfo.tattoo_bColor);
		femaleCustomInfo.matNipId = savedata.matNipId;
		savedata.nipColor.CopyToHoneySelect(femaleCustomInfo.nipColor);
		femaleCustomInfo.matUnderhairId = savedata.matUnderhairId;
		savedata.underhairColor.CopyToHoneySelect(femaleCustomInfo.underhairColor);
		savedata.nailColor.CopyToHoneySelect(femaleCustomInfo.nailColor);
		femaleCustomInfo.areolaSize = savedata.nipSize;
		femaleCustomInfo.texBodyDetailId = savedata.texBodyId + 1;
		femaleCustomInfo.bodyDetailWeight = savedata.bodyDetailWeight;
		femaleCustomInfo.bustSoftness = savedata.bustSoftness;
		femaleCustomInfo.bustWeight = savedata.bustWeight;
		CharFileInfoClothesFemale charFileInfoClothesFemale = new CharFileInfoClothesFemale();
		charFileInfoClothesFemale.swimType = false;
		charFileInfoClothesFemale.clothesId[0] = savedata.coord[0].clothesTopId;
		savedata.coord[0].clothesTopColor.CopyToHoneySelect(charFileInfoClothesFemale.clothesColor[0]);
		charFileInfoClothesFemale.clothesId[1] = savedata.coord[0].clothesBotId;
		savedata.coord[0].clothesBotColor.CopyToHoneySelect(charFileInfoClothesFemale.clothesColor[1]);
		charFileInfoClothesFemale.clothesId[2] = savedata.coord[0].braId;
		savedata.coord[0].braColor.CopyToHoneySelect(charFileInfoClothesFemale.clothesColor[2]);
		charFileInfoClothesFemale.clothesId[3] = savedata.coord[0].shortsId;
		savedata.coord[0].shortsColor.CopyToHoneySelect(charFileInfoClothesFemale.clothesColor[3]);
		charFileInfoClothesFemale.clothesId[7] = savedata.coord[0].glovesId;
		savedata.coord[0].glovesColor.CopyToHoneySelect(charFileInfoClothesFemale.clothesColor[7]);
		charFileInfoClothesFemale.clothesId[8] = savedata.coord[0].panstId;
		savedata.coord[0].panstColor.CopyToHoneySelect(charFileInfoClothesFemale.clothesColor[8]);
		charFileInfoClothesFemale.clothesId[9] = savedata.coord[0].socksId;
		savedata.coord[0].socksColor.CopyToHoneySelect(charFileInfoClothesFemale.clothesColor[9]);
		charFileInfoClothesFemale.clothesId[10] = savedata.coord[0].shoesId;
		savedata.coord[0].shoesColor.CopyToHoneySelect(charFileInfoClothesFemale.clothesColor[10]);
		for (int l = 0; l < 5; l++)
		{
			charFileInfoClothesFemale.accessory[l].type = savedata.accessory[0, l].accessoryType;
			charFileInfoClothesFemale.accessory[l].id = savedata.accessory[0, l].accessoryId;
			charFileInfoClothesFemale.accessory[l].parentKey = savedata.accessory[0, l].parentKey;
			charFileInfoClothesFemale.accessory[l].addPos = savedata.accessory[0, l].plusPos;
			charFileInfoClothesFemale.accessory[l].addRot = savedata.accessory[0, l].plusRot;
			charFileInfoClothesFemale.accessory[l].addScl = savedata.accessory[0, l].plusScl;
			savedata.accessoryColor[0, l].CopyToHoneySelect(charFileInfoClothesFemale.accessory[l].color);
		}
		femaleCoordinateInfo.SetInfo(CharDefine.CoordinateType.type00, charFileInfoClothesFemale);
		CharFileInfoClothesFemale charFileInfoClothesFemale2 = new CharFileInfoClothesFemale();
		charFileInfoClothesFemale2.swimType = true;
		charFileInfoClothesFemale2.clothesId[4] = savedata.coord[1].swimsuitId;
		savedata.coord[1].swimsuitColor.CopyToHoneySelect(charFileInfoClothesFemale2.clothesColor[4]);
		charFileInfoClothesFemale2.clothesId[5] = savedata.coord[1].swimTopId;
		savedata.coord[1].swimTopColor.CopyToHoneySelect(charFileInfoClothesFemale2.clothesColor[5]);
		charFileInfoClothesFemale2.clothesId[6] = savedata.coord[1].swimBotId;
		savedata.coord[1].swimBotColor.CopyToHoneySelect(charFileInfoClothesFemale2.clothesColor[6]);
		charFileInfoClothesFemale2.clothesId[7] = savedata.coord[1].glovesId;
		savedata.coord[1].glovesColor.CopyToHoneySelect(charFileInfoClothesFemale2.clothesColor[7]);
		charFileInfoClothesFemale2.clothesId[8] = savedata.coord[1].panstId;
		savedata.coord[1].panstColor.CopyToHoneySelect(charFileInfoClothesFemale2.clothesColor[8]);
		charFileInfoClothesFemale2.clothesId[9] = savedata.coord[1].socksId;
		savedata.coord[1].socksColor.CopyToHoneySelect(charFileInfoClothesFemale2.clothesColor[9]);
		charFileInfoClothesFemale2.clothesId[10] = savedata.coord[1].shoesId;
		savedata.coord[1].shoesColor.CopyToHoneySelect(charFileInfoClothesFemale2.clothesColor[10]);
		charFileInfoClothesFemale2.hideSwimOptTop = ((savedata.stateSwimOptTop != 0) ? true : false);
		charFileInfoClothesFemale2.hideSwimOptBot = ((savedata.stateSwimOptBot != 0) ? true : false);
		for (int m = 0; m < 5; m++)
		{
			charFileInfoClothesFemale2.accessory[m].type = savedata.accessory[1, m].accessoryType;
			charFileInfoClothesFemale2.accessory[m].id = savedata.accessory[1, m].accessoryId;
			charFileInfoClothesFemale2.accessory[m].parentKey = savedata.accessory[1, m].parentKey;
			charFileInfoClothesFemale2.accessory[m].addPos = savedata.accessory[1, m].plusPos;
			charFileInfoClothesFemale2.accessory[m].addRot = savedata.accessory[1, m].plusRot;
			charFileInfoClothesFemale2.accessory[m].addScl = savedata.accessory[1, m].plusScl;
			savedata.accessoryColor[1, m].CopyToHoneySelect(charFileInfoClothesFemale2.accessory[m].color);
		}
		femaleCoordinateInfo.SetInfo(CharDefine.CoordinateType.type02, charFileInfoClothesFemale2);
		ChangeCoordinateType(CharDefine.CoordinateType.type00);
		return true;
	}
}
