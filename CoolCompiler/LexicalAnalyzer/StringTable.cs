using System.Collections.Generic;

namespace CoolCompiler
{
    public class StringTable
    {
        private List<string> _table = new List<string>();

        public int addString(string inputString)
        {
            var stringIndex = _table.IndexOf(inputString);

            if (stringIndex != -1)
            {
                return stringIndex;
            }
            
            _table.Add(inputString);
            return _table.Count;
        }

        public string GetString(int index) => _table.Count < index ? string.Empty : _table[index - 1];

    }
}