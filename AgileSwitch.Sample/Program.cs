﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileSwitch.Sample
{
    class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public void Eat() { Console.WriteLine("eating"); }
    }

    class Dog : Animal
    {
        public void Bark() { Console.WriteLine("barking"); }
    }

    class Cat : Animal
    {
        public void Meow() { Console.WriteLine("meow meow"); }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Sample1_FallThroughAllCasesWithoutBreak();
            Sample2_HandleDiffeentTypesWithBreak();

            Console.Read();
        }

        static void Sample1_FallThroughAllCasesWithoutBreak()
        {
            var dog = new Dog { Name = "puppy", Age = 1 };

            Switch.On(dog)
                .Case(d => d.Age > 5, d => Console.WriteLine("it's an old dog"))
                .Case(d => d.Name == "puppy", d => d.Eat())
                .Case(d => d.Age < 3, d => d.Bark())
                .Default(d => Console.WriteLine("all cases fall through"));

            Console.WriteLine("-------- end of sample 1 --------");
        }


        static void Sample2_HandleDiffeentTypesWithBreak()
        {
            Animal animal;
            if (new Random().Next(100) < 50)
            {
                animal = new Dog { Name = "dog", Age = 7 };
            }
            else
            {
                animal = new Cat { Name = "cat", Age = 7 };
            }

            Switch.On(animal)
                .Case(a => a.Age > 5, a => Console.WriteLine("it's old"))
                .Case(new Dog(), a => Console.WriteLine("can't happen"))
                .Case<Dog>(d => d.Bark())
                    .Break()
                .Case<Cat>(c => c.Meow())
                    .Break()
                .Case(a => a.Age > 0, a => Console.WriteLine("should never happen because of break"))
                .Default(a => Console.WriteLine("should never happen because of break"));

            Console.WriteLine("-------- end of sample 2 --------");
        }
    }
}
