namespace RxMessageBus.Demo.Models
{
    public class Item
    {
        public string Name { get; set; }
        
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"{Name} - ${Quantity}";
        }
    }
}