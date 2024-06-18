using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();
    [SerializeField] private Sprite BGimage;
    public List<Button> btns = new List<Button>();
    private bool firstGuess, SecondGuess;
    private int countGuesses, CountCorrectGuesses, gameGuesses;

    private string firstGuessPuzzle, secondGuessPuzzle;

    private int firstGuessIndex, secondGuessIndex;

    private void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Candy");
    }
    private void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        gameGuesses = gamePuzzles.Count / 2;
    }
    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("pButton");
        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = BGimage;

        }
    }

    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for(int i = 0; i<looper; i++)
        {
            if(index == looper / 2)
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => pickAPuzzle());
        }
    }
    public void pickAPuzzle()
    {
       if(!firstGuess)
        {
            firstGuess = true;
            //get parse of name
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }else if(!SecondGuess){
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];
            countGuesses++;
            StartCoroutine(checkIfThePuzzleMatch());
        }
    }

    IEnumerator checkIfThePuzzleMatch()
    {
        yield return new WaitForSeconds(1f);
        if(firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(0.5f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;
            btns[firstGuessIndex].image.color = new Color(0,0,0,0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);
            checkIfTheGameIsFinished();
        }else
        {
            yield return new WaitForSeconds(0.5f);

            btns[firstGuessIndex].image.sprite = BGimage;
            btns[secondGuessIndex].image.sprite = BGimage;
        }
        yield return new WaitForSeconds(0.5f);
        firstGuess = SecondGuess = false;
    }
    void checkIfTheGameIsFinished()
    {
        CountCorrectGuesses++;
        if(CountCorrectGuesses == gameGuesses)
        {
            Debug.Log("game finished");
        }
    }

    void shuffle(List<Sprite> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
