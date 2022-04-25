using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITestDll 
{
    
}
public class TestDll1:ITestDll
{
    public int i;
}


public class TestDll2 : ITestDll
{
    public string str;
}