namespace Model
{
    public interface IValueLookup<in TIn, out TOut>
    {
        TOut GetLookupValue(TIn key);
    }
}