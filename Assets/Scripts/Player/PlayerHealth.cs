using UnityEngine;
using System.Collections;

public class PlayerHealth
{
    PlayerConfiguration PlayerConfiguration; 
    PlayerGeneral PLAYER;

    public PlayerHealth(PlayerConfiguration playerConfiguration, PlayerGeneral player)
    {
        this.PlayerConfiguration = playerConfiguration;
        this.PLAYER = player;
    }


    #region Regeneration
    public void RegenerateHealth(int amount = 0)
    { 
        PlayerConfiguration.health += (amount == 0) ? PlayerConfiguration.healthRegen : amount;
        PlayerConfiguration.health = Mathf.Clamp(PlayerConfiguration.health, 0, PlayerConfiguration.maxHealth);

        // Aquí podrías agregar efectos de regeneración, sonidos, etc.
    }
    #endregion

    #region Damage
    // Recibir Daño
    public void TakeDamage(int damage)
    {
        PlayerConfiguration.health -= damage;
        PlayerConfiguration.health = Mathf.Clamp(PlayerConfiguration.health, 0, PlayerConfiguration.maxHealth);

        if (PlayerConfiguration.health <= 0)
        {
            // Aquí podrías manejar la muerte del jugador, como reproducir una animación, reiniciar el nivel, etc.

            PLAYER.STATEMACHINE.ChangeState(PLAYER.STATES.DeadState(PLAYER.STATEMACHINE));

            return;
        }

        Debug.Log("Damage");
        // Aquí podrías agregar efectos de daño, sonidos, etc.
    }

    // Morir
    public void Die()
    {
        // Aquí podrías manejar la muerte del jugador, como reproducir una animación, reiniciar el nivel, etc.
        Debug.Log("Jugador ha muerto");
    }
    #endregion
}