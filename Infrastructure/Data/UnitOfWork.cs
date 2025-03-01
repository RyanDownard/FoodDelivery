﻿using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IGenericRepository<Category> _Category;
        private IGenericRepository<FoodType> _FoodType;
        private IGenericRepository<MenuItem> _MenuItem;
        private IGenericRepository<ApplicationUser> _ApplicationUser;
        private IGenericRepository<ShoppingCart> _shoppingCart;
        private IGenericRepository<OrderHeader> _orderHeader;
        private IGenericRepository<OrderDetails> _orderDetails;

        public IGenericRepository<ApplicationUser> ApplicationUser
        {
            get
            {
                if (_ApplicationUser == null) _ApplicationUser = new GenericRepository<ApplicationUser>(_dbContext);
                return _ApplicationUser;
            }
        }

        public IGenericRepository<Category> Category
        {
            get
            {
                if (_Category == null)
                {
                    _Category = new GenericRepository<Category>(_dbContext);
                }
                return _Category;
            }
        }

        public IGenericRepository<FoodType> FoodType
        {
            get
            {
                if (_FoodType == null)
                {
                    _FoodType = new GenericRepository<FoodType>(_dbContext);
                }
                return _FoodType;
            }
        }

        public IGenericRepository<MenuItem> MenuItem
        {
            get
            {
                if (_MenuItem == null)
                {
                    _MenuItem = new GenericRepository<MenuItem>(_dbContext);
                }
                return _MenuItem;
            }
        }

        public IGenericRepository<ShoppingCart> ShoppingCart
        {
            get
            {
                if (_shoppingCart == null)
                {
                    _shoppingCart = new GenericRepository<ShoppingCart>(_dbContext);
                }
                return _shoppingCart;
            }
        }

        public IGenericRepository<OrderHeader> OrderHeader
        {
            get
            {
                if(_orderHeader == null)
                {
                    _orderHeader = new GenericRepository<OrderHeader>(_dbContext);
                }
                return _orderHeader;
            }
        }

        public IGenericRepository<OrderDetails> OrderDetails
        {
            get
            {
                if (_orderDetails == null)
                {
                    _orderDetails = new GenericRepository<OrderDetails>(_dbContext);
                }
                return _orderDetails;
            }
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
