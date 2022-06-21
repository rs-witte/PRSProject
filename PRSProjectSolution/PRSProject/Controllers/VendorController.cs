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
    [Route("api/Vendors")] //Will default to specified URL/route https://localhost:####/api/Vendors
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly PRSDbContext _context;

        public VendorController(PRSDbContext context)
        {
            _context = context;
        }

        // GET: List/Search - ALL Vendors
        // Purpose: Returns ALL information for ALL vendors represented on Vendors table
        [HttpGet] //Defaults to specified URL/route, api/Vendors
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendors()
        {
          if (_context.Vendors == null)
          {
              return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            return await _context.Vendors.ToListAsync();
        }

        // GET: Search - By ID
        // Purpose: Returns single specified user and ALL of the user's information
        [HttpGet("{id}")] //Defines precise route  - api/Vendors/<insert Id>
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
          if (_context.Vendors == null)
          {
              return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null)
            {
                return NotFound("Invalid Vendor ID. Match Not Found."); //404 Error & Detail Message
            }

            return vendor;
        }

        // TODO: GET: Search - By Name
        // Purpose:Returns vendors meeting search criteria ("like/contains") and ALL information for matching vendor(s)
        //[HttpGet]
        //[Route("VendorName/{name}")] //Defines precise route - api/User/Username/<insert username>
        //                               //Could add regex to differentiate between ID vs. Username search 
        //public async Task<ActionResult<Vendor>> GetVendorByName(string vendorName)
        //{
        //    if (_context.Vendors == null)
        //    {
        //        return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
        //    }

        //    var vendor = await _context.Vendors.Where(n => n.Name == vendorName).FirstAsync();

        //    if (vendor == null)
        //    {
        //        return NotFound("Invalid Vendor Name. Match Not Found."); //404 Error & Detail Message
        //    }

        //    return vendor;
        //}


        // PUT: Update Vendor
        //Purpose: Update an existing vendor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")] //Defines precise route - api/Vendors/<insert Id>
        public async Task<IActionResult> PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return BadRequest("ID Mismatch Detected. Cannot Modify ID."); //404 Error & Detail Message
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
                {
                    return NotFound("Invalid ID. Cannot Modify. Vendor Does Not Exist."); //404 Error & Detail Message
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); //Successful update does not currently return a message  
            //TODO: Add successful update message? 
        }

        // POST: Create Vendor
        // Purpose: Add a new Vendor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost] //Defaults to specified URL/route, api/Vendors
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor)
        {
          if (_context.Vendors == null)
          {
              return Problem("Entity set 'PRSDbContext.Vendors'  is null.");
          }
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            // Wizard-Created Line Removed: CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
            return vendor; 
        }

        // DELETE: Delete Vendor
        // Purpose: Delete an existing vendor
        // TODO: Review allowing vendor delete without other checks, could create orphans on associated table(s)
        //    Cascading delete likely wouldn't be ideal, removes vendor AND any associated data on other table(s)
        //    Idea: Would be best to interpret DELETE request as "INACTIVATE" for vendors with related entries on other table(s)
        //          ---> Add "isInactive" or "isActive" column to DB table
        [HttpDelete("{id}")] //Defines precise route - api/Vendors/<insert Id>
        public async Task<IActionResult> DeleteVendor(int id)
        {
            if (_context.Vendors == null)
            {
                return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound("Invalid ID. Cannot Delete. Vendor Does Not Exist"); //404 Error & Detail Message
            }

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VendorExists(int id)
        {
            return (_context.Vendors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
