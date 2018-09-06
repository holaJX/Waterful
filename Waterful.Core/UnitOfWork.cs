using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Waterful.Core.Repository;

namespace Waterful.Core
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly PomeloMySqlDbContext _db;
        public UnitOfWork(PomeloMySqlDbContext dbContext)
        {
            this._db = dbContext;
        }

        #region Method

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return _db.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        public int SaveChange()
        {
            return _db.SaveChanges();
        }

        public Task<int> SaveChangeAsync()
        {
            return _db.SaveChangesAsync();
        }
        #endregion

        #region Property
        private IUserRepository userRepository;

        public IUserRepository UserRepository
        {
            get
            {
                return userRepository ??
                    (userRepository = new UserRepository(_db));
            }


        }
        private IWorkerRepository workerRepository;

        public IWorkerRepository WorkerRepository
        {
            get
            {
                return workerRepository ??
                    (workerRepository = new WorkerRepository(_db));
            }


        }

        private IProductRepository productRepository;

        public IProductRepository ProductRepository
        {
            get
            {
                return productRepository ??
                    (productRepository = new ProductRepository(_db));
            }


        }

        private IOrderRepository orderRepository;

        public IOrderRepository OrderRepository
        {
            get
            {
                return orderRepository ??
                    (orderRepository = new OrderRepository(_db));
            }


        }


        private IOrderItemRepository orderItemRepository;

        public IOrderItemRepository OrderItemRepository
        {
            get
            {
                return orderItemRepository ??
                    (orderItemRepository = new OrderItemRepository(_db));
            }


        }

        private ICustomerRepository customerRepository;

        public ICustomerRepository CustomerRepository
        {
            get
            {
                return customerRepository ??
                    (customerRepository = new CustomerRepository(_db));
            }


        }


        private ICouponUseRepository couponUseRepository;

        public ICouponUseRepository CouponUseRepository
        {
            get
            {
                return couponUseRepository ??
                    (couponUseRepository = new CouponUseRepository(_db));
            }


        }

        private ICouponRepository couponRepository;

        public ICouponRepository CouponRepository
        {
            get
            {
                return couponRepository ??
                    (couponRepository = new CouponRepository(_db));
            }


        }

        private IAftersaleRepository aftersaleRepository;

        public IAftersaleRepository AftersaleRepository
        {
            get
            {
                return aftersaleRepository ??
                    (aftersaleRepository = new AftersaleRepository(_db));
            }


        }

        private IAddressRepository addressRepository;

        public IAddressRepository AddressRepository
        {
            get
            {
                return addressRepository ??
                    (addressRepository = new AddressRepository(_db));
            }


        }


        private ICaptchaRepository captchaRepository;

        public ICaptchaRepository CaptchaRepository
        {
            get
            {
                return captchaRepository ??
                    (captchaRepository = new CaptchaRepository(_db));
            }


        }

        private ICommissionRepository commissionRepository;

        public ICommissionRepository CommissionRepository
        {
            get
            {
                return commissionRepository ??
                    (commissionRepository = new CommissionRepository(_db));
            }


        }

        private IUserinfoRepository userinfoRepository;

        public IUserinfoRepository UserinfoRepository
        {
            get
            {
                return userinfoRepository ??
                    (userinfoRepository = new UserinfoRepository(_db));
            }


        }

        private IUserchatRepository userchatRepository;

        public IUserchatRepository UserchatRepository
        {
            get
            {
                return userchatRepository ??
                    (userchatRepository = new UserchatRepository(_db));
            }


        }

        #endregion

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
