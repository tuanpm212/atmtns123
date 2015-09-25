﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Website.Data
{
    public class Repository<T> where T : class 
    {
        #region Variables
            string MsgCode, MsgMessage;
        #endregion

        #region Get Data

        public List<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            try
            {
                List<T> list;
                using (var context = new DBEntities())
                {
                    IQueryable<T> dbQuery = context.Set<T>();

                    //Apply eager loading
                    foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);

                    list = dbQuery
                        .AsNoTracking()
                        .ToList<T>();
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public List<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            try
            {
                List<T> list;
                using (var context = new DBEntities())
                {
                    IQueryable<T> dbQuery = context.Set<T>();

                    //Apply eager loading
                    foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);

                    list = dbQuery
                        .AsNoTracking()
                        .Where(where)
                        .ToList<T>();
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            using (var context = new DBEntities())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }

        public T GetByID(object ID)
        {
            try
            {
                using (var context = new DBEntities())
                {
                    return context.Set<T>().Find(ID);
                }
            }
            catch
            {
                return null;
            }
        }

        public string GetMessage(string Code)
        {
            try
            {
                using (var context = new DBEntities())
                {
                    return context.Messages.Find(Code).Messages;
                }
            }
            catch
            {
                return null;
            }
        }

        public string GetMessage()
        {
            using (var context = new DBEntities())
            {
                return context.Messages.Find(MsgCode).Messages;
            }
        }

        public void AddMassage(string Code, string Message)
        {
            MsgCode = Code;
            MsgMessage = Message;
        }

        #endregion

        #region Insert, Update, Delete

        public bool Add(T item)
        {
            try
            {
                using (var context = new DBEntities())
                {
                    context.Entry(item).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                using (var context = new DBEntities())
                {
                    foreach (T item in items)
                    {
                        context.Entry(item).State = System.Data.Entity.EntityState.Added;
                    }
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Update(T item)
        {
            try
            {

                using (var context = new DBEntities())
                {
                    DbSet dbSet = context.Set<T>();
                    dbSet.Attach(item);
                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Update(List<T> items)
        {
            try
            {
                using (var context = new DBEntities())
                {
                    foreach (T item in items)
                    {
                        context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(object ID)
        {
            try
            {
                using (var context = new DBEntities())
                {
                    DbSet<T> dbSet;
                    dbSet = context.Set<T>();
                    T obj = context.Set<T>().Find(ID);
                    if (context.Entry(obj).State == EntityState.Detached)
                    {
                        dbSet.Attach(obj);
                    }
                    dbSet.Remove(obj);
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(List<T> items)
        {
            try
            {
                using (var context = new DBEntities())
                {
                    foreach (T item in items)
                    {
                        context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}