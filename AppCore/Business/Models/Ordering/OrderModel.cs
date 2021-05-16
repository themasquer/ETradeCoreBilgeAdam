namespace AppCore.Business.Models.Ordering
{
    // Sıralama
    public class OrderModel
    {
        public string Expression { get; set; }
        public bool DirectionAscending { get; set; } = true;
    }
}
