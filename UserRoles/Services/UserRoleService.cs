using EmployeeManagement.Models;
using EmployeeManagement;
using NHibernate;
using System.Collections.Generic;
using ISession = NHibernate.ISession;

namespace EmployeeManagement.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly ISession _session;

        public UserRoleService(ISession session)
        {
            _session = session;
        }

        public IList<UserRole> GetAllRoles()
        {
            return _session.Query<UserRole>().ToList();
        }

        public UserRole GetRoleById(int id)
        {
            return _session.Get<UserRole>(id);
        }

        public void AddRole(UserRole role)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(role);
                transaction.Commit();
            }
        }

        public void UpdateRole(UserRole role)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Update(role);
                transaction.Commit();
            }
        }

        public void DeleteRole(int id)
        {
            using (var transaction = _session.BeginTransaction())
            {
                var role = _session.Get<UserRole>(id);
                if (role != null)
                {
                    _session.Delete(role);
                    transaction.Commit();
                }
            }
        }
    }
}
