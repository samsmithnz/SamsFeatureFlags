using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace FeatureFlags.Models
{
    public class FeatureFlag : TableEntity
    {
        public FeatureFlag(string name)
        {
            PartitionKey = "FeatureFlag";
            RowKey = name;
        }

        public FeatureFlag()
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }

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
                    DevLastViewDate = DateTime.Now;
                }
                devViewCount = value;
            }
        }
        public DateTime? DevLastViewDate { get; set; }

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
                    QALastViewDate = DateTime.Now;
                }
                qaViewCount = value;
            }
        }
        public DateTime? QALastViewDate { get; set; }

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
                    ProdLastViewDate = DateTime.Now;
                }
                prodViewCount = value;
            }
        }
        public DateTime? ProdLastViewDate { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
