using Snowflake;
using System;
using System.Collections.Generic;
using System.Text;

namespace Waterful.Core
{
    public class IdGenerationService
    {
        private readonly static IdWorker _idWorker = new IdWorker(1, 4);
        public long GenerateId()
        {

            return _idWorker.NextId();
        }
    }
}
