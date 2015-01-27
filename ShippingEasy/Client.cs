using System;
using Newtonsoft.Json;

namespace ShippingEasy
{
    public class Client
    {
        public Connection Connection { get; private set; }

        public Client(string apiKey, string apiSecret, string baseUrl)
        {
            Connection = new Connection(apiKey, apiSecret, baseUrl);
        }

        public Client(Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Create a new order in your ShippingEasy account
        /// </summary>
        /// <param name="storeApiKey">The Store API Key that identifies the store where the order will be created.
        /// <remarks>Not to be confused with your account's API Key which is used for authentication.</remarks>
        /// </param>
        /// <param name="order">The details of the order that will be created within the ShippingEasy system.</param>
        /// <returns></returns>
        public CreateOrderResponse CreateOrder(string storeApiKey, Order order)
        {
            var body = String.Format("{{\"order\": {0}}}", OrderToJson(order));
            var responseBody = Connection.CreateOrderFromJson(storeApiKey, body);
            return new ResponseParser().Parse<CreateOrderResponse>(responseBody);
        }

        /// <summary>
        /// Downloads orders from your ShippingEasy account
        /// </summary>
        /// <param name="query">Optional values used to limit the orders that are returned.</param>
        /// <returns></returns>
        public OrderQueryResponse GetOrders(OrderQuery query = null)
        {
            query = query ?? new OrderQuery();
            var responseBody = query.StoreKey != null ?
                Connection.GetStoreOrdersJson(query.StoreKey, query.ToDictionary()) :
                Connection.GetAllOrdersJson(query.ToDictionary());
            return new ResponseParser().Parse<OrderQueryResponse>(responseBody);
        }

        public static string OrderToJson(Order order)
        {
            return JsonConvert.SerializeObject(order, Formatting.Indented, Serialization.Settings);
        }
    }
}