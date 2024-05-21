using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 구조체에서는 필드에서 static이나 const 로 지정하지 않는 한, 클래스처럼 내부에서 초기화 할 수 없다.
/// 매개변수가 없는 생성자는 오류발생. 모든 변수를 기본 할당값으로 지정해주는 생성자가 제공된다.
/// </summary>
public struct Weapon
{
    public string name;
    public int Attack;
}
/// <summary>
/// class는 참조타입, struct는 값타입. 구조체는 원본을 참조하지 않고 복사해서 전달한다.
/// </summary>
public class Character : MonoBehaviour
{
    public string Charactername;
    public int exp = 0;
    public Character()
    {
        Charactername = "Not Assigned";
    }
    public Character(string name)
    {
        this.Charactername = name;
    }
    public virtual void PrintStatInfo()
    {
        Debug.LogFormat("Hero:{0}-{1} EXP", this.name, this, exp);
    }
}

public class Warrior : Character
{
    /// <summary>
    /// 생성자에서의 :base()가 언리얼의 Super:: 와 같은 역할을 하는듯.
    /// </summary>
    Warrior():base()
    {

    }
    Warrior(string name,Weapon weapon):base(name)
    {
        this.weapon = weapon;
    }

    public override void PrintStatInfo()
    {
        //base.PrintStatInfo();
        Debug.LogFormat("{0},pick up your{1} ", this.Charactername, this.weapon.name);

    }
    public Weapon weapon;

}
