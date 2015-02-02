using System;
using System.Linq;

namespace Model
{
    public static class Enums
    {
        public static T[] GetAllValues<T>()
        {
            return Enum.GetValues(typeof(T)).OfType<T>().ToArray();            
        }       
    }
}