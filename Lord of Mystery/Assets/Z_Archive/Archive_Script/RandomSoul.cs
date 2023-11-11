using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSoul : MonoBehaviour
{

    public SpriteRenderer FieldSpriteRenderer;
    public GameObject soulPieceRoot;
    public GameObject soulPiece;

    public float GenerationOffset;

    private void Start()
    {
        if (FieldSpriteRenderer == null)
        {
            FieldSpriteRenderer = GameObject.Find("Field_Boarder").GetComponent<SpriteRenderer>();
        }
        
        soulPieceRoot = GameObject.Find("SoulPieceRoot");

        StartCoroutine(Generate());
    }
    


    void GenerateSoul()
    {
        GameObject soulpiece = Instantiate(soulPiece);
        
        // randomize the location of soul and limit within the panel
        soulpiece.transform.localPosition =
            new Vector3
            (
                Random.Range(
                    FieldSpriteRenderer.transform.position.x - FieldSpriteRenderer.bounds.size.x / 2 - GenerationOffset,
                    FieldSpriteRenderer.transform.position.x + FieldSpriteRenderer.bounds.size.x / 2 +
                    GenerationOffset),
                Random.Range(
                    FieldSpriteRenderer.transform.position.y - FieldSpriteRenderer.bounds.size.y / 2 - GenerationOffset,
                    FieldSpriteRenderer.transform.position.y + FieldSpriteRenderer.bounds.size.y / 2 +
                    GenerationOffset),
                soulpiece.transform.position.z
            );
        
        /*// popup effect (scale)
        Transform soultransform = soulpiece.transform;
        soulpiece.transform.localScale =
            new Vector3
            (
                soulpiece.transform.localScale.x * 0.3f,
                soulpiece.transform.localScale.y * 0.3f,
                soulpiece.transform.localScale.z
            );
        soulpiece.transform.DOScale(
            new Vector3(
            soultransform.localScale.x,
            soultransform.localScale.y,
            soulpiece.transform.localScale.z), 0.2f
            );*/

    }

    IEnumerator Generate()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            GenerateSoul();
        }
    }
    
    
}
