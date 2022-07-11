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
    [Route("api/Users")] //Will default to specified URL/route https://localhost:####/api/Users
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PRSDbContext _context;

        public UserController(PRSDbContext context)
        {
            _context = context;
        }

        // GET: List/Search - ALL Users
        // Purpose: Returns ALL information for ALL users represented on Users table
        [HttpGet] //Defaults to specified URL/route, api/Users
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            if (_context.Users == null)
            {
                return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }

            return await _context.Users.ToListAsync(); 
        }

        // GET: Search - By ID
        // Purpose: Returns single specified user and ALL of the user's information
        [HttpGet]
        [Route("{id}")] //Defines precise route  - api/Users/<insert Id>
        public async Task<ActionResult<User>> GetUserByID(int id)
        {
            if (_context.Users == null)
            {
                return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("Invalid User ID. Match Not Found."); //404 Error & Detail Message
            }

            return user;  
        }

        // GET: Search - By Username
        // Purpose:Returns single specified user and ALL of the user's information
        [HttpGet]
        [Route("Username/{username}")] //Defines precise route - api/Users/Username/<insert username>
                                       //Could add regex to differentiate between ID vs. Username search 
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            if (_context.Users == null)
            {
                return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }

            var user = await _context.Users.Where(u => u.Username == username).FirstAsync();

            if (user == null)
            {
                return NotFound("Invalid Username. Match Not Found."); //404 Error & Detail Message
            }

            return user; 
        }


        // GET: Authentication - User Credentials
        // Purpose: Returns user information for single user, credentials (UN & PW) must exactly match entered combination
        [HttpGet]
        [Route("{username}/{password}")] //Defines precise route - api/Users/<insert username>/<insert password>
        public async Task<ActionResult<User>> GetUserCredentials(string username, string password)
        {
            if (_context.Users == null)
            {
                return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }

            var user = await _context.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
      

            if (user == null) //match not found
            {
                return NotFound("Invalid Credentials. Please Try Again."); //404 Error & Detail Message
            }

            
            return user;
            //TODO: Return Only: ID, Firstname, Lastname, isReviewer, isAdmin 
            //Something like ---> return _context.Users.Select(u => new u.Username, u.Firstname, u.Lastname);
        }


        // PUT: Update User
        // Purpose: Update an existing user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")] //Defines precise route - api/Users/<insert Id>
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.ID)
            {
                return BadRequest("ID Mismatch Detected. ID Modification Not Permitted."); //404 Error & Detail Message
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound("Invalid ID. User Does Not Exist"); //404 Error & Detail Message
                }
               // else
               // {
               //     throw;
               // }
            }

            return NoContent(); //Successful update does not currently return a message    
            //TODO: Add successful update message? 
        }

        // POST: Create User
        //Purpose: Add a new user
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost] //Defaults to specified URL/route, api/Users
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)

          {
              return Problem("Entity set 'PRSDbContext.Users'  is null.");
          }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //Wizard-Created Line Removed: CreatedAtAction("GetUser", new { id = user.ID }, user);
            return user; 
        }

        // DELETE: Delete User
        // Purpose: Remove an existing user
        // TODO: Review allowing delete without other checks could create orphans on associated tables
        //    Review allowing cascading delete would remove user AND any associated requests and request lines
        //    Idea: Would be best to interpret DELETE request as "INACTIVATE" for users with entries on Requests and RequestLines
        //          ---> Create/Add "isInactive" or "isActive" column to DB table
        [HttpDelete("{id}")] //Defines precise route - api/Users/<insert Id>
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound("Expected Database Table Missing."); //404 Error & Detail Message
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("Invalid ID. Cannot Delete. User Does Not Exist"); //404 Error & Detail Message); 
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
