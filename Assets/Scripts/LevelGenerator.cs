using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelPieceWithProbability
{
    public LevelPieceBasic levelpiece;
    public float probability = 0.0f;
}

public class LevelGenerator : MonoBehaviour
{
    public Transform levelStartPoint;
    public static LevelGenerator instance;
    public int piecesNumber;
    
    public List<LevelPieceWithProbability> levelPrefabsWithProbability = new();
    public List<LevelPieceBasic> huesPrefabs = new();
    public List<LevelPieceBasic> endGamePrefabs = new();
    public List<LevelPieceBasic> startGamePrefabs = new();
    public List<LevelPieceBasic> pieces = new();

    private int piecesCounter = 0;
    int[] huesCombination = { 0, 1, 2 };
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pieces = new List<LevelPieceBasic>();
        ResetHuesCombination();
        AddPiece();
        AddPiece();
    }

    private void ResetHuesCombination()
    {
        for (var i = huesCombination.Length - 1; i > 0; i--)
        {
            var ri = Random.Range(0, i + 1);
            (huesCombination[i], huesCombination[ri]) = (huesCombination[ri], huesCombination[i]);
        }
    }

    private bool IsHueTaken(int num)
    {
        return num switch
        {
            0 => GameManager.instance.hasRedHue,
            1 => GameManager.instance.hasGreenHue,
            2 => GameManager.instance.hasBlueHue,
            _ => false
        };
    }

    public void RestartGenerator()
    {
        piecesCounter = 0;
        ResetHuesCombination();
        while (pieces.Count > 0)
        {
            var oldestPiece = pieces[0];
            pieces.RemoveAt(0);
            Destroy(oldestPiece.gameObject);
        }
        AddPiece();
        AddPiece();
    }

    public void AddPiece()
    {
        LevelPieceBasic piece;
        
        if (piecesCounter == piecesNumber - 1)
            piece = Instantiate(endGamePrefabs[Random.Range(0, endGamePrefabs.Count)], transform, false);
        else if(piecesCounter == 0)
            piece = Instantiate(startGamePrefabs[Random.Range(0, startGamePrefabs.Count)], transform, false);
        else if (piecesCounter >= piecesNumber) return;
        else if (piecesCounter == piecesNumber / 4 && !IsHueTaken(huesCombination[0]))
            piece = Instantiate(huesPrefabs[huesCombination[0]], transform, false);
        else if(piecesCounter == piecesNumber / 2 && !IsHueTaken(huesCombination[1]))
            piece = Instantiate(huesPrefabs[huesCombination[1]], transform, false);
        else if (piecesCounter == piecesNumber * 3 / 4 && !IsHueTaken(huesCombination[2]))
            piece = Instantiate(huesPrefabs[huesCombination[2]], transform, false);
        else
        {
            var random = Random.Range(0.0f, 1.0f);
            var randomIndex = levelPrefabsWithProbability.Count - 1;
            for(var i=0; i<levelPrefabsWithProbability.Count - 1; i++)
                if (random < levelPrefabsWithProbability[i].probability)
                {
                    randomIndex = i;
                    break;
                }
            piece = Instantiate(levelPrefabsWithProbability[randomIndex].levelpiece, transform, false);
        }

        piecesCounter++;
        if (pieces.Count == 0)
            piece.transform.position = new Vector2(
                levelStartPoint.position.x - piece.startPoint.localPosition.x,
                levelStartPoint.position.y - piece.startPoint.localPosition.y);
        else piece.transform.position = new Vector2(
            pieces[^1].exitPoint.position.x - piece.startPoint.localPosition.x,
            pieces[^1].exitPoint.position.y - piece.startPoint.localPosition.y);
        pieces.Add(piece);
    }

    public void RemoveOldestPiece()
    {
        if (pieces.Count < 3) return;
        var oldestPiece = pieces[0];
        pieces.RemoveAt(0);
        Destroy(oldestPiece.gameObject);
    }
    
}
