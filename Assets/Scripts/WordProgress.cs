using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WordProgress
{
    public string currentWord;
    public bool[] filledLetters;
    public List<int> filledIndices = new List<int>();
    
    public void SetWord(string word)
    {
        currentWord = word.ToUpper();
        filledLetters = new bool[currentWord.Length];
        filledIndices.Clear();
    }
    
    public bool IsLetterNeeded(char letter)
    {
        letter = char.ToUpper(letter);
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (currentWord[i] == letter && !filledLetters[i])
            {
                return true;
            }
        }
        return false;
    }
    
    public bool FillLetter(char letter)
    {
        letter = char.ToUpper(letter);
        bool filled = false;
        
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (currentWord[i] == letter && !filledLetters[i])
            {
                filledLetters[i] = true;
                filledIndices.Add(i);
                filled = true;
            }
        }
        
        return filled;
    }
    
    public void RemoveLastFilledLetter()
    {
        if (filledIndices.Count > 0)
        {
            int lastIndex = filledIndices[filledIndices.Count - 1];
            filledLetters[lastIndex] = false;
            filledIndices.RemoveAt(filledIndices.Count - 1);
        }
    }
    
    public void RemoveRandomFilledLetter()
    {
        if (filledIndices.Count > 0)
        {
            // Pick a random filled letter
            int randomIndexInList = Random.Range(0, filledIndices.Count);
            int letterIndex = filledIndices[randomIndexInList];
            
            // Remove it
            filledLetters[letterIndex] = false;
            filledIndices.RemoveAt(randomIndexInList);
        }
    }
    
    public bool IsComplete()
    {
        foreach (bool filled in filledLetters)
        {
            if (!filled) return false;
        }
        return true;
    }
    
    public int GetProgress()
    {
        return filledIndices.Count;
    }
    
    public string GetDisplayString()
    {
        string display = "";
        for (int i = 0; i < currentWord.Length; i++)
        {
            if (filledLetters[i])
            {
                display += currentWord[i];
            }
            else
            {
                display += "_";
            }
            display += " ";
        }
        return display.Trim();
    }
}
