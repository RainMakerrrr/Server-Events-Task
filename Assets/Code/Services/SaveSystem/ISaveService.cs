using Code.Data;

namespace Code.Services.SaveSystem
{
    public interface ISaveService
    {
        void Save(SaveData data);
        SaveData Load();
    }
}