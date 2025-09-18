using System;
using System.Collections;
using TMPro;
using UnityEngine;


using Object = UnityEngine.Object;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    private TMP_Text _textbox;

    //basic typewriter functionality
    private int _currentVisableCharacterIndex;
    private Coroutine _typewriterCoroutine;
    private bool _readyForNewText = true;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctuationDelay;

    [Header("Typewriter settings")]
    [SerializeField] private float charactersPerSecond = 20;
    [SerializeField] private float interpunctuationDelay = 0.5f;

    //Skipping text functionality
    public bool CurrentlySkipping { get; private set; }
    private WaitForSeconds _skipDelay;

    [Header("Skip options")]
    [SerializeField] private bool quickSkip;
    [SerializeField][Min(1)] private int SkipSpeedup = 5;

    //Event functionality
    [Header("Event variables")]
    [SerializeField][Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f;
    public static event Action CompleteTextRevealed;
    //public static event Action<char> CharacterRevealed;

    private WaitForSeconds _textboxFullEventDelay;


    private void Awake()
    {
        _textbox = GetComponent<TMP_Text>();

        _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);

        _skipDelay = new WaitForSeconds(1 / charactersPerSecond * SkipSpeedup);
        _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
    }

    private void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText);
    }

    private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);
    }

    private void OnLeftMouseClick()
    {
        if (_textbox.maxVisibleCharacters != _textbox.textInfo.characterCount - 1)
            Skip();
    }

    public void PrepareForNewText(Object obj)
    {
        if (!_readyForNewText)
            return;
        
        _readyForNewText = false;

        //make sure theres only one coroutine at a time
        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);
        
        _textbox.maxVisibleCharacters = 0;
        _currentVisableCharacterIndex = 0;

        _typewriterCoroutine = StartCoroutine(routine: Typewriter());
    }

    private IEnumerator Typewriter()
    {
        TMP_TextInfo textInfo = _textbox.textInfo;

        while (_currentVisableCharacterIndex < textInfo.characterCount + 1)
        {
            var lastCharacterIndex = textInfo.characterCount - 1;

            if(_currentVisableCharacterIndex == lastCharacterIndex)
            {
                Debug.Log("Line complete.");
                _textbox.maxVisibleCharacters++;
                yield return _textboxFullEventDelay;
                CompleteTextRevealed?.Invoke();
                _readyForNewText = true;

                yield break;
            }

            char character = textInfo.characterInfo[_currentVisableCharacterIndex].character;

            _textbox.maxVisibleCharacters++;

            //give a longer pause after certain puncuation marks
            if(!CurrentlySkipping &&
                character == '?' || character == '.' || character == '!' || character == ',')
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return CurrentlySkipping ? _skipDelay : _simpleDelay; //another way of writing an if statement
            }

            //CharacterRevealed?.Invoke(character);
            _currentVisableCharacterIndex++;
        }
    }

    void Skip()
    {
        if (CurrentlySkipping)
            return;

        CurrentlySkipping = true;

        if (!quickSkip)
        {
            StartCoroutine(routine: SkipSpeedupReset());
            return;
        }

        //skip to show all characters now
        StopCoroutine(_typewriterCoroutine);
        _textbox.maxVisibleCharacters = _textbox.textInfo.characterCount;
        _readyForNewText = true;
        CompleteTextRevealed?.Invoke();
    }

    private IEnumerator SkipSpeedupReset()
    {
        yield return new WaitUntil(() => _textbox.maxVisibleCharacters == _textbox.textInfo.characterCount - 1);
        CurrentlySkipping = false;
    }
}
