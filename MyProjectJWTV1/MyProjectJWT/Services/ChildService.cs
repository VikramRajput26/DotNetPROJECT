using MyProjectJWT.Context;
using MyProjectJWT.DTO;
using MyProjectJWT.Interfaces;
using MyProjectJWT.Models;

namespace MyProjectJWT.Services
{
    public class ChildService : IChildService
    {
        private readonly JwtContext _context;

        public ChildService(JwtContext context)
        {
            _context = context;
        }

        public List<ChildDTO> GetChildDetails()
        {
            var children = _context.Children.Select(c => new ChildDTO
            {
                ChildId = c.ChildId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                BloodType = c.BloodType,
                Gender = c.Gender,
                DateOfBirth = c.DateOfBirth,
                UserId = c.UserId
            }).ToList();

            return children;
        }

        public ChildDTO GetChildById(int id)
        {
            var child = _context.Children.Find(id);
            if (child == null) return null;

            return new ChildDTO
            {
                ChildId = child.ChildId,
                FirstName = child.FirstName,
                LastName = child.LastName,
                BloodType = child.BloodType,
                Gender = child.Gender,
                DateOfBirth = child.DateOfBirth,
                UserId = child.UserId
            };
        }

        public ChildDTO AddChild(CreateChildDTO createChildDto)
        {
            var child = new Child
            {
                FirstName = createChildDto.FirstName,
                LastName = createChildDto.LastName,
                BloodType = createChildDto.BloodType,
                Gender = createChildDto.Gender,
                DateOfBirth = createChildDto.DateOfBirth,
                UserId = createChildDto.UserId // UserId is now set from the DTO
            };

            _context.Children.Add(child);
            _context.SaveChanges();

            // Return the created Child as a ChildDTO
            return new ChildDTO
            {
                ChildId = child.ChildId,
                FirstName = child.FirstName,
                LastName = child.LastName,
                BloodType = child.BloodType,
                Gender = child.Gender,
                DateOfBirth = child.DateOfBirth,
                UserId = child.UserId
            };
        }


        public ChildDTO UpdateChild(ChildDTO childDto)
        {
            var child = _context.Children.Find(childDto.ChildId);
            if (child == null) return null;

            child.FirstName = childDto.FirstName;
            child.LastName = childDto.LastName;
            child.BloodType = childDto.BloodType;
            child.Gender = childDto.Gender;
            child.DateOfBirth = childDto.DateOfBirth;
            child.UserId = childDto.UserId;

            _context.SaveChanges();

            return childDto;
        }

        public bool DeleteChild(int id)
        {
            var child = _context.Children.Find(id);
            if (child == null) return false;

            _context.Children.Remove(child);
            _context.SaveChanges();

            return true;
        }
    }
}
