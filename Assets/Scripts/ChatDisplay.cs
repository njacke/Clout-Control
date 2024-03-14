using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Data.Common;

public class ChatDisplay : MonoBehaviour
{
    float genreAffinityBreakpoint = 0.5f;

    // TMPUGUI fields
    [SerializeField] TextMeshProUGUI name1;
    [SerializeField] TextMeshProUGUI name2;
    [SerializeField] TextMeshProUGUI name3;
    [SerializeField] TextMeshProUGUI name4;
    [SerializeField] TextMeshProUGUI name5;
    [SerializeField] TextMeshProUGUI name6;
    [SerializeField] TextMeshProUGUI name7;
    [SerializeField] TextMeshProUGUI name8;
    [SerializeField] TextMeshProUGUI name9;
    [SerializeField] TextMeshProUGUI name10;
    [SerializeField] TextMeshProUGUI name11;
    [SerializeField] TextMeshProUGUI name12;
    [SerializeField] TextMeshProUGUI name13;
    [SerializeField] TextMeshProUGUI name14;
    [SerializeField] TextMeshProUGUI name15;
    [SerializeField] TextMeshProUGUI name16;
    [SerializeField] TextMeshProUGUI name17;
    [SerializeField] TextMeshProUGUI name18;
    [SerializeField] TextMeshProUGUI name19;
    [SerializeField] TextMeshProUGUI name20;
    [SerializeField] TextMeshProUGUI name21;
    [SerializeField] TextMeshProUGUI name22;
    [SerializeField] TextMeshProUGUI name23;
    [SerializeField] TextMeshProUGUI name24;
    [SerializeField] TextMeshProUGUI name25;
    [SerializeField] TextMeshProUGUI message1;
    [SerializeField] TextMeshProUGUI message2;
    [SerializeField] TextMeshProUGUI message3;
    [SerializeField] TextMeshProUGUI message4;
    [SerializeField] TextMeshProUGUI message5;
    [SerializeField] TextMeshProUGUI message6;
    [SerializeField] TextMeshProUGUI message7;
    [SerializeField] TextMeshProUGUI message8;
    [SerializeField] TextMeshProUGUI message9;
    [SerializeField] TextMeshProUGUI message10;
    [SerializeField] TextMeshProUGUI message11;
    [SerializeField] TextMeshProUGUI message12;
    [SerializeField] TextMeshProUGUI message13;
    [SerializeField] TextMeshProUGUI message14;
    [SerializeField] TextMeshProUGUI message15;
    [SerializeField] TextMeshProUGUI message16;
    [SerializeField] TextMeshProUGUI message17;
    [SerializeField] TextMeshProUGUI message18;
    [SerializeField] TextMeshProUGUI message19;
    [SerializeField] TextMeshProUGUI message20;
    [SerializeField] TextMeshProUGUI message21;
    [SerializeField] TextMeshProUGUI message22;
    [SerializeField] TextMeshProUGUI message23;
    [SerializeField] TextMeshProUGUI message24;
    [SerializeField] TextMeshProUGUI message25;

    List<TextMeshProUGUI> namesList;
    List<TextMeshProUGUI> messagesList;

    public enum MessageType{
        General,
        NewSub,
        NewDonation,
        SocialAction,
        GameChange,
        GamePlay,
        CameraSizeChange,
    }

    private List<string> generalPosMessages = new List<string>(){
        "CatJam",
        "GOOD STREAM",
        "we vibin'",
        "how u doin bb?",
        "luv ur stream",
        "what we up to?",
        "HI MUM",
        "sup chat",
        "<3",
        "i'm #1 fan"
    };

    private List<string> generalNegMessages = new List<string>(){
        "omg you suck!",
        "get a job",
        "e-girl LMAO",
        "boooring",
        "ResidentSleeper",
        "simps only",
        "pls end stream",
        "BOOOOO",
        "seen better",
        "down bad"
    };

    private List<string> newSubMessages = new List<string>(){
        "i just subbed",
        "have my sub",
        "sub def worth",
        "SUB ARMY",
        "subchain inc"
    };

    private List<string> newDonationMessages = new List<string>(){
        "glad I fund you!",
        "take my money",
        "buy smth nice",
        "child support",
        "cha-ching"
    };

    private List<string> SAFlirtMessages = new List<string>(){
        "lookin' good",
        "nice nice",
        "boobas",
        "for me?",
        "MWAAA"
    };

    private List<string> SAGiggleMessages = new List<string>(){
        "KEKW",
        "sooo funny",
        "cutieeee",
        "nice one",
        "fun at parties"
    };

    private List<string> SAHypeMessages = new List<string>(){
        "woop woop",
        "let's gooo",
        "PogChamp",
        "so exciting",
        "hypetrain!!!"
    };

    private List<string> SARageMessages = new List<string>(){
        "ARRRRRR",
        "calma calma",
        "REEEEEE",
        "u mad bra?",
        "OVER 9000"
    };

    private List<string> gameChangePosMessages = new List<string>(){
        "luv dis game",
        "PepeGamer",
        "we playin'",
        "finish today?",
        "new game?"
    };

    private List<string> gameChangeNegMessages = new List<string>(){
        "why dis game",
        "omg not this",
        "bad game",
        "this again?",
        "IQ10 game lul"
    };

    private List<string> gamePlayPosMessages = new List<string>(){
        "doing great",
        "sooo good",
        "nice progress",
        "I can help!",
        "this has MP?"        
    };

