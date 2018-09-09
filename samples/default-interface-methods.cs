using System;
// Run mode:
//   value.Inspect()      — often better than Console.WriteLine
//   Inspect.Heap(object) — structure of an object in memory (heap)
//   Inspect.Stack(value) — structure of a stack value
public static class Program {
    public static void Main() {
        //Class classInstance = new Class();
        //classInstance.PublicMethod();
        // error CS1061: 'Class' does not contain a definition for 'PublicMethod' and no extension method 'PublicMethod' accepting a first argument of type 'Class' could be found (are you missing a using directive or an assembly reference?)


        IInterface interfaceInstance = new Class();
        interfaceInstance.PublicMethod();
    }
}

interface IPainter {
    string Name { get; }
    int WorksCount { get; }
    int Age { get; }

    string Signature => $"{Name} with {WorksCount}";
    int Performance => Age / WorksCount;
}

class PainterUser : IPainter {
    public string Name { get; set; }
    public int WorksCount { get; set; }
    public int Age { get; }
    
    public string CssColor { get; set; }
    public Guid MongoDbId { get; set; }
    public int SqlId { get; set; }
}

interface IInterface
{
    void DefaultMethod() { Console.WriteLine("Hello World"); }
    public void PublicMethod() {}
    internal void InternalMethod() {}
    private protected void PrivateProtectedMethod() {}
    private void PivateMethod() {}   
    static void StaticMethod() {}
    int DefaultProperty => 42;
}

class Class : IInterface 
{
	void IInterface.DefaultMethod()
    {
        Console.WriteLine("Hello");
    }
}

/*
interface IInvalidInterface
{
    internal protected void InternalProtectedMethod() {}   
    // error CS0106: The modifier 'protected internal' is not valid for this item
    protected void ProtectedMethod() {}
    // error CS0106: The modifier 'protected' is not valid for this item    
}
*/
/* 
    public interface IEnumerable<T>
    {        
        int Count(){
            return Enumerable.Count(this);
        }
    }

    public interface IList<T> : ICollection<T>
    {        
        T this[int index] { get; set; }
        int IndexOf(T item);
        void Insert(int index, T item);
        void RemoveAt(int index);

        void Clear();
        
        int IEnumerable<T>.Count(){
            return this.Count;
        }
    }
    */