using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bakery.Data;
using Bakery.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BakeryDataContext _context;
        public IndexModel(BakeryDataContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; }

        public Product FeaturedProduct { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _context.Products.AsNoTracking().ToListAsync();
            FeaturedProduct = Products[new Random().Next(Products.Count)];
        }
    }
}
