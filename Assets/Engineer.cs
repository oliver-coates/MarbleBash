

using UnityEngine;


public class Engineer : MonoBehaviour
{
    public void Update()
    {
        
    }
}

public abstract class Person
{
    public string name = "Unnamed person";
    
    public abstract void Talk();

    public virtual void Walk()
    {
        Debug.Log("I am walking!");
    }

    public void Test(Person myPerson)
    {
        myPerson.Talk();
    }
}


public class Artist : Person
{
    public override void Talk()
    {
        
    }
}