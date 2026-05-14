using System;
using System.Collections.Generic;

namespace FoodDeliverySystem
{
    // 5. Payment interface (Dependency Inversion Principle)
    public interface IPaymentProcessor
    {
        void ProcessPayment(double amount);
    }

    public class CreditCardPayment : IPaymentProcessor
    {
        public void ProcessPayment(double amount) => 
            Console.WriteLine($"Processing credit card payment of ${amount:F2}...");
    }

    // 4. Encapsulated order details
    public class Order
    {
        public string ItemName { get; }
        public double Price { get; }

        public Order(string itemName, double price)
        {
            ItemName = itemName;
            Price = price;
        }
    }

    // 1. Abstract Restaurant
    public abstract class Restaurant
    {
        public string Name { get; }
        protected Restaurant(string name) => Name = name;

        // 3. Different delivery rules (Polymorphism)
        public abstract double CalculateDeliveryFee(double orderAmount);

        public void PlaceOrder(Order order, IPaymentProcessor paymentProcessor)
        {
            double deliveryFee = CalculateDeliveryFee(order.Price);
            double total = order.Price + deliveryFee;

            Console.WriteLine($"\n--- Order for {Name} ---");
            Console.WriteLine($"Item: {order.ItemName}");
            Console.WriteLine($"Subtotal: ${order.Price:F2}");
            Console.WriteLine($"Delivery Fee: ${deliveryFee:F2}");
            Console.WriteLine($"Total: ${total:F2}");

            paymentProcessor.ProcessPayment(total);
        }
    }

    // 2. FastFood Restaurant
    public class FastFood : Restaurant
    {
        public FastFood(string name) : base(name) { }

        // Fast food has a flat delivery fee
        public override double CalculateDeliveryFee(double orderAmount) => 5.00;
    }

    // 2. Kenacvale Restaurant (Fine Dining)
    public class Kenacvale : Restaurant
    {
        public Kenacvale(string name) : base(name) { }

        // Kenacvale has a percentage-based fee + service charge
        public override double CalculateDeliveryFee(double orderAmount) => (orderAmount * 0.15) + 10.00;
    }

    // 6. Add restaurants without changing system (Open/Closed Principle)
    // The DeliveryManager handles any Restaurant type without knowing their specific delivery logic.
    public class DeliveryManager
    {
        private readonly List<Restaurant> _restaurants = new List<Restaurant>();

        public void RegisterRestaurant(Restaurant restaurant) => _restaurants.Add(restaurant);

        public void ShowAvailableRestaurants()
        {
            Console.WriteLine("Available Restaurants:");
            foreach (var r in _restaurants)
            {
                Console.WriteLine($"- {r.Name} ({r.GetType().Name})");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var manager = new DeliveryManager();
            
            // Registering different types of restaurants
            manager.RegisterRestaurant(new FastFood("KFC"));
            manager.RegisterRestaurant(new Kenacvale("Kenacvale"));

            manager.ShowAvailableRestaurants();

            // Client chooses a restaurant and a payment method
            Restaurant fastFood = new FastFood("KFC");
            Restaurant kenacvale = new Kenacvale("Kenacvale");
            IPaymentProcessor payment = new CreditCardPayment();

            // Create orders
            Order burger = new Order("Zinger Box", 12.50);
            Order khnkali = new Order("Khnkali", 85.00);

            // Process orders
            fastFood.PlaceOrder(burger, payment);
            kenacvale.PlaceOrder(khnkali, payment);
        }
    }
}
