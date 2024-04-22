using UnityEngine;

public interface IDamageable
{
    public float Vida { get; set; }
    public bool Targeteable { get; set; }
    public bool Invencible { get; set; }
    public void RecibirDmg(float dmg, Vector2 knockback);
    public void RecibirDmg(float dmg);
    public void Destruir();
    public bool Perdidacontrol { get; set; }
    public bool Golpeado { get; set; }
}
