﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public bool mIsKingAlive = true;

    public GameObject mPiecePrefab;

    private List<BasePiece> mWhitePieces = null;
    private List<BasePiece> mBlackPieces = null;
    private List<BasePiece> mPromotedPieces = new List<BasePiece>();


    private string[] mPieceOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "R", "KN", "B", "Q", "K", "B", "KN", "R"
    };

    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        {"P",  typeof(Pawn)},
        {"R",  typeof(Rook)},
        {"KN", typeof(Knight)},
        {"B",  typeof(Bishop)},
        {"K",  typeof(King)},
        {"Q",  typeof(Queen)}
    };

    public void Setup(Board board)
    {
        // Create white pieces
        mWhitePieces = CreatePieces(Color.white, new Color32(80, 124, 159, 255), board);

        // Create place pieces
        mBlackPieces = CreatePieces(Color.black, new Color32(210, 95, 64, 255), board);

        // Place pieces
        PlacePieces(1, 0, mWhitePieces, board);
        PlacePieces(6, 7, mBlackPieces, board);

        // White goes first
        SwitchSides(Color.black);
    }

    private List<BasePiece> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<BasePiece> newPieces = new List<BasePiece>();

        for (int i = 0; i < mPieceOrder.Length; i++)
        {

            // Get the type, apply to new object
            string key = mPieceOrder[i];
            Type pieceType = mPieceLibrary[key];

            // Store new piece
            BasePiece newPiece = CreatePiece(pieceType);
            newPieces.Add(newPiece);

            // Setup piece
            newPiece.Setup(teamColor, spriteColor, this);
        }

        return newPieces;
    }

    private BasePiece CreatePiece(Type pieceType)
    {
        // Create new object
        GameObject newPieceObject = Instantiate(mPiecePrefab);
        newPieceObject.transform.SetParent(transform);

        // Set scale and position
        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;

        BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);

        return newPiece;
    }
    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePiece> pieces, Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            // Place pawns    
            pieces[i].Place(board.mAllCells[i, pawnRow]);

            // Place royalty
            pieces[i + 8].Place(board.mAllCells[i, royaltyRow]);
        }
    }

    private void SetInteractive(List<BasePiece> allPieces, bool value)
    {
        foreach (BasePiece piece in allPieces)
            piece.enabled = value;
    }

    public void SwitchSides(Color color)
    {
        if (!mIsKingAlive)
        {
            ResetPieces();

            mIsKingAlive = true;

            color = Color.black;
        }

        bool isBlackTurn = color == Color.white ? true : false;

        SetInteractive(mWhitePieces, !isBlackTurn);
        SetInteractive(mBlackPieces, isBlackTurn);

        //Set promoted interacvtivity
        foreach(BasePiece piece in mPromotedPieces)
        {
            bool isBlackPiece = piece.mColor != Color.white ? true : false;
            bool isPartOfTeam = isBlackPiece == true ? isBlackTurn : !isBlackTurn;

            piece.enabled = isPartOfTeam;
        }
    }

    public void ResetPieces()
    {
        foreach(BasePiece piece in mPromotedPieces)
        {
            piece.Kill();

            Destroy(piece.gameObject);
        }

        foreach (BasePiece piece in mWhitePieces)
            piece.Reset();

        foreach (BasePiece piece in mBlackPieces)
            piece.Reset();
    }
    
    public void PromotePiece(Pawn pawn, Cell cell, Color teamColor, Color spriteColor)
    {
        // Kill pawn
        pawn.Kill();

        // Create
        BasePiece promotedPiece = CreatePiece(typeof(Queen));
        promotedPiece.Setup(teamColor, spriteColor, this);

        // Place
        promotedPiece.Place(cell);

        // Add
        mPromotedPieces.Add(promotedPiece);
    }
}
