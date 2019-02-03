using System;
using System.Runtime.CompilerServices;

[NonNullTypes(true)]
public class C {
    [NonNullTypes(true)]
    public void M() {
        object? nullable = null;        
        
        nullable.ToString();

        if(nullable != null)
        	nullable.ToString();
        
        if (nullable is string)
        	nullable.ToString();

        nullable!.ToString();

        object notnull = default;

        notnull = nullable;
        notnull = null;
        
        MethodNotNull(notnull);

        MethodNotNull(nullable);
        
        string? nullableString = "";
        if(!string.IsNullOrEmpty(nullableString))
        	nullableString.ToString();
        
        Console.WriteLine(nullable);
        Console.WriteLine(notnull);
        Console.WriteLine(notnull);
    }
    
    [NonNullTypes(true)]
    static void Main(string[] args)
    {
        object? nullable = null;
        object notnull = null;
    }
    
    [NonNullTypes(true)]
    void MethodNotNull(object notnull){

    }

    
    void Method(object? nullable, object notnull){
        nullable.ToString();
        
        if(nullable != null)
        	nullable.ToString();
        
        if (nullable is string)
        	nullable.ToString();
                        
        nullable!.ToString();    
    }

    void Generic<T>(T? gen) where T: class
    {
        MethodNotNull(gen);
    }

}