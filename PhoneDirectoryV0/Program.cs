using System;
using PhoneDirectoryV0.Helper;

namespace PhoneDirectoryV0;

public class Program
{
    static void Main()
    {
        var phoneBook = new PhoneDictionary();

        phoneBook.AddEntry("John", "Doe", "123-456-7890");
        phoneBook.AddEntry("Johe", "Smith", "987-654-3210");
        phoneBook.AddEntry("John", "Doe", "555-555-5555");
        phoneBook.AddEntry("Alice", "Johnson", "111-222-3333");
        phoneBook.AddEntry("Bob", "Brown", "444-555-6666");
        phoneBook.RemoveEntry("Jane", "Smith");
        
        // var johnNumbers = phoneBook.GetPhoneNumbers("John", "Doe");
        //
        // foreach (var number in johnNumbers)
        //     Console.WriteLine(number);
        
        var result = phoneBook.FindByThreeLetters("Joh");
        foreach (var entry in result)
        {
            Console.WriteLine($"{entry.Key}: {string.Join(", ", entry.Value)}");
        }
    }
    
}
