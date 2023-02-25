using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposControllers : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public equiposControllers(equiposContext equiposContexto)
        
        {
             _equiposContexto = equiposContexto;

        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get() 
        {
            List<equipos> listadoequipo = (from e in _equiposContexto.equipos
                                           select e).ToList();

            if(listadoequipo.Count == 0) 
            {
                return NotFound();
            }
            return Ok(listadoequipo);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id) 
        {
            equipos? equipo =  (from e in _equiposContexto.equipos
                                where e.id_equipos== id
                                select e).FirstOrDefault();
            if (equipo == null) 
            {
                return NotFound();  
            }
            return Ok(equipo);
        }
        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro) 
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();
            if (equipo == null) 
            {
                return NotFound();
            }
            return Ok(equipo);



        }
        [HttpGet]
        [Route("ADD")]
        public IActionResult GuardarEquipo ([FromBody]equipos equipo)
        {
            try
            {
                _equiposContexto.equipos.Add(equipo);
                _equiposContexto.SaveChanges(); 
                return Ok(equipo);  
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/(id)")]
        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificar) 
        {
            equipos? equipoActual = (from e in _equiposContexto.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();

            if (equipoActual == null)
            {
                return NotFound();
            }

            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(equipoModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id) 
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();

            if (equipo == null) 
                return NotFound();

            _equiposContexto.equipos.Attach(equipo);
            _equiposContexto.equipos.Remove(equipo);
            _equiposContexto.SaveChanges();
            return Ok(equipo);
        }


    }

}
