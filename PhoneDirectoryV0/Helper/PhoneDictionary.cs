
namespace PhoneDirectoryV0.Helper;

public class PhoneDictionary
{
    private readonly Dictionary<char, Dictionary<char, Dictionary<char, Dictionary<string, HashSet<string>>>>>
                        _phoneDirectory = new();
    
    private static string GetKey(string firstName, string lastName) =>
        $"{firstName} {lastName}".Trim().ToLower();

    private static char[] GetChars(string lowerKey)
    {
        
        var firstChar = lowerKey.Length > 0 ? lowerKey[0] : '\0';
        var secondChar = lowerKey.Length > 1 ? lowerKey[1] : '\0';
        var thirdChar = lowerKey.Length > 2 ? lowerKey[2] : '\0';
        return new[] {firstChar, secondChar, thirdChar};
    }
    public void AddEntry(string firstName, string lastName, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName)
                                                 || string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("First name, last name and phone number cannot be null or empty.");

        var key = GetKey(firstName, lastName);
        var chars = GetChars(key);
        if (!_phoneDirectory.ContainsKey(chars[0]))
            _phoneDirectory[chars[0]] = new Dictionary<char, Dictionary<char, Dictionary<string, HashSet<string>>>>();
        var firstDict = _phoneDirectory[chars[0]];
        
        if (!firstDict.ContainsKey(chars[1]))
            firstDict[chars[1]] = new Dictionary<char, Dictionary<string, HashSet<string>>>();
        var secondDict = firstDict[chars[1]];
        
        if (!secondDict.ContainsKey(chars[2])) 
            secondDict[chars[2]] = new Dictionary<string, HashSet<string>>();
        var thirdDict = secondDict[chars[2]];
        
        if (!thirdDict.ContainsKey(key))
            thirdDict[key] = new HashSet<string>();
        thirdDict[key].Add(phoneNumber);
    }

    public void RemoveEntry(string firstName, string lastName)
    {
        if( string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("First name and last name cannot be null or empty.");

        var key = GetKey(firstName, lastName);
        var chars = GetChars(key);

        if (_phoneDirectory.TryGetValue(chars[0], out var existingDict1) &&
            existingDict1.TryGetValue(chars[1], out var existingDict2) &&
            existingDict2.TryGetValue(chars[2], out var existingDict3) &&
            existingDict3.Remove(key) && existingDict3.Count == 0)
        {
            existingDict2.Remove(chars[2]);
            if (existingDict2.Count == 0)
            {
                existingDict1.Remove(chars[1]);
                if (existingDict1.Count == 0)
                {
                    _phoneDirectory.Remove(chars[0]);
                }
            }
        }
        
    }
    public IEnumerable<string> GetPhoneNumbers(string firstName, string lastName)
    {
        if( string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("First name and last name cannot be null or empty.");
        
        var key = GetKey(firstName, lastName);
        var chars = GetChars(key.ToLower());

        if (_phoneDirectory.TryGetValue(chars[0], out var existingDict1) &&
            existingDict1.TryGetValue(chars[1], out var existingDict2) &&
            existingDict2.TryGetValue(chars[2], out var existingDict3) &&
            existingDict3.TryGetValue(key, out var phoneNumbers))
        {
            return phoneNumbers;
        }
        return Enumerable.Empty<string>();
    }
    
    public List<KeyValuePair<string, HashSet<string>>> FindByThreeLetters(string threeLetters)
    {
        if (string.IsNullOrWhiteSpace(threeLetters) || threeLetters.Length < 3)
            throw new ArgumentException("Input must be exactly three letters.");

        var chars = GetChars(threeLetters);

        var results = new List<KeyValuePair<string, HashSet<string>>>();

        if (_phoneDirectory.TryGetValue(chars[0], out var dict1) &&
            dict1.TryGetValue(chars[1], out var dict2) &&
            dict2.TryGetValue(chars[2], out var dict3))
        {
            foreach (var entry in dict3)
            {
                results.Add(entry);
            }
        }

        return results;
    }
}