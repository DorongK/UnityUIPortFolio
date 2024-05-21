using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ü������ �ʵ忡�� static�̳� const �� �������� �ʴ� ��, Ŭ����ó�� ���ο��� �ʱ�ȭ �� �� ����.
/// �Ű������� ���� �����ڴ� �����߻�. ��� ������ �⺻ �Ҵ簪���� �������ִ� �����ڰ� �����ȴ�.
/// </summary>
public struct Weapon
{
    public string name;
    public int Attack;
}
/// <summary>
/// class�� ����Ÿ��, struct�� ��Ÿ��. ����ü�� ������ �������� �ʰ� �����ؼ� �����Ѵ�.
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
    /// �����ڿ����� :base()�� �𸮾��� Super:: �� ���� ������ �ϴµ�.
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