    private List<string> gamePlayNegMessages = new List<string>(){
        "sooo bad",
        "l2p lol",
        "skill issue",
        "best gamer EU",
        "backseat gaming"
    };

    private List<string> gamePlayNoneMessages = new List<string>(){
        "nice 'gameplay'",
        "play smth...",
        "WEN GAME",
        "waiting room",
        "where n00ds"
    };

    // only used when changed to large camera
    private List<string> CameraSizeChangeMessages = new List<string>(){
        "Kreygasm",
        "mmmmm hehe",
        "HD 'gameplay'",
        "#goals",
        "bigger better"
    };

    void Start(){
        namesList = new List<TextMeshProUGUI>(){
            name1, name2, name3, name4, name5,
            name6, name7, name8, name9, name10,
            name11, name12, name13, name14, name15,
            name16, name17, name18, name19, name20,
            name21, name22, name23, name24, name25,
        };

        messagesList = new List<TextMeshProUGUI>(){
            message1, message2, message3, message4, message5,
            message6, message7, message8, message9, message10,
            message11, message12, message13, message14, message15,
            message16, message17, message18, message19, message20,
            message21, message22, message23, message24, message25,
        };

        foreach (TextMeshProUGUI name in namesList){
            name.text = "";            
        }

        foreach (TextMeshProUGUI message in messagesList){
            message.text = "";            
        }        
    }

    public void UpdateChatDisplay(Viewer viewer, MessageType messageType){
        for (int i = 0; i < namesList.Count - 1; i++){
            namesList[i].text = namesList[i+1].text;
        }

        for (int i = 0; i < messagesList.Count - 1; i++){
            messagesList[i].text = messagesList[i+1].text;
        }

        namesList[namesList.Count -1].text = viewer.Name + ":";
        messagesList[messagesList.Count -1].text = GenerateMessage(viewer, messageType);
    }

    private string GetRandomMsgFromList(List<string> list){
        int randomIndex = Random.Range(0, list.Count);
        string message = list[randomIndex];
        return message;
    }

    private string GenerateMessage(Viewer viewer, MessageType messageType){        
        var message = "";

        var currentGenre = GameManager.Instance.GetCurrentGameGenre();

        var gameGenreAffinity = GetViewerGenreAffinity(currentGenre, viewer);

        switch(messageType){
            case MessageType.General:
                if(viewer.StreamSatisfaction > 0){
                    message = GetRandomMsgFromList(generalPosMessages);
                }
                else{
                    message = GetRandomMsgFromList(generalNegMessages);
                }
                break;

            case MessageType.NewSub:
                message = GetRandomMsgFromList(newSubMessages);
                break;

            case MessageType.NewDonation:
                message = GetRandomMsgFromList(newDonationMessages);
                break;

            case MessageType.SocialAction:
                switch(GameManager.Instance.GetCurrentSocialAction()){
                    case GameManager.SocialActions.Flirt:
                        message = GetRandomMsgFromList(SAFlirtMessages);
                        break;
                    case GameManager.SocialActions.Giggle:
                        message = GetRandomMsgFromList(SAGiggleMessages);
                        break;
                    case GameManager.SocialActions.Hype:
                        message = GetRandomMsgFromList(SAHypeMessages);
                        break;
                    case GameManager.SocialActions.Rage:
                        message = GetRandomMsgFromList(SARageMessages);
                        break;
                    default:
                        break;
                }
                break;

            case MessageType.GameChange:
                
                if (currentGenre != GameManager.GameGenres.None){
                    if(gameGenreAffinity > genreAffinityBreakpoint ){
                        message = GetRandomMsgFromList(gameChangePosMessages);
                    }
                    else{
                        message = GetRandomMsgFromList(gameChangeNegMessages);
                    }
                }
                break;

            case MessageType.GamePlay:
                
                if (currentGenre != GameManager.GameGenres.None){
                    if(gameGenreAffinity > genreAffinityBreakpoint ){
                        message = GetRandomMsgFromList(gamePlayPosMessages);
                        Debug.Log(viewer.Name + "'s game genre affinity is " + gameGenreAffinity);
                    }
                    else{
                        message = GetRandomMsgFromList(gamePlayNegMessages);
                        Debug.Log(viewer.Name + "'s game genre affinity is " + gameGenreAffinity);
                    }
                }
                else{
                    message = GetRandomMsgFromList(gamePlayNoneMessages);
                }
                break;

            case MessageType.CameraSizeChange:
                message = GetRandomMsgFromList(CameraSizeChangeMessages);  
                break;

            default:
                break;
        }

        return message;
    }

    private float GetViewerGenreAffinity(GameManager.GameGenres currentGenre, Viewer viewer){
        float gameGenreAffinity = 0f;

        switch (currentGenre){
            case GameManager.GameGenres.RPG:
                gameGenreAffinity = viewer.AffinityForRPG;
                break;
            case GameManager.GameGenres.Arcade:
                gameGenreAffinity = viewer.AffinityForArcade;
                break;
            case GameManager.GameGenres.Action:
                gameGenreAffinity = viewer.AffinityForAction;
                break;
            case GameManager.GameGenres.Simulation:
                gameGenreAffinity = viewer.AffinityForSimulation;
                break;

            default:
                break;
        }

        return gameGenreAffinity;
    }
}
