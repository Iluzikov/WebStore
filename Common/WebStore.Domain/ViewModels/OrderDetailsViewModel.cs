namespace WebStore.Domain.ViewModels
{
    public class OrderDetailsViewModel
    {
        public CartViewModel Cart { get; set; }
        public OrderViewModel Order { get; set; } = new OrderViewModel();
    }
}
