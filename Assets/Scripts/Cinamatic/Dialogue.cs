using Assets.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Dialogue : MonoBehaviour
{


    List <List<string>> Sentences = new()
    {
        new List<string>
        {

            "God_Ha Ha it works!!! Hello my friend." ,
            "PlayerA_..." ,
            "God_Now you are probably thinking to yourself what are you doing here and who are you... Well my friend before you start asking questions you must first learn to crawl." ,
            "PlayerA_..?" ,
            "God_Let me take you to the place where your adventure will begin. And so it's time to start learning."
        }

    };



    [Header("Dependencies")]
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private float DialogueSpeed;
    [SerializeField] private List<GameObject> Actors;
    [SerializeField] private GameObject actualActorCloud;
    
    //private script variables
    private bool DialogueInProgress = false;
    private int index = 0;
    private static int act = 0;
    private bool isFirstLineReaded = false;
    private bool isDuringTyping = false;

    [Header("Events")]
    public UnityEvent OnDialogueDone;
    public UnityEvent OnDialogueStart;
    // Update is called once per frame
    void Update()
    {
        if(index == 0 && DialogueInProgress && !isFirstLineReaded)
        {
            NextSentence();
            isFirstLineReaded=true;
        }else if (Input.GetKeyDown(KeyCode.Space) && DialogueInProgress && !isDuringTyping)
        {
                NextSentence();
        }
    }

    

   public void StartDialogue()
    {
        Debug.Log("Rozpoczêto dialog\n");
        DialogueInProgress = true;
        OnDialogueStart.Invoke();
    }
    public void ResumeDirector()
    {
        PlayableDirector director = GetComponent<PlayableDirector>();
        director.Resume();
    }
    public void PauseDirector()
    {
        PlayableDirector director = GetComponent<PlayableDirector>();
        director.Pause();
    }
    public void NextAct()
    {
        act++;
    }
    void NextSentence()
    {
        Debug.Log("Kolejna linia tekstu\n");
        GameObject previousActorCloud = null;
        GameObject nextActorCloud = null;
        if (index > 0)
        {
            previousActorCloud = Actors.First(a => a.name.Contains(Sentences[act][index - 1].Split("_")[0])).transform.GetChild(0).gameObject;
            previousActorCloud.SetActive(false);
        }
        if (index < Sentences[act].Count)
        {
            Debug.Log($"Sentencja nr {index}");
           nextActorCloud = Actors.First(a => a.name.Contains(Sentences[act][index].Split("_")[0])).transform.GetChild(0).gameObject;
        }

        //TODO: pierwsza sentencja nie ma dymka. jebia siê dymki animacji
        if (index < Sentences[act].Count) {
                     
            DialogueText.text = "";
            nextActorCloud.SetActive(true);
            actualActorCloud = nextActorCloud;
            if (Sentences[act][index].EndsWith("?"))
            {
                SetQuestionState(false);
            }
            else if (Sentences[act][index].EndsWith("!"))
            {
                SetLoudState(false);
            }
            else if (Sentences[act][index].EndsWith("."))
            {
                SetNormalState(false);
            }

            StartCoroutine(WriteSentence());
        }
        else
        {
            DialogueText.text = "";
            OnDialogueDone.Invoke();
            DialogueInProgress = false;
            if(previousActorCloud is not null && nextActorCloud is not null)
            {
                previousActorCloud.SetActive(false);
                nextActorCloud.SetActive(false);
            }

        }
    }

    public void SetNormalState(bool isOneFrame)
    {
        if(isOneFrame)
        {
            actualActorCloud.GetComponent<Animator>().SetTrigger("OneFrameNormal");
        }
        else
        {
            actualActorCloud.GetComponent<Animator>().SetTrigger("Normal");

        }
    }

    public void SetQuestionState(bool isOneFrame)
    {
        if(isOneFrame)
        {
            actualActorCloud.GetComponent<Animator>().SetTrigger("OneFrameQuestion");
        }
        else
        {

            actualActorCloud.GetComponent<Animator>().SetTrigger("Question");
        }
    }

    public void SetLoudState(bool isOneFrame)
    {
        if(isOneFrame)
        {
            actualActorCloud.GetComponent<Animator>().SetTrigger("OneFrameLoud");
        }
        else
        {
            actualActorCloud.GetComponent<Animator>().SetTrigger("Loud");

        }
    }



    IEnumerator WriteSentence()
    {
        Debug.Log("Wypisujê linijkê\n");
        isDuringTyping = true;
        foreach (char c in Sentences[act][index].Split("_")[1].ToCharArray())
        {
            DialogueText.text += c;
            yield return new WaitForSeconds(DialogueSpeed);
            if(Input.GetKeyDown(KeyCode.Space)) {
                DialogueText.text = Sentences[act][index].Split("_")[1];
                break;
            }
        }
        DialogueText.text += "\n\nPress Spacebar to continue";
        index++;
        isDuringTyping = false;
    }
}
