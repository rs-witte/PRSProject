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
    [Route("api/RequestLines")] //Will default to specified URL/route https://localhost:####/api/RequestLines
    [ApiController]
    public class RequestLineController : ControllerBase
    {
        private readonly PRSDbContext _context;

        public RequestLineController(PRSDbContext context)
        {
            _context = context;
        }

        // GET: List/Search - ALL Request Lines
        // Purpose: Returns ALL information for ALL request lines represented on table
        [HttpGet] //Defaults to specified URL/route, api/RequestLines
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLines()
        {
          if (_context.RequestLines == null)
          {
              return NotFound("Expected Database Table Missing."); //404 Error & Detail Message;
            }
            return await _context.RequestLines.ToListAsync();
        }

        // GET: Search - By ID
        // Purpose: Returns single specified request line entry and ALL of the request line's information
        [HttpGet("{id}")] //Defines precise route  - api/RequestLines/<insert Id>
        public async Task<ActionResult<RequestLine>> GetRequestLine(int id)
        {
          if (_context.RequestLines == null)
          {
              return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            var requestLine = await _context.RequestLines.FindAsync(id);

            if (requestLine == null)
            {
                return NotFound("Invalid Request Line ID. Match Not Found."); //404 Error & Detail Message
            }

            return requestLine;
        }

        // PUT: Update Request Line
        // Purpose: Update an existing request line
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")] //Defines precise route - api/RequestLines/<insert Id>
        public async Task<IActionResult> PutRequestLine(int id, RequestLine requestLine)
        {
            if (id != requestLine.ID)
            {
                return BadRequest("ID Mismatch Detected. Cannot Modify ID."); //404 Error & Detail Message
            }



            _context.Entry(requestLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                RecalculateTotal(requestLine.RequestID);
            }
            catch (Exception Ex)
            {
                if (!RequestLineExists(id))
                {
                    return NotFound("Invalid ID. Request Line Does Not Exist"); //404 Error & Detail Message
                    
                }
                else
                {
                    return BadRequest(Ex.Message); 
                }  
            
            }

            return NoContent();
        }

        // POST: Create Request Line
        // Purpose: Create a new request line
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost] //Defaults to specified URL/route, api/RequestLines 
        public async Task<ActionResult<RequestLine>> PostRequestLine(RequestLine requestLine)
        {
          if (_context.RequestLines == null)
          {
              return Problem("Entity set 'PRSDbContext.RequestLines'  is null.");
          }
            _context.RequestLines.Add(requestLine);
            await _context.SaveChangesAsync();

            RecalculateTotal(requestLine.RequestID);

            return CreatedAtAction("GetRequestLine", new { id = requestLine.ID }, requestLine);
        }

        // DELETE: Delete Request Line
        // Purpose: Remove an existing request line
        [HttpDelete("{id}")] //Defines precise route - api/RequestLines/<insert Id>
        public async Task<IActionResult> DeleteRequestLine(int id)
        {
            if (_context.RequestLines == null)
            {
                return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            var requestLine = await _context.RequestLines.FindAsync(id);
            if (requestLine == null)
            {
                return NotFound("Invalid ID. Cannot Delete. User Does Not Exist"); //404 Error & Detail Message); 
            }

            _context.RequestLines.Remove(requestLine);
            await _context.SaveChangesAsync();

            RecalculateTotal(requestLine.RequestID);

            return NoContent();
        }

        private bool RequestLineExists(int id)
        {
            return (_context.RequestLines?.Any(e => e.ID == id)).GetValueOrDefault();
        }
        private void RecalculateTotal(int requestId)
        {
            decimal total = _context.RequestLines.Include(rl => rl.Product)
                   .Where(rl => rl.RequestID == requestId)
                   .Sum(rl => rl.Product.Price * rl.Quantity);
            var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);
            request.Total = total;
            //TODO: Add Try...Catch
            _context.SaveChanges();
        
        }

    }
}
