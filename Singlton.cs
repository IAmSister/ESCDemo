using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singlton<T> where T:class,new ()
{
    private static T ins;
    public static object obj = new object();
    public static T Ins
    {
        get
        {
            if (ins==null)
            {
                lock(obj)
                {
                    if (ins==null)
                    {
                        ins = new T();
                    }
                }

            }
            return ins;
        }
    }
}
