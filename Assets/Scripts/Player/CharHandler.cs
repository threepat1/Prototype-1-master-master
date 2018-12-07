using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KingdomGates
{
    public class CharHandler : MonoBehaviour
    {
        public bool alive;
        public int maxHealth = 100;
        public static float curHealth;

        public bool damaged;
        public Image damageImage;
        public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
        public Color flashColour = new Color(1f, 1f, 1f, 1f);
        public bool takeDMG;
        //public int damage = 25;
        public int playerDMG = 1;
        public BoxCollider axeCollider;
        public UIShake uiShake;

        public Slider healthSlider;
        public float lerpSpeed;

        private void Awake()
        {
            // Health starts at max health
            curHealth = maxHealth;
            // Player starts alive
            alive = true;

        
        }

        private void Update()
        {

            if (curHealth >= 10)
            {
                if (curHealth != healthSlider.value)
                {
                    // Set the health bar's value to the current health.
                    healthSlider.value = Mathf.Lerp(healthSlider.value, curHealth, Time.deltaTime * lerpSpeed);
                }
            }
            
            if (damaged)
            {
                // ... set the colour of the damageImage to the flash colour.
                damageImage.color = flashColour;
            }
            // Otherwise...
            else
            {
                // ... transition the colour back to clear.
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }

            // Reset the damaged flag.
            damaged = false;


            // If current health is less than or equal to 0 and the player is alive
            if (curHealth <= 0 && alive)
            {
                // Make the player dead
                alive = false;
                Dead();
            }
            
        }


        public void TakeDamage(int npcDmg)
        {
            curHealth -= npcDmg;
            damaged = true;
            uiShake.ShakeUI(20f, 1f);
            
        }
        // When the player is dead
        public void Dead()
        {
            // Deactivate the player
            gameObject.SetActive(false);

            SceneManager.LoadScene(1);
        }
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {

                NPCHandler npcHealth = other.GetComponent<NPCHandler>();
                if (npcHealth != null)
                {
                    npcHealth.TakeDMG( playerDMG, axeCollider);
                    
                    GameObject.Destroy(other.gameObject);
                }

                else if (GetComponent<PlayerController>().moveSpeed >= 30)
                {
                    npcHealth.TakeDMG( playerDMG, axeCollider);
                }
            }

        }

    }
}