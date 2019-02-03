using System;
using System.Collections.Generic;

public class Program {
    static void Main() {
        var images = GetImages();
        foreach (var image in images)
        {
            if(image is Image(_, "Малевич", _))
                Console.WriteLine(image.Name);
        }
    }

    static void Simple(){
        object image = new Image {
            Name = "Черный квадрат",
            Author = "Малевич"
        };

        {
            // without positional pattern
            if (image is Image img && img.Name == "Черный квадрат" && img.Author == "Малевич")
                Console.WriteLine(img.Name);        
        }
        {
            // with positional pattern
            if(image is Image("Черный квадрат", "Малевич", _) img)
                Console.WriteLine(img.Name);
        }
        {
            if(image is Image img){
                img.Deconstruct(out string name, out string author, out _);
                if(name == "Черный квадрат" && author == "Малевич")
                    Console.WriteLine(img.Name);
            }
        }
    }

    static void Complicated(){
        object image = new Image {
            Name = "Черный квадрат",
            Author = "Малевич",
            Shape = new Square {
                Color = "black",
                Size = 80
            }
        };

        {
            if(image is Image(_, _, Shape("black", _)) img)
                Console.WriteLine(img.Name);
        }
        {
            // sub-pattern can be simplified
            if(image is Image(_, _, ("black", _)) img)
                Console.WriteLine(img.Name);
        }
    }

    void Paramters(){
        object image = new Image {
            Name = "Черный квадрат",
            Author = "Малевич"
        };

        if(image is Image { Name: "Черный квадрат", Author: "Малевич" } img)
                Console.WriteLine(img.Name);
    }

    void ParamtersStatic(){
        object image = new Image {
            Name = "Черный квадрат",
            Author = "Малевич"
        };

        if(image is Image(_, String(7), _) img)
            Console.WriteLine(img.Name);
    }

    void Recursive(){
        object image = new Image {
            Name = "Черный квадрат",
            Author = "Малевич",
            Shape = new Square {
                Color = "black",
                Size = 80
            }
        };

        if(image is Image { Name: "Черный квадрат", Author: "Малевич", Shape: Square("black", _) } img)
            Console.WriteLine(img.Name);
    }

    void UglyCode(){
        object image = new Image {
            Name = "Черный квадрат",
            Author = "Малевич",
            Shape = new Square {
                Color = "black",
                Size = 80
            }
        };

        switch(image){
            case Image(String(14), String(7), (String(12), 80)) img:
                Console.WriteLine(img.Name);
                break;
            case Image(_, _, (_, 80)) img:
                Console.WriteLine(img.Name);
                break;
        }
    }

    static Image[] GetImages(){
        return new[] {
            new Image {
                Name = "Черный квадрат",
                Author = "Малевич",
                Shape = new Square {
                    Color = "black",
                    Size = 80
                }
            },
            new Image {
                Name = "Черный крест",
                Author = "Малевич",
                Shape = new Cross {
                    Color = "black",
                    Size = 79
                }
            },
            new Image {
                Name = "Черный круг",
                Author = "Малевич",
                Shape = new Circle {
                    Color = "black",
                    Size = 105
                }
            }            
        };
    }
}

public class Image {
    public string Name { get; set; }
    public string Author { get;set; }    
    public Shape Shape { get; set; }
    public void Deconstruct(out string name, out string author, out Shape shape){
        name = Name;
        author = Author;
        shape = Shape;
    }
}

public class Shape {
    public string Color { get; set; }
    public int Size { get; set; }
    public void Deconstruct(out string color, out int size){
        color = Color;
        size = Size;
    }
}

public class Square : Shape { }
public class Cross : Shape { }
public class Circle : Shape { }



public static class Extentions{
    public static void Deconstruct(this string str, out int length){
        length = str.Length;
    }
    
}

public class AllPatterns{
    void Patterns(){
        object obj = null;
        bool r;
        r = obj is Image;
        r = obj is 42;
        r = obj is var variable;
        r = obj is var _;        
    }
}

public class Option{}
public class Some : Option{
    public object Value { get; set; }
    public void Deconstruct(out object value){
        value = Value;
    }
}
public class None : Option { 
	public void Deconstruct(){}
}

public class OptionTest{
    void Test(Option opt){
        Option option = opt;
        switch(option){
            case Some(var value):
                Console.WriteLine(value);
                break;
            case None():
                Console.WriteLine("Null");
                break;            
        }
    }
}
