// using System;
// using System.Text.Json.Serialization;
//
// namespace Brokers.Coinbase.Models
// {
//     public class OrderModel
//     {
//         public string Id { get; set; }
//         public string Status { get; set; }
//         /// <summary>
//         /// Total amount of money charged by Coinbase, including fees
//         /// </summary>
//         public Balance Total { get; set; }
//         public PaymentMethod PaymentMethod { get; set; }
//         /// <summary>
//         /// Subtotal amount of money charged by Coinbase for buying/selling crypto, does not include fees
//         /// </summary>
//         public Balance Subtotal { get; set; }
//         public Balance Amount { get; set; }
//         /// <summary>
//         /// Price of crypto when buying
//         /// </summary>
//         public Balance UnitPrice { get; set; }
//         public Balance Fee { get; set; }
//         public DateTime? CreatedAt { get; set; }
//         public DateTime? UpdatedAt { get; set; }
//         [JsonIgnore] public OrderStatus OrderStatus => Status.ParseCoinbaseStringToEnum<OrderStatus>();
//     }
//
//     public enum OrderStatus 
//     {
//         Unknown,
//         Created,
//         Completed,
//         Canceled,
//         Quote
//     }
//
// }
