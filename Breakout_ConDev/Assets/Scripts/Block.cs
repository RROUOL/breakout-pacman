using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public SpriteRenderer solid;
    public SpriteRenderer blue;
    public SpriteRenderer white;
    public Collider2D collider;

    public bool eaten { get; private set; }
    public bool fragile { get; private set; }

    private void Awake()
    {
        this.eaten = false;
        this.fragile = false;
        this.solid.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false;
    }

    public void EnableBreak(float duration)
    {
        CancelInvoke();
        Invoke(nameof(DisableBreak), duration);

        this.fragile = true;
        this.solid.enabled = false;
        this.blue.enabled = true;
        this.white.enabled = false;

        Invoke(nameof(Flash), duration / 2.0f);
    }

    private void Flash()
    {
        if (!this.eaten)
        {
            this.blue.enabled = false;
            this.white.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.fragile)
            {
                TimerManager.myTimer = TimerManager.myTimer + 15;
                FindObjectOfType<GameManager>().BlockEaten(this);
                Eaten();
            }
            else
            {
                FindObjectOfType<GameManager>().BlockThud(this);
            }
        }
    }

    private void Eaten()
    {
        this.eaten = true;
        this.SetComponent(false);
    }

    public void DisableBreak()
    {
        if (collider.enabled)
        {
            this.fragile = false;
            this.solid.enabled = true;
            this.blue.enabled = false;
            this.white.enabled = false;
        }
    }

    public void ResetState()
    {
        this.SetComponent(true);
        this.fragile = false;
        this.eaten = false;
        this.solid.enabled = true;
    }

    public void SetComponent(bool tf)
    {
        collider.enabled = tf;
        solid.enabled = tf;
        if (!solid.enabled)
        {
            blue.enabled = false;
            white.enabled = false;
        }
    }

    public void OnEnable()
    {
        this.fragile = false;
        this.solid.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false;
    }
}
