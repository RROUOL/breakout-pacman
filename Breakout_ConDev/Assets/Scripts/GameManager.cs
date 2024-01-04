using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;

    public Block[] blocks;

    public Pacman pacman;

    public Transform pellets;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text levelCompText;

    [SerializeField] private AudioSource soundManager;

    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip levelComplete;
    [SerializeField] private AudioClip pickupCoin;
    [SerializeField] private AudioClip powerUp;
    [SerializeField] private AudioClip hitHurt;
    [SerializeField] private AudioClip pacDeath;
    [SerializeField] private AudioClip blockThud;

    public int ghostMultiplier { get; private set; } = 1;
    public int blockMultiplier { get; private set; } = 1;
    public static int score { get; private set; }
    public static int deathMultiplier { get; private set; }
    public int lives { get; private set; }

    private void Start()
    {
        NewGame();
        TimerManager.isCountingDown = true;
        score = 0;
        deathMultiplier = 0;
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        if (TimerManager.timeUp)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(false);
        }
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < this.blocks.Length; i++)
        {
            if (!this.blocks[i].collider.enabled)
            {
                this.blocks[i].SetComponent(false);
            }
        }
        PacmanDeathAnim();
        SceneManager.LoadScene(2);
    }

    private void NewGame()
    {
        NewRound();
    }

    private void NewRound()
    {
        levelCompText.enabled = false;
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        for (int i = 0; i < this.blocks.Length; i++)
        {
            //this.blocks[i].SetComponent(true);
            this.blocks[i].ResetState();
        }
        ResetState();
    }

    private void ResetState()
    {
        ResetGhostMultiplier();

        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }

        this.pacman.ResetState();
        
    }

    private void GameOver()
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < this.blocks.Length; i++)
        {
            if (!this.blocks[i].collider.enabled)
            {
                this.blocks[i].SetComponent(false);
            }
        }

        this.pacman.gameObject.SetActive(false);
    }

    public void GhostEaten(Ghost ghost)
    {
        //int points = ghost.points * this.ghostMultiplier;
        //SetScore(this.score + (ghost.points * this.ghostMultiplier));
        this.ghostMultiplier++;
        soundManager.PlayOneShot(hitHurt);
    }

    public void BlockThud(Block block)
    {
        soundManager.PlayOneShot(blockThud);
    }

    public void BlockEaten(Block block)
    {
        this.blockMultiplier++;
        soundManager.PlayOneShot(explosion);
    }

    public void PacmanDeathAnim()
    {
        GameObject dAnim = Instantiate(this.pacman.deathAnim, this.pacman.transform.position, Quaternion.identity);
        this.pacman.moveAnim.enabled = false;
        this.pacman.collider.enabled = false;
        soundManager.PlayOneShot(pacDeath);

        Destroy(dAnim, 2.0f);
    }

    public void PacmanDeathAnim(float duration)
    {
        GameObject dAnim = Instantiate(this.pacman.deathAnim, this.pacman.transform.position, Quaternion.identity);
        this.pacman.moveAnim.enabled = false;
        this.pacman.collider.enabled = false;
        soundManager.PlayOneShot(pacDeath);

        Destroy(dAnim, 2.0f);
        Invoke(nameof(PacmanEaten), duration);
    }

    public void PacmanEaten()
    {
        //this.pacman.deathAnim.GetComponent<AnimatedSprite>().Restart();
        //this.pacman.deathAnim.SetActive(false);
        deathMultiplier++;
        this.pacman.moveAnim.enabled = true;
        this.pacman.collider.enabled = true;
        this.pacman.gameObject.SetActive(false);

        ResetState();
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        if (pellet.gameObject.tag != "PowerPellet")
        {
            soundManager.PlayOneShot(pickupCoin);
        }

        if (!HasRemainingPellets())
        {
            score++;
            soundManager.PlayOneShot(levelComplete);
            levelCompText.enabled = true;
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        soundManager.PlayOneShot(powerUp);

        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        for (int i = 0; i < this.blocks.Length; i++)
        {
            if (this.blocks[i].collider.enabled)
            {
                this.blocks[i].EnableBreak(pellet.duration);
            }
        }

        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                if (pellet.gameObject.tag != "PowerPellet")
                {
                    return true;
                }
                
            }
        }
        return false;
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}