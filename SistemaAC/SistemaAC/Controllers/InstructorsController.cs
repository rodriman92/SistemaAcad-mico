using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaAC.Data;
using SistemaAC.Models;
using SistemaAC.ModelsClass;

namespace SistemaAC.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private InstructorModels instructor;

        public InstructorsController(ApplicationDbContext context)
        {
            _context = context;
            instructor = new InstructorModels(context);

        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Instructor.ToListAsync());
        }

        public List<IdentityError> guardarInstructor(List<Instructor> response,int funcion)
        {
            return instructor.guardarInstructor(response, funcion );
        }

        public List<object[]> filtrarInstructores(int numPagina, string valor, string order)
        {
            return instructor.filtrarInstructores(numPagina, valor, order);
        }

        public List<IdentityError> deleteInstructor(int id)
        {
            return instructor.deleteInstructor(id);
        }

        public List<Instructor> getInstructor(int id)
        {
            return instructor.getInstructor(id);
        }
    }


 
}
