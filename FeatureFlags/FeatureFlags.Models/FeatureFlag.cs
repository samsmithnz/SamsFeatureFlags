using Azure;
using Azure.Data.Tables;
using System;

namespace FeatureFlags.Models
{
    public class FeatureFlag : ITableEntity
    {
        public FeatureFlag(string name)
        {
            PartitionKey = "FeatureFlag";
            RowKey = name;
        }

        public FeatureFlag()
        {
        }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        //PR
        public bool PRIsEnabled { get; set; }
        private int prViewCount;
        public int PRViewCount
        {
            get
            {
                return prViewCount;
            }
            set
            {
                if (value > prViewCount)
                {
                    PRLastViewDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                }
                prViewCount = value;
            }
        }
        public DateTime? PRLastViewDate { get; set; }
        
        //Dev
        public bool DevIsEnabled { get; set; }
        private int devViewCount;
        public int DevViewCount
        {
            get
            {
                return devViewCount;
            }
            set
            {
                if (value > devViewCount)
                {
                    DevLastViewDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                }
                devViewCount = value;
            }
        }
        public DateTime? DevLastViewDate { get; set; }

        //QA
        public bool QAIsEnabled { get; set; }
        private int qaViewCount;
        public int QAViewCount
        {
            get
            {
                return qaViewCount;
            }
            set
            {
                if (value > qaViewCount)
                {
                    QALastViewDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                }
                qaViewCount = value;
            }
        }
        public DateTime? QALastViewDate { get; set; }

        //Prod
        public bool ProdIsEnabled { get; set; }
        private int prodViewCount;
        public int ProdViewCount
        {
            get
            {
                return prodViewCount;
            }
            set
            {
                if (value > prodViewCount)
                {
                    ProdLastViewDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                }
                prodViewCount = value;
            }
        }
        public DateTime? ProdLastViewDate { get; set; }

        public DateTime LastUpdated { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
