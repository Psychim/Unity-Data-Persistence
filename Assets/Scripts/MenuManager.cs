using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class SaveData{
    public HighScoreData highScoreData;
    public string lastUser;
}
[System.Serializable]
public class HighScoreData{
    public string username;
    public int score;
}
public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public string username;
    [SerializeField] private TextMeshProUGUI usernameInput;
    [SerializeField] private TextMeshProUGUI bestscoreText;
    public HighScoreData highScoreData=new HighScoreData();
    private void Awake()
    {
        if(Instance!=null){
            Destroy(this);
            return;
        }
        Instance=this;
        DontDestroyOnLoad(gameObject);
    }
    void Start(){
        LoadData();
        if(highScoreData.username!=null){
            bestscoreText.text=$"Best Score : {highScoreData.username}";
        }
    }
    public void StartGame(){
        username=usernameInput.text;
        Debug.Log(username);
        SceneManager.LoadScene("main");
    }
    public void Exit(){
        SaveData();
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }

    public void SaveData(){
        SaveData saveData=new SaveData();
        saveData.highScoreData=highScoreData;
        saveData.lastUser=username;
        string json=JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath+"/savefile.json",json);
    }
    public void LoadData(){
        string path=Application.persistentDataPath+"/savefile.json";
        if(File.Exists(path)){
            string json=File.ReadAllText(path);
            SaveData data=JsonUtility.FromJson<SaveData>(json);
            username=data.lastUser;
            highScoreData=data.highScoreData;
        }
    }
}
