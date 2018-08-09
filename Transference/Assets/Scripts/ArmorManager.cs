using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorManager : MonoBehaviour {

    private void Start()
    {
        test t1 = new test();
        test.Check(t1, null);
        test t2 = t1;
        t2.name = "joe";
        t1.name = "bob";
        //t1 and t2 share same memory location so tests will return true;
        bool tests = test.Check(t1, t2);
        Debug.Log(tests);

        test t3 = new test();
        t3.name = "bob";

        tests = test.Check(t1, t3);
        //t1 was overwritten by t2 si this returns true;
        Debug.Log(tests);

        t2.name = "joe";
        
        //joe and bob are not the same name so returns false;
        tests = test.Check(t2, t3);
        Debug.Log(tests);

        tests = t1.Equals(t2);
        Debug.Log(tests);

        tests = t1.Equals(t3);
        Debug.Log(tests);

        tests = t1.Equals(null);
        Debug.Log(tests);

        t1 = null;
        tests = t1.Equals(null);
        Debug.Log(tests);

    }

}
