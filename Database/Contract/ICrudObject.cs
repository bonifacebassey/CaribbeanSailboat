namespace CaribbeanSailboat.Database.Contract;

public interface ICrudObject<T> where T : class
{
    T CreateItem();
    void AddItem(T item);
    void UpdateItem(T item);
    void DeleteItem(T item);
}
