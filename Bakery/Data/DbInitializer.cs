using Bakery.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BakeryDataContext context)
        {
            context.Database.EnsureCreated();

            if(context.Products.Any())
            {
                return;
            }

            var products = new Product[]
            {
                new Product{Name="Carrot Cake",Description="A scrumptious mini-carrot cake encrusted with sliced almonds",Price=8.99m,ImageName="carrot_cake.jpg"},
                new Product{Name="Lemon Tart",Description="A delicious lemon tart with fresh meringue cooked to perfection",Price=9.99m,ImageName="lemon_tart.jpg"},
                new Product{Name="Cupcakes",Description="Delectable vanilla and chocolate cupcakes",Price=5.99m,ImageName="cupcakes.jpg"},
                new Product{Name="Bread",Description="Fresh baked French-style bread",Price=1.49m,ImageName="bread.jpg"},
                new Product{Name="Pear Tart",Description="A glazed pear tart topped with sliced almonds and a dash of cinnamon",Price=5.99m,ImageName="pear_tart.jpg"},
                new Product{Name="Chocolate Cake",Description="Rich chocolate frosting cover this chocolate lover’s dream.",Price=8.99m,ImageName="chocolate_cake.jpg"},
            
            };

            foreach (Product product in products)
            {
                context.Products.Add(product);
            }
            context.SaveChanges();
        }
    }
}
