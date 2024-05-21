using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.DataAccess;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {   
        private readonly ApplicationContext _context;
        private readonly TokenService _tokenService;
        public ProductController(ApplicationContext context,TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login-user")]
        public IActionResult LoginUser([FromBody] LoginRequest request)
        {
            if (request.Username == "user" && request.Password == "password")
            {
                var token = _tokenService.GenerateToken(request.Username, "User");
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("login-admin")]
        public IActionResult LoginAdmin([FromBody] LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = _tokenService.GenerateToken(request.Username, "Admin");
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        [HttpGet("Products")]
        [Authorize(Roles ="User")]
        public IActionResult Get()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }
        
        [HttpGet("Product/{id}")]
        [Authorize(Roles ="Admin")]
        public IActionResult GetProduct([FromRoute] int id) {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if(product == null) {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("Products")]
        [Authorize(Roles ="Admin")]
        public IActionResult Save([FromBody]Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("Products")]
        [Authorize(Roles ="Admin")]
        public IActionResult Update([FromBody]Product product)
        {
            var result = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == product.Id);
            if(result == null) {
                return NotFound();
            }
            _context.Products.Update(product);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("Products")]
        [Authorize(Roles ="Admin")]
        public IActionResult Delete([FromQuery] int id) 
        {
            var deleteProduct = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if(deleteProduct == null) {
                return NotFound();
            }

            // _context.Products.Remove(deleteProduct);
            _context.Products.Entry(deleteProduct).State = EntityState.Deleted;
            _context.SaveChanges();
            return Ok();
            }
    }

}