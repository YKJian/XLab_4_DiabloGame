namespace Lesson4_MyDiabloGame;

class Program
{
    private static List<Monster> _monsters;
    
    static void Main(string[] args)
    {
        string input = string.Empty;
        _monsters = new List<Monster>();
        
        Player player = new Player();
        Skeleton skeleton = new Skeleton();
        
        IDamageable[] array = new IDamageable[]
        {
            player,
            skeleton
        };

        foreach (var element in array)
        {
            if (element is Player specificPlayer)
                specificPlayer.TakeDamage(50);
            else element.TakeDamage(100);
        }
        
        /*do
        {
            Console.WriteLine("1) Add Skeleton");
            Console.WriteLine("2) Add Ghost");
            Console.WriteLine("3) Upgrade first monster");
            Console.WriteLine("4) Print all monsters");
            Console.WriteLine("5) Attack first monster");
            
            // ??: if input is null then input = ""
            input = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;
            Console.Clear();
            
            switch (input)
            {
                case "1": 
                    _monsters.Add(CreateMonster<Skeleton>());
                    break;
                case "2": 
                    _monsters.Add(CreateMonster<Ghost>());
                    break;
                case "3":
                {
                    _monsters[0] = new ArmoredMonster(50, _monsters[0]);
                    _monsters[0] = new InvisibleMonster(0.5f, _monsters[0]);

                    break;
                }
                case "4":
                {
                    foreach (var monster in _monsters)
                    {
                        Console.WriteLine(monster.GetType().Name);
                    }

                    break;
                }
                case "5":
                {
                    Console.WriteLine("Жизни: " + _monsters[0].Hp);
                    _monsters[0].TakeDamage(100);
                    Console.WriteLine("Жизни: " + _monsters[0].Hp);

                    break;
                }
            }
            
        } while (input != "q");*/
        
        var arrayInt = new int[10];
        var listInt = new List<int>()
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9
        };

        var enumerable = Return();
        
        foreach (var item in enumerable)
        {
            Console.WriteLine(item);
        }
        
        /*WriteCollection(arrayInt);
        WriteCollection(listInt);*/
    }

    public static IEnumerable<int> Return()
    {
        yield return 0;
        yield return 1;
        yield return 5;
        yield return 7;
    }
    
    private static void WriteCollection(IEnumerable<int> collection)
    {
        foreach (var item in collection)
        {
            Console.WriteLine(item);
        }
    }
    
    private static T CreateMonster<T>() 
        where T : Monster, new()
    {
        return new T();
    }
}

public interface IDamageable
{
    public void TakeDamage(int damage);
}

public class Player : IDamageable
{
    public void TakeDamage(int damage)
    {
        Console.WriteLine("Player got damage");
    }
}

public abstract class Monster : IDamageable
{
    private int _hp = 100;

    public int Hp
    {
        get => _hp;
        private set
        {
            if (value < 0) _hp = 0;
            else _hp = value;
        }
    }
    
    // template method (it's WITHOUT!!! virtual)
    // basic logic of taking damage is in base
    public virtual void TakeDamage(int damage)
    {
        Hp -= damage;
        //Hp -= AffectDamage(damage);
    }

    // extra logic is in heirs, such as upgrades
    // compute damage in heirs
    // protected virtual int AffectDamage(int damage) => damage;
    
    public abstract void Move();
}

public class Skeleton : Monster
{
    // here too 
    
    public override void Move()
    {
        Console.WriteLine("Skeleton walks");
    }
}

public class Ghost : Monster
{
    // we moved it to the decorator
    /*private float _invisible;

    protected override int AffectDamage(int damage)
    {
        return (int)(damage * (1 - _invisible));
    }
    */

    public override void Move()
    {
        Console.WriteLine("Ghost flies");
    }
}

// DECORATOR pattern
public abstract class DecoratorMonster: Monster
{
    protected Monster Monster;

    protected DecoratorMonster(Monster monster)
    {
        Monster = monster;        
    }

    public override void TakeDamage(int damage)
    {
        Monster.TakeDamage(AffectDamage(damage));
    }

    protected abstract int AffectDamage(int damage);
    
    /*{
        return base.AffectDamage(damage);
    }*/
}

public class ArmoredMonster : DecoratorMonster
{
    private int _armor;
    
    public ArmoredMonster(int armor, Monster monster) 
        : base(monster)
    {
        _armor = armor;
    }
    
    public override void Move() => Monster.Move();

    protected override int AffectDamage(int damage)
    {
        return damage - _armor;
        // return Monster.AffectDamage(damage) - _armor;
    }
}

public class InvisibleMonster : DecoratorMonster
{
    private float _invisible;
    
    public InvisibleMonster(float invisible, Monster monster) 
        : base(monster)
    {
        _invisible = invisible;
    }
    
    public override void Move() => Monster.Move();

    protected override int AffectDamage(int damage)
    {
        return (int)(damage * (1 - _invisible));
    }
}