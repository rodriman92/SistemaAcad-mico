using Microsoft.AspNetCore.Identity;
using SistemaAC.Data;
using SistemaAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaAC.ModelsClass
{
    public class InstructorModels
    {
        private ApplicationDbContext context;
        private List<IdentityError> identityError;
        private string code = "", des = "";
        private Boolean estados;

        public InstructorModels(ApplicationDbContext context)
        {
            this.context = context;
            identityError = new List<IdentityError>();
        }

        public List<IdentityError> guardarInstructor(List<Instructor> response, int funcion)
        {
            switch (funcion)
            {
                case 0:
                    if (response[0].Estado)
                    {
                        estados = false;
                    }
                    else
                    {
                        estados = true;
                    }
                    break;
                case 1:
                    estados = response[0].Estado;
                    break;

            }
            var instructor = new Instructor
            {
                ID = response[0].ID,
                Especialidad = response[0].Especialidad,
                Nombres = response[0].Nombres,
                Apellidos = response[0].Apellidos,
                FechaNacimiento = response[0].FechaNacimiento,
                Documento = response[0].Documento,
                Email = response[0].Email,
                Telefono = response[0].Telefono,
                Direccion = response[0].Direccion,
                Estado = estados,

            };
            try
            {
                context.Update(instructor);
                context.SaveChanges();
                code = "1";
                des = "Save";
            }
            catch (Exception ex)
            {

                code = "0";
                des = ex.Message;
            }

            identityError.Add(new IdentityError
            {
                Code = code,
                Description = des
            });
            return identityError;
        }

        internal List<IdentityError> deleteInstructor(int id)
        {
            var instructor = context.Instructor.SingleOrDefault(c => c.ID == id);
            if (instructor==null)
            {
                code = "0";
                des = "Not";
            }
            else
            {
                context.Instructor.Remove(instructor);
                context.SaveChanges();
                code = "1";
                des = "Delete";
            }
            identityError.Add(new IdentityError
            {
                Code = code,
                Description = des

            });
            return identityError;
        
        }

        internal List<Instructor> getInstructor(int id)
        {
            return context.Instructor.Where(c => c.ID == id).ToList();

        }

        public List<object[]> filtrarInstructores(int numPagina, string valor, string order)
        {
            int cant, numRegistros = 0, inicio = 0, reg_por_pagina = 1;
            int can_paginas, pagina = 0, count = 1;
            string dataFilter = "", paginador = "", Estado = null;
            List<object[]> data = new List<object[]>();
            IEnumerable<Instructor> query;
            List<Instructor> instructor = null;

            instructor = context.Instructor.OrderBy(p => p.Nombres).ToList();
            numRegistros = instructor.Count;
            if ((numRegistros % reg_por_pagina) > 0)
            {
                numRegistros += 1;
            }
            inicio = (numPagina - 1) * reg_por_pagina;
            can_paginas = (numRegistros / reg_por_pagina);
            if (valor == "null")
                query = instructor.Skip(inicio).Take(reg_por_pagina);
            else
                query = instructor.Where(p => p.Documento.StartsWith(valor) || p.Nombres.StartsWith(valor) || p.Apellidos.StartsWith(valor)).Skip(inicio).Take(reg_por_pagina);
            cant = query.Count();
            foreach (var item in query)
            {
                if (item.Estado == true)
                    Estado = "<a onclick='editarInstructor(" + item.ID + ',' + 0 + ")' class='btn btn-success'>Activo</a>";
                else
                    Estado = "<a onclick='editarInstructor(" + item.ID + ',' + 0 + ")' class='btn btn-danger'>No activo</a>";

                dataFilter += "<tr>" +
                   "<td>" + item.Especialidad + "</td>" +
                   "<td>" + item.Documento + "</td>" +
                   "<td>" + item.Nombres + "</td>" +
                   "<td>" + item.Apellidos + "</td>" +
                   "<td>" + item.FechaNacimiento + "</td>" +
                   "<td>" + item.Telefono + "</td>" +
                   "<td>" + item.Email + "</td>" +
                   "<td>" + item.Direccion + "</td>" +
                   "<td>" + Estado + " </td>" +
                   "<td>" +
                   "<a data-toggle='modal' data-target='#modalAI' onclick='editarInstructor(" + item.ID + ',' + 1 + ")'  class='btn btn-success'>Editar</a>" +
                   "</td>" +
                   "<td>" +
                   "<a data-toggle='modal' data-target='#ModalDeleteAI' onclick='deleteInstructor(" + item.ID + ")'  class='btn btn-danger'>Borrar</a>" +
                   "</td>" +
               "</tr>";
            }
            if (valor == "null")
            {
                if (numPagina > 1)
                {
                    pagina = numPagina - 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarInstructores(" + 1 + ',' + '"' +
                        order + '"' + ")'> << </a>" +
                        "<a class='btn btn-default' onclick='filtrarInstructores(" + pagina + ',' + '"' + order +
                        '"' + ")'> < </a>";
                }
                if (1 < can_paginas)
                {
                    for (int i = numPagina; i <= can_paginas; i++)
                    {
                        paginador += "<strong class='btn btn-success' onclick='filtrarInstructores(" + i +
                            ',' + '"' + order + '"' + ")'>" + i + "</strong>";

                        if (count == 5)
                        {
                            break;
                        }
                        count++;
                    }
                }
                if (numPagina < can_paginas)
                {
                    pagina = numPagina + 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarInstructores(" + pagina + ',' + '"' +
                        order + '"' + ")'> > </a>" +
                        "<a class='btn btn-default' onclick='filtrarInstructores(" + can_paginas + ',' + '"' + order +
                        '"' + ")'> >> </a>";
                }
            }
            object[] dataObj = { dataFilter, paginador };
            data.Add(dataObj);
            return data;
        }
    }
}
