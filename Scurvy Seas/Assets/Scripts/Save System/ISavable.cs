public interface ISavable
{
    public void Save(ref SaveData saveData);
    public void Load(SaveData saveData);
}
