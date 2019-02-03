using System;
using System.Collections.Generic;

public class C {
    internal Point M() {
        {
        	Point p = new (1, 2);
        }
        {
            var p = new Point(1, 2);
        }
        {
        	List<int> list = new();
        }
        {
            Point p = new() {
                X = 1,
                Y = 2
            };
        }
        {
            bool zero = false;
            //Point p = zero ? new(0, 0) : new(1, 2);
        }
        
        Method(new());
        
       	Scale? s = new(10); 
        {
            Dictionary<int, List<(string name, int age)>> users = new();
        }        
        {
            var users = new Dictionary<int, List<(string name, int age)>>();
        }
        {
            Draw(new List<int> {1, 2, 3}, new Point(8, 9), new Scale(1));
            Draw(new() {1, 2, 3}, new(8, 9), new (1));
        }        
        return new(1, 2);
    }
    internal Point P => new(3, 4);
    private void Method(Point p){}
    private Dictionary<int, List<(string name, int age)>> users1 = new();
    public Dictionary<int, List<(string name, int age)>> Users1 => users1 ?? (users1 = new());

    private Dictionary<int, List<(string name, int age)>> users = new Dictionary<int, List<(string name, int age)>>();
    public Dictionary<int, List<(string name, int age)>> Users => users ?? (users = new Dictionary<int, List<(string name, int age)>>());
    
    void Draw(List<int> items, Point point, Scale scale)
    {
    }
}

class Store{
    public Store(Dictionary<int, List<(string name, int age)>> users){
        Users = users ?? new();
    }   
    public Dictionary<int, List<(string name, int age)>> Users { get;}
}

class Store1{
    public Store1(Dictionary<int, List<(string name, int age)>> users){
        Users = users ?? new Dictionary<int, List<(string name, int age)>>();
    }   
    public Dictionary<int, List<(string name, int age)>> Users { get;}
}

class Point
{
    public Point(int x, int y){
        X = x;
        Y = y;
    }
    
    public Point(){}

    public int X { get; set; } 
    public int Y { get; set; }
}

struct Scale
{    
    public Scale(int amount){}
}