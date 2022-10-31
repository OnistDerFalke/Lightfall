using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Transform levelStartPoint;
    public static LevelGenerator instance;

    public List<LevelPieceBasic> levelPrefabs = new();
    public List<LevelPieceBasic> pieces = new();
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pieces = new List<LevelPieceBasic>();
        AddPiece();
        AddPiece();
    }

    public void RestartGenerator()
    {

        while (pieces.Count > 0)
        {
            LevelPieceBasic oldestPiece = pieces[0];
            pieces.RemoveAt(0);
            Destroy(oldestPiece.gameObject);
        }
        AddPiece();
        AddPiece();
    }

    public void AddPiece()
    {
        var randomIndex = Random.Range(0, levelPrefabs.Count);
        var piece = Instantiate(levelPrefabs[randomIndex], transform, false);
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
        if (pieces.Count < 8) return;
        LevelPieceBasic oldestPiece = pieces[0];
        pieces.RemoveAt(0);
        Destroy(oldestPiece.gameObject);
    }
    
}
