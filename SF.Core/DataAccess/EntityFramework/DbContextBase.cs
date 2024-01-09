using Microsoft.EntityFrameworkCore;
using SF.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data;
using SF.Core.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SF.Core.Utilities.Comparators;
using SF.Core.Utilities.ExtensionMethods;
using System.Linq;
using SF.Core.CrossCuttingConcerns;

namespace SF.Core.DataAccess.EntityFramework
{
    public class DbContextBase : DbContext
    {



        public DbContextBase(DbContextOptions<DbContextBase> options)
    : base(options)
        {
        }



        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<AuditLogDetail> AuditLogDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AuditLogDetailMap());
        }


        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var modified = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified || x.State == EntityState.Deleted);
            var changeLogs = GenerateAuditLogs(modified.ToList());

            AuditLog.AddRange(changeLogs);




            var modified2 = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted);


            foreach (var item in modified2)
            {
                if (item.Entity is IEntity entity)
                {

                    if (item.State == EntityState.Added)
                    {
                        item.CurrentValues[nameof(IEntity.EklemeTarihi)] = DateTime.Now;
                        item.CurrentValues[nameof(IEntity.EkleyenId)] = CurrentUser.Id;

                        //soft delete ise eklendiğinde silindisi false olmalı

                        if (item.Entity is IEntitySoftDelete entityDeleted)
                        {

                            item.CurrentValues[nameof(IEntitySoftDelete.Silindi)] = false;
                        }



                    }
                    else if (item.State == EntityState.Modified)
                    {

                        item.CurrentValues[nameof(IEntity.DegistirmeTarihi)] = DateTime.Now;
                        item.CurrentValues[nameof(IEntity.DegistirenId)] = CurrentUser.Id;
                    }

                    else if (item.State == EntityState.Deleted) //soft delete işlemi
                    {

                        if (item.Entity is IEntitySoftDelete entityDeleted)
                        {
                            item.CurrentValues.SetValues(item.GetDatabaseValues()); //fk bağlantıların kopmaması için gerekli

                            item.CurrentValues[nameof(IEntitySoftDelete.Silindi)] = true;
                            item.CurrentValues[nameof(IEntity.DegistirmeTarihi)] = DateTime.Now;
                            item.CurrentValues[nameof(IEntity.DegistirenId)] = CurrentUser.Id;
                            item.State = EntityState.Modified;

                        }

                    }
                    item.CurrentValues[nameof(IEntity.RowVersion)] = item.CurrentValues[nameof(IEntity.RowVersion)].ToInt32() + 1;
                    //   entity.RowVersion = entity.RowVersion + 1;

                }


            }


            return base.SaveChanges();
        }



        #region private funcs

        private IEnumerable<AuditLog> GenerateAuditLogs(IEnumerable<EntityEntry> entries)
        {
            foreach (var entity in entries)
            {



                if (entity.Entity is IAuditable) // sadece loglanması gerekenler
                {
                    var auditLog = GenerateAuditLog(entity);

                    if (auditLog != null)
                    {
                        yield return auditLog;
                    }

                }

            }
        }

        private AuditLog GenerateAuditLog(EntityEntry entity)
        {
            if (entity.State == EntityState.Detached)
            {
                // no need to log audit entries.
                return null;
            }

            string crud = null;
            if (entity.State == EntityState.Modified)
            {
                crud = "U";
            }
            else if (entity.State == EntityState.Deleted)
            {
                crud = "D";
            }

            var auditLog = new AuditLog
            {
                Islem = crud,
                TabloAdi = GetTableName(entity), // entity.Entity.GetType().Name.Split('_')[0],
                KayitId = GetPrimaryKeyValue(entity),
                EklemeTarihi = DateTime.Now,
                KullaniciId = CurrentUser.Id,
                Ip = CurrentUser.Ip,
                AuditLogDetail = GenerateChangeLogDetails(entity)
            };


            return auditLog;
        }




        /// <summary>
        ///   GENEL KURAL her tabloda Id kolonu olacak , ara tablolarda dahil
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private long GetPrimaryKeyValue(EntityEntry entity)
        {



            if (entity.Property("Id") == null)
            {
                var type = entity.Entity.GetType();
                var name = type.Name;

                if (type.BaseType != null)
                {
                    //EF tipin  herzaman base entity si var
                    name = type.BaseType?.Name;
                }

                throw new MissingPrimaryKeyException($"{name} table has no Id column");
            }

            //GENEL KURAL her tabloda Id kolonu olacak , ara tablolarda dahil
            return entity.Property("Id").CurrentValue.ToLong();



            /*
			var type = entity.Entity.GetType();
			IEnumerable<System.Reflection.PropertyInfo> keys = GetPrimaryKeyProperties(type);
		  

		  
			int result =-1 ;
			foreach (var key in keys)
			{
				result = key.GetValue(entity.Entity).ToInt32();
			}

			return result;*/
        }
        private string GetTableName(EntityEntry entity)
        {
            var type = entity.Entity.GetType();
            if (type.BaseType != null)
            {
                //EF tipin  herzaman base entity si var
                return type.BaseType?.Name;
            }

            return type.Name;





        }



        private static List<AuditLogDetail> GenerateChangeLogDetails(EntityEntry entity)
        {
            var details = new List<AuditLogDetail>();


            var values = entity.State == EntityState.Deleted ? entity.OriginalValues : entity.CurrentValues;

            foreach (var propName in values.Properties)
            {
                var detail = GenerateChangeLogDetail(entity, propName.Name);

                if (detail != null)
                {
                    details.Add(detail);
                }
            }

            return details;
        }

        private static AuditLogDetail GenerateChangeLogDetail(EntityEntry entity, string propName)
        {


            object eskiVeri = null;
            //var current = entry.CurrentValues[property.Name]
            if (entity.State == EntityState.Modified || entity.State == EntityState.Deleted)
            {


                eskiVeri = entity.GetDatabaseValues().GetValue<object>(propName);
                //  eskiVeri = entity.OriginalValues.GetValue<object>(propName);
            }

            object yeniVeri;

            if (entity.State == EntityState.Deleted)
            {
                yeniVeri = null;
            }
            else
            {
                yeniVeri = entity.Property(propName).CurrentValue;
            }

            if (AreEqual(entity, propName, eskiVeri, yeniVeri))
            {
                return null;
            }

            return new AuditLogDetail
            {
                KolonAdi = propName,
                EskiVeri = eskiVeri?.ToString(),
                YeniVeri = yeniVeri?.ToString(),
            };

        }

        private static bool AreEqual(EntityEntry entity, string propName, object eskiVeri, object yeniVeri)
        {
            // Eski ve yeni veriler ayni olsa bile kaydediyoruz.
            // return false;

            var propertyType = entity.Entity.GetType().GetProperty(propName)?.PropertyType;
            var comparator = ComparatorFactory.GetComparator(propertyType);

            return comparator.AreEqual(eskiVeri, yeniVeri);

        }



        #endregion

    }
}
