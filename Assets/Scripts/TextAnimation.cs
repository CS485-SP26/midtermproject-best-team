using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;
    public string[] stringArray;
    [SerializeField] float timeBtwnChars;
    [SerializeField] float timeBtwnWords;
    int i =0;
    void Start()
    {
        EndCheck();
    }

private IEnumerator TextVisible()
    {
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters= _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            _textMeshPro.maxVisibleCharacters = counter;

        if(counter >= totalVisibleCharacters)
            {
                i +=1;
                StopCoroutine(TextVisible());
                Invoke("EndCheck", timeBtwnWords);
            }
            counter+= 1;
            yield return new WaitForSeconds(timeBtwnChars);
        }

    }

    void EndCheck()
    {
        if(i<=stringArray.Length - 1) {

                _textMeshPro.text=stringArray[i];
                StartCoroutine(TextVisible());
    }
    }
}
