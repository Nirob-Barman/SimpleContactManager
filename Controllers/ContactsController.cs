using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleContactManager.Data;
using SimpleContactManager.Middleware;
using SimpleContactManager.Models;
using SimpleContactManager.Shared;

namespace SimpleContactManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ContactsController(AppDbContext context, ILogger<ExceptionMiddleware> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostContact([FromBody] Contact contact)
        {
            if (!ModelState.IsValid)
            {
                var modelStateErrors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                _logger.LogWarning("Validation failed for contact creation. Errors: {Errors}", string.Join(", ", modelStateErrors));
                return BadRequest(new Response<Contact>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "Validation failed",
                    Data = null,
                    Errors = modelStateErrors
                });
            }
            var existingContact = await _context.Contacts
                .Where(c => c.PhoneNumber == contact.PhoneNumber || c.Email == contact.Email)
                .Select(c => new { c.PhoneNumber, c.Email })
                .FirstOrDefaultAsync();

            var duplicateErrorMessages = new List<string>();
            if (existingContact != null)
            {
                if (contact.PhoneNumber == existingContact.PhoneNumber)
                {
                    duplicateErrorMessages.Add($"A contact with the phone number '{existingContact.PhoneNumber}' already exists.");
                }
                if (contact.Email == existingContact.Email)
                {
                    duplicateErrorMessages.Add($"A contact with the email address '{existingContact.Email}' already exists.");
                }
            }
            if (duplicateErrorMessages.Any())
            {
                //_logger.LogWarning("Duplicate contact detected. Errors: {Errors}", string.Join(", ", duplicateErrorMessages));
                return Conflict(new Response<Contact>
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Success = false,
                    Message = "Duplicate contact detected",
                    Data = null,
                    Errors = duplicateErrorMessages
                });
            }

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return Ok(new Response<Contact>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = "Contact created successfully",
                Data = new Contact
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    PhoneNumber = contact.PhoneNumber,
                    Email = contact.Email,
                    Address = contact.Address
                },
                Errors = null
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetContacts(
            [FromQuery] string searchTerm = "",
            [FromQuery] string sortBy = "Name",
            [FromQuery] bool sortDescending = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                return BadRequest(new Response<PagedData<Contact>>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "Invalid page number",
                    Data = null,
                    Errors = new List<string> { "Page number must be greater than or equal to 1." }
                });
            }

            if (pageSize <= 0)
            {
                return BadRequest(new Response<PagedData<Contact>>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "Invalid page size",
                    Data = null,
                    Errors = new List<string> { "Page size must be greater than 0." }
                });
            }

            var query = _context.Contacts.AsQueryable();

            // Search functionality
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                query = query.Where(c =>
                    c.Name!.ToLower().Contains(searchTerm) ||
                    c.PhoneNumber!.Contains(searchTerm) ||
                    c.Email!.ToLower().Contains(searchTerm));
            }

            // Sorting functionality
            switch (sortBy.ToLower())
            {
                case "name":
                    query = sortDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name);
                    break;
                case "phonenumber":
                    query = sortDescending ? query.OrderByDescending(c => c.PhoneNumber) : query.OrderBy(c => c.PhoneNumber);
                    break;
                case "email":
                    query = sortDescending ? query.OrderByDescending(c => c.Email) : query.OrderBy(c => c.Email);
                    break;
                default:
                    query = sortDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name);
                    break;
            }

            var totalContacts = await query.CountAsync();

            if (totalContacts == 0)
            {
                return Ok(new Response<PagedData<Contact>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Success = true,
                    Message = "No contacts found",
                    Data = new PagedData<Contact>
                    {
                        Data = new List<Contact>(),
                        TotalCount = 0,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        CurrentPageDataCount = totalContacts
                    },
                    Errors = null
                });
            }

            var contacts = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new Response<PagedData<Contact>>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = "Contacts retrieved successfully",
                Data = new PagedData<Contact>
                {
                    CurrentPageDataCount = contacts.Count,
                    Data = contacts,
                    TotalCount = totalContacts,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                Errors = null
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _context.Contacts.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (contact == null)
            {
                return NotFound(new Response<Contact>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false,
                    Message = "Contact not found",
                    Data = null,
                    Errors = new List<string> { "The requested contact does not exist." }
                });
            }

            return Ok(new Response<Contact>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = "Contact retrieved successfully",
                Data = contact,
                Errors = null
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] Contact updatedContact)
        {
            if (!ModelState.IsValid)
            {
                var modelStateErrors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                _logger.LogWarning("Validation failed for contact update. Errors: {Errors}", string.Join(", ", modelStateErrors));
                return BadRequest(new Response<Contact>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "Validation failed",
                    Data = null,
                    Errors = modelStateErrors
                });
            }

            var existingContact = await _context.Contacts.FindAsync(id);

            if (existingContact == null)
            {
                return NotFound(new Response<Contact>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false,
                    Message = "Contact not found",
                    Data = null,
                    Errors = new List<string> { "The contact you are trying to update does not exist." }
                });
            }

            // Check for duplicates
            var duplicateContact = await _context.Contacts
                .Where(c => (c.PhoneNumber == updatedContact.PhoneNumber || c.Email == updatedContact.Email) && c.Id != id).FirstOrDefaultAsync();

            if (duplicateContact != null)
            {
                var duplicateErrorMessages = new List<string>();
                if (updatedContact.PhoneNumber == duplicateContact.PhoneNumber)
                {
                    duplicateErrorMessages.Add($"A contact with the phone number '{duplicateContact.PhoneNumber}' already exists.");
                }
                if (updatedContact.Email == duplicateContact.Email)
                {
                    duplicateErrorMessages.Add($"A contact with the email address '{duplicateContact.Email}' already exists.");
                }

                if (duplicateErrorMessages.Any())
                {
                    return Conflict(new Response<Contact>
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        Success = false,
                        Message = "Duplicate contact detected",
                        Data = null,
                        Errors = duplicateErrorMessages
                    });
                }
            }

            // Update fields
            existingContact.Name = updatedContact.Name;
            existingContact.PhoneNumber = updatedContact.PhoneNumber;
            existingContact.Email = updatedContact.Email;
            existingContact.Address = updatedContact.Address;

            _context.Contacts.Update(existingContact);
            await _context.SaveChangesAsync();

            return Ok(new Response<Contact>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = "Contact updated successfully",
                Data = existingContact,
                Errors = null
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound(new Response<Contact>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Success = false,
                    Message = "Contact not found",
                    Data = null,
                    Errors = new List<string> { "The contact you are trying to delete does not exist." }
                });
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return Ok(new Response<Contact>
            {
                Success = true,
                Message = "Contact deleted successfully",
                Data = contact,
                Errors = null
            });
        }
    }
}
