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
    [Route("api/Requests")] //Will default to specified URL/route https://localhost:####/api/Requests
    [ApiController] 
    public class RequestController : ControllerBase
    {
        private readonly PRSDbContext _context;

        public RequestController(PRSDbContext context)
        {
            _context = context;
        }

        // GET: List/Search - ALL Requests
        // Purpose: Returns ALL information for ALL requests represented on table
        [HttpGet] //Defaults to specified URL/route, api/Requests
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
          if (_context.Requests == null)
          {
              return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            return await _context.Requests.ToListAsync();
        }

        // GET: Search - By Request ID
        // Purpose: Returns single specified request and ALL of the request's information
        [HttpGet("{id}")] //Defines precise route  - api/Requests/<insert Id>
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
          if (_context.Requests == null)
          {
              return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound("Invalid Request ID. Match Not Found."); //404 Error & Detail Message
            }

            return request;
        }

        
        //GET: List Review Status Requests 
        //Purpose: Returns all REVIEW status requests for all users EXCEPT User ID added to URL (logged in user)
        [HttpGet("review/{UserId}")] //Defines precise route  - api/Requests/review/<insert Id>
        public async Task<ActionResult<IEnumerable<Request>>> GetReviews(int UserId)
        {
            if (_context.Requests == null)
            {
                return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
             var requests = await _context.Requests.Where(r => r.Status == "REVIEW" && r.UserId != UserId).ToListAsync(); 
        
            if (requests == null)
            {
                return NotFound("Invalid Request ID. Match Not Found."); //404 Error & Detail Message
            }

            return requests;
        }


        // PUT: Update Request Details - By Request ID
        // Purpose: Update/modify specified existing request's details
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")] //Defines precise route - api/Requests/<insert Id>
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID Mismatch Detected. Cannot Modify ID."); //404 Error & Detail Message
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound("Invalid ID. Request Does Not Exist"); //404 Error & Detail Message
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        //PUT: Update Request Status (REVIEW - By Request ID)
        //Purpose: Change existing request's status to REVIEW (Total > $50) 
        //Purpose: Automatically sets existing request's status to APPROVED (Total <= $50)  
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/review")] //Defines precise route  - api/Requests/<insert Id>/review
        public async Task<ActionResult<Request>> PutReview(int id)
        {
           
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound("Invalid Request ID. Match Not Found."); //404 Error & Detail Message
            }
            if (request.Total <= 50m)
            {
                request.Status = "APPROVED";
            }
            else
            {
                request.Status = "REVIEW";
            }

           await _context.SaveChangesAsync();
            return request;
        }


        //PUT: Update Request Status (APPROVE - By Request ID)
        // Purpose: Update status of existing request to APPROVED
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/approve")] //Defines precise route  - api/Requests/<insert Id>/approve
        public async Task<ActionResult<Request>> PutApprove(int id)
        {

            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound("Invalid Request ID. Match Not Found."); //404 Error & Detail Message
            }
  
                request.Status = "APPROVED";

            await _context.SaveChangesAsync();
            return request;
        }


        //PUT: Update Request Status (REJECT - By Request ID)
        //Purpose: Update status of existing specified request to REJECTED
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/reject")] //Defines precise route  - api/Requests/<insert Id>/reject
        public async Task<ActionResult<Request>> PutReject(int id)
        {

            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound("Invalid Request ID. Match Not Found."); //404 Error & Detail Message
            }

            request.Status = "REJECTED";

            await _context.SaveChangesAsync();
            return request;
        }

        // POST: Create Request
        // Purpose: Add a new request
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost] // Defaults to specified URL/route, api/Requests
        public async Task<ActionResult<Request>> PostRequest(Request request) 
        {
          if (_context.Requests == null)
          {
              return Problem("Entity set 'PRSDbContext.Requests'  is null.");
          }
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: Delete Request
        // Purpose: Remove an existing request
        [HttpDelete("{id}")] //Defines precise route - api/Requests/<insert Id>
        public async Task<IActionResult> DeleteRequest(int id)
        {
            if (_context.Requests == null)
            {
                return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound("Invalid ID. Cannot Delete. User Does Not Exist"); //404 Error & Detail Message); 
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return (_context.Requests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
