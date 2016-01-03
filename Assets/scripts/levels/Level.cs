﻿using UnityEngine;
using System.Collections.Generic;

using SimpleJSON;

// Объект уровня для сериализации
public class Level
{
	public int Number;
	public List< List<PlaneProperties> > Planes;
	public float CameraRotationSpeed;
    public string CameraRotationScript;
	public float FallingSpeed;
	
	public bool LoadLevel(TextAsset file)
	{
		string jsonStr = file.text;
		
		var parsedJson = JSON.Parse(jsonStr);
		
		Number = parsedJson["Number"].AsInt;
        // Скрипт вращения камеры
        if (parsedJson["CameraRotationScriptName"] == null)
            CameraRotationScript = "UniformCameraRotation";
        else 
            CameraRotationScript = parsedJson["CameraRotationScriptName"];
        // Параметры скрипта вращения камеры
		if (parsedJson["CameraRotationSpeed"] == null)
            CameraRotationSpeed = 0f;
        else 
            CameraRotationSpeed = parsedJson["CameraRotationSpeed"].AsFloat;
                
		FallingSpeed = parsedJson["FallingSpeed"].AsFloat;
		
		Planes = new List< List<PlaneProperties> >();
		var planesArray = parsedJson["Planes"].AsArray;
        int textures_count = 0;
		
        for (int i = 0; i < planesArray.Count; ++i)
		{
			Planes.Add(new List<PlaneProperties>());
			var propsArray = planesArray[i].AsArray;
		  
			for (int j = 0; j < propsArray.Count; ++j)
			{
				var currentProp = new PlaneProperties();
                currentProp.TexturePath = "levels/" + Number.ToString() + "/planes/";
                // Если номер текстуры не указан
				if (propsArray[j]["TexturePath"] == null)
                {
                    currentProp.TexturePath += (++textures_count).ToString();
                    Debug.Log(currentProp.TexturePath);
                }
                else 
                {
                    currentProp.TexturePath += propsArray[j]["TexturePath"];
                    if (!propsArray[j]["TexturePath"].ToString().Contains("deco"))
                    {
                        textures_count++;
                    }
                }
                // Параметры спавна
                if (propsArray[j]["SpawnRange"] == null)
                {
                    currentProp.SpawnMinimum = Vector3.zero;
                    currentProp.SpawnMaximum = Vector3.zero;
                }
                else 
                {
                    var spawnRangeArray = propsArray[j]["SpawnRange"].AsArray;
                    currentProp.SpawnMinimum = new Vector3(spawnRangeArray[0].AsFloat, spawnRangeArray[2].AsFloat, 
                                                           spawnRangeArray[4].AsFloat);
                    currentProp.SpawnMinimum = new Vector3(spawnRangeArray[1].AsFloat, spawnRangeArray[3].AsFloat, 
                                                           spawnRangeArray[5].AsFloat);
                }
                // Скрипт движения
                if (propsArray[j]["MovementScriptName"] == null)
                    currentProp.MovementScriptName = "Idle";
                else 
                    currentProp.MovementScriptName = propsArray[j]["MovementScriptName"];
                // Параметры скрипта движения
                if (propsArray[j]["MovementSpeed"] == null)
                {
                    currentProp.MovementSpeed = Vector3.zero;
                }
                else 
                {
                    var movementSpeedArray = propsArray[j]["MovementSpeed"].AsArray;
                    currentProp.MovementSpeed = new Vector3(movementSpeedArray[0].AsFloat, movementSpeedArray[1].AsFloat, 
                                                           movementSpeedArray[2].AsFloat);
                }
                if (propsArray[j]["RotationSpeed"] == null)
                    currentProp.RotationSpeed = 0.0f;
                else 
                    currentProp.RotationSpeed = propsArray[j]["RotationSpeed"].AsFloat;
                    
				Planes[i].Add(currentProp);
			}
		}
		return true;
	}
}
