using MyProjectJWT.Context;
using MyProjectJWT.DTO;
using MyProjectJWT.Interfaces;
using MyProjectJWT.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace MyProjectJWT.Services
{
    public class VaccineService : IVaccineService
    {
        private readonly JwtContext _context;

        public VaccineService(JwtContext context)
        {
            _context = context;
        }

        // Get all vaccines
        public List<VaccineDTO> GetAllVaccines()
        {
            return _context.Vaccines
                .Select(v => new VaccineDTO
                {
                    VaccineId = v.VaccineId,
                    VaccineName = v.VaccineName,
                    Description = v.Description,
                    RecommendedAge = v.RecommendedAge,
                    SideEffects = v.SideEffects
                })
                .ToList();
        }

        // Get vaccine by Id
        public VaccineDTO GetVaccineById(int id)
        {
            var vaccine = _context.Vaccines
                .Where(v => v.VaccineId == id)
                .Select(v => new VaccineDTO
                {
                    VaccineId = v.VaccineId,
                    VaccineName = v.VaccineName,
                    Description = v.Description,
                    RecommendedAge = v.RecommendedAge,
                    SideEffects = v.SideEffects
                })
                .FirstOrDefault();

            return vaccine;
        }

        // Add a new vaccine
        public VaccineDTO AddVaccine(CreateVaccineDTO createVaccine)
        {
            var vaccine = new Vaccines
            {
                VaccineName = createVaccine.VaccineName,
                Description = createVaccine.Description,
                RecommendedAge = createVaccine.RecommendedAge,
                SideEffects = createVaccine.SideEffects
            };

            _context.Vaccines.Add(vaccine);
            _context.SaveChanges();

            return new VaccineDTO
            {
                VaccineId = vaccine.VaccineId,
                VaccineName = vaccine.VaccineName,
                Description = vaccine.Description,
                RecommendedAge = vaccine.RecommendedAge,
                SideEffects = vaccine.SideEffects
            };
        }

        // Update an existing vaccine
        public VaccineDTO UpdateVaccine(VaccineDTO vaccineDTO)
        {
            var vaccine = _context.Vaccines.Find(vaccineDTO.VaccineId);

            if (vaccine == null)
            {
                return null;
            }

            vaccine.VaccineName = vaccineDTO.VaccineName;
            vaccine.Description = vaccineDTO.Description;
            vaccine.RecommendedAge = vaccineDTO.RecommendedAge;
            vaccine.SideEffects = vaccineDTO.SideEffects;

            _context.Vaccines.Update(vaccine);
            _context.SaveChanges();

            return vaccineDTO;
        }

        // Delete a vaccine by Id
        public bool DeleteVaccine(int id)
        {
            var vaccine = _context.Vaccines.Find(id);

            if (vaccine == null)
            {
                return false;
            }

            _context.Vaccines.Remove(vaccine);
            _context.SaveChanges();

            return true;
        }
    }
}
