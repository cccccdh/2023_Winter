using UnityEngine;


public enum MobType
{
    Common, Boss
}

public enum MobName
{
    Bat, Pterodactyl, Cat, Scorpion,
    Earth, Factory, Monolith, Lurker,    
    Count, Eyeball, Dye, Ghast, Leaper,
    Sentry, Skull, SkullSmall, SkullFlaming,
    Zombie, Snake, Witch,
    Dog, Beard, Brain, Mummy,
    Slime, SlimeSmall, SlimeSquareSmall, SlimeSquare
}

public class PixelMobStats : MonoBehaviour
{
    public MobType mobType;
    public MobName mob;

    public int hp; // 체력
    public int maxHp; // 최대 체력
    public float speed; // 이동 속도
    public float reactionDistance; // 반응 거리

    private void Awake()
    {
        SetMobStatus();
    }

    private void SetMobStatus()
    {
        if(mobType == MobType.Common)
        {
            switch (mob)
            {
                // 던전 1
                case MobName.Bat:
                    hp = 2;
                    maxHp = hp;
                    speed = 1.05f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.Pterodactyl:
                    hp = 3;
                    maxHp = hp;
                    speed = 1.3f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.Cat:
                    hp = 2;
                    maxHp = hp;
                    speed = 1.15f;
                    reactionDistance = 6.0f;
                    break;

                // 던전 2
                case MobName.Earth:
                    hp = 3;
                    maxHp = hp;
                    speed = 0.6f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.Factory:
                    hp = 2;
                    maxHp = hp;
                    speed = 0.75f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.Monolith:
                    hp = 3;
                    maxHp = hp;
                    speed = 0.95f;
                    reactionDistance = 6.0f;
                    break;

                // 던전 3
                case MobName.Count:
                    hp = 4;
                    maxHp = hp;
                    speed = 0.95f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.Eyeball:
                    hp = 2;
                    maxHp = hp;
                    speed = 0.85f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.Dye:
                    hp = 2;
                    maxHp = hp;
                    speed = 0.95f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.Ghast:
                    hp = 3;
                    maxHp = hp;
                    speed = 1.1f;
                    reactionDistance = 6.0f;
                    break;

                // 던전 4
                case MobName.Sentry:
                    hp = 3;
                    maxHp = hp;
                    speed = 0.95f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.Skull:
                    hp = 2;
                    maxHp = hp;
                    speed = 0.95f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.SkullSmall:
                    hp = 2;
                    maxHp = hp;
                    speed = 0.95f;
                    reactionDistance = 6.0f;
                    break;

                // 던전 5
                case MobName.Zombie:
                    hp = 3;
                    maxHp = hp;
                    speed = 1.0f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.Snake:
                    hp = 2;
                    maxHp = hp;
                    speed = 0.95f;
                    reactionDistance = 6.0f;
                    break;

                // 던전 6
                case MobName.Dog:
                    hp = 3;
                    maxHp = hp;
                    speed = 1.05f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.Beard:
                    hp = 3;
                    maxHp = hp;
                    speed = 0.85f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.Brain:
                    hp = 2;
                    maxHp = hp;
                    speed = 0.85f;
                    reactionDistance = 6.0f;
                    break;

                // 던전 7
                case MobName.Slime:
                    hp = 2;
                    maxHp = hp;
                    speed = 0.75f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.SlimeSmall:
                    hp = 2;
                    maxHp = hp;
                    speed = 0.75f;
                    reactionDistance = 6.0f;
                    break;
                case MobName.SlimeSquareSmall:
                    hp = 2;
                    maxHp = hp;
                    speed = 0.75f;
                    reactionDistance = 6.0f;
                    break;
            }
        }else
        {
            switch (mob)
            {
                // 던전 1
                case MobName.Scorpion:
                    hp = 10;
                    maxHp = hp;
                    speed = 1.25f;
                    reactionDistance = 6.0f;
                    break;
                // 던전 2
                case MobName.Lurker:
                    hp = 12;
                    maxHp = hp;
                    speed = 1.5f;
                    reactionDistance = 6.0f;
                    break;
                // 던전 3
                case MobName.Leaper:
                    hp = 14;
                    maxHp = hp;
                    speed = 2.0f;
                    reactionDistance = 6.0f;
                    break;
                // 던전 4
                case MobName.SkullFlaming:
                    hp = 16;
                    maxHp = hp;
                    speed = 1.9f;
                    reactionDistance = 6.0f;
                    break;
                // 던전 5
                case MobName.Witch:
                    hp = 18;
                    maxHp = hp;
                    speed = 1.8f;
                    reactionDistance = 6.0f;
                    break;
                // 던전 6
                case MobName.Mummy:
                    hp = 20;
                    maxHp = hp;
                    speed = 1.4f;
                    reactionDistance = 6.0f;
                    break;
                // 던전 7
                case MobName.SlimeSquare:
                    hp = 20;
                    maxHp = hp;
                    speed = 1.05f;
                    reactionDistance = 6.0f;
                    break;
            }
        }
        
    }
}
