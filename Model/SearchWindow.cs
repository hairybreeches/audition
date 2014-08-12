
namespace Model
{
    public class SearchWindow
    {
        TimeFrame TimeFrame { get; set; }
        Period Period { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, in the period {1}", TimeFrame, Period);
        }
    }
}