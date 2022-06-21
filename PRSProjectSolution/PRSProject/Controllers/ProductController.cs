using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRSProject.Models;

namespace PRSProject.Controllers
{
    [Route("api/Products")] //Will default to specified URL/route https://localhost:####/api/Products
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly PRSDbContext _context;

        public ProductController(PRSDbContext context)
        {
            _context = context;
        }

        // GET: List/Search - ALL Products
        // Purpose: Returns ALL information for ALL products represented on table
        [HttpGet] //Defaults to specified URL/route, api/Products
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            return await _context.Products.ToListAsync();
        }

        // GET: Search - By ID
        // Purpose: Returns single specified product and ALL of the product's information
        [HttpGet("{id}")]//Defines precise route  - api/Products/<insert Id>
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound("Invalid Product ID. Match Not Found."); //404 Error & Detail Message
            }

            return product;
        }

        // PUT: Update Product
        // Purpose: Update an existing product 
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")] //Defines precise route - api/Products/<insert Id>
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("ID Mismatch Detected. Cannot Modify ID."); //404 Error & Detail Message
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound("Invalid ID. Product Does Not Exist"); //404 Error & Detail Message
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: Create a product
        // Purpose: Add a new product 
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost] //Defaults to specified URL/route, api/Products
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'PRSDbContext.Products'  is null.");
          }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: Delete Product
        // Purpose: Remove an existing product
        [HttpDelete("{id}")] //Defines precise route - api/Products/<insert Id>
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Invalid ID. Cannot Delete. User Does Not Exist"); //404 Error & Detail Message); 
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
