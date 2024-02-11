using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamouflagDNA : MonoBehaviour {
    //gene for colour
    public float r;
    public float g;
    public float b;

    private bool dead = false;
    public float timeToDie = 0;
    SpriteRenderer personSprite;
    Collider2D personSpriteCollider;
    internal object genes;

    private void Start() {
        personSprite = GetComponent<SpriteRenderer>();
        personSpriteCollider = GetComponent<Collider2D>();

        personSprite.color = new Color(r,g,b);
    }

    private void OnMouseDown() {
        dead = true;
        timeToDie = CamouflagPopulationManager.elapsedTime;
        personSprite.enabled = false;
        personSpriteCollider.enabled = false;
    }
}
