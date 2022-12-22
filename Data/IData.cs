
namespace ReMouse.Data
{
    public interface IData
    {
        string Name { get; }

        string SaveAsJson();
    }
}