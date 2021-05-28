using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteTecnico.Models;
using TesteTecnico.Models.Classes;

namespace TesteTecnico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartoesController : ControllerBase
    {
        private readonly BankContext _context;

        public CartoesController(BankContext context) 
        {
            _context = context;
        }

        // GET: api/Cartoes/5
        [HttpGet("{email}")]
        public async Task<ActionResult<Cartao>> GetCartao(string email)
        {
            if (email == null)
            {
                return BadRequest();
            }

            var cartao =  _context.Cartoes.Where(a => a.Email == email);

            if (cartao == null)
            {
                return NotFound();
            }
            
            return Ok(cartao);
        }

        // POST: api/Cartoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cartao>> PostCartao(string email)
        {
            if (!ValidaEmail(email))
            {
                return BadRequest("Email Inválido");
            }
            Cartao cartao = new Cartao();
            cartao.Numero = CreateCartao();
            cartao.Email = email;

            _context.Cartoes.Add(cartao);
            await _context.SaveChangesAsync();

            return Ok(cartao.Numero);
        }

        private string CreateCartao()
        {
            Random rnd = new Random();
            double rndNumber = Convert.ToDouble(DateTime.Now.ToString("ddMMyyyyHH00")) + rnd.Next(10, 99);
            return rndNumber.ToString();
        }

        private bool ValidaEmail (string email)
        {
            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            if (rg.IsMatch(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       
    }
}
