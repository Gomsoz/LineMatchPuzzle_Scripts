using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;

public class JsonManager
{
    public void SaveScore(int level, int score)
    {
        JObject saveData = new JObject();

        string dataLevel = $"Level_{level}";

        saveData[dataLevel] = score;

        // 파일로 저장 
        string savestring = JsonConvert.SerializeObject(saveData, Formatting.Indented); 
        // JObject를 Serialize하여 json string 생성 
        File.WriteAllText("Assets/Resources/Data/ScoreData.json", savestring); // 생성된 string을 파일에 쓴다 }

        //출처: https://blog.komastar.kr/232 [World of Komastar]
    }

    public int LoadScore(int level)
    {

        string loadString = File.ReadAllText("Assets/Resources/Data/ScoreData.json");
        JObject loadData = JObject.Parse(loadString);

        int score = 0;
        string dataLevel = $"Level_{level}";

        if (loadData[dataLevel] == null)
            return 0;

        score = (int)loadData[dataLevel];

        return score;
    }

    public StageLevelData LoadStageLevelData(int level)
    {
        Debug.Log("Load Data... : Start Load the Data");
        string loadString = File.ReadAllText("Assets/Resources/Data/StageLevelData.json");
        JObject loaddata = JObject.Parse(loadString); // JObject 파싱 

        string targetLevel = $"Level_{level}";

        StageLevelData levelData = new StageLevelData();
        levelData.numOfBlockColors = (int)loaddata[targetLevel]["numOfBlockColors"];
        levelData.numOfNoneBlocks = (int)loaddata[targetLevel]["numOfNoneBlocks"];
        levelData.numOfGoalLines = (int)loaddata[targetLevel]["numOfGoalLines"];
        levelData.boardSize = (int)loaddata[targetLevel]["boardSize"];

        JArray loadarray = (JArray)loaddata[targetLevel]["boardMap"];
        levelData.boardMap = new int[levelData.boardSize + 1, levelData.boardSize + 1];

        int dataCnt = 0;
        for (int i = 1; i < levelData.boardSize + 1; i++)
        {
            for(int j = 1; j < levelData.boardSize + 1; j++)
            {
                levelData.boardMap[i,j] = (int)loadarray[dataCnt];
                dataCnt++;
            }
        }

        Debug.Log("Load Data... : Finish Load the Data");
        return levelData;

    }
        //출처: https://blog.komastar.kr/232 [World of Komastar]
}
