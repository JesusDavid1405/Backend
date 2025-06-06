﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;
using Data.Interfaces;
using Entity.context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    /// <summary>
    /// Repositorio concreto para la entidad Person.
    /// Hereda los métodos genéricos de GenericRepository e implementa IPersonRepository,
    /// permitiendo así extender o sobreescribir funcionalidades específicas si es necesario.
    /// 
    /// Esta clase actúa como punto central para acceder a la información de personas,
    /// y puede crecer con operaciones específicas de la entidad (ej. búsqueda por DNI, nombre, etc.).
    /// </summary>
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        /// <summary>
        /// Constructor del repositorio de personas.
        /// Recibe el contexto de base de datos y el logger para rastreo de operaciones.
        /// Estos se pasan al repositorio base para ser utilizados en operaciones comunes.
        /// </summary>
        /// <param name="context">Instancia de ApplicationDbContext para acceso a datos.</param>
        /// <param name="logger">Instancia de ILogger para registrar logs y advertencias.</param>
        public PersonRepository(ApplicationDbContext context, ILogger<PersonRepository> logger)
        : base(context, logger) { }
        // Aquí pueden agregarse métodos específicos para Person, por ejemplo:
        // public async Task<Person?> GetByDocumentAsync(string dni)
        // {
        //     return await _context.People.FirstOrDefaultAsync(p => p.Document == dni);
        // }
    }
}
