using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Task4.DAL.CommandBuilders
{
    public class ConcurrentAddWhenNotExistCommandBuilder<TEntity>
        : IGenericSqlCommandBuilder<TEntity> where TEntity : class
    {
        protected class MappingDetail
        {
            public string ColumnName { get; set; }
            public string PropertyName { get; set; }
            public string ParamName { get; set; }
        }

        public DbContext Context { get; private set; }

        public ConcurrentAddWhenNotExistCommandBuilder(DbContext context)
        {
            Context = context;
        }

        private MappingFragment mappingFragment;
        protected MappingFragment Fragment
        {
            get
            {
                if (mappingFragment != null)
                {
                    return mappingFragment;
                }
                mappingFragment = ((StorageMappingItemCollection)((IObjectContextAdapter)Context).ObjectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace))
                    .OfType<EntityContainerMapping>()
                    .Single()
                    .EntitySetMappings
                    .Single(e => e.EntitySet.ElementType.Name == typeof(TEntity).Name)
                    .EntityTypeMappings
                    .Single()
                    .Fragments
                    .Single();
                return mappingFragment;
            }
        }

        private EntitySet entitySet;
        protected EntitySet EntitySet
        {
            get
            {
                if (entitySet == null)
                {
                    entitySet = Fragment.StoreEntitySet;
                }
                return entitySet;
            }
        }

        private ICollection<MappingDetail> mappingDetails;

        protected ICollection<MappingDetail> MappingDetails
        {
            get
            {
                if (mappingDetails == null)
                {
                    var c = EntitySet.ElementType.KeyMembers.Select(x => x.Name).ToHashSet();

                    mappingDetails = Fragment.PropertyMappings
                    .OfType<ScalarPropertyMapping>()
                    .Where(x => !x.Column.IsStoreGeneratedIdentity)
                    .Where(x => !c.Contains(x.Property.Name))
                        .Select(x => new MappingDetail()
                        {
                            ColumnName = x.Column.Name,
                            PropertyName = x.Property.Name,
                            ParamName = $"@_{x.Column.Name}"
                        }).ToList();
                }
                return mappingDetails;
            }
        }

        private string commandText;
        public string CommandText
        {
            get
            {
                if (commandText == null)
                {
                    string columnStringList = string
                        .Concat(MappingDetails
                        .Select(x => $"{x.ColumnName},"))
                        .TrimEnd(new char[] { ',' });

                    string paramStringList = string
                        .Concat(MappingDetails
                        .Select(x => $"{x.ParamName},"))
                        .TrimEnd(new char[] { ',' });

                    string tableName = $"[{EntitySet.Schema}].[{EntitySet.Table}]";
                    object configuration = ((IObjectContextAdapter)Context)
                        .ObjectContext
                        .CreateObjectSet<TEntity>()
                        .EntitySet
                        .MetadataProperties
                        .Where(x => x.Name == "Configuration")
                        .First()
                        .Value;

                    var a = configuration.GetType();

                    string uniquePropertyName = (configuration
                        .GetType()
                        .GetProperty("PropertyIndexes", BindingFlags.NonPublic | BindingFlags.Instance)
                        .GetValue(configuration) as IEnumerable<object>)
                        .SingleOrDefault().ToString();

                    var uniqueItem = MappingDetails
                        .SingleOrDefault(x => x.PropertyName == uniquePropertyName);

                    StringBuilder stringBulder = new StringBuilder();
                    stringBulder
                        .Append($"INSERT INTO {tableName} ({columnStringList})")
                        .Append($"SELECT ({paramStringList}) WHERE NOT EXISTS ")
                        .Append($"(SELECT 1 FROM {tableName} WITH (UPDLOCK)")
                        .Append($"WHERE {tableName}.{uniqueItem.ColumnName} = {MappingDetails.Single(x => x.PropertyName == uniqueItem.PropertyName).ParamName});"); ;
                    commandText = stringBulder.ToString();
                }
                return commandText;
            }
        }

        public SqlParameter[] GetParameters(TEntity entity)
        {
            var entityType = entity.GetType();
            return mappingDetails.Select(x =>
                new SqlParameter(x.ParamName, entityType.GetProperty(x.PropertyName).GetValue(entity))
                ).ToArray();
        }



    }
}